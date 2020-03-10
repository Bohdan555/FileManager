using TransactionManager.Components.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Transaction = TransactionManager.Components.Models.Transaction;
using System;
using System.Globalization;
using TransactionManager.Components.Domain.Enums;

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

                if (string.IsNullOrEmpty(item.Amount))
                {
                    errors.Add(new ValidationItem
                    {
                        Name = nameof(item.Amount),
                        Message = $"{nameof(item.Amount)} field is required in {index + 1} transaction"
                    });
                }
                else if(!double.TryParse(item.Amount, out var parsedAmount))
                {
                    errors.Add(new ValidationItem
                    {
                        Name = nameof(item.Amount),
                        Message = $"{nameof(item.Amount)} field is invalid in {index + 1} transaction"
                    });
                }

                if (string.IsNullOrEmpty(item.Status))
                {
                    errors.Add(new ValidationItem
                    {
                        Name = nameof(item.Status),
                        Message = $"{nameof(item.Status)} field is required in {index + 1} transaction"
                    });
                }
                else if (!Enum.TryParse<Status>(item.Status, out var parsedStatus))
                {
                    errors.Add(new ValidationItem
                    {
                        Name = nameof(item.Status),
                        Message = $"{nameof(item.Status)} field is invalid in {index + 1} transaction"
                    });
                }

                if (string.IsNullOrEmpty(item.TransactionDate))
                {
                    errors.Add(new ValidationItem
                    {
                        Name = nameof(item.TransactionDate),
                        Message = $"{nameof(item.TransactionDate)} field is required in {index + 1} transaction"
                    });
                }
                else if (!DateTime.TryParseExact(item.TransactionDate, validDateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parseDate))
                {
                    errors.Add(new ValidationItem
                    {
                        Name = nameof(item.TransactionDate),
                        Message = $"{nameof(item.TransactionDate)} field is invalid in {index + 1} transaction"
                    });
                }
            }
            return await Task.FromResult(new ValidationResult(errors));
        }
        private static readonly string[] validDateFormats = new string[] { "yyyy-MM-ddTHH:mm:ss", "dd/MM/yyyy HH:mm:ss" };
    }
}
