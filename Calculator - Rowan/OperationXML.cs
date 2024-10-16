using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Calculator___Rowan
{
    [XmlRoot("Operation")]
    public class OperationXML
    {
        [XmlAttribute("ID")]
        public string ID { get; set; }

        [XmlElement("Value")]
        public List<string> Value { get; set; }

        [XmlElement("Operation")]
        public List<OperationXML> NestedOperations { get; set; }
    }

    [XmlRoot("Maths")]
    public class Maths
    {
        [XmlElement("Operation")]
        public OperationXML Operation { get; set; }
    }
}
