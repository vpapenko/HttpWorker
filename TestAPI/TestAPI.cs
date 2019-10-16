using HttpWorker;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TestAPI
{
    /// <summary>
    /// Test implementation of JSONPlaceholder API
    /// </summary>
    public class JSONPlaceholderTestAPI
        : ApiClientBase
    {
        private static readonly Uri baseUrl = new Uri(@"https://jsonplaceholder.typicode.com/");

        /// <summary>
        /// Method to test one of the test API of JSONPlaceholder
        /// </summary>
        /// <param name="id">Any id</param>
        /// <returns></returns>
        public async Task<JObject> TestMethod1(int id)
        {
            HttpCall<JObject> call = new HttpCall<JObject>(TestMethod1ResponseConverter)
            {
                HttpType = HttpCallTypeEnum.Get,
                Uri = new Uri(baseUrl, string.Format("posts/{0}", id.ToString()))
            };

            AddCall(call);
            return await call.Task;
        }

        /// <summary>
        /// Function to convert string HTTP response to JObject
        /// </summary>
        /// <param name="statusCode">HTTP status code of the request</param>
        /// <param name="response">Response from server</param>
        /// <returns></returns>
        private JObject TestMethod1ResponseConverter(HttpStatusCode statusCode, string response)
        {
            if(statusCode != HttpStatusCode.OK)
            {
                throw new Exception("Invalid status code.");
            }
            try
            {
                return JsonConvert.DeserializeObject<JObject>(response);
            }
            catch (JsonReaderException)
            {
                throw new Exception("Invalid response.");
            }
        }
    }
}
