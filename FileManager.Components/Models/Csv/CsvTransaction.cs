using CsvHelper.Configuration.Attributes;
using System;

namespace TransactionManager.Components.Models.Csv
{
    [Serializable]
    public class CsvTransaction
    {
        [Name("Id")]        
        public string TransactionId { get; set; }

        [Name("Amount")]       
        public string Amount { get; set; }

        [Name("Currency Code")]
        public string CurrencyCode { get; set; }

        [Name("Transaction Date")]
        public string TransactionDate { get; set; }

        [Name("Status")]
        public string Status { get; set; }
    }
}
