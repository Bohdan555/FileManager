using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TransactionManager.Components.Models
{
    [Serializable]
    [XmlRoot("Transactions")]
    public class XmlTransactions
    {
        [XmlElement("Transaction")]
        public List<XmlTransaction> Transactions;
    }
}
