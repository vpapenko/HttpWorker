namespace TestWindowsFormsApp
{
    partial class TestForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.RunButton = new System.Windows.Forms.Button();
            this.requestsCount = new System.Windows.Forms.NumericUpDown();
            this.networkNotAvailable = new System.Windows.Forms.Label();
            this.working = new System.Windows.Forms.Label();
            this.longOperationInProcess = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.parallelCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.requestsCount)).BeginInit();
            this.SuspendLayout();
            // 
            // RunButton
            // 
            this.RunButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RunButton.Location = new System.Drawing.Point(713, 12);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(75, 23);
            this.RunButton.TabIndex = 0;
            this.RunButton.Text = "Run";
            this.RunButton.UseVisualStyleBackColor = true;
            this.RunButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // requestsCount
            // 
            this.requestsCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.requestsCount.Location = new System.Drawing.Point(521, 15);
            this.requestsCount.Name = "requestsCount";
            this.requestsCount.Size = new System.Drawing.Size(120, 20);
            this.requestsCount.TabIndex = 1;
            this.requestsCount.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // networkNotAvailable
            // 
            this.networkNotAvailable.AutoSize = true;
            this.networkNotAvailable.Location = new System.Drawing.Point(12, 17);
            this.networkNotAvailable.Name = "networkNotAvailable";
            this.networkNotAvailable.Size = new System.Drawing.Size(107, 13);
            this.networkNotAvailable.TabIndex = 2;
            this.networkNotAvailable.Text = "NetworkNotAvailable";
            // 
            // working
            // 
            this.working.AutoSize = true;
            this.working.Location = new System.Drawing.Point(255, 17);
            this.working.Name = "working";
            this.working.Size = new System.Drawing.Size(47, 13);
            this.working.TabIndex = 3;
            this.working.Text = "Working";
            // 
            // longOperationInProcess
            // 
            this.longOperationInProcess.AutoSize = true;
            this.longOperationInProcess.Location = new System.Drawing.Point(125, 17);
            this.longOperationInProcess.Name = "longOperationInProcess";
            this.longOperationInProcess.Size = new System.Drawing.Size(124, 13);
            this.longOperationInProcess.TabIndex = 4;
            this.longOperationInProcess.Text = "LongOperationInProcess";
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 41);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(386, 394);
            this.listBox1.TabIndex = 5;
            // 
            // listBox2
            // 
            this.listBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(407, 41);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(381, 394);
            this.listBox2.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(480, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Count";
            // 
            // parallelCheckBox
            // 
            this.parallelCheckBox.AutoSize = true;
            this.parallelCheckBox.Checked = true;
            this.parallelCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.parallelCheckBox.Location = new System.Drawing.Point(647, 16);
            this.parallelCheckBox.Name = "parallelCheckBox";
            this.parallelCheckBox.Size = new System.Drawing.Size(60, 17);
            this.parallelCheckBox.TabIndex = 7;
            this.parallelCheckBox.Text = "Parallel";
            this.parallelCheckBox.UseVisualStyleBackColor = true;
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.parallelCheckBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.longOperationInProcess);
            this.Controls.Add(this.working);
            this.Controls.Add(this.networkNotAvailable);
            this.Controls.Add(this.requestsCount);
            this.Controls.Add(this.RunButton);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.requestsCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button RunButton;
        private System.Windows.Forms.NumericUpDown requestsCount;
        private System.Windows.Forms.Label networkNotAvailable;
        private System.Windows.Forms.Label working;
        private System.Windows.Forms.Label longOperationInProcess;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox parallelCheckBox;
    }
}

