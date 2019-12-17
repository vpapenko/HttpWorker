using System.Net;
using System.Net.Http;
using System.Threading;

namespace HttpWorkerNUnitTests.Common
{
    internal class Converter
    {
        public int SleepTime { get; set; }

        public int IntConverter(HttpResponseMessage response, string content)
        {
            Thread.Sleep(SleepTime);
            return IntConverterStatic(response, content);
        }

        public static int IntConverterStatic(HttpResponseMessage response, string content)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new TestException();
            }

            return int.Parse(content);
        }
    }
}
