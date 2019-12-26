using HttpWorker;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestAPI
{
    /// <summary>
    /// Test implementation of JSONPlaceholder API
    /// </summary>
    public class JsonPlaceholderTestApi
        : ApiClientBase
    {
        private static readonly Uri BaseUrl = new Uri(@"https://jsonplaceholder.typicode.com/");

        /// <summary>
        /// Method to test one of the test API of JSONPlaceholder
        /// </summary>
        /// <param name="id">Any id</param>
        /// <returns></returns>
        public async Task<JObject> TestMethod1(int id)
        {
            var uri = new Uri(BaseUrl, $"posts/{id.ToString()}");
            return await AddGetCall(uri, TestMethod1ResponseConverter);
        }

        /// <summary>
        /// Function to convert string HTTP response to JObject
        /// </summary>
        /// <param name="response">HTTP response of operation</param>
        /// <param name="content">Response from server</param>
        /// <returns></returns>
        private JObject TestMethod1ResponseConverter(HttpResponseMessage response, string content)
        {
            if(response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Invalid status code {response.StatusCode}");
            }
            try
            {
                return JsonConvert.DeserializeObject<JObject>(content);
            }
            catch (JsonReaderException)
            {
                throw new Exception("Invalid response.");
            }
        }
    }
}
