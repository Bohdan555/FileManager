using System;
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
        public string TransactionDate { get; set; }

        [XmlElement("Status")]
        public string Status { get; set; }                
    }
}
