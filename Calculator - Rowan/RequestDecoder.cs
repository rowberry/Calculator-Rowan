using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using System.Text.Json;

namespace Calculator___Rowan
{
    internal class RequestDecoder
    {
        public string error = string.Empty;
        public Root operations = null;
        public Maths operationsXML = null;

        public RequestDecoder(string requestBody, string mediatype) {

            if(mediatype == "application/json")
            {
                //This is probably a json object
                try
                {
                    operations = decodeJSON(requestBody);

                    if(operations == null)
                    {
                        throw new Exception("Invalid request type");
                    }
                }
                catch {
                    error = "Unable to decode JSON request";
                }
            }
            else if (mediatype == "application/xml")
            {
                //this is probably an xml object
                try
                {
                    operationsXML = decodeXML(requestBody);

                    if (operationsXML == null)
                    {
                        throw new Exception("Invalid request type");
                    }
                }
                catch
                {
                    error = "Unable to decode XML request";
                }
            }
            else
            {
                //We are just going to try both
                try
                {
                    //first try JSON
                    operations = decodeJSON(requestBody);

                    if (operations == null)
                    {
                        throw new Exception("Invalid request type");
                    }
                }
                catch
                {
                    //Lets try XML
                    try
                    {
                        operationsXML = decodeXML(requestBody);

                        if (operationsXML == null)
                        {
                            throw new Exception("Invalid request type");
                        }
                    }
                    catch {
                        error = "Unable to decode the request";
                    }
                }
            }
        }

        private Root decodeJSON(string requestBody)
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            
            return System.Text.Json.JsonSerializer.Deserialize<Root>(requestBody, options);
        }

        private Maths decodeXML(string requestBody)
        {
            System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Maths));
            // Deserialize the XML data
            using (StringReader reader = new StringReader(requestBody))
            {
                return (Maths)xmlSerializer.Deserialize(reader);
            }
        }
    }
}
