using ConversionAPI.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Xml.Linq;
using System.Text;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;

namespace ConversionAPI.Processors
{
    public class ConversionProcessor : IConversionProcessor
    {
        private readonly IConversionService _conversionService;
        private static readonly XDeclaration _defaultDeclaration = new("1.0", null, null);
        public ConversionProcessor(IConversionService conversionService)
        {
            _conversionService = conversionService ?? throw new ArgumentNullException(nameof(conversionService));
        }
        public async Task<String> GetAll(string apiUrl, string type)
        {
            try
            {
                var jsonld = await _conversionService.GetAll(apiUrl);
                var jsonString = ConvertJsonLdToJson(jsonld);

                if (type.ToLower() == "json")
                {
                    return jsonString.ToString();
                }

                if (type.ToLower() == "csv")
                {
                    var csvString = ConvertJsonToCsv(jsonString);
                    return csvString;
                }

                if (type.ToLower() == "xml")
                {
                    string xmlString = ConvertJsonToXml(jsonString);
                    return xmlString;
                }


                return "Unrecognized Type";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static string ConvertJsonLdToJson(JObject jsonld)
        {
            JArray objectList = (JArray)jsonld["@list"];
            foreach (JObject obj in objectList)
            {
                if (obj.TryGetValue("@Id", out JToken idToken))
                {
                    obj.Remove("@Id");
                    obj.AddFirst(new JProperty("Id", idToken));
                }
                List<JProperty> propertiesToRemove = new List<JProperty>();
                foreach (JProperty prop in obj.Properties().ToList())
                {
                    if (prop.Name.StartsWith("@"))
                    {
                        string newName = prop.Name.Substring(1);
                        propertiesToRemove.Add(prop);
                        obj.Add(newName, prop.Value);
                    }
                }
                foreach (JProperty prop in propertiesToRemove)
                {
                    prop.Remove();
                }

            }

            string jsonString = JsonConvert.SerializeObject(objectList, Formatting.Indented, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            });
            return jsonString;
        }

        public static string ConvertJsonToXml(string json)
        {
            JArray jsonArray = JArray.Parse(json);

            XmlDocument xmlDocument = new XmlDocument();
            XmlElement rootNode = xmlDocument.CreateElement("object-list");
            xmlDocument.AppendChild(rootNode);

            foreach (JObject jsonObject in jsonArray)
            {
                XmlElement rowNode = xmlDocument.CreateElement("object");
                foreach (KeyValuePair<string, JToken> property in jsonObject)
                {
                    XmlElement propertyNode = xmlDocument.CreateElement(property.Key);
                    propertyNode.InnerText = property.Value.ToString();
                    rowNode.AppendChild(propertyNode);
                }
                rootNode.AppendChild(rowNode);
            }

            string xmlString = xmlDocument.OuterXml;
            return xmlString;
        }

        public static string ConvertJsonToCsv(string json)
        {
            var data = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(json);

            var csvBuilder = new StringBuilder();
            var header = string.Join(",", data[0].Keys);
            csvBuilder.AppendLine(header);

            foreach (var row in data)
            {
                var values = new List<string>();
                foreach (var key in row.Keys)
                {
                    values.Add(row[key]);
                }
                var rowString = string.Join(",", values);
                csvBuilder.AppendLine(rowString);
            }

            return csvBuilder.ToString();
        }      
    }
}
