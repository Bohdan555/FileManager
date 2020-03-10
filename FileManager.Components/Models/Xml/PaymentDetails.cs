using System;
using System.Xml.Serialization;

namespace TransactionManager.Components.Models
{
    [Serializable]
    public class PaymentDetails
    {        
        [XmlElement("Amount")]
        public string Amount { get; set; }        

        [XmlElement("CurrencyCode")]
        public string CurrencyCode { get; set; }        
    }
}
