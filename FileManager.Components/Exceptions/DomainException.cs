using TransactionManager.Components.Models;
using System;
using System.Collections.Generic;

namespace TransactionManager.Components.Exceptions
{
    public class DomainException: Exception
    {
        public List<string> Errors { get; set; }

        public DomainException(string message, List<string> errors) : base(message)           
        {
            this.Errors = errors;
        }

        public DomainException(string message, Exception innerException): base(message, innerException)
        {
        }        

        public DomainException(string message): base(message)
        { 
        }

        public override string ToString()
        {         
            return $"{Message}: {string.Join(",", Errors)}";        
        }
    }
}
