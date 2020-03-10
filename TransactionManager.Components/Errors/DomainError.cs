using System.Collections.Generic;

namespace TransactionManager.Components.Errors
{
    public class DomainError
    {
        public string Message { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public DomainError(string message, IEnumerable<string> errors)
        {
            this.Message = message;
            this.Errors = errors;
        }

        public DomainError(string message)
        {
            Message = message;
        }        
    }
}
