using FluentValidation;
using Logiwa.Application.Commands;
using Logiwa.Application.Exceptions;
using Logiwa.Application.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Logiwa.Application.Handler;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IProductRepository _productRepository;
    private readonly IValidator<DeleteProductCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteProductCommandHandler> _logger;

    public DeleteProductCommandHandler(
        IProductRepository productRepository,
        IValidator<DeleteProductCommand> validator,
        IUnitOfWork unitOfWork,
        ILogger<DeleteProductCommandHandler> logger)
    {
        _productRepository = productRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Handling DeleteProductCommand for Product Id: {request.Id}");

        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            _logger.LogInformation("Validating request...");
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(Environment.NewLine, validationResult.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Validation failed: {Errors}", errors);
                throw new ValidationException("Validation failed: " + errors);
            }

            var existingProduct =
                await _productRepository.GetSingleAsync(s => s.Id.Equals(request.Id), "", cancellationToken);
            if (existingProduct == null)
            {
                _logger.LogWarning("Product not found with Id: {ProductId}", request.Id);
                throw new BusinessException($"Product not found with Id: {request.Id}");
            }

            existingProduct.IsDeleted = true;
            existingProduct.UpdatedDate = DateTime.UtcNow;

            _productRepository.Update(existingProduct);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _unitOfWork.CommitTransaction();

            _logger.LogInformation("Product deleted (soft delete) successfully.");
            return true;
        }
        catch (ValidationException vex)
        {
            _logger.LogError(vex, "Validation exception occurred while deleting product.");
            _unitOfWork.RollbackTransaction();
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while handling DeleteProductCommand.");
            _unitOfWork.RollbackTransaction();
            throw;
        }
    }
}