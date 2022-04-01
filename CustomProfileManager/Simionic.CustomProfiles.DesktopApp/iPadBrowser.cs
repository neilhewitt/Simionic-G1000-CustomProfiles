using iMobileDevice;
using iMobileDevice.Afc;
using iMobileDevice.HouseArrest;
using iMobileDevice.iDevice;
using iMobileDevice.Lockdown;
using iMobileDevice.Plist;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simionic.CustomProfiles.DesktopApp
{
    public partial class iPadBrowser : Form
    {
        private MainForm _mainForm;
        private iPadFileManager _iPadFileManager;

        public iPadBrowser(MainForm mainForm)
        {
            _mainForm = mainForm;
            _iPadFileManager = new iPadFileManager();

            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            DeviceList.ClearSelected();
            DeviceList.Items.Clear();
            _iPadFileManager.GetDevices();
            foreach (string device in _iPadFileManager.Devices)
            {
                DeviceList.Items.Add(device);
            }
            ExtractButton.Enabled = false;

            base.OnShown(e);
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DeviceList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DeviceList.SelectedIndex >= 0)
            {
                ExtractButton.Enabled = true;
            }
            else
            {
                ExtractButton.Enabled = false;
            }
        }

        private void ExtractButton_Click(object sender, EventArgs e)
        {
            if (DeviceList.SelectedIndex == -1 && DeviceList.Items.Count > 1)
            {
                using (DialogCenteringService service = new DialogCenteringService(this))
                {
                    MessageBox.Show("Please select a device from the list and try again.");
                }
                return;
            }
            else if (DeviceList.SelectedIndex == -1 && DeviceList.Items.Count > 0)
            {
                DeviceList.SelectedIndex = 0;
            }

            string deviceName = (string)DeviceList.SelectedItem;

            using (DialogCenteringService service = new DialogCenteringService(this))
            {
                MessageBox.Show($"Will extract database from {deviceName}. Next, select a location to save the extracted file to.", "Extracting database");
            }

            byte[] buffer = _iPadFileManager.ExtractAppSharedFileFromiPad(deviceName, "ACCustom.db");

            using (FolderBrowserDialog folderBrowseDialog = new FolderBrowserDialog())
            {
                folderBrowseDialog.InitialDirectory = Environment.CurrentDirectory;
                if (folderBrowseDialog.ShowDialog() == DialogResult.OK)
                {
                    string folderPath = folderBrowseDialog.SelectedPath;
                    this.Enabled = false;

                    string path = Path.Combine(folderPath, "ACCustom.db");
                    File.WriteAllBytes(path, buffer);
                    _mainForm.NotifyDBExportedTo(path);
                    _mainForm.NotifyiPadConnected((string)DeviceList.SelectedItem);
                    this.Close();
                }
                else
                {
                    return;
                }
            }
        }
    }
}
