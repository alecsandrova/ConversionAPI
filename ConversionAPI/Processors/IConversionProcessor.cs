namespace ConversionAPI.Processors
{
    public interface IConversionProcessor
    {
        Task<String> GetAll(string apiUrl);
    }
}
