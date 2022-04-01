using Avalonia.Controls;
using Avalonia.Interactivity;
using iMobileDevice;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using Simionic.CustomProfiles.Core;
using Simionic.CustomProfiles.ImportExport;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Simionic.CustomProfiles.ImportExportApp
{
    public partial class MainWindow : Window
    {
        private CustomProfileDB _profileDB;
        private List<int> _selectedProfileIndexes = new List<int>();
        private bool _showAlerts = true;

        private const string NO_PROFILE_MSG = "-- This database has no profiles --";

        public MainWindow()
        {
            InitializeComponent();

            SaveButton.IsEnabled = false;
            ImportButton.IsEnabled = false;
            RemoveButton.IsEnabled = false;
            ExportButton.IsEnabled = false;
            PushButton.IsEnabled = false;

            NativeLibraries.Load();
        }

        public void NotifyDBExportedTo(string path)
        {
            LoadDatabase(path);
        }

        public void NotifyiPadConnected(string deviceName)
        {
            //iPadName.Text = deviceName;
            //iPadStatus.Text = "Connected";
            //iPadStatus.ForeColor = Color.Green;
            //PushButton.Enabled = true;
            //ExtractButton.Enabled = false;
            //OpenDatabaseButton.Enabled = false;
        }

        private void LoadDatabase(string path)
        {
            _profileDB = new CustomProfileDB(path);
            UpdateProfileList();

            ImportButton.IsEnabled = true;
            LoadButton.IsEnabled = false;
            ExtractButton.IsEnabled = false;
        }

        private async void LoadButtonClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Directory = "c:\\";
            openFileDialog.Filters.Add(new FileDialogFilter() { Name = "Sqlite DB files", Extensions = { "db" } });

            string[] result = await openFileDialog.ShowAsync(this);
            if (result != null)
            {
                string path = result.FirstOrDefault();
                try
                {
                    LoadDatabase(path);
                }
                catch (Exception ex)
                {
                    ShowMessageBox($"An unexpected error occurred reading the database file. Please check the file and try again. Error details:\n\n{ ex.Message }", "Error");
                }
            }            
        }

        private void UpdateProfileList()
        {
            List<Profile> profiles = new();
            foreach (var profile in _profileDB.Profiles.OrderBy(x => x.Name))
            {
                profiles.Add(profile);
            }

            if (_profileDB.Profiles.Count() == 0)
            {
                ProfileList.Items = new string[1] { NO_PROFILE_MSG };
                ProfileList.IsEnabled = false;
                SaveButton.IsEnabled = false;
            }
            else
            {
                ProfileList.IsEnabled = true;
                SaveButton.IsEnabled = true;
                ProfileList.Items = profiles;
            }
        }

        private void ExtractButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void PushButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void ImportButtonClick(object sender, RoutedEventArgs e)
        {

        }
        private void ExportButtonClick(object sender, RoutedEventArgs e)
        {

        }
        private void RemoveButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void ShowMessageBox(string message, string caption)
        {
            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(caption, message);
            messageBoxStandardWindow.Show();
        }

        private void ShowMessageBox(string message, string caption, ButtonEnum buttons)
        {
            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = buttons,
                        ContentTitle = caption,
                        ContentMessage = message
                    }
                );
            messageBoxStandardWindow.Show();
        }
    }
}
