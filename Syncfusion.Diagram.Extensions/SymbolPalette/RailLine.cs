using Syncfusion.Windows.Diagram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Syncfusion.Diagram.Extensions.SymbolPalette
{
    public class RailLine : RailBase
    {
        /// <summary>
        /// The drawing group contains the additional graphic obj to draw with the line connector
        /// </summary>
        private readonly DrawingGroup m_drawingGroup;

        public RailLine()
        {
            this.ConnectorType = ConnectorType.Straight;
            //this.LineStyle.Fill = Brushes.Transparent;
            //this.LineStyle.Stroke = Brushes.Transparent;

            this.m_drawingGroup = new DrawingGroup();

        }

        /// <summary>
        /// Called whenever the head node, tail node or position of the node is changed.
        /// </summary>

        protected override void OnRender(DrawingContext drawingContext)
        {
            // add drawing group to line connector
            drawingContext.DrawDrawing(this.m_drawingGroup);
            base.OnRender(drawingContext);
        }


    }
}
