using Syncfusion.Windows.Diagram;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Utilities;

namespace Syncfusion.Diagram.Extensions
{
    /// <summary>
    /// \ingroup Pl
    /// This class extends syncfusion diagram model
    /// if you want more details please go to: https://www.syncfusion.com/products/wpf/diagram
    /// </summary>
    public class ErdDiagramModel:DiagramModel
    {
        #region Variables
            private string _connectorssource;
      
        public object ModelSource
        {
            get { return GetValue(ModelSourceProperty); }
            set
            {
                if (value == null)
                {
                    ClearValue(ModelSourceProperty);
                }
                else
                {
                    SetValue(ModelSourceProperty, value);
                }
            }
        }
        /// <summary>
        /// This is overall dependency properties
        /// that can be bound to model with tables and connection
        /// </summary>
        public static readonly DependencyProperty ModelSourceProperty
      = DependencyProperty.Register("ModelSource", typeof(object), typeof(ErdDiagramModel),
           new FrameworkPropertyMetadata((object)null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnModelSourceChanged)));

        private Style _connectorstyle;
        /// <summary>
        /// Property for binding style to connector
        /// </summary>
        public static readonly DependencyProperty ConnectorStyleProperty
            = DependencyProperty.Register("ConnectorStyle", typeof(Style), typeof(ErdDiagramModel),
                                          new FrameworkPropertyMetadata((Style)null,
                                                                     new PropertyChangedCallback(OnConnectorsStyleChanged)));
        public Style ConnectorStyle
        {
            get { return (Style)GetValue(ConnectorStyleProperty); }
            set
            {

                SetValue(ConnectorStyleProperty, value);

            }
        }
        /// <summary>
        /// Name of property in bind source that is collection of tables to bind
        /// </summary>
        public string NodeSource
        {
            get; set;
        }

        /// <summary>
        /// Name of property in bind source that is collection of connections to bind
        /// </summary>
        public string ConnectorSource { get; set; }
        #endregion
        private bool NodesAddedDone = false;
        private bool ConnectorAddedDone = false;
        private List<string> tname;
        public List<string> TableNames { get { tname=GetNodeNames(); return tname; } set { tname = value; }  }
        public ErdLineConnector savedconnector;

        public ErdDiagramModel() : base()
        {
            Changed += ErdDiagramControl_Changed;
            Nodes.CollectionChanged += Nodes_CollectionChanged;
            Connections.CollectionChanged += Connections_CollectionChanged;
            

        }

        private void Connections_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
          /*  if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add && !ConnectorAddedDone)
            {
               
                

                
               
                    SynchronizeConnectors();
                    
               

               

            }*/
        }

        public void SynchronizeConnectors()
        {
            string node_source = NodeSource;
            string connector_source = ConnectorSource;
            var k = ModelSource;
            var type = k.GetType();
            var connectorsproperty = type.GetProperty(connector_source);
            var connectors = connectorsproperty.GetValue(k) as IList;
            if (connectors.Count == Connections.Count)
            {
                for (int i = 0; i < this.Connections.Count; i++)
                {
                    ErdLineConnector lc = this.Connections[i] as ErdLineConnector;
                    lc.DataContext = connectors[i];
                    lc.Style = ConnectorStyle as Style;
                    lc.ApplyTemplate();
                }
                foreach (Node n in this.Nodes)
                {
                    // find a ContentPresenter of that list item.. [Call FindVisualChild Method]
                    ContentPresenter ContentPresenterObj = WpfUIUtilities.FindVisualChild<ContentPresenter>(n);

                    // call FindName on the DataTemplate of that ContentPresenter
                    DataTemplate DataTemplateObj = ContentPresenterObj.ContentTemplate;

                    // Label Chk = (Label)DataTemplateObj.FindName("lbl", ContentPresenterObj);
                    Header header = (Header)WpfUIUtilities.FindVisualChild<Header>(ContentPresenterObj);
                    if (header == null) continue;
                    string content = header.ItemName;

                   
                    foreach (var ee in this.Connections)
                    {
                        if (!(ee is ErdLineConnector)) continue;
                        ErdLineConnector er = ee as ErdLineConnector;
                        if (er.HeadNodeName == content)
                        {
                            er.HeadNode = n;
                            
                        }
                        if (er.TailNodeName == content)
                            er.TailNode = n;
                        er.ApplyTemplate();
                    }
                }
            }
        }

        private void Nodes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
           if (e.Action==System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
               /* string node_source = NodeSource;
                string connector_source = ConnectorSource;
                var k = ModelSource;
                var type = k.GetType();
                var nodesproperty = type.GetProperty(node_source);
                var nodes = nodesproperty.GetValue(k) as IList;
                for (int i = 0; i < this.Nodes.Count; i++)
                {
                    Node n = this.Nodes[i] as Node;
                    if (n.ContentTemplate != null)
                        continue;
                    n.ContentTemplate = this.ItemTemplate;
                   
                    n.Content = nodes[i];
                    n.ApplyTemplate();
                    string name=GetNodeName(n);
                }*/

            }
        }

        private void ErdDiagramControl_Changed(object sender, EventArgs e)
        {
           /* int i = 0;
           
            foreach (Node n in this.Nodes)
            {
                // find a ContentPresenter of that list item.. [Call FindVisualChild Method]
                ContentPresenter ContentPresenterObj = WpfUIUtilities.FindVisualChild<ContentPresenter>(n);

                // call FindName on the DataTemplate of that ContentPresenter
                DataTemplate DataTemplateObj = ContentPresenterObj.ContentTemplate;

                // Label Chk = (Label)DataTemplateObj.FindName("lbl", ContentPresenterObj);
                Header header = (Header)WpfUIUtilities.FindVisualChild<Header>(ContentPresenterObj);
                if (header == null) continue;
                string content = header.ItemName;

                i++;
                foreach (var ee in this.Connections)
                {
                    if (!(ee is ErdLineConnector)) continue;
                    ErdLineConnector er = ee as ErdLineConnector;
                    if (er.HeadNodeName == content)
                    {
                        er.HeadNode = n;
                        
                    }
                    if (er.TailNodeName == content)
                        er.TailNode = n;
                    er.ApplyTemplate();
                }
            }
            if (i>0)
            {
                List<ErdLineConnector> lst = new List<ErdLineConnector>();
                foreach (var ee in this.Connections)
                {
                    if (!(ee is ErdLineConnector)) continue;
                    ErdLineConnector er = ee as ErdLineConnector;
                    if (er.HeadNode == null && er.TailNode == null)
                    {
                        lst.Add(er);
                    }
                }
                foreach(ErdLineConnector d in lst)
                {
                    this.Connections.Remove(d);
                }
            }*/
        }




      

        private static void OnModelSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ErdDiagramModel erd = d as ErdDiagramModel;

            string node_source = erd.NodeSource;
            string connector_source = erd.ConnectorSource;
            var newElement = e.NewValue;
            var type = newElement.GetType();
            var nodesproperty = type.GetProperty(node_source);
            var connectorproperty = type.GetProperty(connector_source);
            if (nodesproperty == null || connectorproperty == null)
                return;
            
            var nodes = nodesproperty.GetValue(newElement) as IList;
            var connectors = connectorproperty.GetValue(newElement) as IList;
            if (!(nodes is IList) || !(connectors is IList))
                return;
            int i = 1;
            foreach (var node in nodes)
            {
                Node newnode = new Node();
                newnode.OffsetX = 300 * i;
                i++;
                newnode.OffsetY = 10*i;
                newnode.ContentTemplate = erd.ItemTemplate;
                newnode.Content = node;
                erd.Nodes.Add(newnode);
            }
           
            foreach (var connector in connectors)
            {
                ErdLineConnector lc = new ErdLineConnector();
                lc.Content = connector;
                lc.DataContext = connector;
                lc.Style = erd.ConnectorStyle;
                lc.ApplyTemplate();

                

               
                erd.Connections.Add(lc);
            }
            


        }
        private static void OnConnectorsStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {







        }


        #region Utility Functions













        public string GetNodeName(Node node)
    {
        ContentPresenter ContentPresenterObj = WpfUIUtilities.FindVisualChild<ContentPresenter>(node);

        // call FindName on the DataTemplate of that ContentPresenter
        DataTemplate DataTemplateObj = ContentPresenterObj.ContentTemplate;

        // Label Chk = (Label)DataTemplateObj.FindName("lbl", ContentPresenterObj);
        Header header = (Header)WpfUIUtilities.FindVisualChild<Header>(ContentPresenterObj);
        string content = header.ItemName;
        return content;

    }

  
   

 

      


       


        /// <summary>
        /// Get a selected checkbox items
        /// </summary>
        private static Node GetNodeByName(ErdDiagramModel erd, string name)
        {
            try
            {
                for (int i = 0; i < erd.Nodes.Count; i++)
                {
                    // Get a all list items from listbox

                    Node currentNode = erd.Nodes[i] as Node;
                    // find a ContentPresenter of that list item.. [Call FindVisualChild Method]
                    ContentPresenter ContentPresenterObj = WpfUIUtilities.FindVisualChild<ContentPresenter>(currentNode);

                    // call FindName on the DataTemplate of that ContentPresenter
                    DataTemplate DataTemplateObj = ContentPresenterObj.ContentTemplate;
                    Label Chk = (Label)DataTemplateObj.FindName("lbl", ContentPresenterObj);
                    string content = Chk.Content as string;
                    if (content == name)
                        return currentNode;

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return null;
        }

      

        public Header FindHeader(Node n)
        {
            ContentPresenter ContentPresenterObj = WpfUIUtilities.FindVisualChild<ContentPresenter>(n);

            // call FindName on the DataTemplate of that ContentPresenter
            DataTemplate DataTemplateObj = ContentPresenterObj.ContentTemplate;

            // Label Chk = (Label)DataTemplateObj.FindName("lbl", ContentPresenterObj);
            Header header = (Header)WpfUIUtilities.FindVisualChild<Header>(ContentPresenterObj);
            return header;
        }
        private List<string> GetNodeNames()
        {
            List<string> lst = new List<string>();
            foreach(var n in this.Nodes)
            {
                Node node = n as Node;
                lst.Add(GetNodeName(node));
            }
            return lst;
        }
        public Node GetNodeByName(string name)
        {
            foreach (var n in this.Nodes)
            {
                Node node = n as Node;
                string nodename=GetNodeName(node);
                if (nodename == name)
                    return node;
            }
            return null;
        }

        public void SynchronizeConnector()
        {
            string node_source = NodeSource;
            string connector_source = ConnectorSource;
            var k = ModelSource;
            var type = k.GetType();
            var connectorsproperty = type.GetProperty(connector_source);
            var connectors = connectorsproperty.GetValue(k) as IList;
            if (connectors.Count == Connections.Count)
            {
               
                    ErdLineConnector lc = this.Connections[this.Connections.Count-1] as ErdLineConnector;
                    lc.DataContext = connectors[connectors.Count-1];
                    lc.Style = ConnectorStyle as Style;
                    lc.ApplyTemplate();
                
                foreach (Node n in this.Nodes)
                {
                    // find a ContentPresenter of that list item.. [Call FindVisualChild Method]
                    ContentPresenter ContentPresenterObj = WpfUIUtilities.FindVisualChild<ContentPresenter>(n);

                    // call FindName on the DataTemplate of that ContentPresenter
                    DataTemplate DataTemplateObj = ContentPresenterObj.ContentTemplate;

                    // Label Chk = (Label)DataTemplateObj.FindName("lbl", ContentPresenterObj);
                    Header header = (Header)WpfUIUtilities.FindVisualChild<Header>(ContentPresenterObj);
                    if (header == null) continue;
                    string content = header.ItemName;


                  
                     
                        ErdLineConnector er = this.Connections[this.Connections.Count-1] as ErdLineConnector;
                        if (er.HeadNodeName == content)
                        {
                            er.HeadNode = n;
                            
                        }
                    if (er.TailNodeName == content)
                    {
                        er.TailNode = n;
                        
                    }
                        er.ApplyTemplate();
                    
                }
            }
        }

        #endregion
    }
}
