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
            Run((int)numericUpDown1.Value).RunAndForget();
        }

        public async Task Run(int count)
        {
            Guid runId = Guid.NewGuid();
            for (var id = 1; id <= numericUpDown1.Value; id++)
            {
                listBox1.Items.Add(string.Format("{0}. Request for id {1}", runId.ToString(), id.ToString()));
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                try
                {
                    var r = await api.TestMethod1(id);
                    listBox2.Items.Add(string.Format("{0}. Request for id {1} is completed.", runId.ToString(), r["id"].ToString()));
                    listBox2.SelectedIndex = listBox2.Items.Count - 1;
                }
                catch (Exception ex)
                {
                    listBox2.Items.Add(string.Format("{0}. Exception while call for id {1}. {2}", runId.ToString(), id.ToString(), ex.ToString()));
                    listBox2.SelectedIndex = listBox2.Items.Count - 1;
                }
            }
            listBox1.Items.Add(string.Format("{0}. All requests are send.", runId.ToString()));
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
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
    }

    public static class TaskExtensions
    {
        public static async void RunAndForget(this Task task)
        {
            await task;
        }
    }
}
