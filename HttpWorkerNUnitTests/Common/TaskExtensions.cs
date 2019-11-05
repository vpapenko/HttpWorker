using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HttpWorkerNUnitTests.Common
{
    public static class TaskExtensions
    {
        public static async void RunAndForget(this Task task)
        {
            await task;
        }
    }
}
