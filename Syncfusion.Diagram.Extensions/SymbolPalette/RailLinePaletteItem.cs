using Syncfusion.Windows.Diagram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Syncfusion.Diagram.Extensions.SymbolPalette
{
    public class RailLinePaletteItem : SymbolPaletteItem
    {
        public override object CloneContent()
        {
            ErdLineConnector connector = new ErdLineConnector();
            connector.StartPointPosition = new Point(300, 500);
            connector.EndPointPosition = new Point(300, 700);
            return connector;
          
            
        }
    }
}
