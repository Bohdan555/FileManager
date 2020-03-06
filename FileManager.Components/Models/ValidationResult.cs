using TransactionManager.Components.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransactionManager.Components.Models
{
    public class ValidationResult
    {
        public ValidationResult(List<ValidationItem> errors)
        {
            Errors = errors;
        }

        public List<ValidationItem> Errors { get; } = new List<ValidationItem>();

        public void ThrowIfHasErrors()
        {
            if (Errors.Count > 0)
            {
                throw new DomainException("Model is invalid", Errors);
            }
        }
    }
}
