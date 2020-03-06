using System;
using System.Globalization;
using System.Xml.Serialization;

namespace TransactionManager.Components.Models
{
    [Serializable]
    public class XmlTransaction
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlElement("PaymentDetails")]
        public PaymentDetails PaymentDetails { get; set; }

        [XmlElement("TransactionDate")]
        public string TransactionDate
        {
            set
            {
                DateTime parseDate;
                if (DateTime.TryParseExact(value, validDateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out parseDate))
                {
                    transactionDate = parseDate;
                }
            }
            get
            {
                return transactionDate.HasValue ? transactionDate.Value.ToString("dd/MM/yyyy HH:mm:ss") : default;                
            }
        }

        [XmlElement("Status")]
        public string Status { get; set; }

        [XmlIgnore]
        public DateTime? transactionDate;

        [XmlIgnore]
        private static readonly string[] validDateFormats = new string[] { "dd/MM/yyyy", "dd/MM/yyyy HH:mm:ss" };
    }
}
