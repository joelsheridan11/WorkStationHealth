namespace WorkStationDetails
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.wksBtn = new System.Windows.Forms.Button();
            this.copyBtn = new System.Windows.Forms.Button();
            this.wksTxt = new System.Windows.Forms.TextBox();
            this.outputTxt = new System.Windows.Forms.RichTextBox();
            this.EventBtn = new System.Windows.Forms.Button();
            this.svcButton = new System.Windows.Forms.Button();
            this.patchBtn = new System.Windows.Forms.Button();
            this.sccmBtn = new System.Windows.Forms.Button();
            this.softwareBtn = new System.Windows.Forms.Button();
            this.logonBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Workstation Name";
            // 
            // wksBtn
            // 
            this.wksBtn.Location = new System.Drawing.Point(316, 12);
            this.wksBtn.Name = "wksBtn";
            this.wksBtn.Size = new System.Drawing.Size(122, 23);
            this.wksBtn.TabIndex = 1;
            this.wksBtn.Text = "Workstation Details";
            this.wksBtn.UseVisualStyleBackColor = true;
            this.wksBtn.Click += new System.EventHandler(this.wksBtn_Click);
            // 
            // copyBtn
            // 
            this.copyBtn.Location = new System.Drawing.Point(599, 12);
            this.copyBtn.Name = "copyBtn";
            this.copyBtn.Size = new System.Drawing.Size(75, 23);
            this.copyBtn.TabIndex = 2;
            this.copyBtn.Text = "Copy";
            this.copyBtn.UseVisualStyleBackColor = true;
            this.copyBtn.Click += new System.EventHandler(this.copyBtn_Click);
            // 
            // wksTxt
            // 
            this.wksTxt.Location = new System.Drawing.Point(12, 25);
            this.wksTxt.Name = "wksTxt";
            this.wksTxt.Size = new System.Drawing.Size(128, 20);
            this.wksTxt.TabIndex = 3;
            // 
            // outputTxt
            // 
            this.outputTxt.Location = new System.Drawing.Point(12, 127);
            this.outputTxt.Name = "outputTxt";
            this.outputTxt.Size = new System.Drawing.Size(662, 558);
            this.outputTxt.TabIndex = 4;
            this.outputTxt.Text = "";
            // 
            // EventBtn
            // 
            this.EventBtn.Location = new System.Drawing.Point(458, 12);
            this.EventBtn.Name = "EventBtn";
            this.EventBtn.Size = new System.Drawing.Size(122, 23);
            this.EventBtn.TabIndex = 6;
            this.EventBtn.Text = "Event Viewer";
            this.EventBtn.UseVisualStyleBackColor = true;
            this.EventBtn.Click += new System.EventHandler(this.EventBtn_Click);
            // 
            // svcButton
            // 
            this.svcButton.Location = new System.Drawing.Point(316, 41);
            this.svcButton.Name = "svcButton";
            this.svcButton.Size = new System.Drawing.Size(122, 23);
            this.svcButton.TabIndex = 7;
            this.svcButton.Text = "Services";
            this.svcButton.UseVisualStyleBackColor = true;
            this.svcButton.Click += new System.EventHandler(this.svcButton_Click);
            // 
            // patchBtn
            // 
            this.patchBtn.Location = new System.Drawing.Point(458, 41);
            this.patchBtn.Name = "patchBtn";
            this.patchBtn.Size = new System.Drawing.Size(122, 23);
            this.patchBtn.TabIndex = 8;
            this.patchBtn.Text = "Patches";
            this.patchBtn.UseVisualStyleBackColor = true;
            this.patchBtn.Click += new System.EventHandler(this.patchBtn_Click);
            // 
            // sccmBtn
            // 
            this.sccmBtn.Location = new System.Drawing.Point(597, 41);
            this.sccmBtn.Name = "sccmBtn";
            this.sccmBtn.Size = new System.Drawing.Size(75, 23);
            this.sccmBtn.TabIndex = 9;
            this.sccmBtn.Text = "Connect";
            this.sccmBtn.UseVisualStyleBackColor = true;
            this.sccmBtn.Click += new System.EventHandler(this.sccmBtn_Click);
            // 
            // softwareBtn
            // 
            this.softwareBtn.Location = new System.Drawing.Point(316, 70);
            this.softwareBtn.Name = "softwareBtn";
            this.softwareBtn.Size = new System.Drawing.Size(122, 23);
            this.softwareBtn.TabIndex = 10;
            this.softwareBtn.Text = "Software Installed";
            this.softwareBtn.UseVisualStyleBackColor = true;
            this.softwareBtn.Click += new System.EventHandler(this.softwareBtn_Click);
            // 
            // logonBtn
            // 
            this.logonBtn.Location = new System.Drawing.Point(458, 70);
            this.logonBtn.Name = "logonBtn";
            this.logonBtn.Size = new System.Drawing.Size(122, 23);
            this.logonBtn.TabIndex = 11;
            this.logonBtn.Text = "Log On/Off Times";
            this.logonBtn.UseVisualStyleBackColor = true;
            this.logonBtn.Click += new System.EventHandler(this.logonBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 697);
            this.Controls.Add(this.logonBtn);
            this.Controls.Add(this.softwareBtn);
            this.Controls.Add(this.sccmBtn);
            this.Controls.Add(this.patchBtn);
            this.Controls.Add(this.svcButton);
            this.Controls.Add(this.EventBtn);
            this.Controls.Add(this.outputTxt);
            this.Controls.Add(this.wksTxt);
            this.Controls.Add(this.copyBtn);
            this.Controls.Add(this.wksBtn);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Workstation Details";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button wksBtn;
        private System.Windows.Forms.Button copyBtn;
        private System.Windows.Forms.TextBox wksTxt;
        private System.Windows.Forms.RichTextBox outputTxt;
        private System.Windows.Forms.Button EventBtn;
        private System.Windows.Forms.Button svcButton;
        private System.Windows.Forms.Button patchBtn;
        private System.Windows.Forms.Button sccmBtn;
        private System.Windows.Forms.Button softwareBtn;
        private System.Windows.Forms.Button logonBtn;
    }
}

