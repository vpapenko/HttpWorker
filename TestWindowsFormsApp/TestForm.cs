using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAPI;

namespace TestWindowsFormsApp
{
    public partial class TestForm : Form
    {
        readonly JSONPlaceholderTestAPI api = new JSONPlaceholderTestAPI();

        public TestForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            api.PropertyChanged += Api_PropertyChanged;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Run((int)requestsCount.Value, parallelCheckBox.Checked).RunAndForget();
        }

        public async Task Run(int count, bool parallel)
        {
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

            AddToListBox(listBox1, string.Format("{0}. All requests are send.", runId.ToString()));
        }

        public async Task Run(Guid runId, int id)
        {
            AddToListBox(listBox1, string.Format("{0}. Request for id {1}", runId.ToString(), id.ToString()));
            try
            {
                var r = await api.TestMethod1(id);
                AddToListBox(listBox2, string.Format("{0}. Request for id {1} is completed.", runId.ToString(), r["id"].ToString()));
            }
            catch (Exception ex)
            {
                AddToListBox(listBox2, string.Format("{0}. Exception while call for id {1}. {2}", runId.ToString(), id.ToString(), ex.ToString()));
            }
        }

        private void Api_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            JSONPlaceholderTestAPI api = (JSONPlaceholderTestAPI)sender;
            switch (e.PropertyName)
            {
                case (nameof(api.LongOperationInProcess)):
                    longOperationInProcess.BackColor = api.LongOperationInProcess ? Color.Yellow : Control.DefaultBackColor;
                    break;
                case (nameof(api.NetworkNotAvailable)):
                    networkNotAvailable.BackColor = api.NetworkNotAvailable ? Color.Red : Control.DefaultBackColor;
                    break;
                case (nameof(api.Working)):
                    working.BackColor = api.Working ? Color.Green : Control.DefaultBackColor;
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
