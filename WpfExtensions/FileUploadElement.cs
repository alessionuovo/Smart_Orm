using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
/// <summary>
/// This namespace represents
/// the extensions of wpf controls
/// that are needed to current application
/// </summary>
namespace WpfExtensions
{
    /** \ingroup Pl
     <summary>
     Inherits ItemsControl.
     This class represents a control that used to upload one file to application
    </summary>
    /// contains : 
    /// <list type="Bullet">
    /// <item><term>Textbox txbFileName</term><description> to write a file name to upload</description>
    /// <item></item>Button btnBrowse- To open the file dialog to choose the file
    /// Filter-Filters the format and Extensions to upload
    /// 
    /// </list>
   */
    public class FileUploadElement:ItemsControl
    {
        public TextBox txbFileName { get; set; }
        public Button btnBrowse { get; set; }
        public event EventHandler Uploaded;
        protected Label lblError;
        /// <summary>
        /// Filters file formats that this control can upload
        /// </summary>
        public string Filter { get; internal set; }

        public FileUploadElement():base()
        {
            txbFileName = new TextBox();
            btnBrowse = new Button();
            btnBrowse.Content = "Browse";
            btnBrowse.Click += BtnBrowse_Click;
            Loaded += FileUploadElement_Loaded;
            lblError = new Label();
        }

        private void BtnBrowse_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
           
            openFileDialog.Filter = string.Format("{0} Files  |*.{1}", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Filter as string), Filter); ;
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == true)
            {
                txbFileName.Text = openFileDialog.FileName;
                if (Uploaded != null)
                    Uploaded(this, e);
            }
        }

        private void FileUploadElement_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            StackPanel spUploadContainer = new StackPanel();
            spUploadContainer.Orientation = Orientation.Horizontal;
            spUploadContainer.Children.Add(txbFileName);
            txbFileName.Width = 700;

            spUploadContainer.Children.Add(btnBrowse);
            spUploadContainer.Children.Add(lblError);
            this.Items.Add(spUploadContainer);
            
        }

        internal void ShowError(string value)
        {
            lblError.Content = value;
        }

        internal void Reload()
        {
            lblError.Content = "";
            txbFileName.Text = "";
        }
    }
}
