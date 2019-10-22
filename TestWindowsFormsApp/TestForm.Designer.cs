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
            this.networkNotAvailableLabel = new System.Windows.Forms.Label();
            this.workingLabel = new System.Windows.Forms.Label();
            this.longOperationInProcessLabel = new System.Windows.Forms.Label();
            this.requestListBox = new System.Windows.Forms.ListBox();
            this.responseListBox = new System.Windows.Forms.ListBox();
            this.countLabel = new System.Windows.Forms.Label();
            this.parallelCheckBox = new System.Windows.Forms.CheckBox();
            this.unprocessedLabel = new System.Windows.Forms.Label();
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
            // networkNotAvailableLabel
            // 
            this.networkNotAvailableLabel.AutoSize = true;
            this.networkNotAvailableLabel.Location = new System.Drawing.Point(12, 17);
            this.networkNotAvailableLabel.Name = "networkNotAvailableLabel";
            this.networkNotAvailableLabel.Size = new System.Drawing.Size(107, 13);
            this.networkNotAvailableLabel.TabIndex = 2;
            this.networkNotAvailableLabel.Text = "NetworkNotAvailable";
            // 
            // workingLabel
            // 
            this.workingLabel.AutoSize = true;
            this.workingLabel.Location = new System.Drawing.Point(255, 17);
            this.workingLabel.Name = "workingLabel";
            this.workingLabel.Size = new System.Drawing.Size(47, 13);
            this.workingLabel.TabIndex = 3;
            this.workingLabel.Text = "Working";
            // 
            // longOperationInProcessLabel
            // 
            this.longOperationInProcessLabel.AutoSize = true;
            this.longOperationInProcessLabel.Location = new System.Drawing.Point(125, 17);
            this.longOperationInProcessLabel.Name = "longOperationInProcessLabel";
            this.longOperationInProcessLabel.Size = new System.Drawing.Size(124, 13);
            this.longOperationInProcessLabel.TabIndex = 4;
            this.longOperationInProcessLabel.Text = "LongOperationInProcess";
            // 
            // requestListBox
            // 
            this.requestListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.requestListBox.FormattingEnabled = true;
            this.requestListBox.Location = new System.Drawing.Point(12, 41);
            this.requestListBox.Name = "requestListBox";
            this.requestListBox.Size = new System.Drawing.Size(386, 394);
            this.requestListBox.TabIndex = 5;
            // 
            // responseListBox
            // 
            this.responseListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.responseListBox.FormattingEnabled = true;
            this.responseListBox.Location = new System.Drawing.Point(407, 41);
            this.responseListBox.Name = "responseListBox";
            this.responseListBox.Size = new System.Drawing.Size(381, 394);
            this.responseListBox.TabIndex = 5;
            // 
            // countLabel
            // 
            this.countLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.countLabel.AutoSize = true;
            this.countLabel.Location = new System.Drawing.Point(480, 17);
            this.countLabel.Name = "countLabel";
            this.countLabel.Size = new System.Drawing.Size(35, 13);
            this.countLabel.TabIndex = 6;
            this.countLabel.Text = "Count";
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
            // unprocessedLabel
            // 
            this.unprocessedLabel.AutoSize = true;
            this.unprocessedLabel.Location = new System.Drawing.Point(308, 17);
            this.unprocessedLabel.Name = "unprocessedLabel";
            this.unprocessedLabel.Size = new System.Drawing.Size(79, 13);
            this.unprocessedLabel.TabIndex = 3;
            this.unprocessedLabel.Text = "Unprocessed 0";
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.parallelCheckBox);
            this.Controls.Add(this.countLabel);
            this.Controls.Add(this.responseListBox);
            this.Controls.Add(this.requestListBox);
            this.Controls.Add(this.longOperationInProcessLabel);
            this.Controls.Add(this.unprocessedLabel);
            this.Controls.Add(this.workingLabel);
            this.Controls.Add(this.networkNotAvailableLabel);
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
        private System.Windows.Forms.Label networkNotAvailableLabel;
        private System.Windows.Forms.Label workingLabel;
        private System.Windows.Forms.Label longOperationInProcessLabel;
        private System.Windows.Forms.ListBox requestListBox;
        private System.Windows.Forms.ListBox responseListBox;
        private System.Windows.Forms.Label countLabel;
        private System.Windows.Forms.CheckBox parallelCheckBox;
        private System.Windows.Forms.Label unprocessedLabel;
    }
}

