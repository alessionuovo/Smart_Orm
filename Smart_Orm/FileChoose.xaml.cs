using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using WpfExtensions;
using FileManagement;
using UploadManagement;
using Utilities.Definitions;
using Syncfusion.Windows.Tools.Controls;
using System.IO;
using Utilities;

namespace Smart_Orm
{
    /// <summary>
    /// <summary>
    /// This class is form with option of
    /// uploading files that represent database
    /// (upload includes validation)
    /// </summary>
    public partial class FileChoose : ChooseForm
    {
       
        public FileChoose()
        {
            InitializeComponent();
            testo.btnNext.Click += BtnNext_Click;
            testo.btnNext.IsEnabled = false;
            testo.btnBack.Click += BtnBack_Click;
            testo.btnFinish.Click += BtnFinish_Click;
            testo.btnFinish.IsEnabled = false;
            testo.btnCancel.Click += BtnCancel_Click;
            // test.btnNext.Click += BtnNext_Click;
          
            this.Loaded += FileChoose_Loaded;
        }
        private List<string> f = new List<string>();
        private void FileChoose_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            OpenWindow open = new OpenWindow();
                open.Show();
            this.Close();
        }

        private void BtnFinish_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
            this.Close();
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            btnBrowse.IsEnabled = false;
            UploadManager m = new UploadManager();
           // List<string> f = tesr.GetItems();
         
            foreach(string s in f)
            {
                int begin = s.LastIndexOf("\\") + 1;
                string fileName = s.Substring(begin);
                List<string> elements = new List<string>();
                if (elements.Contains(fileName))
                {
                    MessageBox.Show(string.Format("Sorry you can upload another file with name '{0}', because you have already uploaded file with this name, please fix", fileName));
                    return;
                }
                elements.Add(fileName);

            }
            bool result=m.Execute(f, "xml");
            testo.btnNext.IsEnabled = true;
            if (!result)
            {
                btnBrowse.IsEnabled = true;
                ShowErrors(m.GetErrors());
            }
            else
            {
                testo.btnNext.IsEnabled = false;
                /*MessageBoxResult msg = MessageBox.Show("Do you want to upload another files?", "Upload", MessageBoxButton.YesNo);
                if (msg==MessageBoxResult.Yes)
                {
                  //  tesr.Reload();
                   
                    SystemDefinitionsManager.DefinitionsManager.SetUploadStateToTemp();
                }
                else
                {*/
                testo.btnFinish.IsEnabled = true;
                //}
            }
         }

        private void ShowErrors(SortedList<string, string> errors)
        {
            List<string> lstItems = new List<string>();
            icErrors.ItemsSource = errors;
            icErrors.Visibility = Visibility.Visible;
            foreach (var error in errors)
            {

                foreach (var Item in this.icFiles.Items)
                {
                    string c = Item as string;


                    if (System.IO.Path.GetFileNameWithoutExtension(c) == error.Key)
                    {
                        var ctrl = icFiles.ItemContainerGenerator.ContainerFromItem(c);
                        
                        var txb = WpfUIUtilities.FindVisualChildByName<TextBlock>(ctrl as DependencyObject, "tbFileName");
                        txb.Foreground = Brushes.Red;
                    }
                }
                /*        FileUploadElement fueUploader = WpfUIUtilities.FindVisualChild<FileUploadElement>(Item as DependencyObject);
                        if (fueUploader != null && fueUploader.txbFileName.Text == error.Key)
                            fueUploader.ShowError(error.Value);
                    }
                    FileUploadElement fueUploader = WpfUIUtilities.FindVisualChild<FileUploadElement>(Item as DependencyObject);
                    if (fueUploader != null && fueUploader.txbFileName.Text == error.Key)
                        fueUploader.ShowError(error.Value);*/
                
            }
        }

        private void tesr_WasUpload(object sender, EventArgs e)
        {
            testo.btnNext.IsEnabled = true;
        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult me = MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButton.YesNo);
            if (me == MessageBoxResult.Yes)
                this.Close();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowser fb = new FolderBrowser();
            fb.DialogTitle = "Choose Folder for files that represent database";

            //  fb.SelectedChanged += Fb_SelectedChanged;
            bool? k = fb.ShowDialog();
            if (k.Value)
            {

                if (fb.SelectedDirectory != null)
                {
                    f = Directory.GetFiles(fb.SelectedDirectory).ToList();
                    icFiles.ItemsSource = Directory.GetFiles(fb.SelectedDirectory);
                }
                testo.btnNext.IsEnabled = true;

            }
        }
    }
}
