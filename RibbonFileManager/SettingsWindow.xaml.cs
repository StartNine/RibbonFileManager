﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using Start9.UI.Wpf.Windows;
using System.Collections.ObjectModel;
using System.Reflection;
using MessageBox = Start9.UI.Wpf.Windows.MessageBox;

namespace RibbonFileManager
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : DecoratableWindow
    {
        public SettingsWindow()
        {
            InitializeComponent();

            foreach (string s in Directory.EnumerateDirectories(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"Start9\File Commander\Skins")))
            {
                try
                {
                    ((App)App.Current).SkinManager.LoadSkinFromFolder(s);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.GetType().Name.ToString());
                }
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
            base.OnClosing(e);
        }


        private void SettingsWindow_IsVisibleChanged(Object sender, DependencyPropertyChangedEventArgs e)
        {
            if (((Boolean)e.OldValue == false) && ((Boolean)e.NewValue == true))
                PopulateGui();
        }

        public void PopulateGui()
        {
            //Interface mode
            if (Config.Instance.InterfaceMode == Config.InterfaceModeType.Ribbon)
                InterfaceModeComboBox.SelectedIndex = 0;
            else if (Config.Instance.InterfaceMode == Config.InterfaceModeType.CommandBar)
                InterfaceModeComboBox.SelectedIndex = 1;
            else
                InterfaceModeComboBox.SelectedIndex = 2;

            //Tabs mode
            if (Config.Instance.TabsMode == Config.TabDisplayMode.Toolbar)
                (TabBehaviourRadioButtonsStackPanel.Children[1] as RadioButton).IsChecked = true;
            else if (Config.Instance.TabsMode == Config.TabDisplayMode.Disabled)
                (TabBehaviourRadioButtonsStackPanel.Children[2] as RadioButton).IsChecked = true;
            else
                (TabBehaviourRadioButtonsStackPanel.Children[0] as RadioButton).IsChecked = true;

            //Enhanced Folder Icons
            ShowEnhancedFolderIconsCheckBox.IsChecked = Config.Instance.ShowEnhancedFolderIcons;

            //Lock Panes
            LockPanesToggleSwitch.IsChecked = Config.Instance.LockPanes;

            //Status bar
            StatusBarToggleSwitch.IsChecked = Config.Instance.ShowStatusBar;

            //Titlebar text
            TitlebarTextToggleSwitch.IsChecked = Config.Instance.ShowTitlebarText;

            //Titlebar icon
            TitlebarIconToggleSwitch.IsChecked = Config.Instance.ShowTitlebarIcon;

            //Open folders in new window
            OpenFoldersInNewWindowCheckBox.IsChecked = Config.Instance.OpenFoldersInNewWindow;
            /*if (Config.Instance.OpenFoldersInNewWindow)
                OpenFoldersInNewWindowCheckBox.IsChecked = true;
            else
                OpenFoldersInNewWindowCheckBox.IsChecked = false;*/

            //Item selection CheckBoxes
            ShowItemSelectionCheckBoxesCheckBox.IsChecked = Config.Instance.ShowItemSelectionCheckBoxes;

            SkinSettingsFrame.Navigate(((App)App.Current).SkinManager.ActiveSkin.GetSettingsPage());
        }

        private void Start9SettingsButton_Click(Object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ApplyButton_Click(Object sender, RoutedEventArgs e)
        {
            ApplySettings();

            if (sender == OkButton)
                Close();
        }

        public void ApplySettings()
        {
            //Interface mode
            if (InterfaceModeComboBox.SelectedIndex == 0)
                Config.Instance.InterfaceMode = Config.InterfaceModeType.Ribbon;
            else if (InterfaceModeComboBox.SelectedIndex == 1)
                Config.Instance.InterfaceMode = Config.InterfaceModeType.CommandBar;
            else
                Config.Instance.InterfaceMode = Config.InterfaceModeType.None;

            //Tabs mode
            if ((TabBehaviourRadioButtonsStackPanel.Children[1] as RadioButton).IsChecked.Value)
                Config.Instance.TabsMode = Config.TabDisplayMode.Toolbar;
            else if ((TabBehaviourRadioButtonsStackPanel.Children[2] as RadioButton).IsChecked.Value)
                Config.Instance.TabsMode = Config.TabDisplayMode.Disabled;
            else
                Config.Instance.TabsMode = Config.TabDisplayMode.Titlebar;

            //Enhanced Folder Icons
            Config.Instance.ShowEnhancedFolderIcons = ShowEnhancedFolderIconsCheckBox.IsChecked.Value;

            //Lock Panes
            Config.Instance.LockPanes = LockPanesToggleSwitch.IsChecked.Value;

            //Status bar
            Config.Instance.ShowStatusBar = StatusBarToggleSwitch.IsChecked.Value;

            //Titlebar text
            Config.Instance.ShowTitlebarText = TitlebarTextToggleSwitch.IsChecked.Value;

            //Titlebar icon
            Config.Instance.ShowTitlebarIcon = TitlebarIconToggleSwitch.IsChecked.Value;

            //Open folders in new window
            Config.Instance.OpenFoldersInNewWindow = OpenFoldersInNewWindowCheckBox.IsChecked.Value;
            /*if (OpenFoldersInNewWindowCheckBox.IsChecked.Value)
                Config.Instance.OpenFoldersInNewWindow = true;
            else
                Config.Instance.OpenFoldersInNewWindow = false;*/

            //Item selection CheckBoxes
            Config.Instance.ShowItemSelectionCheckBoxes = ShowItemSelectionCheckBoxesCheckBox.IsChecked.Value;

            if (true) //InstalledSkinsListView.SelectedIndex != ((App)App.Current).SkinManager.Skins.IndexOf(((App)App.Current).SkinManager.ActiveSkin))
            {
                if (InstalledSkinsListView.SelectedIndex >= 0)
                    ((App)App.Current).SkinManager.ActiveSkin = ((App)App.Current).SkinManager.Skins.ElementAt(InstalledSkinsListView.SelectedIndex);
                else
                    ((App)App.Current).SkinManager.ActiveSkin = ((App)App.Current).SkinManager.DefaultSkin;

                SkinSettingsFrame.Navigate(((App)(App.Current)).SkinManager.ActiveSkin.GetSettingsPage());
            }

            Config.InvokeConfigUpdated();
        }

        private void CancelButton_Click(Object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
