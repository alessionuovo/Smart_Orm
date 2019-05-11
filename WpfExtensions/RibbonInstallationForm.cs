using Syncfusion.Windows.Tools.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfExtensions;
namespace WPfExtensions
{
    /** \ingroup Pl
     <summary>
     This class is common to all forms that 
     are part of installation and/or configuration
     of the application.
     </summary>
    */
    public class InstallationForm : Window
    {
        public InstallationForm() : base()
        {
            this.AddLogicalChild(InstallGrid);
            this.Loaded += InstallationForm_Loaded; 
        }

        private void InstallationForm_Loaded(object sender, RoutedEventArgs e)
        {
           // InstallGrid.btnCancel.Click += BtnCancel_Click;
        }

        /// <summary>
        /// Grid that regularly is shown in the bottom and
        /// indicates that this is installation/configuration wizard.
        /// </summary>
        ///
        public InstallationGrid InstallGrid { get; set; }
         
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult me=MessageBox.Show("Are you sure you want to exit?" , "Exit", MessageBoxButton.YesNo);
            if (me == MessageBoxResult.Yes)
                this.Close();
        }
    }
}
