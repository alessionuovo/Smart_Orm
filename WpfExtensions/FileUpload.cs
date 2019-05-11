using Syncfusion.Windows.Tools.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Utilities;

namespace WpfExtensions
{
    /** \ingroup Pl
     <summary>
     This class represents a control  for uploading files
     to application.
     Consists of Header
     and list of: textboxes with browse buttons.
     </summary>
    */
    public class FileUpload : GroupBox
    {
        /// <summary>
        /// Dependency Property that
        /// shows the initial content of the control
        /// </summary>
        public object InitialContent
        {
            get { return GetValue(InitialContentProperty); }
            set
            {
                if (value == null)
                {
                    ClearValue(InitialContentProperty);
                }
                else
                {
                    SetValue(InitialContentProperty, value);
                }
            }
        }
        public event EventHandler WasUpload;
        public static readonly DependencyProperty InitialContentProperty
      = DependencyProperty.Register("InitialContent", typeof(object), typeof(FileUpload),
           new FrameworkPropertyMetadata((object)null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnInitialContentChanged)));

        protected static void OnInitialContentChanged(DependencyObject FileUploadObject, DependencyPropertyChangedEventArgs e)
        {

        }


        protected ItemsControl icFileUploadElements { get; set; }
        public int NumElements { get; set; }
      
        
        protected StackPanel spFileUploadContainer { get; set; }
        public FileUpload() : base()
        {
            spFileUploadContainer = new StackPanel();
            icFileUploadElements = new ItemsControl();
            Loaded += FileUpload_Loaded;

        }

        public void FileUpload_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {



            
           
            for (int i = 0; i < NumElements; i++)
            {
                StackPanel sp = new StackPanel();
                FileUploadElement fue = new FileUploadElement();
                fue.Uploaded += Fue_Uploaded;
                sp.Orientation = Orientation.Horizontal;
                sp.Children.Add(fue);

                sp.Margin = new Thickness(15);

                icFileUploadElements.Items.Add(sp);
            }
            spFileUploadContainer.Children.Add(icFileUploadElements);
            this.InitialContent = spFileUploadContainer;

        }

        private void Fue_Uploaded(object sender, EventArgs e)
        {
            if (WasUpload != null)
                WasUpload(this, e);
        }

        public List<string> GetItems()
        {
            List<string> lstItems = new List<string>();
            foreach (var Item in this.icFileUploadElements.Items)
            {
                FileUploadElement fueUploader = WpfUIUtilities.FindVisualChild<FileUploadElement>(Item as DependencyObject);
                if (fueUploader != null && !string.IsNullOrEmpty(fueUploader.txbFileName.Text))
                    lstItems.Add(fueUploader.txbFileName.Text);
            }
            return lstItems;
        }

        /// <summary>
        /// This method used to show errors that occur
        /// during validation of formats and contents of uploaded files
        /// </summary>
        /// <param name="errors">Represents dictionary of errors: the key is the type if its error/warning/etc and the value is the message to display</param>

        public void ShowErrors(SortedList<string, string> errors)
        {
            List<string> lstItems = new List<string>();
            foreach (var error in errors)
            {
                foreach (var Item in this.icFileUploadElements.Items)
                {
                    FileUploadElement fueUploader = WpfUIUtilities.FindVisualChild<FileUploadElement>(Item as DependencyObject);
                    if (fueUploader != null && fueUploader.txbFileName.Text == error.Key)
                        fueUploader.ShowError(error.Value);
                }
            }

        }
    }
}

