﻿using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RibbonFileManager
{
    public class Config : DependencyObject
    {
        public ObservableCollection<DiskItem> QuickDestinations { get; set; } = new ObservableCollection<DiskItem>()
        {
            new DiskItem(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)), //(@"%userprofile%\Desktop")),
            new DiskItem(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads")), //Environment.ExpandEnvironmentVariables(@"%userprofile%\Downloads")),
            new DiskItem(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)), //Environment.ExpandEnvironmentVariables(@"%userprofile%\Documents")),
            new DiskItem(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)), //Environment.ExpandEnvironmentVariables(@"%userprofile%\Music")),
            new DiskItem(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)), //Environment.ExpandEnvironmentVariables(@"%userprofile%\Pictures")),
            new DiskItem(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos)), //Environment.ExpandEnvironmentVariables(@"%userprofile%\Videos"))
        };

        public enum InterfaceModeType
        {
            CommandBar,
            Ribbon,
            None
        }

        public enum TabDisplayMode
        {
            Titlebar,
            Toolbar,
            Disabled
        }

        public InterfaceModeType InterfaceMode
        {
            get => (InterfaceModeType)GetValue(InterfaceModeProperty);
            set => SetValue(InterfaceModeProperty, value);
        }

        public static readonly DependencyProperty InterfaceModeProperty =
            DependencyProperty.Register(nameof(InterfaceMode), typeof(InterfaceModeType), typeof(Config), new FrameworkPropertyMetadata(InterfaceModeType.Ribbon, FrameworkPropertyMetadataOptions.AffectsRender));

        public Boolean LockPanes
        {
            get => (Boolean) GetValue(LockPanesProperty);
            set => SetValue(LockPanesProperty, value);
        }

        public static readonly DependencyProperty LockPanesProperty =
            DependencyProperty.Register(nameof(LockPanes), typeof(Boolean), typeof(Config), new PropertyMetadata(true));

        public Boolean ShowStatusBar
        {
            get => (Boolean) GetValue(ShowStatusBarProperty);
            set => SetValue(ShowStatusBarProperty, value);
        }

        public static readonly DependencyProperty ShowStatusBarProperty =
            DependencyProperty.Register(nameof(ShowStatusBar), typeof(Boolean), typeof(Config), new PropertyMetadata(true));

        public Boolean ShowEnhancedFolderIcons
        {
            get => (Boolean)GetValue(ShowEnhancedFolderIconsProperty);
            set => SetValue(ShowEnhancedFolderIconsProperty, value);
        }

        public static readonly DependencyProperty ShowEnhancedFolderIconsProperty =
            DependencyProperty.Register(nameof(ShowEnhancedFolderIcons), typeof(Boolean), typeof(Config), new PropertyMetadata(false));

        public Boolean OpenFoldersInNewWindow
        {
            get => (Boolean)GetValue(OpenFoldersInNewWindowProperty);
            set => SetValue(OpenFoldersInNewWindowProperty, value);
        }

        public static readonly DependencyProperty OpenFoldersInNewWindowProperty =
            DependencyProperty.Register(nameof(OpenFoldersInNewWindow), typeof(Boolean), typeof(Config), new PropertyMetadata(false));

        public Boolean ShowItemSelectionCheckBoxes
        {
            get => (Boolean)GetValue(ShowItemSelectionCheckBoxesProperty);
            set => SetValue(ShowItemSelectionCheckBoxesProperty, value);
        }

        public static readonly DependencyProperty ShowItemSelectionCheckBoxesProperty =
            DependencyProperty.Register(nameof(ShowItemSelectionCheckBoxes), typeof(Boolean), typeof(Config), new PropertyMetadata(false));

        public TabDisplayMode TabsMode
        {
            get => (TabDisplayMode)GetValue(TabsModeProperty);
            set => SetValue(TabsModeProperty, value);
        }

        public static readonly DependencyProperty TabsModeProperty =
            DependencyProperty.Register(nameof(TabsMode), typeof(TabDisplayMode), typeof(Config), new PropertyMetadata(TabDisplayMode.Titlebar));

        public Boolean ShowTitlebarText
        {
            get => (Boolean) GetValue(ShowTitlebarTextProperty);
            set => SetValue(ShowTitlebarTextProperty, value);
        }

        public static readonly DependencyProperty ShowTitlebarTextProperty =
            DependencyProperty.Register(nameof(ShowTitlebarText), typeof(Boolean), typeof(Config), new PropertyMetadata(true));

        public System.Windows.Controls.Dock DetailsPanePlacement
        {
            get => (System.Windows.Controls.Dock)GetValue(DetailsPanePlacementProperty);
            set => SetValue(DetailsPanePlacementProperty, value);
        }

        public static readonly DependencyProperty DetailsPanePlacementProperty =
            DependencyProperty.Register(nameof(DetailsPanePlacement), typeof(System.Windows.Controls.Dock), typeof(Config), new PropertyMetadata(System.Windows.Controls.Dock.Right));

        public static Boolean ShowItemCheckboxes { get; set; } = false;

        static String _favoritesPath = Environment.ExpandEnvironmentVariables(@"%appdata%\Start9\TempData\" + Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location) + "_Favorites.txt");

        public static ObservableCollection<DiskItem> Favorites
        {
            get
            {
                var items = new ObservableCollection<DiskItem>();
                if (!File.Exists(_favoritesPath))
                {
                    var dir = Path.GetDirectoryName(_favoritesPath);

                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    File.WriteAllLines(_favoritesPath, new List<String>()
                    {
                        @"%userprofile%\Documents",
                        @"%userprofile%\Pictures",
                        @"%userprofile%\Downloads"
                    }.ToArray());
                }

                var lines = File.ReadAllLines(_favoritesPath);
                foreach (var l in lines)
                {
                    if (l.Contains('?'))
                    {
                        var parts = l.Split('?');
                        for (var i = 0; i < parts.Count(); i++)
                        {
                            var s = parts[i];
                        }
                        items.Add(new DiskItem(parts[0])
                        {
                            ItemDisplayName = parts[1]
                        });
                    }
                    else
                        items.Add(new DiskItem(l));
                }
                return items;
            }
            set
            {
                var paths = new List<String>();
                foreach (var d in value)
                {
                    paths.Add(d.ItemPath);
                }
                File.WriteAllLines(_favoritesPath, paths.ToArray());
            }
        }

        public static ObservableCollection<DiskItem> ComputerSubfolders
        {
            get
            {
                var collection = new ObservableCollection<DiskItem>();
                String[] _userFolders = { @"%userprofile%\Desktop", @"%userprofile%\Documents", @"%userprofile%\Downloads", @"%userprofile%\Music", @"%userprofile%\Pictures", @"%userprofile%\Videos" };
                foreach (var s in _userFolders)
                    collection.Add(new DiskItem(Environment.ExpandEnvironmentVariables(s)));

                foreach (var s in DriveInfo.GetDrives().Where(d => Directory.Exists(d.Name)))
                {
                    collection.Add(new DiskItem(Environment.ExpandEnvironmentVariables(s.Name)));
                }
                return collection;
            }
            set
            {
            }
        }

        public static ObservableCollection<DiskItem> ClipboardContents { get; set; } = new ObservableCollection<DiskItem>();
        public static Boolean Cut { get; set; } = false;

        public static ObservableCollection<DiskItem> PasteIn(String targetPath)
        {
            var success = new ObservableCollection<DiskItem>();
            foreach (var d in ClipboardContents)
            {
                var baseFilename = targetPath + @"\" + Path.GetFileName(d.ItemPath);
                var outFilename = baseFilename;

                var interval = 1;

                var ext = Path.GetExtension(baseFilename);
                while (File.Exists(outFilename) || Directory.Exists(outFilename))
                {
                    outFilename = baseFilename + " (" + interval.ToString() + ")";
                    if (!String.IsNullOrEmpty(ext))
                    {
                        if (!outFilename.EndsWith(ext))
                        {
                            if (outFilename.Contains(ext))
                            {
                                outFilename = outFilename.Replace(ext, "");
                                outFilename += ext;
                            }
                        }
                    }
                    interval += 1;
                }

                if (Cut)
                {
                    if (File.Exists(d.ItemPath))
                    {
                        File.Move(d.ItemPath, outFilename);
                    }
                    else
                    {
                        Directory.Move(d.ItemPath, outFilename);
                    }
                }
                else
                {
                    if (File.Exists(d.ItemPath))
                    {
                        File.Copy(d.ItemPath, outFilename);
                    }
                    else
                    {
                        FileSystem.CopyDirectory(d.ItemPath, outFilename, UIOption.AllDialogs);
                    }
                }
                success.Add(new DiskItem(outFilename));
            }
            return success;
        }

        public static SettingsWindow SettingsWindow = new SettingsWindow();

        public static Config Instance = new Config();

        private Config()
        { }

        internal static void InvokeConfigUpdated()
        {
            ConfigUpdated?.Invoke(Instance, null);
        }

        public static event EventHandler ConfigUpdated;
    }
}
