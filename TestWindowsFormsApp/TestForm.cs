using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAPI;

namespace TestWindowsFormsApp
{
    public partial class TestForm : Form
    {
        private readonly JsonPlaceholderTestApi _api = new JsonPlaceholderTestApi();

        public TestForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _api.PropertyChanged += Api_PropertyChanged;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Run((int)requestsCount.Value, parallelCheckBox.Checked).RunAndForget();
        }

        public async Task Run(int count, bool parallel)
        {
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));
            var runId = Guid.NewGuid();
            for (var id = 1; id <= requestsCount.Value; id++)
            {
                if(parallel)
                {
                    Run(runId, id).RunAndForget();
                }
                else
                {
                    await Run(runId, id);
                }
            }

            AddToListBox(requestListBox, $"{runId.ToString()}. All requests are send.");
        }

        public async Task Run(Guid runId, int id)
        {
            AddToListBox(requestListBox, $"{runId.ToString()}. Request for id {id.ToString()}");
            try
            {
                var r = await _api.TestMethod1(id);
                AddToListBox(responseListBox, $"{runId.ToString()}. Request for id {r["id"]} is completed.");
            }
            catch (Exception ex)
            {
                AddToListBox(responseListBox,
                    $"{runId.ToString()}. Exception while call for id {id.ToString()}. {ex}");
            }
        }

        private void Api_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var api = (JsonPlaceholderTestApi)sender;
            switch (e.PropertyName)
            {
                case (nameof(api.LongOperationInProcess)):
                    longOperationInProcessLabel.BackColor = api.LongOperationInProcess ? Color.Yellow : Control.DefaultBackColor;
                    break;
                case (nameof(api.NetworkNotAvailable)):
                    networkNotAvailableLabel.BackColor = api.NetworkNotAvailable ? Color.Red : Control.DefaultBackColor;
                    break;
                case (nameof(api.Working)):
                    workingLabel.BackColor = api.Working ? Color.Green : Control.DefaultBackColor;
                    break;
                case (nameof(api.CountOfUnprocessedHttpCalls)):
                    unprocessedLabel.Text = $"Unprocessed {api.CountOfUnprocessedHttpCalls}";
                    break;
            }
        }

        private void AddToListBox(ListBox listBox, string text)
        {
            listBox.Items.Add(text);
            listBox.SelectedIndex = listBox.Items.Count - 1;
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
