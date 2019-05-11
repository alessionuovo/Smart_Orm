using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Utilities;
using Utilities.Definitions;

namespace WpfExtensions
{
    /** \ingroup Bll
     <summary>
     Inherits FileUpload
     This class represents a file upload control
     that can upload only specified by some way
     </summary>
    */
   public class FormattedFileUpload:FileUpload
    {
        protected string Format;
        public FormattedFileUpload():base()
        {
            Format = SystemDefinitionsManager.DefinitionsManager.GetSystemFormat();
            Loaded += FormattedFileUpload_Loaded;
            
        }

        private void FormattedFileUpload_Loaded(object sender, RoutedEventArgs e)
        {
            SetControlsToFormat();
        }

        private void SetControlsToFormat()
        {
            foreach (var Item in this.icFileUploadElements.Items)
            {

                FileUploadElement fileUploadElement = WpfUIUtilities.FindVisualChild<FileUploadElement>(Item as DependencyObject);
                if (fileUploadElement == null)
                    fileUploadElement = WpfUIUtilities.FindLogicalChild<FileUploadElement>(Item as DependencyObject);

                string filter = Format;
                fileUploadElement.Filter = filter;
            }
        }

        public string GetFormat()
        {
            return Format;
        }

        public void Reload()
        {
            foreach (var Item in this.icFileUploadElements.Items)
            {

                FileUploadElement fileUploadElement = WpfUIUtilities.FindVisualChild<FileUploadElement>(Item as DependencyObject);
                if (fileUploadElement == null)
                    fileUploadElement = WpfUIUtilities.FindLogicalChild<FileUploadElement>(Item as DependencyObject);

                fileUploadElement.Reload();
            }
        }
    }
}
