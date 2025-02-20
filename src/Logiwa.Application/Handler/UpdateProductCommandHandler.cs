using FluentValidation;
using Logiwa.Application.Commands;
using Logiwa.Application.Exceptions;
using Logiwa.Application.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Logiwa.Application.Handler
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
    {
        private readonly IProductRepository _productRepository;
        private readonly IValidator<UpdateProductCommand> _validator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateProductCommandHandler> _logger;

        public UpdateProductCommandHandler(
            IProductRepository productRepository,
            IValidator<UpdateProductCommand> validator,
            IUnitOfWork unitOfWork,
            ILogger<UpdateProductCommandHandler> logger)
        {
            _productRepository = productRepository;
            _validator = validator;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling UpdateProductCommand for Product Id: {request.Id}");

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

                existingProduct.Name = request.Name;
                existingProduct.CategoryId = request.CategoryId;
                existingProduct.Description = request.Description;
                existingProduct.UpdatedDate = DateTime.UtcNow;

                _productRepository.Update(existingProduct);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _unitOfWork.CommitTransaction();

                _logger.LogInformation("Product update completed successfully.");
                return Unit.Value;
            }
            catch (ValidationException vex)
            {
                _logger.LogError(vex, "Validation exception occurred while updating product.");
                _unitOfWork.RollbackTransaction();
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while handling UpdateProductCommand.");
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }
    }
}