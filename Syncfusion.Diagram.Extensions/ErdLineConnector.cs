using Syncfusion.Diagram.Extensions.Positioning;
using Syncfusion.Windows.Diagram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Syncfusion.Diagram.Extensions
{
    /// <summary>
    /// Shape and style that show the cardinality of relation
    /// </summary>
    struct Decorator
    {
        public DecoratorShape shape { get; set; }
        public DecoratorStyle style { get; set; }
    }
    /// <summary>
    /// Extends syncfusion Connector in order to support 
    /// table names, and table relation properties from binding
    /// More info about the base class in https://www.syncfusion.com/products/wpf/diagram
    /// </summary>
    public class ErdLineConnector : LineConnector
    {
        #region Variables
        /// <summary>
        /// Name of table
        /// </summary>
        public static readonly DependencyProperty HeadNodeNameProperty
            = DependencyProperty.Register("HeadNodeName", typeof(string), typeof(ErdLineConnector),
                                          new FrameworkPropertyMetadata((string)null,
                                                                     new PropertyChangedCallback(OnHeadNodeNameChanged)));
        public ErdLineConnector():base()
        {
           this.ConnectorType = ConnectorType.Arc;
            this.VerticalAlignment = VerticalAlignment.Bottom;
            /*this.LabelOrientation = LabelOrientation.Horizontal;
            this.Width = 10;
            //this.ConnectorType = ConnectorType.Orthogonal;
            this.HeadDecoratorShape = DecoratorShape.Arrow;
            this.TailDecoratorShape = DecoratorShape.None;
            this.LineStyle.Fill = Brushes.Blue;
            this.LineStyle.Stroke = Brushes.Blue;
            this.LineStyle.StrokeThickness = 2.0;
            this.LabelAngle = 90;
           // this.StartPointPosition = new Point(50, 50);
           // this.EndPointPosition = new Point(80, 50);*/
            this.ConnectionEndSpace = 0;
            
            this.MouseLeftButtonDown += ErdLineConnector_MouseLeftButtonDown;
        }

        private void ErdLineConnector_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.IsSelected = false;
        }

        public string HeadNodeName
        {
            get { return (string)GetValue(HeadNodeNameProperty); }
            set
            {

                SetValue(HeadNodeNameProperty, value);

            }
        }

        /// <summary>
        /// Name of table
        /// </summary>
        public static readonly DependencyProperty TailNodeNameProperty
            = DependencyProperty.Register("TailNodeName", typeof(string), typeof(ErdLineConnector),
                                          new FrameworkPropertyMetadata((string)null,
                                                                     new PropertyChangedCallback(OnTailNodeNameChanged)));

    

        public string TailNodeName
        {
            get { return (string)GetValue(TailNodeNameProperty); }
            set
            {

                SetValue(TailNodeNameProperty, value);

            }
        }
        /// <summary>
        /// Relation Cardinality
        /// </summary>
        public static readonly DependencyProperty HeadNodeRelationProperty
            = DependencyProperty.Register("HeadNodeRelation", typeof(string), typeof(ErdLineConnector),
                                          new FrameworkPropertyMetadata((string)null,
                                                                     new PropertyChangedCallback(OnHeadNodeRelationChanged)));

       

        public string HeadNodeRelation
        {
            get { return (string)GetValue(HeadNodeRelationProperty); }
            set
            {

                SetValue(HeadNodeRelationProperty, value);

            }
        }

        public static readonly DependencyProperty TailNodeRelationProperty
            = DependencyProperty.Register("TailNodeRelation", typeof(string), typeof(ErdLineConnector),
                                          new FrameworkPropertyMetadata((string)null,
                                                                     new PropertyChangedCallback(OnTailNodeRelationChanged)));

      
        /// <summary>
        /// Relation Cardinality
        /// </summary>
        public string TailNodeRelation
        {
            get { return (string)GetValue(TailNodeRelationProperty); }
            set
            {

                SetValue(TailNodeRelationProperty, value);

            }
        }

        public static readonly DependencyProperty ConnectorNameProperty
          = DependencyProperty.Register("ConnectorName", typeof(string), typeof(ErdLineConnector),
                                        new FrameworkPropertyMetadata((string)null,
                                                                   new PropertyChangedCallback(OnConnectorsNameChanged)));

     

        public string ConnectorName
        {
            get { return (string)GetValue(ConnectorNameProperty); }
            set
            {

                SetValue(ConnectorNameProperty, value);

            }
        }
        #endregion

      


        private static void OnHeadNodeNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }

        private static void OnTailNodeNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }

        private static void OnHeadNodeRelationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ErdLineConnector lineconnector = d as ErdLineConnector;
            Decorator decorator = GetShape(lineconnector, e.NewValue.ToString());
            lineconnector.HeadDecoratorShape = decorator.shape;
            lineconnector.HeadDecoratorStyle = decorator.style;
            if (lineconnector.HeadNode!=null && lineconnector.TailNode!=null)
            {
                RegularConnectorPositionStrategy r = new RegularConnectorPositionStrategy();
                r.headNodeHeigth = lineconnector.HeadNode.ActualHeight;
                r.headNodeWidth = lineconnector.HeadNode.ActualWidth;
                r.headNodePosition = new Point(lineconnector.HeadNode.OffsetX, lineconnector.HeadNode.OffsetY);
                r.tailNodeHeight = lineconnector.TailNode.ActualHeight;
                r.tailNodeWidth = lineconnector.TailNode.ActualWidth;
                r.tailNodePosition = new Point(lineconnector.TailNode.OffsetX, lineconnector.TailNode.OffsetY);
                KeyValuePair<Point, Point> kvp = r.GetPosition();
                lineconnector.StartPointPosition = kvp.Key;
                lineconnector.EndPointPosition = kvp.Value;
            }
           // lineconnector.TailDecoratorPosition = new Point(lineconnector.HeadNode.Position.X, lineconnector.HeadNode.Position.Y);
        }

        private static void OnTailNodeRelationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ErdLineConnector lineconnector = d as ErdLineConnector;
            Decorator decorator = GetShape(lineconnector, e.NewValue.ToString());
            lineconnector.TailDecoratorShape = decorator.shape;
            lineconnector.TailDecoratorStyle = decorator.style;
            if (lineconnector.HeadNode != null && lineconnector.TailNode != null)
            {
                RegularConnectorPositionStrategy r = new RegularConnectorPositionStrategy();
                r.headNodeHeigth = lineconnector.HeadNode.ActualHeight;
                r.headNodeWidth = lineconnector.HeadNode.ActualWidth;
                r.headNodePosition = new Point(lineconnector.HeadNode.OffsetX, lineconnector.HeadNode.OffsetY);
                r.tailNodeHeight = lineconnector.TailNode.ActualHeight;
                r.tailNodeWidth = lineconnector.TailNode.ActualWidth;
                r.tailNodePosition = new Point(lineconnector.TailNode.OffsetX, lineconnector.TailNode.OffsetY);
                KeyValuePair<Point, Point> kvp = r.GetPosition();
                lineconnector.StartPointPosition = kvp.Key;
                lineconnector.EndPointPosition = kvp.Value;
            }
            //lineconnector.TailDecoratorPosition = new Point(lineconnector.TailNode.Position.X, lineconnector.TailNode.Position.Y);
        }

        private static Decorator GetShape(ErdLineConnector lineconnector, string relationval)
        {
            Decorator d = new Decorator();
            d.shape = DecoratorShape.Circle;
            d.style = new DecoratorStyle();
            switch(relationval.ToUpper())
            {
                case "ONE":  d.style.Fill = new SolidColorBrush(Colors.White);
                             break;
                case "MANY":
                    d.style.Fill = new SolidColorBrush(Colors.Aqua);
                    break;
                case "INHERITANCE":
                    d.shape = DecoratorShape.Arrow;
                     break;
                default: break;
            }
            return d;
        }

        private static void OnConnectorsNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }
    }
}
