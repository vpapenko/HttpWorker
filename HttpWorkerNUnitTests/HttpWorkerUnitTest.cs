using HttpWorker;
using HttpWorkerNUnitTests.Common;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Tests
{
    public class HttpWorkerUnitTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task HttpWorkerTestSimpleRun()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("http://test").Respond("application/string", "10");
            var client = mockHttp.ToHttpClient();
            var worker = new Worker(client);

            HttpCall<int> call = new HttpCall<int>(Converter.IntConverterStatic)
            {
                HttpType = HttpCallTypeEnum.Get,
                Uri = new Uri("http://test")
            };

            var result = await worker.AddCall(call);
            Assert.AreEqual(10, result);
        }

        [Test]
        public void HttpWorkerTestMultipleRun()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("http://test/10").Respond("application/string", "10");
            mockHttp.When("http://test/20").Respond("application/string", "20");
            var client = mockHttp.ToHttpClient();
            var worker = new Worker(client);

            Assert.AreEqual(false, worker.Working);
            Run(10, worker).RunAndForget();
            Run(20, worker).RunAndForget();
            Assert.AreEqual(true, worker.Working);
            Thread.Sleep(300);
            Assert.AreEqual(false, worker.Working);
        }

        [Test]
        public void HttpWorkerTestCountOfUnprocessedHttpCalls()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("http://test/10").Respond("application/string", "10");
            mockHttp.When("http://test/20").Respond("application/string", "20");
            var client = mockHttp.ToHttpClient();
            var worker = new Worker(client);

            Assert.AreEqual(0, worker.CountOfUnprocessedHttpCalls);
            Run(10, worker).RunAndForget();
            Run(20, worker).RunAndForget();
            Assert.AreEqual(2, worker.CountOfUnprocessedHttpCalls);
            Thread.Sleep(3000);
            Assert.AreEqual(0, worker.CountOfUnprocessedHttpCalls);
        }

        [Test]
        public void HttpWorkerTestLongOperationInProcess()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("http://test/10").Respond("application/string", "10");
            mockHttp.When("http://test/20").Respond("application/string", "20");
            var client = mockHttp.ToHttpClient();
            var worker = new Worker(client)
            {
                LongOperationStartTime = 50
            };

            Assert.AreEqual(false, worker.Working);
            Assert.AreEqual(false, worker.LongOperationInProcess);
            Run(10, worker, 200).RunAndForget();
            Run(20, worker, 200).RunAndForget();
            Assert.AreEqual(true, worker.Working);
            Assert.AreEqual(false, worker.LongOperationInProcess);
            Thread.Sleep(100);
            Assert.AreEqual(true, worker.LongOperationInProcess);
            Assert.AreEqual(true, worker.Working);
            Thread.Sleep(400);
            Assert.AreEqual(false, worker.LongOperationInProcess);
            Assert.AreEqual(false, worker.Working);
        }

        [Test]
        public void HttpWorkerTestNetworkNotAvailable()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("http://test").Respond(GetHttpResponseMessage);
            mockHttp.When("http://test").Respond(GetHttpResponseMessage);
            var client = mockHttp.ToHttpClient();
            var worker = new Worker(client)
            {
                RetrySleepTime1 = 50,
                RetrySleepTime2 = 50
            };

            _networkNotAvailable = true;

            Assert.AreEqual(false, worker.Working);
            Assert.AreEqual(false, worker.NetworkNotAvailable);
            Run(worker, 100).RunAndForget();
            Run(worker, 100).RunAndForget();
            Assert.AreEqual(true, worker.Working);
            Assert.AreEqual(false, worker.NetworkNotAvailable);
            Thread.Sleep(200);
            Assert.AreEqual(true, worker.NetworkNotAvailable);
            Assert.AreEqual(true, worker.Working);
            _networkNotAvailable = false;
            Thread.Sleep(200);
            Assert.AreEqual(false, worker.NetworkNotAvailable);
            Assert.AreEqual(false, worker.Working);
        }

        bool _networkNotAvailable = false;
        public Task<HttpResponseMessage> GetHttpResponseMessage()
        {
            if (_networkNotAvailable)
            {
                throw new HttpRequestException();
            }
            else
            {
                var result = new HttpResponseMessage(HttpStatusCode.OK);
                return Task.FromResult(result);
            }
        }

        public async Task Run(int id, Worker worker, int sleepTime = 100)
        {
            var converter = new Converter() { SleepTime = sleepTime };
            HttpCall<int> call = new HttpCall<int>(converter.IntConverter)
            {
                HttpType = HttpCallTypeEnum.Get,
                Uri = new Uri($"http://test/{id}")
            };

            var result = await worker.AddCall(call);
            Assert.AreEqual(id, result);
        }
        public async Task Run(Worker worker, int sleepTime = 100)
        {
            HttpCall call = new HttpCall()
            {
                HttpType = HttpCallTypeEnum.Get,
                Uri = new Uri($"http://test")
            };

            await worker.AddCall(call);
        }
    }
}