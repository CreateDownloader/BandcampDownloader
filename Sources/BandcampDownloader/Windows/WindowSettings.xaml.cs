﻿using System;
using System.IO;
using System.Windows;
using Config.Net;

namespace BandcampDownloader {

    public partial class WindowSettings: Window {

        /// <summary>
        /// True if there are active downloads; false otherwise.
        /// </summary>
        public Boolean ActiveDownloads { get; set; }

        /// <summary>
        /// Creates a new instance of SettingsWindow.
        /// </summary>
        /// <param name="activeDownloads">True if there are active downloads; false otherwise.</param>
        public WindowSettings(Boolean activeDownloads) {
            ActiveDownloads = activeDownloads; // Must be done before UI initialization
            DataContext = WindowMain.userSettings;
            InitializeComponent();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private void ButtonResetSettings_Click(object sender, RoutedEventArgs e) {
            if (MessageBox.Show("Reset all settings to their default values?", "Bandcamp Downloader", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes) {
                ResetSettings();
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e) {
            SaveSettings();
            Close();
        }

        /// <summary>
        /// Resets settings to their default values.
        /// </summary>
        private void ResetSettings() {
            // Save settings we shouldn't reset (as they're not on the Settings window)
            String downloadsPath = WindowMain.userSettings.DownloadsLocation;

            File.Delete(Constants.UserSettingsFilePath);
            WindowMain.userSettings = new ConfigurationBuilder<IUserSettings>().UseIniFile(Constants.UserSettingsFilePath).Build();
            WindowMain.userSettings.DownloadsLocation = downloadsPath;

            // Re-load settings on UI
            userControlSettingsAdvanced.LoadSettings();
            userControlSettingsCoverArt.LoadSettings();
            userControlSettingsLog.LoadSettings();
            userControlSettingsTags.LoadSettings();
        }

        /// <summary>
        /// Save all settings.
        /// </summary>
        private void SaveSettings() {
            userControlSettingsTags.SaveSettings();
            userControlSettingsCoverArt.SaveSettings();
            userControlSettingsLog.SaveSettings();
            userControlSettingsAdvanced.SaveSettings();
        }
    }
}