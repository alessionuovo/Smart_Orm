using System.Windows.Controls;
using System.Windows;
namespace WpfExtensions
{
    /** \ingroup Pl
     <summary>
     This class represents standard installation wizard
     with buttons back, next, cancel and finish.
     
     </summary>
    */
    public class InstallationGrid:StackPanel
    {
        /// <summary>
        /// Back button
        /// </summary>
        public Button btnBack { get; set; }
        /// <summary>
        /// Next button
        /// </summary>
        public Button btnNext { get; set; }
        /// <summary>
        /// Cancel button
        /// </summary>
        public Button btnCancel { get; set; }
        /// <summary>
        /// Finish button
        /// </summary>
        public Button btnFinish { get; set; }
        public InstallationGrid():base()
        {
            btnBack = new Button();
            btnBack.Content = "Back";
            this.Children.Add(btnBack);
            btnNext = new Button();
            btnNext.Content = "Next";
            this.Children.Add(btnNext);
            btnFinish = new Button();
            btnFinish.Content = "Finish";
            this.Children.Add(btnFinish);
            btnCancel = new Button();
            btnCancel.Content = "Cancel";
            this.Children.Add(btnCancel);
            this.HorizontalAlignment = HorizontalAlignment.Center;
            Orientation = Orientation.Horizontal;
        }
    }
}