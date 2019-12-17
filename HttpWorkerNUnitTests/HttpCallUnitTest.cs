using System;
using System.Net;
using System.Threading.Tasks;
using HttpWorker;
using HttpWorkerNUnitTests.Common;
using NUnit.Framework;

namespace HttpWorkerNUnitTests
{
    public class HttpCallUnitTest
    {
        [Test]
        public void HttpCallTestSimpleResult()
        {
            var httpCall = new HttpCall<int>(Converter.IntConverterStatic);
            Assert.AreEqual(TaskStatus.WaitingForActivation, httpCall.Task.Status);
            Assert.AreEqual(null, httpCall.Task.Exception);
            httpCall.SetResult(HttpStatusCode.OK, "10");
            Assert.AreEqual(TaskStatus.RanToCompletion, httpCall.Task.Status);
            Assert.AreEqual(10, httpCall.Task.Result);
        }

        [Test]
        public void HttpCallTestWrongHttpStatusCode()
        {
            var httpCall = new HttpCall<int>(Converter.IntConverterStatic);
            Assert.AreEqual(TaskStatus.WaitingForActivation, httpCall.Task.Status);
            Assert.AreEqual(null, httpCall.Task.Exception);
            httpCall.SetResult(HttpStatusCode.BadRequest, "10");
            Assert.AreEqual(TaskStatus.Faulted, httpCall.Task.Status);
            Assert.AreEqual(typeof(TestException), httpCall.Task.Exception.InnerException.GetType());
        }

        [Test]
        public void HttpCallTestExceptionInResponseConverter()
        {
            var httpCall = new HttpCall<int>(Converter.IntConverterStatic);
            Assert.AreEqual(TaskStatus.WaitingForActivation, httpCall.Task.Status);
            Assert.AreEqual(null, httpCall.Task.Exception);
            httpCall.SetResult(HttpStatusCode.OK, "a");
            Assert.AreEqual(TaskStatus.Faulted, httpCall.Task.Status);
            Assert.AreEqual(typeof(FormatException), httpCall.Task.Exception.InnerException.GetType());
        }
    }
}