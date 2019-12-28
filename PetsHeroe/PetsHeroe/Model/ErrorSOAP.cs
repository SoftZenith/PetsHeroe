using System;
using System.Xml.Serialization;

namespace PetsHeroe.Model
{
    [Serializable, XmlRoot("Error")]
    public class ErrorSOAP
    {
        public string ErrorNumber { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorSource { get; set; }
        public int LineNumber { get; set; }
    }
}
