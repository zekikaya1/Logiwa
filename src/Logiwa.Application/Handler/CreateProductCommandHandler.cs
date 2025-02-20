using FluentValidation;
using Logiwa.Application.Commands;
using Logiwa.Application.Exceptions;
using Logiwa.Application.Repositories;
using Logiwa.Core.Entities;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Logiwa.Application.Handler;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Unit>
{
    private readonly IProductRepository _productRepository;
    private readonly IValidator<CreateProductCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateProductCommandHandler> _logger;

    public CreateProductCommandHandler(
        IProductRepository productRepository,
        IValidator<CreateProductCommand> validator, 
        IUnitOfWork unitOfWork,
        ILogger<CreateProductCommandHandler> logger)
    {
        _productRepository = productRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Handling CreateProductCommand for Product Name: {request.Name}");

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
            
            var isExist = await _productRepository.HasAnyProductByName(request.Name);
            if (isExist)
            {
                _logger.LogInformation("product already exists with productName: {request.Name}",request.Name);
                  
                throw new BusinessException($"product already exists with productName:{request.Name}");
            }

            var product = request.Adapt<Product>();
            product.UpdatedDate = DateTime.UtcNow;

            await _productRepository.Insert(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _unitOfWork.CommitTransaction();

            _logger.LogInformation("Product creation completed successfully.");
            return Unit.Value;
        }
        catch (ValidationException vex)
        {
            _logger.LogError(vex, "Validation exception occurred while creating product.");
            _unitOfWork.RollbackTransaction();
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while handling CreateProductCommand.");
            _unitOfWork.RollbackTransaction();
            throw;
        }
    }
}