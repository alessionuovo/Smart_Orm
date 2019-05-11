using Syncfusion.Windows.Diagram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Utilities;

namespace Syncfusion.Diagram.Extensions
{
    /// <summary>
    /// \ingroup Pl
    /// This is extension of Syncfusion Diagram Control
    /// The extension allows data binding for connector
    /// and adds properties that are specific to ER Diagram
    /// if you want to understand the control better
    /// then go to https://www.syncfusion.com/products/wpf/diagram
    /// 
    /// </summary>
    public class ErdDiagramControl:DiagramControl
    {
        public ErdDiagramControl():base()
        {
            Loaded += ErdDiagramControl_Loaded;
            EditState = false;
        }

        public bool AddState { get; set; }
        public bool EditState { get; set; }
        public int NumItemsEdited { get; set; }

        private void ErdDiagramControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ErdDiagramModel model = this.Model as ErdDiagramModel;
            foreach (Node n in model.Nodes)
            {
                ContentPresenter ContentPresenterObj = WpfUIUtilities.FindVisualChild<ContentPresenter>(n);

                // call FindName on the DataTemplate of that ContentPresenter
                DataTemplate DataTemplateObj = ContentPresenterObj.ContentTemplate;

                // Label Chk = (Label)DataTemplateObj.FindName("lbl", ContentPresenterObj);
                Header header = (Header)WpfUIUtilities.FindVisualChild<Header>(ContentPresenterObj);
                /* Panel pnl = n.ContentTemplate.LoadContent() as Panel;
                 if (pnl == null) throw new Exception("The first element of template must be panel");
                 Header header = pnl.Children[0] as Header;*/
                if (header == null) throw new Exception("The first element of template must be header");
                string content = header.ItemName;

               
                foreach (var ee in model.Connections)
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
}
