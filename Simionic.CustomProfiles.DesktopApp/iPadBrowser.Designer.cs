namespace Simionic.CustomProfiles.DesktopApp
{
    partial class iPadBrowser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(iPadBrowser));
            this.DeviceList = new System.Windows.Forms.ListBox();
            this.ListBoxLabel = new System.Windows.Forms.Label();
            this.ExtractButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // DeviceList
            // 
            this.DeviceList.FormattingEnabled = true;
            this.DeviceList.ItemHeight = 25;
            this.DeviceList.Location = new System.Drawing.Point(14, 34);
            this.DeviceList.Name = "DeviceList";
            this.DeviceList.Size = new System.Drawing.Size(405, 404);
            this.DeviceList.TabIndex = 0;
            // 
            // ListBoxLabel
            // 
            this.ListBoxLabel.AutoSize = true;
            this.ListBoxLabel.Location = new System.Drawing.Point(14, 4);
            this.ListBoxLabel.Name = "ListBoxLabel";
            this.ListBoxLabel.Size = new System.Drawing.Size(167, 25);
            this.ListBoxLabel.TabIndex = 1;
            this.ListBoxLabel.Text = "iDevices connected:";
            // 
            // ExtractButton
            // 
            this.ExtractButton.Location = new System.Drawing.Point(425, 34);
            this.ExtractButton.Name = "ExtractButton";
            this.ExtractButton.Size = new System.Drawing.Size(175, 64);
            this.ExtractButton.TabIndex = 2;
            this.ExtractButton.Text = "Extract database";
            this.ExtractButton.UseVisualStyleBackColor = true;
            this.ExtractButton.Click += new System.EventHandler(this.ExtractButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(425, 374);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(175, 64);
            this.CancelButton.TabIndex = 3;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // iPadBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(612, 450);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.ExtractButton);
            this.Controls.Add(this.ListBoxLabel);
            this.Controls.Add(this.DeviceList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "iPadBrowser";
            this.Text = "Find iPad";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListBox DeviceList;
        private Label ListBoxLabel;
        private Button ExtractButton;
        private Button CancelButton;
    }
}