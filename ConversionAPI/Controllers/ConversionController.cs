using ConversionAPI.Processors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace ConversionAPI.Controllers
{
    [ApiController]
    [Route("/get_data")]
    public class ConversionController : ControllerBase
    {
        private readonly IConversionProcessor _conversionProcessor;


        public ConversionController(IConversionProcessor conversionProcessor)
        {
            _conversionProcessor = conversionProcessor ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Get all data
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string apiUrl)
        {
            try
            {
                String conversion = await _conversionProcessor.GetAll(apiUrl);
                return Ok(conversion);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
