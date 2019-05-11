using DiagrmaManagement;
using Microsoft.Win32;
using Syncfusion.Windows.Tools.Controls;
using System;
using System.Collections.Generic;
using System.IO;
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
using UploadManagement;
using Utilities;
using Utilities.Definitions;

namespace Smart_Orm
{
    /// <summary>
    /// Inherits OpenerForm.
    /// Represents a window to choose how to
    /// build initial erd diagram.
    /// Three options available: 
    /// From Files-So upload files in specific format
    /// Empty model-Build from scratch
    /// Load Diagram-continue where you ended last time
    /// </summary>
    public partial class OpenWindow : WpfExtensions.OpenerForm
    {
        
        public OpenWindow():base()
        {
            
            InitializeComponent();
            SystemDefinitionsManager.DefinitionsManager.CreateSystemDirectories();
            icFiles.DataContextChanged += IcFiles_DataContextChanged;
            Loaded += OpenWindow_Loaded;
            testo.btnBack.IsEnabled = false;
            testo.btnNext.IsEnabled = false;
            testo.btnFinish.IsEnabled = false;
            testo.btnBack.Click += BtnBack_Click;
            testo.btnNext.Click += BtnNext_Click;
            testo.btnCancel.Click += BtnCancel_Click;
            testo.btnFinish.Click += BtnFinish_Click;
            lstMode.SelectionChanged += LstMode_SelectionChanged;
          
       
            

        }

      

        private void IcFiles_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UploadManager m = new UploadManager();
            // List<string> f = tesr.GetItems();

            foreach (string s in f)
            {
                int begin = s.LastIndexOf("\\") + 1;
                string fileName = s.Substring(begin);
                if (elements.Contains(fileName))
                {
                    MessageBox.Show(string.Format("Sorry you can upload another file with name '{0}', because you have already uploaded file with this name, please fix", fileName));
                    return;
                }
                elements.Add(fileName);

            }
            bool result = m.Execute(f, "xml");
            testo.btnNext.IsEnabled = true;
            if (!result)
            {
                testo.btnBack.IsEnabled = true;
                lstMode.IsEnabled = true;
                spErrors.Visibility = Visibility.Visible;
                errors = m.GetErrors();
                ShowErrors(errors);
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

      

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Please fix highligted errors in the files";
            if (!string.IsNullOrEmpty(directory))
              ofd.InitialDirectory = directory;
            ofd.ShowDialog();
            lstMode.IsEnabled = true;
        }

        private void BtnFinish_Click(object sender, RoutedEventArgs e)
        {
            testo.btnBack.IsEnabled = false;
            int mode = lstMode.SelectedIndex;
            MainWindow mw = new MainWindow();
            switch (mode)
            {
                case 0:
                   
                    mw.Show();
                    SystemDefinitionsManager.DefinitionsManager.EmptyConstUploadDirectory();
                    SystemDefinitionsManager.DefinitionsManager.SetUploadStateToConst();
                    
                    this.Close();
                    break;
                case 1:
                   
                    mw.Show();
                    this.Close();
                    break;
                case 2:
                    SystemDefinitionsManager.DefinitionsManager.SetUploadStateToConst();
                    DiagramLoader dl = new DiagramLoader();
                    DiagramLoader.DiagramResult d = dl.LoadDiagram();
                    if (d == DiagramLoader.DiagramResult.HAS_FILES)
                    {
                      
                        mw.Show();
                        this.Close();
                    }
                    else if (d == DiagramLoader.DiagramResult.NO_FILES)
                        MessageBox.Show("Sorry, but there are no files from previous diagram, please create another one");
                    else
                        MessageBox.Show("Sorry, all the files from previous are amlformed so you will need to create it from scratch or upload files");
                 
                    break;
            }
        }
        string[] texts = new string[]
        {
            "Creates an empty model in SmartOrm wizard as beginning for visual design of model.",
            "Lets you to upload directory that contains xml files that reside in one directory and contain description of database objects. These files are converted to model from which you begin visual design.",
            "Lets you to continue build diagram from point you have ended last time, this of course is on condition, that you saved it last time you opened the application, and that you did not changed files directly in underlying directory."
    };
        private List<string> f = new List<string>();
        private string directory;
        private SortedList<string, string> errors = new SortedList<string, string>();
        private void LstMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int mode = lstMode.SelectedIndex;
            
            if (mode < texts.Length)
                tbDescription.Text = texts[mode];
            else tbDescription.Text = "";
            bdDescription.Visibility = Visibility.Visible;
            if (mode % 2 != 0)
            {
                testo.btnNext.IsEnabled = true;
                testo.btnFinish.IsEnabled = false;
                FolderBrowser fb = new FolderBrowser();
                fb.DialogTitle = "Choose Folder for files that represent database";

                //  fb.SelectedChanged += Fb_SelectedChanged;
                bool? k = fb.ShowDialog();
                if (k.Value)
                {
                    lstMode.IsEnabled = false;
                    if (fb.SelectedDirectory != null)
                    {
                        directory = fb.SelectedDirectory;
                        f = Directory.GetFiles(fb.SelectedDirectory).ToList();
                        //icFiles.ItemsSource = Directory.GetFiles(fb.SelectedDirectory);
                        Binding b = new Binding()
                        {
                            Source = Directory.GetFiles(fb.SelectedDirectory),
                            NotifyOnSourceUpdated = true,
                            NotifyOnTargetUpdated = true
                    };
                        icFiles.ItemsSource = null;
                    spErrors.Visibility = Visibility.Hidden;
                    icFiles.SetBinding(ItemsControl.ItemsSourceProperty, b);
                        tbUploadInstructions.Visibility = Visibility.Visible;
                        spFiles.Visibility = Visibility.Visible;
                    }
                    

                }
            }
            else
            {
                testo.btnFinish.IsEnabled = true;
                testo.btnNext.IsEnabled = false;
            }
        }

        private void OpenWindow_Loaded(object sender, RoutedEventArgs e)
        {
          //  openerPanel.Children.Add(this.InstallGrid);
        }
        private List<string> elements = new List<string>();
        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            testo.btnBack.IsEnabled = false;
            int mode = lstMode.SelectedIndex;
            switch(mode)
            {
                case 0: MainWindow mw = new MainWindow();
                    mw.Show();
                    SystemDefinitionsManager.DefinitionsManager.EmptyConstUploadDirectory();
                    SystemDefinitionsManager.DefinitionsManager.SetUploadStateToConst();
                    this.Close();
                    break;
                case 1:
                    elements.Clear();
                    UploadManager m = new UploadManager();
                    // List<string> f = tesr.GetItems();
                    
                    foreach (string s in f)
                    {
                        int begin = s.LastIndexOf("\\") + 1;
                        string fileName = s.Substring(begin);
                        if (elements.Contains(fileName))
                        {
                            MessageBox.Show(string.Format("Sorry you can upload another file with name '{0}', because you have already uploaded file with this name, please fix", fileName));
                            return;
                        }
                        elements.Add(fileName);

                    }
                    bool result = m.Execute(f, "xml");
                    testo.btnNext.IsEnabled = true;
                    tbUploadInstructions.Visibility = Visibility.Hidden;
                    if (!result)
                    {
                        testo.btnBack.IsEnabled = true;
                        testo.btnNext.IsEnabled = false;
                        lstMode.IsEnabled = true;
                        icErrors.ItemsSource = m.GetErrors();
                        ShowErrors(m.GetErrors());

                    }
                    else
                    {
                        MessageBox.Show("Uploaded successfully database files", "Upload Result", MessageBoxButton.OK);
                        tbHeader.Text = "List Of Uploaded Files";
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
                    break;
                case 2: DiagramLoader dl = new DiagramLoader();
                    DiagramLoader.DiagramResult d =dl.LoadDiagram();
                    if (d == DiagramLoader.DiagramResult.HAS_FILES)
                    {
                        MainWindow mw1 = new MainWindow();
                        mw1.Show();
                        this.Close();
                    }
                    else if (d==DiagramLoader.DiagramResult.NO_FILES)
                        MessageBox.Show("Sorry, but there are no files from previous diagram, please create another one");
                    else
                        MessageBox.Show("Sorry, all the files from previous are amlformed so you will need to create it from scratch or upload files");
                    break;
            }
            lstMode.IsEnabled = false;
        }

        private void ShowErrors(SortedList<string, string> errors)
        {
            List<string> lstItems = new List<string>();
          
            spErrors.Visibility = Visibility.Visible;
            
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

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult me = MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButton.YesNo);
            if (me == MessageBoxResult.Yes)
                this.Close();
        }
        private void Rg_SelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            

        }

        private void lstMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
