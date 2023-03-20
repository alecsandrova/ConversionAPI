using ConversionAPI.Services;

namespace ConversionAPI.Processors
{
    public class ConversionProcessor : IConversionProcessor
    {
        private readonly IConversionService _conversionService;
        public ConversionProcessor(IConversionService conversionService)
        {
            _conversionService = conversionService ?? throw new ArgumentNullException(nameof(conversionService));
        }
        public async Task<String> GetAll(string apiUrl)
        {
            try
            {
                var rockets = await _conversionService.GetAll(apiUrl);


                return rockets.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
