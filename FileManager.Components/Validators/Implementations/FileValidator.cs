using TransactionManager.Components.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Transaction = TransactionManager.Components.Models.Transaction;

namespace TransactionManager.Components.Validators.Implementations
{
    public class FileValidator : IValidator<List<Transaction>>
    {
        public async Task<ValidationResult> Validate(List<Transaction> target)
        {
            Transaction item;
            var errors = new List<ValidationItem>();
            
            for (var index = 0; index < target.Count; index++)
            {
                item = target[index];
                if (string.IsNullOrEmpty(item.TransactionId))
                    errors.Add(new ValidationItem
                    {
                        Name = nameof(item.TransactionId),
                        Message = $"{nameof(item.TransactionId)} field is required in {index + 1} transaction"
                    });

                if (string.IsNullOrEmpty(item.CurrencyCode))
                    errors.Add(new ValidationItem
                    {
                        Name = nameof(item.CurrencyCode),
                        Message = $"{nameof(item.CurrencyCode)} field is required in {index + 1} transaction"
                    });

                if (!item.Amount.HasValue)
                    errors.Add(new ValidationItem
                    {
                        Name = nameof(item.Amount),
                        Message = $"{nameof(item.Amount)} field is required in {index + 1} transaction"
                    });

                if (string.IsNullOrEmpty(item.Status))
                    errors.Add(new ValidationItem
                    {
                        Name = nameof(item.Status),
                        Message = $"{nameof(item.Status)} field is required in {index + 1} transaction"
                    });

                if (!item.TransactionDate.HasValue)
                    errors.Add(new ValidationItem
                    {
                        Name = nameof(item.TransactionDate),
                        Message = $"{nameof(item.TransactionDate)} field is required in {index + 1} transaction"
                    });
            }
            return await Task.FromResult(new ValidationResult(errors));
        }        
    }
}
