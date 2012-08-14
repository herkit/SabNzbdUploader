using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

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
            var api = new Arasoft.SabNzdbUploader.Core.Api.SabNzbdApi();
            List<string> categories = api.GetCategories();
            CategorySelector.ItemsSource = categories;

            if (App.NZBFile.Exists)
                FilenameLabel.Content = App.NZBFile.Name;
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            string category = "*";
            if (CategorySelector.SelectedIndex >= 0)
                category = (string)CategorySelector.SelectedValue;

            var api = new Arasoft.SabNzdbUploader.Core.Api.SabNzbdApi();
            api.UploadFile(App.NZBFile, category);

            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
