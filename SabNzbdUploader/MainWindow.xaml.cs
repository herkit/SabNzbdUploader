using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.IO;
using Arasoft.SabNzdbUploader.Core;
using System.Reflection;
using System.ComponentModel;

namespace SabNzbdUploader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            var executingFile = Assembly.GetExecutingAssembly().Location;
            if (!FileTypeTools.IsAssociated(".nzb", "Nzb_uploader_file", executingFile))
                if (MessageBox.Show("NZB files are not associated with this application, do you want to associate now?", "Not associated", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    FileTypeTools.SetAssociation(".nzb", "Nzb_uploader_file", executingFile, "Usenet NZB file");

            var index = CategorySelector.Items.IndexOf(Properties.Settings.Default.lastCategory);
            if (index >= 0)
                CategorySelector.SelectedItem = CategorySelector.Items[index];

            if (App.NZBFile.Exists)
                FilenameLabel.Content = App.NZBFile.Name;
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            UploadButton.IsEnabled = false;
            CancelButton.IsEnabled = false;
            CategorySelector.IsEnabled = false;
            UploadingIndicator.Visibility = System.Windows.Visibility.Visible;


            BackgroundWorker fileUploader = new BackgroundWorker();
            fileUploader.DoWork += fileUploader_DoWork;
            fileUploader.RunWorkerCompleted += fileUplaoder_RunWorkerCompleted;

            string category = CategorySelector.SelectedIndex >= 0 ? (string)CategorySelector.SelectedValue : "*";

            fileUploader.RunWorkerAsync(new Tuple<FileInfo, string>(App.NZBFile, category));

            Properties.Settings.Default.lastCategory = category;
            Properties.Settings.Default.Save();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private static Arasoft.SabNzdbUploader.Core.Api.SabNzbdApi GetApi()
        {
            return new Arasoft.SabNzdbUploader.Core.Api.SabNzbdApi(Properties.Settings.Default.sabRootUrl, Properties.Settings.Default.sabApiKey);
        }

        void fileUploader_DoWork(object sender, DoWorkEventArgs e)
        {
            var arg = (Tuple<FileInfo, string>)e.Argument;
            var api = GetApi();
            api.UploadFile(arg.Item1, arg.Item2);

        }

        void fileUplaoder_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Close();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            var api = GetApi();
            List<string> categories = api.GetCategories();
            CategorySelector.ItemsSource = categories;
        }
    }
}
