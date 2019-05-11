using FileManagement;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Utilities;
using Utilities.Definitions;

namespace WpfExtensions
{
    /** \ingroup Pl
     <summary>
     This class represents file upload control 
     that user can choose format from list of formats
     and upload files to server.
     </summary>
    */
    public class SelectedFormatFileUpload:FormattedFileUpload
    {
        private ListBox lstFormats { get; set; }
        public SelectedFormatFileUpload():base()
        {
            
            Loaded += SelectedFormatFileUpload_Loaded;
            lstFormats = new ListBox();
            SystemDefinitionsManager definitionsmanager = SystemDefinitionsManager.DefinitionsManager;
            lstFormats.ItemsSource = definitionsmanager.GetAvailableFormats().Values;
            
            spFileUploadContainer.Children.Insert(0, lstFormats);
            //FileUploadElements.Items.Add(lstFormats);
        }

        private void SelectedFormatFileUpload_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
          
            lstFormats.SelectionChanged += LstFormats_SelectionChanged;
          
            
        }

        

        public string GetFormat()
        {
            return lstFormats.SelectedValue as string;
        }

        //This method is called when format was changed
        //and sets the format on each of the file upload elements
        private void LstFormats_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          foreach(var Item in this.icFileUploadElements.Items)
            {
               
                FileUploadElement fileUploadElement = WpfUIUtilities.FindVisualChild<FileUploadElement>(Item as DependencyObject);
                if (fileUploadElement == null)
                    fileUploadElement = WpfUIUtilities.FindLogicalChild<FileUploadElement>(Item as DependencyObject);
              
                string filter= lstFormats.SelectedValue as string;
                fileUploadElement.Filter = filter;
            }
          
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

