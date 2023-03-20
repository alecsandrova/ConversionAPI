using Newtonsoft.Json.Linq;

namespace ConversionAPI.Services
{
    public interface IConversionService
    {
        Task<JObject> GetAll(string apiUrl);
    }
}
