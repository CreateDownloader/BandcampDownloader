﻿using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace BandcampDownloader {

    public partial class UserControlSettingsGeneral: UserControl, IUserControlSettings {

        public UserControlSettingsGeneral() {
            InitializeComponent();
            // Save data context for bindings
            DataContext = App.UserSettings;
        }

        /// <summary>
        /// Cancels the changes already applied.
        /// </summary>
        public void CancelChanges() {
            LanguageHelper.ApplyLanguage(App.UserSettings.Language.ToString());
        }

        /// <summary>
        /// Loads settings from App.UserSettings.
        /// </summary>
        public void LoadSettings() {
            // Reload DataContext in case settings have changed
            DataContext = App.UserSettings;
            // No need to call UpdateTarget, it is done automatically
        }

        /// <summary>
        /// Saves settings to App.UserSettings.
        /// </summary>
        public void SaveSettings() {
            checkBoxCheckForUpdates.GetBindingExpression(CheckBox.IsCheckedProperty).UpdateSource();
            checkBoxEnableApplicationSounds.GetBindingExpression(CheckBox.IsCheckedProperty).UpdateSource();
            checkBoxVerboseLog.GetBindingExpression(CheckBox.IsCheckedProperty).UpdateSource();
            comboBoxLanguage.GetBindingExpression(ComboBox.SelectedValueProperty).UpdateSource();
        }

        private async void ButtonCheckForUpdates_Click(object sender, RoutedEventArgs e) {
            Version latestVersion;
            try {
                latestVersion = await UpdatesHelper.GetLatestVersionAsync();
            } catch (CouldNotCheckForUpdatesException) {
                MessageBox.Show(Properties.Resources.messageBoxCheckForUpdatesError, "Bandcamp Downloader", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
            if (currentVersion.CompareTo(latestVersion) < 0) {
                // The latest version is newer than the current one
                var windowUpdate = new WindowUpdate() {
                    ShowInTaskbar = true,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                };
                windowUpdate.Show();
            } else {
                MessageBox.Show(string.Format(Properties.Resources.messageBoxNoUpdateAvailable, currentVersion), "Bandcamp Downloader", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ComboBoxLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            // Apply selected language
            LanguageHelper.ApplyLanguage(comboBoxLanguage.SelectedValue.ToString());
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e) {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}