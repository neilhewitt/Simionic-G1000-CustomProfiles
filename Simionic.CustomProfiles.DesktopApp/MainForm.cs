using iMobileDevice;
using iMobileDevice.iDevice;
using iMobileDevice.Lockdown;
using Simionic.CustomProfiles.Core;
using Simionic.CustomProfiles.ImportExport;
using System;
using System.Collections.ObjectModel;

namespace Simionic.CustomProfiles.DesktopApp
{
    public partial class MainForm : Form
    {
        private CustomProfileDB _profileDB;
        private List<int> _selectedProfileIndexes = new List<int>();
        private bool _showAlerts = true;
        private bool _removedLastProfile = false;

        private const string NO_PROFILE_MSG = "-- This database has no profiles --";

        public MainForm()
        {
            InitializeComponent();
        }

        public void NotifyDBExportedTo(string path)
        {
            LoadDatabase(path);
        }

        public void NotifyiPadConnected(string deviceName)
        {
            iPadName.Text = deviceName;
            iPadStatus.Text = "Connected";
            iPadStatus.ForeColor = Color.Green;
            PushButton.Enabled = true;
            ExtractButton.Enabled = false;
            OpenDatabaseButton.Enabled = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            SaveChangesButton.Enabled = false;
            ImportButton.Enabled = false;
            RemoveButton.Enabled = false;
            ExportButton.Enabled = false;
            PushButton.Enabled = false;

            NativeLibraries.Load();

            base.OnLoad(e);
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
        }

        private void OpenDatabaseButton_Click(object sender, EventArgs e)
        {
            var path = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Sqlite DB files (*.db)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    path = openFileDialog.FileName;
                    try
                    {
                        LoadDatabase(path); 
                    }
                    catch (Exception ex)
                    {
                        ShowMessageBox($"An unexpected error occurred reading the database file. Please check the file and try again. Error details:\n\n{ ex.Message }", "Error");
                    }
                }
                else
                {
                    return;
                }
            }
        }

        private void LoadDatabase(string path)
        {
            _profileDB = new CustomProfileDB(path);
            UpdateProfileList();

            ImportButton.Enabled = true;
            OpenDatabaseButton.Enabled = false;
            ExtractButton.Enabled = false;
        }

        private void ProfileList_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedProfileIndexes.Clear();
            for (int i = 0; i < ProfileList.Items.Count; i++)
            {
                if (ProfileList.GetSelected(i))
                {
                    _selectedProfileIndexes.Add(i);
                }
            }

            if (_selectedProfileIndexes.Count > 0)
            {
                ExportButton.Enabled = true;
                RemoveButton.Enabled = true;
            }
            else
            {
                ExportButton.Enabled= false;
                RemoveButton.Enabled= false;
            }
        }

        private async void ExportButton_Click(object sender, EventArgs e)
        {
            if (_selectedProfileIndexes.Count > 0)
            {
                using (FolderBrowserDialog folderBrowseDialog = new FolderBrowserDialog())
                {
                    folderBrowseDialog.InitialDirectory = Environment.CurrentDirectory;
                    if (folderBrowseDialog.ShowDialog() == DialogResult.OK)
                    {
                        string folderPath = folderBrowseDialog.SelectedPath;
                        
                        ExportButton.Enabled = false;
                        string exportText = ExportButton.Text;
                        ExportButton.Text = "Exporting...";
                        await Task.Delay(1000);
                        ExportButton.Text = exportText;

                        try
                        {
                            string message = $"Exported {_selectedProfileIndexes.Count} profiles. Each profile has been saved as a JSON file which can be imported into another database, or uploaded to the\nG1000 Profile Database (https://g1000profiledb.com).\n";
                            foreach (int selectedIndex in _selectedProfileIndexes)
                            {
                                Profile profile = (Profile)ProfileList.Items[selectedIndex];
                                profile.SaveAsJson(folderPath);
                                message += $"\n\t{profile.Name}";
                            }

                            if (_showAlerts) ShowMessageBox(message, "Export complete");
                        }
                        catch
                        {
                            ShowMessageBox("An unexpected error occurred.", "Error");
                        }
                        finally
                        {
                            ExportButton.Enabled = true;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "JSON files (*.json)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string path = openFileDialog.FileName;
                    try
                    {
                        Profile profile = _profileDB.ImportProfileFromJson(path);
                        UpdateProfileList();
                        if (_showAlerts) ShowMessageBox($"Added profile '{profile.Name}'.\n\nNote that this will not be written to the custom profile database until you click 'Save changes', and if you exit the program without doing so, these changes will be lost.", "Profile imported");
                    }
                    catch (Exception ex)
                    {
                        ShowMessageBox("An unexpected error occurred reading the database file. Please check the file and try again.", "Error");
                    }
                }
                else
                {
                    return;
                }
            }
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (!_showAlerts || ShowMessageBox("This will remove the selected profiles from the database. Are you sure?\n\n(Changes don't take final effect until you click 'Save changes'.)", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                foreach (int selectedIndex in _selectedProfileIndexes)
                {
                    _profileDB.RemoveProfile((Profile)ProfileList.Items[selectedIndex]);
                    if (_profileDB.Profiles.Count() == 0) _removedLastProfile = true;
                }

                UpdateProfileList();
            }
        }

        private void SaveChangesButton_Click(object sender, EventArgs e)
        {
            if (!_showAlerts || ShowMessageBox("This will save your changes to the database. This action cannot be undone. Are you sure?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                SaveChangesButton.Enabled = false;
                string saveText = SaveChangesButton.Text;
                SaveChangesButton.Text = "Saving...";
                Task.Delay(1000).Wait();
                SaveChangesButton.Text = saveText;

                _profileDB.SaveToDatabase();
                if (!_removedLastProfile) SaveChangesButton.Enabled = true;
                _removedLastProfile = false;

                if (_showAlerts) ShowMessageBox("Changes saved to database.", "Saved");
            }
        }

        private void UpdateProfileList()
        {
            ProfileList.Items.Clear();
            foreach (Profile profile in _profileDB.Profiles.OrderBy(x => x.Name))
            {
                ProfileList.Items.Add(profile);
                ProfileList.Enabled = true;
                SaveChangesButton.Enabled = true;
            }

            if (_profileDB.Profiles.Count() == 0)
            {
                ProfileList.Items.Add(NO_PROFILE_MSG);
                ProfileList.Enabled = false;
                if (!_removedLastProfile) SaveChangesButton.Enabled = false;
            }
        }

        private void SuppressAlertsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            _showAlerts = !SuppressAlertsCheckbox.Checked;
        }

        private void ExtractButton_Click(object sender, EventArgs e)
        {
            iPadBrowser iPadBrowser = new iPadBrowser(this);
            iPadBrowser.StartPosition = FormStartPosition.CenterParent;
            iPadBrowser.ShowDialog();
        }

        private void PushButton_Click(object sender, EventArgs e)
        {
            if (!_showAlerts || ShowMessageBox($"This will save your changes, and push the resulting database file back to '{iPadName.Text}'. " +
                "This will overwrite the existing database. A backup of your existing database will be created. Are you sure you want to do this?", "Push to iPad", MessageBoxButtons.YesNo)
                == DialogResult.Yes)
            {
                this.Enabled = false;
                try
                {
                    byte[] data = _profileDB.SaveToDatabase();
                    iPadFileManager iPadFileManager = new iPadFileManager();
                    iPadFileManager.PushAppSharedFileToiPad(iPadName.Text, "ACCustom.db", data);

                    ShowMessageBox($"Successfully pushed the database to {iPadName.Text}.", "Success");
                }
                catch (Exception ex)
                {
                    ShowMessageBox("An unexpected error occurred. Is your iPad still connected?", "Push to iPad failed");
                }
                finally
                {
                    this.Enabled = true;
                }
            }
        }

        private DialogResult ShowMessageBox(string message, string caption)
        {
            using (DialogCenteringService service = new DialogCenteringService(this))
            {
                return MessageBox.Show(message, caption);
            }
        }

        private DialogResult ShowMessageBox(string message, string caption, MessageBoxButtons buttons)
        {
            using (DialogCenteringService service = new DialogCenteringService(this))
            {
                return MessageBox.Show(message, caption, buttons);
            }
        }
    }
}