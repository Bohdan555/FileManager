using CsvHelper.Configuration.Attributes;
using System;

namespace TransactionManager.Components.Models.Csv
{
    public class CsvTransaction
    {
        [Name("Id")]
        public string Id { get; set; }

        [Name("Amount")]
        public decimal? Amount { get; set; }

        [Name("Currency Code")]
        public string CurrencyCode { get; set; }

        [Name("Transaction Date")]
        public DateTime? TransactionDate { get; set; }

        [Name("Status")]
        public string Status { get; set; }
    }
}
