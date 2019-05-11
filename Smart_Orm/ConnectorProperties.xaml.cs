using DiagramContent;
using DiagramManagement;
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

namespace Smart_Orm
{
    /// <summary>
    /// This class represents form to choose
    /// properties for newly inserted connector.
    /// </summary>
    public partial class ConnectorProperties : Window
    {
        private string firstTable;
        private string firstRelation;
        private string secondTable;
        private string secondRelation;
        private string name;
        private bool insertMode;
        private bool ExitWithSave = false;
        public bool Result { get; set; }
        public int NumConnectorsAdded { get; private set; }
        public ConnectorProperties()
        {
           
            InitializeComponent();
            Structure st = Structure.Struct;
            FirstTable.DataContext = st;
            SecondTable.DataContext = st;
            this.Closing += ConnectorProperties_Closing;
            insertMode = true;
            Result = true;
        }

        private void ConnectorProperties_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!ExitWithSave)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Are you sure you want to close, without save, all changes will discard", "Close", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No)
                    e.Cancel = true;
                else Result = false;
            }
        }

        public ConnectorProperties(string name)
        {
            Name = name;
            this.Closing += ConnectorProperties_Closing;
        }
        public string GetName()
        {
            return name;
        }
        public ConnectorProperties(string name, string firstTable, string firstRelation, string secondTable, string secondRelation) : this(name)
        {
            InitializeComponent();
            this.firstTable = firstTable;
            this.firstRelation = firstRelation;
            this.secondTable = secondTable;
            this.secondRelation = secondRelation;
            Structure st = Structure.Struct;
            Model m = st.model;
           
             
                FirstTable.DataContext = st;
                SecondTable.DataContext = st;
            this.Loaded += ConnectorProperties_Loaded;



            Result = true;
            /*this.name = name;
            this.Closing += ConnectorProperties_Closing;*/
        }

        private void ConnectorProperties_Loaded(object sender, RoutedEventArgs e)
        {
            Structure st = Structure.Struct;
            Model m = st.model;
            var tablesFirst = m.tables.Where(x => x.Name == firstTable);
            var tablesSecond = m.tables.Where(y => y.Name == secondTable);
            if (tablesFirst.Count()>0)
            {
                var table = tablesFirst.First();
                FirstTable.SelectedValue = table;
            }
            if (tablesSecond.Count() > 0)
            {
                var table = tablesSecond.First();
                SecondTable.SelectedValue = table;
            }
            var firstrelationitems = FirstRelation.Items;
            foreach(var item in firstrelationitems)
            {
                ComboBoxItem cbi = item as ComboBoxItem;
                if (cbi!=null)
                {
                    if (cbi.Content == null)
                        continue;
                    if (cbi.Content.ToString().ToUpper()==firstRelation.ToUpper())
                    {
                        FirstRelation.SelectedItem = cbi;
                        break;
                    }
                }
            }
            var secondrelationitems = SecondRelation.Items;
            foreach (var item in secondrelationitems)
            {
                ComboBoxItem cbi = item as ComboBoxItem;
                if (cbi != null)
                {
                    if (cbi.Content == null)
                        continue;
                    if (cbi.Content.ToString().ToUpper() == secondRelation.ToUpper())
                    {
                        SecondRelation.SelectedItem = cbi;
                        break;
                    }
                }
            }
           
    
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            Structure st = Structure.Struct;
            Model m = st.model;
            
            if (FirstTable.SelectedItem==null || SecondTable.SelectedItem==null || FirstRelation.SelectedItem==null || SecondRelation.SelectedItem==null)
            {
                MessageBox.Show("Sorry, you haven't configured some of properties of the connection, please check");
                return;
            }
            if (insertMode)
            {

                Connection cnt = new Connection();
                cnt.FirstTable = (FirstTable.SelectedItem as DiagramContent.Table).Name;
                cnt.SecondTable = (SecondTable.SelectedItem as DiagramContent.Table).Name;
                cnt.FirstRelation = (FirstRelation.SelectedItem as ComboBoxItem).Content.ToString();
                cnt.SecondRelation = (SecondRelation.SelectedItem as ComboBoxItem).Content.ToString();

                m.connections.Add(cnt);
                ChangeManager.Manager.Add(cnt, State.Add);
            }
            else
            {
                var conns=m.connections.Where(x => x.Name == name);
                if (conns.Count()>0)
                {
                    Connection cnt = conns.First();
                    cnt.FirstTable = (FirstTable.SelectedItem as DiagramContent.Table).Name;
                    cnt.SecondTable = (SecondTable.SelectedItem as DiagramContent.Table).Name;
                    cnt.FirstRelation = (FirstRelation.SelectedItem as ComboBoxItem).Content.ToString();
                    cnt.SecondRelation = (SecondRelation.SelectedItem as ComboBoxItem).Content.ToString();
                    //ChangeManager.Manager.Add(cnt, State.Update);
                }
            }

            ExitWithSave = true;
            this.Close();
        }

        private List<Control> GetAllChildren(DependencyObject lbiContainer)
        {
            List<Control> lst = new List<Control>();
            for(var i=0; i<VisualTreeHelper.GetChildrenCount(lbiContainer); i++)
            {
                var child = VisualTreeHelper.GetChild(lbiContainer, i);
                if (child is Control)
                    lst.Add(child as Control);
                lst.AddRange(GetAllChildren(child));

            }
            return lst;
        }

        private void Connector_Quantity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
           
        }

        private void SecondProperty_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void FirstTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void SecondTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
         
        }
    }
}
