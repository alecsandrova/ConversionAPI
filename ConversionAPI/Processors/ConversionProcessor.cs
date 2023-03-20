using ConversionAPI.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Text.Json.Nodes;

namespace ConversionAPI.Processors
{
    public class ConversionProcessor : IConversionProcessor
    {
        private readonly IConversionService _conversionService;
        public ConversionProcessor(IConversionService conversionService)
        {
            _conversionService = conversionService ?? throw new ArgumentNullException(nameof(conversionService));
        }
        public async Task<String> GetAll(string apiUrl, string type)
        {
            try
            {
                var conversion = await _conversionService.GetAll(apiUrl);
                JArray objectList = (JArray)conversion["@list"];
                foreach (JObject obj in objectList)
                {
                    if (obj.TryGetValue("@Id", out JToken idToken))
                    {
                        obj.Remove("@Id");
                        obj.AddFirst(new JProperty("id", idToken));
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


                if (type.ToLower() == "json")
                {
                    return jsonString.ToString();
                }

                return conversion.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
