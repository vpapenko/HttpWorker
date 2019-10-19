using System;
using System.Threading.Tasks;
using TestAPI;

namespace TestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var asyncUI = new AsyncUI();
            asyncUI.Start();
        }

        /// <summary>
        /// Class for simple async UI.
        /// </summary>
        private class AsyncUI
        {
            JSONPlaceholderTestAPI api = new JSONPlaceholderTestAPI();
            public AsyncUI()
            {
                api.PropertyChanged += Api_PropertyChanged;
            }

            public void Start()
            {
                Console.WriteLine("How many requests? Try to turn Internet connection off while process of requests.");
                while (true)
                {
                    if (int.TryParse(Console.ReadLine(), out int count))
                    {
                        //Run all requests and don't wait for results.
                        for (var i = 1; i <= count; i++)
                        {
                            Run(i).RunAndForget();
                        }
                        Console.WriteLine("All requests are send.");
                    }
                    else
                    {
                        Console.WriteLine("Wrong input");
                    }
                }
            }

            public async Task Run(int id)
            {
                Console.WriteLine("Request for id {0}", id.ToString());
                try
                {
                    var r = await api.TestMethod1(id);
                    Console.WriteLine("Request for id {0} is completed.", r["id"].ToString());
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Exception while call for id {0}. {1}", id.ToString(), ex.ToString());
                }
            }

            /// <summary>
            /// Print all updates of API statuses
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private static void Api_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
            {
                JSONPlaceholderTestAPI api = (JSONPlaceholderTestAPI)sender;
                switch (e.PropertyName)
                {
                    case (nameof(api.LongOperationInProcess)):
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(nameof(api.LongOperationInProcess) + ": {0}", api.LongOperationInProcess.ToString());
                        Console.ResetColor();
                        break;
                    case (nameof(api.NetworkNotAvailable)):
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(nameof(api.NetworkNotAvailable) + ": {0}", api.NetworkNotAvailable.ToString());
                        Console.ResetColor();
                        break;
                    case (nameof(api.Working)):
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(nameof(api.Working) + ": {0}", api.Working.ToString());
                        Console.ResetColor();
                        break;
                }
            }
        }
    }

    public static class TaskExtensions
    {
        public static async void RunAndForget(this Task task)
        {
            await task;
        }
    }
}
