using HttpWorker;
using HttpWorkerNUnitTests.Common;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Tests
{
    public class HttpCallUnitTest
    {
        [Test]
        public void HttpCallTestSimpleResult()
        {
            HttpCall<int> HhttpCall = new HttpCall<int>(Converter.IntConverterStatic);
            Assert.AreEqual(TaskStatus.WaitingForActivation, HhttpCall.Task.Status);
            Assert.AreEqual(null, HhttpCall.Task.Exception);
            HhttpCall.SetResult(HttpStatusCode.OK, "10");
            Assert.AreEqual(TaskStatus.RanToCompletion, HhttpCall.Task.Status);
            Assert.AreEqual(10, HhttpCall.Task.Result);
        }

        [Test]
        public void HttpCallTestWrongHttpStatusCode()
        {
            HttpCall<int> HttpCall = new HttpCall<int>(Converter.IntConverterStatic);
            Assert.AreEqual(TaskStatus.WaitingForActivation, HttpCall.Task.Status);
            Assert.AreEqual(null, HttpCall.Task.Exception);
            HttpCall.SetResult(HttpStatusCode.BadRequest, "10");
            Assert.AreEqual(TaskStatus.Faulted, HttpCall.Task.Status);
            Assert.AreEqual(typeof(TestException), HttpCall.Task.Exception.InnerException.GetType());
        }

        [Test]
        public void HttpCallTestExceptionInResponseConverter()
        {
            HttpCall<int> HttpCall = new HttpCall<int>(Converter.IntConverterStatic);
            Assert.AreEqual(TaskStatus.WaitingForActivation, HttpCall.Task.Status);
            Assert.AreEqual(null, HttpCall.Task.Exception);
            HttpCall.SetResult(HttpStatusCode.OK, "a");
            Assert.AreEqual(TaskStatus.Faulted, HttpCall.Task.Status);
            Assert.AreEqual(typeof(FormatException), HttpCall.Task.Exception.InnerException.GetType());
        }
    }
}