using System.IO;
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

namespace MySpace_Cleaner
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string tempPath = System.IO.Path.GetTempPath();
                string[] files = Directory.GetFiles(tempPath, "*.*", SearchOption.AllDirectories);

                long totalSize = 0;
                int fileCount = 0;

                foreach (string file in files)
                {
                    try
                    {
                        
                        FileInfo fi = new FileInfo(file);
                        totalSize += fi.Length;
                        fileCount++;
                    }
                    catch (Exception ex)
                    {
                   

                    }
                }
                StatusText.Text = $"Found {fileCount} temp files using {totalSize / (1024 * 1024)} MB";
            }
            catch (Exception ex)
            {

                StatusText.Text = $"Scan failed: {ex.Message}";
            }
        }

        private void cleanBtn_Click(object sender, RoutedEventArgs e)
        {
            // Show confirmation dialog
            MessageBoxResult result = MessageBox.Show(
                "Are you sure you want to delete all temporary files?",
                "Confirm Cleanup",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            // If user clicks "No", cancel the deletion
            if (result != MessageBoxResult.Yes)
            {
                StatusText.Text = "Cleanup canceled by user.";
                return;
            }
            try
            {
                string tempPath = System.IO.Path.GetTempPath();
                string[] files = Directory.GetFiles(tempPath, "*.*", SearchOption.AllDirectories);

                int deletedCount = 0;
                foreach (string file in files)
                {
                    try
                    {
                        File.SetAttributes(file, FileAttributes.Normal); // in case file is read-only
                        File.Delete(file);
                        deletedCount++;
                    }
                    catch
                    {
                        // Could not delete (e.g., file in use), just skip
                    }
                }

                StatusText.Text = $"Cleanup complete: {deletedCount} files deleted.";
            }
            catch (Exception ex)
            {
                StatusText.Text = $"Cleanup failed: {ex.Message}";
            }

    }
    }
}