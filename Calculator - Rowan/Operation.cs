using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Calculator___Rowan
{
    public class MyOperation
    {
        [JsonPropertyName("@ID")]
        public string ID { get; set; }
        public List<string> Value { get; set; }

        [JsonPropertyName("MyOperation")]
        public MyOperation NestedOperation { get; set; }
    }

    public class MyMaths
    {
        public MyOperation MyOperation { get; set; }
    }

    public class Root
    {
        public MyMaths MyMaths { get; set; }
    }
}
