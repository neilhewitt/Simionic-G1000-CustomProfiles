namespace Simionic.CustomProfiles.DesktopApp
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.OpenDatabaseButton = new System.Windows.Forms.Button();
            this.SaveChangesButton = new System.Windows.Forms.Button();
            this.ProfileList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ExportButton = new System.Windows.Forms.Button();
            this.ImportButton = new System.Windows.Forms.Button();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.SuppressAlertsCheckbox = new System.Windows.Forms.CheckBox();
            this.ExtractButton = new System.Windows.Forms.Button();
            this.PushButton = new System.Windows.Forms.Button();
            this.iPadLabel = new System.Windows.Forms.Label();
            this.iPadStatus = new System.Windows.Forms.Label();
            this.iPadName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // OpenDatabaseButton
            // 
            this.OpenDatabaseButton.Location = new System.Drawing.Point(12, 41);
            this.OpenDatabaseButton.Name = "OpenDatabaseButton";
            this.OpenDatabaseButton.Size = new System.Drawing.Size(162, 60);
            this.OpenDatabaseButton.TabIndex = 0;
            this.OpenDatabaseButton.Text = "Open from disk";
            this.OpenDatabaseButton.UseVisualStyleBackColor = true;
            this.OpenDatabaseButton.Click += new System.EventHandler(this.OpenDatabaseButton_Click);
            // 
            // SaveChangesButton
            // 
            this.SaveChangesButton.Location = new System.Drawing.Point(12, 294);
            this.SaveChangesButton.Name = "SaveChangesButton";
            this.SaveChangesButton.Size = new System.Drawing.Size(162, 60);
            this.SaveChangesButton.TabIndex = 1;
            this.SaveChangesButton.Text = "Save to disk";
            this.SaveChangesButton.UseVisualStyleBackColor = true;
            this.SaveChangesButton.Click += new System.EventHandler(this.SaveChangesButton_Click);
            // 
            // ProfileList
            // 
            this.ProfileList.FormattingEnabled = true;
            this.ProfileList.ItemHeight = 25;
            this.ProfileList.Location = new System.Drawing.Point(205, 41);
            this.ProfileList.Name = "ProfileList";
            this.ProfileList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.ProfileList.Size = new System.Drawing.Size(409, 379);
            this.ProfileList.TabIndex = 2;
            this.ProfileList.SelectedIndexChanged += new System.EventHandler(this.ProfileList_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(204, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 25);
            this.label1.TabIndex = 3;
            this.label1.Text = "Profiles";
            // 
            // ExportButton
            // 
            this.ExportButton.Location = new System.Drawing.Point(648, 135);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(162, 60);
            this.ExportButton.TabIndex = 4;
            this.ExportButton.Text = "Export selected";
            this.ExportButton.UseVisualStyleBackColor = true;
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // ImportButton
            // 
            this.ImportButton.Location = new System.Drawing.Point(648, 267);
            this.ImportButton.Name = "ImportButton";
            this.ImportButton.Size = new System.Drawing.Size(162, 60);
            this.ImportButton.TabIndex = 5;
            this.ImportButton.Text = "Import profile";
            this.ImportButton.UseVisualStyleBackColor = true;
            this.ImportButton.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // RemoveButton
            // 
            this.RemoveButton.Location = new System.Drawing.Point(648, 201);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(162, 60);
            this.RemoveButton.TabIndex = 6;
            this.RemoveButton.Text = "Remove selected";
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // SuppressAlertsCheckbox
            // 
            this.SuppressAlertsCheckbox.AutoSize = true;
            this.SuppressAlertsCheckbox.Location = new System.Drawing.Point(648, 391);
            this.SuppressAlertsCheckbox.Name = "SuppressAlertsCheckbox";
            this.SuppressAlertsCheckbox.Size = new System.Drawing.Size(158, 29);
            this.SuppressAlertsCheckbox.TabIndex = 7;
            this.SuppressAlertsCheckbox.Text = "Suppress alerts";
            this.SuppressAlertsCheckbox.UseVisualStyleBackColor = true;
            this.SuppressAlertsCheckbox.CheckedChanged += new System.EventHandler(this.SuppressAlertsCheckbox_CheckedChanged);
            // 
            // ExtractButton
            // 
            this.ExtractButton.Location = new System.Drawing.Point(12, 107);
            this.ExtractButton.Name = "ExtractButton";
            this.ExtractButton.Size = new System.Drawing.Size(162, 60);
            this.ExtractButton.TabIndex = 8;
            this.ExtractButton.Text = "Extract from iPad";
            this.ExtractButton.UseVisualStyleBackColor = true;
            this.ExtractButton.Click += new System.EventHandler(this.ExtractButton_Click);
            // 
            // PushButton
            // 
            this.PushButton.Location = new System.Drawing.Point(12, 360);
            this.PushButton.Name = "PushButton";
            this.PushButton.Size = new System.Drawing.Size(162, 60);
            this.PushButton.TabIndex = 9;
            this.PushButton.Text = "Push to iPad";
            this.PushButton.UseVisualStyleBackColor = true;
            this.PushButton.Click += new System.EventHandler(this.PushButton_Click);
            // 
            // iPadLabel
            // 
            this.iPadLabel.AutoSize = true;
            this.iPadLabel.Location = new System.Drawing.Point(648, 41);
            this.iPadLabel.Name = "iPadLabel";
            this.iPadLabel.Size = new System.Drawing.Size(49, 25);
            this.iPadLabel.TabIndex = 10;
            this.iPadLabel.Text = "iPad:";
            // 
            // iPadStatus
            // 
            this.iPadStatus.AutoSize = true;
            this.iPadStatus.ForeColor = System.Drawing.Color.Gray;
            this.iPadStatus.Location = new System.Drawing.Point(648, 66);
            this.iPadStatus.Name = "iPadStatus";
            this.iPadStatus.Size = new System.Drawing.Size(149, 25);
            this.iPadStatus.TabIndex = 11;
            this.iPadStatus.Text = "Ready to connect";
            // 
            // iPadName
            // 
            this.iPadName.AutoSize = true;
            this.iPadName.Location = new System.Drawing.Point(693, 41);
            this.iPadName.Name = "iPadName";
            this.iPadName.Size = new System.Drawing.Size(52, 25);
            this.iPadName.TabIndex = 12;
            this.iPadName.Text = "none";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(822, 441);
            this.Controls.Add(this.iPadName);
            this.Controls.Add(this.iPadStatus);
            this.Controls.Add(this.iPadLabel);
            this.Controls.Add(this.PushButton);
            this.Controls.Add(this.ExtractButton);
            this.Controls.Add(this.SuppressAlertsCheckbox);
            this.Controls.Add(this.RemoveButton);
            this.Controls.Add(this.ImportButton);
            this.Controls.Add(this.ExportButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ProfileList);
            this.Controls.Add(this.SaveChangesButton);
            this.Controls.Add(this.OpenDatabaseButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Simionic Custom Profile Manager";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button OpenDatabaseButton;
        private Button SaveChangesButton;
        private ListBox ProfileList;
        private Label label1;
        private Button ExportButton;
        private Button ImportButton;
        private Button RemoveButton;
        private CheckBox SuppressAlertsCheckbox;
        private Button ExtractButton;
        private Button PushButton;
        private Label iPadLabel;
        private Label iPadStatus;
        private Label iPadName;
    }
}