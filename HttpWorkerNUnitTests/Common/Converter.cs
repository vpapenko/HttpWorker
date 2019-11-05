using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace HttpWorkerNUnitTests.Common
{
    internal class Converter
    {
        public int SleepTime { get; set; }

        public int IntConverter(HttpStatusCode statusCode, string response)
        {
            Thread.Sleep(SleepTime);
            return Converter.IntConverterStatic(statusCode, response);
        }

        public static int IntConverterStatic(HttpStatusCode statusCode, string response)
        {
            if (statusCode != HttpStatusCode.OK)
            {
                throw new TestException();
            }

            return int.Parse(response);
        }
    }
}
