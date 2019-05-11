using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using Utilities.Definitions;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Syncfusion.Diagram.Extensions
{
    /// <summary>
    /// \ingroup Pl
    /// Represents a property of the table(in the graphics)
    /// </summary>
    public class BodyElement:EditableStackPanel
    {
        private ContextMenu chbContextMenu;
        private Image imgPrimaryKey;
        private ComboBox lstType;
        private TextBlock tbLeft;
        private TextBlock tbType;
        private TextBlock tbRight;
        public BodyElement():base()
        {
            
            chbContextMenu = new ContextMenu();
            imgPrimaryKey = new Image();
            var bitmapImage = new BitmapImage();
            var imageUri = "keyicon.png";
            bitmapImage.BeginInit();
           
            bitmapImage.UriSource = new Uri("сonfigure-2.png", UriKind.RelativeOrAbsolute);
            imgPrimaryKey.Source = bitmapImage;
            bitmapImage.EndInit();
           
         //   imgPrimaryKey.Width = 200;
            BuildContextMenu();
            this.ContextMenu = chbContextMenu;
            this.Children.Insert(0, imgPrimaryKey);
            
            
            lstType = new ComboBox();
           
            lstType.Items.Add("Choose");
            List<string> lstTypes = SystemDefinitionsManager.DefinitionsManager.GetDataTypes();
            foreach (string type in lstTypes)
            {
               lstType.Items.Add(type);
            }
            lstType.SelectionChanged += LstType_SelectionChanged;
            tbType = new TextBlock();
            lstType.Visibility = Visibility.Collapsed;
            lstType.KeyDown += LstType_KeyDown;
            tbType.MouseDown += TbType_MouseDown;
            tbLeft = new TextBlock();
            tbLeft.Text = "(";
            tbRight = new TextBlock();
            tbRight.Text = ")";
            this.Children.Add(tbLeft);
            this.Children.Add(lstType);
            this.Children.Add(tbType);
            this.Children.Add(tbRight);
            foreach(FrameworkElement c in this.Children)
            {
                c.Margin = new Thickness(5, 0, 0, 0);
            }

           // if (!IsPrimary)
              //  imgPrimaryKey.Visibility = Visibility.Collapsed;
        }
        static BodyElement()
        {
            BodyElement.ItemNameProperty.OverrideMetadata(typeof(BodyElement), new FrameworkPropertyMetadata(new PropertyChangedCallback(NameChanged)));
        }

        private static void NameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BodyElement erd = d as BodyElement;
            string value = e.NewValue as string;
            if (string.IsNullOrEmpty(value))
            {
                erd.editable = true;
                erd.tbType.Visibility = Visibility.Collapsed;
                erd.lstType.Visibility = Visibility.Visible;
                erd.lstType.SelectedIndex = 0;
                erd.lstType.IsSynchronizedWithCurrentItem = true;
            }
           
        }

        private void LstType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstType.SelectedValue != null && lstType.SelectedValue.ToString()!="Choose")
            {
                lstType.Visibility = Visibility.Collapsed;
                tbType.Visibility = Visibility.Visible;
                ItemType = lstType.SelectedValue.ToString();
                tbType.Text = ItemType;
                RaiseEndEdit();
            }
        }

        private void TbType_MouseDown(object sender, MouseButtonEventArgs e)
        {
            tbType.Visibility = Visibility.Collapsed;
            lstType.Visibility = Visibility.Visible;
            lstType.SelectedValue = ItemType;
            RaiseBeginEdit();
        }

        private void BuildContextMenu()
        {
            MenuItem mnuPrimaryKey = new MenuItem();
            mnuPrimaryKey.Header = "Primary Key";
            mnuPrimaryKey.IsCheckable = true;
            mnuPrimaryKey.Click += MnuPrimaryKey_Click;
            chbContextMenu.Items.Add(mnuPrimaryKey);
        }

        private void MnuPrimaryKey_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            IsPrimary = mi.IsChecked;
            var bitmapImage = new BitmapImage();
            var imageUri = "сonfigure.png";
            bitmapImage.BeginInit();


            bitmapImage.UriSource = new Uri(imageUri, UriKind.RelativeOrAbsolute);
            if (IsPrimary)
                imageUri = "сonfigure-2.png";
            imgPrimaryKey.Source = bitmapImage;
           
            bitmapImage.EndInit();
        }

        private void TbType_KeyDown(object sender, KeyEventArgs e)
        {
           
                tbType.Visibility = Visibility.Collapsed;
                lstType.Visibility = Visibility.Visible;
                lstType.SelectedValue = ItemType;
           
        }

        private void LstType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                lstType.Visibility = Visibility.Collapsed;
               tbType.Visibility = Visibility.Visible;
               ItemType = lstType.SelectedValue.ToString();
                tbType.Text = ItemType;
            }
        }

        private void ChbPrimaryKey_KeyDown(object sender, KeyEventArgs e)
        {
            /*if (e.Key == Key.Return)
            {
                chbPrimaryKey.Visibility = Visibility.Collapsed;
                tbPrimaryKey.Visibility = Visibility.Visible;
                IsPrimary= chbPrimaryKey.IsChecked.Value;
                if (IsPrimary)
                    tbPrimaryKey.Text = "Primary";
                else tbPrimaryKey.Text = "";
            }*/
        }

        private void TbPrimaryKey_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
          /*  if (e.Key == Key.Return)
            {
                tbPrimaryKey.Visibility = Visibility.Collapsed;
                chbPrimaryKey.Visibility = Visibility.Visible;
                chbPrimaryKey.IsChecked = IsPrimary;
            }*/
        }

        public static readonly DependencyProperty ItemTypeProperty
           = DependencyProperty.Register("ItemType", typeof(string), typeof(EditableStackPanel),
                                         new FrameworkPropertyMetadata((string)null,
                                                                    new PropertyChangedCallback(OnTypeChanged)));

        private static void OnTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BodyElement erd = d as BodyElement;
            string value = e.NewValue as string;
            if (erd.editable)
            {
                if (!string.IsNullOrEmpty(value))
                erd.lstType.SelectedValue = value;
            }
            else erd.tbType.Text = value;
        }

        public string ItemType
        {
            get { return (string)GetValue(ItemTypeProperty); }
            set
            {
                if (value == null)
                {
                    ClearValue(ItemTypeProperty);
                }
                else
                {
                    SetValue(ItemTypeProperty, value);
                }
            }
        }
        public static readonly DependencyProperty IsPrimaryProperty
           = DependencyProperty.Register("IsPrimary", typeof(bool), typeof(EditableStackPanel),
                                         new FrameworkPropertyMetadata((bool)false,
                                                                    new PropertyChangedCallback(OnPrimaryChanged)));

        private static void OnPrimaryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            /*BodyElement erd = d as BodyElement;
            erd.IsPrimary = mi.IsChecked;
            if (IsPrimary)
                tbPrimaryKey.Text = "Primary Key";
            else tbPrimaryKey.Text = "Not Primary Key";*/
        }

        public bool IsPrimary
        {
            get { return (bool)GetValue(IsPrimaryProperty); }
            set
            {
                if (value == null)
                {
                    ClearValue(IsPrimaryProperty);
                }
                else
                {
                    SetValue(IsPrimaryProperty, value);
                }
            }
        }
    }
}
