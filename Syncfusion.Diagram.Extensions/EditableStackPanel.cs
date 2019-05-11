using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
/// <summary>
/// \ingroup Pl
/// This namespace is responsible for some extensions that 
/// help creating ERD of database
/// </summary>
namespace Syncfusion.Diagram.Extensions
{
    /// <summary>
    /// This class represents Panel that can be edited
    /// used for Table Name , and for each Property in table of course can be used wider
    /// </summary>
    public class EditableStackPanel : StackPanel
    {
        /// <summary>
        /// The value that was before you began the last edit
        /// </summary>
        public string PreviousValue { get; set; }
        private TextBlock tb;
        protected bool editable = false;
        protected bool InEditing = false;
        private TextBox tbb;
        /// <summary>
        /// Event that responsible for checking that value is illegal (not ambiguos etc)
        /// </summary>
        public event EventHandler<ValueEventArgs> Validate;
        /// <summary>
        /// This event tells that user began to edit
        /// </summary>
        public event EventHandler BeginEdit;
        /// <summary>
        /// This event tells that user ended edit
        /// </summary>
        public event EventHandler EndEdit;
        public EditableStackPanel() : base()
        {
            this.Orientation = Orientation.Horizontal;
            tb = new TextBlock();
            tbb = new TextBox();
            tbb.Width = 60;
            this.Children.Add(tb);
            this.Children.Add(tbb);
            
            
            tbb.Visibility = Visibility.Collapsed;
            tb.KeyDown += Tb_KeyDown;
            tbb.KeyDown += Tbb_KeyDown;
            tbb.LostFocus += Tbb_LostFocus;
            tb.MouseDown += Tb_MouseDown;
            

        }

        private void Tbb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Validate != null)
                Validate(this, new ValueEventArgs() { Value = tbb.Text });
        }

        private void Tb_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                InEditing = true;
                tb.Visibility = Visibility.Collapsed;
                tbb.Visibility = Visibility.Visible;
                tbb.Text = tb.Text;
                if (BeginEdit != null)
                    BeginEdit(sender, e);
                //ItemName = tb.Text;
               
            }
        }

        private void Tbb_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

           if (e.Key == Key.Return)
            {
                InEditing = false;
                if (Validate != null)
                    Validate(this, new ValueEventArgs() { Value = tbb.Text });
                
            }
        }

        /// <summary>
        /// Supposed to be called when validate was successfull
        /// </summary>
        public void SaveValue()
        {
            tbb.Visibility = Visibility.Collapsed;
            tb.Visibility = Visibility.Visible;

            tb.Text = tbb.Text;
            ItemName = tbb.Text;
            if (EndEdit != null && !InEditing)
                EndEdit(this, new EventArgs());
        }
        public string GetCurrentValue()
        {
            return tb.Text;
        }
        protected void RaiseBeginEdit()
        {
            if (BeginEdit != null)
                BeginEdit(this, new EventArgs());
        }
        protected void RaiseEndEdit()
        {
            if (EndEdit != null)
                EndEdit(this, new EventArgs());
        }
        private void Tb_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                tb.Visibility = Visibility.Collapsed;
                tbb.Visibility = Visibility.Visible;
                tbb.Text = ItemName;
            }
        }

        public static readonly DependencyProperty ItemNameProperty
           = DependencyProperty.Register("ItemName", typeof(string), typeof(EditableStackPanel),
                                         new FrameworkPropertyMetadata((string)null,
                                                                    new PropertyChangedCallback(OnConnectorsSourceChanged)));

        private static void OnConnectorsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EditableStackPanel erd = d as EditableStackPanel;
            string value = e.NewValue as string;
            if (string.IsNullOrEmpty(value))
            {
                erd.editable = true;
                erd.tbb.Visibility = Visibility.Visible;
                erd.tb.Visibility = Visibility.Collapsed;
            }
            if (erd.editable)
                erd.tbb.Text = value;
            else erd.tb.Text = value;
            erd.PreviousValue = e.NewValue as string;
        }

        public string ItemName
        {
            get { return (string)GetValue(ItemNameProperty); }
            set
            {
                if (value == null)
                {
                    ClearValue(ItemNameProperty);
                }
                else
                {
                    SetValue(ItemNameProperty, value);
                }
            }
        }
    }
}
