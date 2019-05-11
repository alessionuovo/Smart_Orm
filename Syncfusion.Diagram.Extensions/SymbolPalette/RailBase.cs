using Syncfusion.Windows.Diagram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Syncfusion.Diagram.Extensions.SymbolPalette
{
    public class RailBase : ErdLineConnector
    {
        private readonly Guid m_customId;

        public RailBase()
        {
            this.m_customId = Guid.NewGuid();
            this.LinkedNodes = new CollectionExt();
            this.HeadDecoratorShape = DecoratorShape.None;
            this.TailDecoratorShape = DecoratorShape.None;
        }

        public Guid CustomId
        {
            get { return m_customId; }
        }

        public CollectionExt LinkedNodes
        {
            get { return (CollectionExt)GetValue(LinkedNodesProperty); }
            set { SetValue(LinkedNodesProperty, value); }
        }

        public static readonly DependencyProperty LinkedNodesProperty =
            DependencyProperty.Register("LinkedNodes", typeof(CollectionExt), typeof(RailBase), new PropertyMetadata(null));
    }
}
