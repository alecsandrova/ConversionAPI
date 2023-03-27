using NUnit.Framework;
using Moq;
using ConversionAPI.Services;
using ConversionAPI.Processors;
using Newtonsoft.Json.Linq;

namespace ConversionAPI.Tests
{
    public class ConversionTest
    {
        [TestFixture]
        public class ConversionProcessorTests
        {
            private IConversionProcessor _conversionProcessor;

            [SetUp]
            public void Setup()
            {
                // Use a mock implementation of IConversionService for testing
                var conversionService = new MockConversionService();
                _conversionProcessor = new ConversionProcessor(conversionService);
            }

            [Test]
            public async Task TestGetAllJson()
            {
                string apiUrl = "http://example.com/api";
                string type = "json";

                string result = await _conversionProcessor.GetAll(apiUrl, type);

                Assert.IsNotNull(result);
                // Assert that the result is a valid JSON string
                JToken.Parse(result);
            }

            [Test]
            public async Task TestGetAllCsv()
            {
                string apiUrl = "http://example.com/api";
                string type = "csv";

                string result = await _conversionProcessor.GetAll(apiUrl, type);

                Assert.IsNotNull(result);
                // Assert that the result is a valid CSV string
                Assert.IsTrue(result.Contains(","));
            }

            [Test]
            public async Task TestGetAllXml()
            {
                string apiUrl = "http://example.com/api";
                string type = "xml";

                string result = await _conversionProcessor.GetAll(apiUrl, type);

                Assert.IsNotNull(result);
                // Assert that the result is a valid XML string
                Assert.IsTrue(result.Contains("<?xml"));
            }
        }

        // Mock implementation of IConversionService for testing purposes
        public class MockConversionService : IConversionService
        {
            public Task<JObject> GetAll(string apiUrl)
            {
                // Return some test data as a JObject
                JObject testData = JObject.Parse(@"{
                '@list': [
                    { '@Id': 1, '@Name': 'Alice', 'Age': 30 },
                    { '@Id': 2, '@Name': 'Bob', 'Age': 40 },
                    { '@Id': 3, '@Name': 'Charlie', 'Age': 50 }
                ]
            }");
                return Task.FromResult(testData);
            }
        }
    }
}
