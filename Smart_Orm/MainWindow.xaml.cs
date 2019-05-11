using DiagramContent;
using DiagramManagement;
using Microsoft.Win32;
using Smart_Orm.DiagramResources;
using Syncfusion.Diagram.Extensions;
using Syncfusion.Diagram.Extensions.Positioning;
using Syncfusion.Diagram.Extensions.SymbolPalette;
using Syncfusion.Windows.Diagram;
using Syncfusion.Windows.Forms.Tools.Navigation;
using Syncfusion.Windows.Shared;
using Syncfusion.Windows.Tools.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
//using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Utilities;

namespace Smart_Orm
{
    public class ObservableCollectionPort: System.Collections.ObjectModel.ObservableCollection<ConnectionPort>
    {

    }
    /// <summary>
    /// This class is form with options of
    /// building erd diagram model
    /// and generating classes that can work 
    /// with the db
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Attached Properteis

        public static Style GetCustomPathStyle(DependencyObject obj)
        {
            return (Style)obj.GetValue(CustomPathStyleProperty);
        }

        public static void SetCustomPathStyle(DependencyObject obj, Style value)
        {
            obj.SetValue(CustomPathStyleProperty, value);
        }

        // Using a DependencyProperty as the backing store for CustomPathStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CustomPathStyleProperty =
            DependencyProperty.RegisterAttached("CustomPathStyle", typeof(Style), typeof(MainWindow), new UIPropertyMetadata(null, OnCustomPathStyleChanged));

        private static void OnCustomPathStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        //For Customizing the Label
        public static TextStyles GetTextStyles(DependencyObject obj)
        {
            return (TextStyles)obj.GetValue(TextStylesProperty);
        }

        public static void SetTextStyles(DependencyObject obj, TextStyles value)
        {
            obj.SetValue(TextStylesProperty, value);
        }

        // Using a DependencyProperty as the backing store for TextStyles.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextStylesProperty =
            DependencyProperty.RegisterAttached("TextStyles", typeof(TextStyles), typeof(MainWindow));

        //For Customizing the Node
        public static ShapeStyles GetShapeStyle(DependencyObject obj)
        {
            return (ShapeStyles)obj.GetValue(ShapeStyleProperty);
        }

        public static void SetShapeStyle(DependencyObject obj, ShapeStyles value)
        {
            obj.SetValue(ShapeStyleProperty, value);
            int i = 0;

        }

        public static readonly DependencyProperty ShapeStyleProperty =
            DependencyProperty.RegisterAttached("ShapeStyle", typeof(ShapeStyles), typeof(MainWindow));

        public static ShapeStyles GetPreviewShapeStyle(DependencyObject obj)
        {

            return (ShapeStyles)obj.GetValue(PreviewShapeStyleProperty);
        }

        //For Customizing the Node while Mouse Move
        public static void SetPreviewShapeStyle(DependencyObject obj, ShapeStyles value)
        {
            obj.SetValue(PreviewShapeStyleProperty, value);
        }

        public static readonly DependencyProperty PreviewShapeStyleProperty =
            DependencyProperty.RegisterAttached("PreviewShapeStyle", typeof(ShapeStyles), typeof(MainWindow));

        //For customizing the Dropped Node from Symbolpalette
        public static Geometry GetData(DependencyObject obj)
        {
            return (Geometry)obj.GetValue(DataProperty);
        }

        public static void SetData(DependencyObject obj, Geometry value)
        {
            obj.SetValue(DataProperty, value);
            
        }

        // Using a DependencyProperty as the backing store for Data.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.RegisterAttached("Data", typeof(Geometry), typeof(MainWindow), new UIPropertyMetadata(null));

        #endregion

        #region Private Properties

        System.Windows.Forms.SaveFileDialog m_SaveFileDialog;
        ShapeStyles m_actualShapeStyles;
        ShapeStyles m_previewShapeStyles;
        TextStyles m_actualTextStyles;
        Style selectedPageTheme;

        #endregion

        #region Public Variables

        private FrameworkElement m_elementToPrint;

        private PrintDialog m_nativePrintDialog = new PrintDialog();

        private VisualBrush m_visualBrush;
        ChildWindow page;
        #endregion

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
            lstGenerationInfo.Items.Add("During this run no generations occurred");
            ErdDiagramBuilder.IsSymbolPaletteEnabled = true;
            ErdDiagramBuilder.NumItemsEdited = 0;
            //collection changed event for Node
            ErdDiagramModel.Nodes.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Nodes_CollectionChanged);
            InitializeDiagramProperties();
            InitializeSymbolPalette();
            ErdControlDiagramView.ZoomFactor = 0.2;
            //Defines the nodes and adds them to the model.
            // createNodes();
            ErdControlDiagramView.ObjectDrawn += new DrawingToolEventHandler(ErdControlDiagramView_ObjectDrawn);
            //Specifies the events.
            Nodeevents();
            ConnectorEvents();
            DependencyPropertyDescriptor prop = DependencyPropertyDescriptor.FromProperty(DiagramView.IsPageSavedProperty, typeof(DiagramView));
            prop.AddValueChanged(ErdControlDiagramView, OnIsPageSavedChanged);
            InitalizePage();
            
            // MainWindow.SetCustomPathStyle(ErdDiagramBuilder, this.Resources["CustomStyle"] as Style);

        }

        #region Node

        //Collection changed event for customizng the Dropped Node from SYmbolPalete
        void Nodes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                (e.NewItems[0] as Node).Loaded += new RoutedEventHandler(Node1_Loaded);
            }
        }

        //Loaded event for the Node
        void Node1_Loaded(object sender, RoutedEventArgs e)
        {
            Node node = sender as Node;
            if (node.GetType() != typeof(Group))
            {
                System.Windows.Shapes.Path p = node.Template.FindName("PART_Shape", node) as System.Windows.Shapes.Path;
                Binding shapeDataBind = new Binding();
                shapeDataBind.Path = new PropertyPath(MainWindow.DataProperty);
                shapeDataBind.RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(Node), 1);
                shapeDataBind.Mode = BindingMode.TwoWay;
                if (p.Data != null)
                {
                    //if (node.Tag == "IsFirstLoaded")
                    MainWindow.SetData(node, p.Data);
                }
                p.SetBinding(System.Windows.Shapes.Path.DataProperty, shapeDataBind);
            }

        }

        //m_actualTextStyles_PropertyChanged
        void m_actualTextStyles_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (ErdControlDiagramView.SelectionList != null && ErdControlDiagramView.SelectionList.Count > 0)
            {
                foreach (Node n in ErdControlDiagramView.SelectionList.OfType<Node>())
                {
                    MainWindow.SetTextStyles(n as Node, m_actualTextStyles.Clone());
                }
            }
        }

        //m_previewShapeStyles_PropertyChanged
        void m_previewShapeStyles_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PaintSelection(StyleStatus.Preview);
        }

        //m_currentShapeStyles_PropertyChanged
        void m_currentShapeStyles_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PaintSelection(StyleStatus.Ok);
        }

        //Craeting the Nodes
        public void createNodes()
        {
            //Defines the nodes and adds them to the model.
            Node NewClient = addNode("NewClient", 205, 100, 0, Shapes.FlowChart_Decision, "New Client", "FlowChart_Decision", 150, 50, "SubtleEffectPurpleStyle");
            Node Register = addNode("Register", 435, 100, 1, Shapes.RoundedSquare, "Register", "RoundedSquare", 150, 50, "SubtleEffectOrangeStyle");
            Node ClientAccountInfo = addNode("ClientAccountInfo", 650, 150, 3, Shapes.FlowChart_Card, "Client Account Info", "FlowChart_Card", 150, 50, "SubtleEffectAquaStyle");
            Node End = addNode("End", 850, 200, 4, Shapes.Ellipse, "End", "Ellipse", 100, 60, "SubtleEffectBlackStyle");
            Node DataEntry = addNode("DataEntry", 180, 300, 3, Shapes.FlowChart_Card, "Data Entry", "FlowChart_Card", 200, 60, "SubtleEffectBlueStyle");
            Node Review = addNode("Review", 675, 300, 2, Shapes.FlowChart_PaperTape, "Review", "FlowChart_PaperTape", 100, 60, "SubtleEffectOliveGreenStyle");

            //Creates the connections for the nodes.
            Connect(ClientAccountInfo, Register, DecoratorShape.None, DecoratorShape.None);
            Connect(NewClient, Register, DecoratorShape.Circle, DecoratorShape.Circle);
            Connect(ClientAccountInfo, Review, DecoratorShape.Arrow, DecoratorShape.Arrow);
            Connect(NewClient, DataEntry, DecoratorShape.Diamond, DecoratorShape.Diamond);
            Connect(DataEntry, Review, DecoratorShape.None, DecoratorShape.Diamond);
            Connect(ClientAccountInfo, End, DecoratorShape.None, DecoratorShape.Arrow);

        }

        //Specifies the Nodeevents and Group related Events.
        void Nodeevents()
        {
            ErdControlDiagramView.NodeClick += new NodeEventHandler(diagramView_NodeClick);
            ErdControlDiagramView.NodeDoubleClick += new NodeEventHandler(diagramView_NodeDoubleClick);
            ErdControlDiagramView.NodeStartLabelEdit += new LabelChangedEventHandler(diagramView_NodeStartLabelEdit);
            ErdControlDiagramView.NodeLabelChanged += new LabelChangedEventHandler(diagramView_NodeLabelChanged);
            ErdControlDiagramView.NodeDragStart += new NodeEventHandler(diagramView_NodeDragStart);
            ErdControlDiagramView.NodeDragEnd += new NodeEventHandler(diagramView_NodeDragEnd);
            ErdControlDiagramView.NodeDrop += new NodeDroppedEventHandler(diagramView_NodeDrop);
            ErdControlDiagramView.PreviewNodeDrop += new PreviewNodeDropEventHandler(ErdControlDiagramView_PreviewNodeDrop);
            ErdControlDiagramView.NodeResizing += new NodeEventHandler(diagramView_NodeResizing);
            ErdControlDiagramView.NodeResized += new NodeEventHandler(diagramView_NodeResized);
            ErdControlDiagramView.NodeRotationChanging += new NodeEventHandler(diagramView_NodeRotationChanging);
            ErdControlDiagramView.NodeRotationChanged += new NodeEventHandler(diagramView_NodeRotationChanged);
            ErdControlDiagramView.NodeSelected += new NodeEventHandler(diagramView_NodeSelected);
            ErdControlDiagramView.NodeUnSelected += new NodeEventHandler(diagramView_NodeUnSelected);
            ErdControlDiagramView.NodeDeleting += new NodeDeleteEventHandler(diagramView_NodeDeleting);
            ErdControlDiagramView.NodeDeleted += new NodeDeleteEventHandler(diagramView_NodeDeleted);
            ErdControlDiagramView.Grouped += new GroupEventHandler(diagramView_Grouped);
            ErdControlDiagramView.Grouping += new GroupEventHandler(diagramView_Grouping);
            ErdControlDiagramView.Ungrouping += new UnGroupEventHandler(diagramView_Ungrouping);
            ErdControlDiagramView.Ungrouped += new UnGroupEventHandler(diagramView_Ungrouped);
            ErdControlDiagramView.NodeMoved += new NodeNudgeEventHandler(diagramView_NodeMoved);
            ErdControlDiagramView.NodeDragging += new NodeEventHandler(ErdControlDiagramView_NodeDragging);
            ErdDiagramBuilder.DataContext = Structure.Struct;
            this.DataContext=Structure.Struct;
            //Connector_Properties.DataContext = Structure.Struct;
        }



        #region Customiztion of Node

        #region ShapeStyle

        //Apply Gradiet color to the Node
        private void applyGradient_Click(object sender, RoutedEventArgs e)
        {
            Button gradientButton = sender as Button;
            m_actualShapeStyles.Fill = (gradientButton.Content as Rectangle).Fill;
        }

        //Customizing the DashArray property of the Node
        private void dashArray_Click(object sender, RoutedEventArgs e)
        {
            DropDownMenuItem dropMenuItem = sender as DropDownMenuItem;
            Image imageTag = dropMenuItem.Header as Image;
            Shape originalShape = imageTag.Tag as Shape;
            m_actualShapeStyles.StrokeDashArray = originalShape.StrokeDashArray;
            m_actualShapeStyles.StrokeDashCap = originalShape.StrokeDashCap;
            foreach (ICommon shape in ErdControlDiagramView.SelectionList)
            {
                if (shape is LineConnector)
                {
                    (shape as LineConnector).LineStyle.StrokeDashArray = m_actualShapeStyles.StrokeDashArray;
                    (shape as LineConnector).LineStyle.StrokeEndLineCap = m_actualShapeStyles.StrokeDashCap;
                    (shape as LineConnector).LineStyle.StrokeStartLineCap = m_actualShapeStyles.StrokeDashCap;
                }
            }
        }

        //Customizing the StokeThickness of the Node
        private void strokeWeight_Click(object sender, RoutedEventArgs e)
        {
            DropDownMenuItem dropMenuItem = sender as DropDownMenuItem;
            Image imageTag = dropMenuItem.Header as Image;
            Shape originalShape = imageTag.Tag as Shape;
            m_actualShapeStyles.StrokeThickness = originalShape.StrokeThickness;
            foreach (IShape shape in ErdControlDiagramView.SelectionList)
            {
                if (shape is LineConnector)
                {
                    if ((shape as LineConnector).IsSelected)
                        (shape as LineConnector).LineStyle.StrokeThickness = originalShape.StrokeThickness;
                }
            }
        }

        //Customizing the Fill of the Node
        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle imageBrush = sender as Rectangle;
            ImageBrush img = imageBrush.Fill as ImageBrush;
            img.TileMode = TileMode.Tile;
            m_actualShapeStyles.Fill = img;
        }

        //Customizing the Fill of the Node using ShapeStyles
        private void gradientColor_ColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NodeCollection diagramNodes = ErdControlDiagramView.SelectionList;
            ColorEdit ce = d as ColorEdit;
            BrushConverter brushConverter = new BrushConverter();
            foreach (Node diagramNode in diagramNodes.OfType<Node>())
            {
                System.Windows.Shapes.Path shapePath = (System.Windows.Shapes.Path)diagramNode.Template.FindName("PART_Shape", diagramNode);
                m_actualShapeStyles.Fill = ce.Brush;//brushConverter.ConvertFromString(e.NewValue.ToString()) as SolidColorBrush;
            }
        }


        //Customizing the Stroke of the Node
        private void ShapeStrokeColor_ColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NodeCollection diagramNodes = ErdControlDiagramView.SelectionList;
            BrushConverter brushConverter = new BrushConverter();
            foreach (Node diagramNode in diagramNodes.OfType<Node>())
            {
                System.Windows.Shapes.Path shapePath = (System.Windows.Shapes.Path)diagramNode.Template.FindName("PART_Shape", diagramNode);
                m_actualShapeStyles.Stroke = brushConverter.ConvertFromString(e.NewValue.ToString()) as SolidColorBrush;
            }
        }

        //Apply GlowEffects to the Node
        private void glowEffects_Click(object sender, RoutedEventArgs e)
        {
            Button glowContent = sender as Button;
            Image glowInformation = glowContent.Content as Image;
            DropShadowEffect glowEffect = glowInformation.Tag as DropShadowEffect;
            m_actualShapeStyles.Effect = glowEffect;
        }

        //Apply Shadow to the Node
        private void shadowEffects_Click(object sender, RoutedEventArgs e)
        {
            Button shadowContent = sender as Button;
            Image shadowInformation = shadowContent.Content as Image;
            DropShadowEffect shadowEffect = shadowInformation.Tag as DropShadowEffect;
            m_actualShapeStyles.Effect = shadowEffect;
        }
        #endregion

        #region Preview Shape Style

        //Apply selected Custom values to the Node while Mouse Hover
        private void RibbonGalleryItem_MouseEnter(object sender, MouseEventArgs e)
        {
            System.Windows.Shapes.Path gallery = sender as System.Windows.Shapes.Path;
            if (gallery.Tag.ToString().Equals("Default"))
            {
                PaintSelection(StyleStatus.Clear);
            }
            else
            {
                m_previewShapeStyles.Fill = gallery.Fill;
                m_previewShapeStyles.Stroke = gallery.Stroke;
                m_previewShapeStyles.StrokeThickness = gallery.StrokeThickness;
                m_previewShapeStyles.Effect = gallery.Effect;
            }
        }

        //Apply selected Custom values to the Node
        private void ShapeStyle_Selected(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Shapes.Path path = sender as System.Windows.Shapes.Path;
            m_actualShapeStyles.Fill = path.Fill;
            m_actualShapeStyles.Stroke = path.Stroke;
            m_actualShapeStyles.StrokeThickness = path.StrokeThickness;
            m_actualShapeStyles.Effect = path.Effect;
        }

        //Apply Gradiet color to the Node
        private void applyGradient_MouseEnter(object sender, MouseEventArgs e)
        {
            Button gradientColor = sender as Button;
            m_previewShapeStyles.Fill = (gradientColor.Content as Rectangle).Fill;
        }

        //Customizing the StokeThickness of the Node
        private void strokeWeight_MouseEnter(object sender, MouseEventArgs e)
        {
            DropDownMenuItem dropDownMenuItem = sender as DropDownMenuItem;
            Image imageContent = dropDownMenuItem.Header as Image;
            Line lineContent = imageContent.Tag as Line;
            m_previewShapeStyles.StrokeThickness = lineContent.StrokeThickness;
        }

        //Customizing the DashArray property of the Node
        private void dashArray_MouseEnter(object sender, MouseEventArgs e)
        {
            DropDownMenuItem dropDownMenuItem = sender as DropDownMenuItem;
            Image imageContent = dropDownMenuItem.Header as Image;
            Line lineContent = imageContent.Tag as Line;
            m_previewShapeStyles.StrokeDashCap = lineContent.StrokeDashCap;
            m_previewShapeStyles.StrokeDashArray = lineContent.StrokeDashArray;
        }

        //Customizing the Stroke of the Node
        private void ShapeStrokeColor_MouseEnter(object sender, MouseEventArgs e)
        {
            BrushConverter brushConverter = new BrushConverter();
            ColorPickerPalette palatte = sender as ColorPickerPalette;
            m_previewShapeStyles.Stroke = brushConverter.ConvertFromString(palatte.Color.ToString()) as SolidColorBrush;
        }

        //Apply Shadow to the Node
        private void shadowEffects_MouseEnter(object sender, MouseEventArgs e)
        {
            Button shadowContent = sender as Button;
            Image shadowInformation = shadowContent.Content as Image;
            DropShadowEffect shadowEffect = shadowInformation.Tag as DropShadowEffect;
            m_previewShapeStyles.Effect = shadowEffect;
        }

        //Apply GlowEffects to the Node
        private void glowEffects_MouseEnter(object sender, MouseEventArgs e)
        {
            Button shadowContent = sender as Button;
            Image shadowInformation = shadowContent.Content as Image;
            DropShadowEffect shadowEffect = shadowInformation.Tag as DropShadowEffect;
            m_previewShapeStyles.Effect = shadowEffect;
        }

        //Apply Shadow to the Node
        private void softEdges_MouseEnter(object sender, MouseEventArgs e)
        {
            Button softEdge = sender as Button;
            Image softEdgeImage = softEdge.Content as Image;
            BlurEffect blurEffect = softEdgeImage.Tag as BlurEffect;
            m_previewShapeStyles.Effect = blurEffect;
        }

        #endregion

        #endregion

        #region Update the Values in Sample

        /// <summary>
        /// Apply the changed Color in the Color Picker Palette
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void ColorPickerPalette_ColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!darkVariation.IsVisible || !lightVariation.IsVisible)
            {
                darkVariation.Visibility = Visibility.Visible;
                lightVariation.Visibility = Visibility.Visible;
            }
            NodeCollection diagramNodes = ErdControlDiagramView.SelectionList;
            BrushConverter brushConverter = new BrushConverter();
            foreach (Node diagramNode in diagramNodes.OfType<Node>())
            {
                System.Windows.Shapes.Path shapePath = (System.Windows.Shapes.Path)diagramNode.Template.FindName("PART_Shape", diagramNode);
                m_actualShapeStyles.Fill = brushConverter.ConvertFromString(e.NewValue.ToString()) as SolidColorBrush;
            }

            UpdateVariation(d as ColorPickerPalette);
            try
            {
                foreach (Object o in ErdControlDiagramView.SelectionList)
                {
                    if (o is LineConnector)
                    {
                        (o as LineConnector).LineStyle.Stroke = brushConverter.ConvertFromString(e.NewValue.ToString()) as SolidColorBrush;
                    }
                }
            }
            catch
            {
            }
        }

        private void UpdateVariation(ColorPickerPalette colorPicker)
        {
            if (FindColor(colorPicker.Color) < 1.0)
            {
                darkVariation.Visibility = Visibility.Collapsed;
            }
            else if (FindColor(colorPicker.Color) > 1.5)
            {
                lightVariation.Visibility = Visibility.Collapsed;
            }
        }

        private void softEdges_Click(object sender, RoutedEventArgs e)
        {
            Button softEdge = sender as Button;
            Image softEdgeImage = softEdge.Content as Image;
            BlurEffect blurEffect = softEdgeImage.Tag as BlurEffect;
            m_actualShapeStyles.Effect = blurEffect;
        }
        #endregion

        #endregion

        // Initialize the diagram page properties.
        private void InitalizePage()
        {
            ErdControlDiagramView.BoundaryConstraintsArea = new Rect(50, 50, 1030, 1190);
            ErdControlDiagramView.SizeToContent = false;
            ErdControlDiagramView.PageBackground = new SolidColorBrush(Colors.White);
            ErdControlDiagramView.OffPageBackground = new SolidColorBrush(Colors.LightSteelBlue);
            InitializeFontComboBox(fontNameBox);
            InitializeFontSizeBox(fontSizeBox);
            InitializeGallary();
            UpdateVariation(shapeFillColor);

            MainWindow.SetCustomPathStyle(ErdDiagramBuilder, this.Resources["CustomStyle"] as Style);
            selectedPageTheme = this.Resources["CustomStyle"] as Style;
        }

        void ErdControlDiagramView_ObjectDrawn(object sender, DrawingToolEventArgs evtArgs)
        {
            if (evtArgs.DrawingObject is Node)
            {
                (evtArgs.DrawingObject as Node).Tag = "IsFirstLoaded";
            }
        }
        private void InitializeDiagramProperties()
        {
            DiagramProperties m_DiagramProperties;
            m_actualShapeStyles = new ShapeStyles();
            m_actualTextStyles = new TextStyles();
            m_previewShapeStyles = new ShapeStyles() { IsPreview = true };
            m_actualShapeStyles.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(m_currentShapeStyles_PropertyChanged);
            m_previewShapeStyles.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(m_previewShapeStyles_PropertyChanged);
            m_actualTextStyles.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(m_actualTextStyles_PropertyChanged);
            m_DiagramProperties = new DiagramProperties
            {
                SymbolPaletteEnable = true,
                HorizantalRulers = true,
                VerticalRulers = true,
                HorizantalLines = false,
                VerticalLines = false,
                ZoomEnabled = true,
                PageEdit = true,
                PanEnabled = false
            };
            this.DataContext = m_DiagramProperties;
        }

        private void InitializeSymbolPalette()
        {
            var group = new SymbolPaletteGroup() { Label = "Erd Items" };
           SymbolPalette.SetFilterIndexes(group, new Int32Collection(new int[] { 0, 6 }));
            ErdDiagramBuilder.SymbolPalette.SymbolGroups.Clear();
            ErdDiagramBuilder.SymbolPalette.SymbolFilters.Clear();
            ErdDiagramBuilder.SymbolPalette.SymbolFilters.Add(new SymbolPaletteFilter() { Label = "Erd Items" });
            //ErdDiagramBuilder.SymbolPalette.SymbolGroups[1].Items[0] = new RailLinePaletteItem();

            EntityPaletteItem epiTableItem = new EntityPaletteItem();
            //sp.ItemName = "PART_Hexagon";
            var entityshape = this.FindResource("EntityShapeImage") as Image;
            epiTableItem.Content = entityshape;

            epiTableItem.ToolTip = "Database Table";
            group.Items.Add(epiTableItem);
           
            
            #region RailLine Item
            int i = 0;
            
            var railLineItem = new RailLinePaletteItem { };
            railLineItem.ItemName = "PART_Orthogonal";
                      
            var railLineShape = this.FindResource("ConnectionShapeImage") as Image;
          
            if (railLineShape != null)
            {
              
                railLineItem.Content = railLineShape;
                railLineItem.ToolTip = "Database Connection";
                group.Items.Add(railLineItem);
               // ErdDiagramBuilder.SymbolPalette.SymbolFilters.Add(new SymbolPaletteFilter() { Label = "Erd Items" });
                this.ErdDiagramBuilder.SymbolPalette.SymbolGroups.Add(group);
              



            }

            #endregion

        }

      

        #endregion

        #region LineConnector

        //Specifies the LineConnector related Events.
        private void ConnectorEvents()
        {
            ErdControlDiagramView.ConnectorStartLabelEdit += new LabelEditConnChangedEventHandler(diagramView_ConnectorStartLabelEdit);
            ErdControlDiagramView.ConnectorDoubleClick += new ConnChangedEventHandler(diagramView_ConnectorDoubleClick);
            ErdControlDiagramView.ConnectorLabelChanged += new LabelConnChangedEventHandler(diagramView_ConnectorLabelChanged);
            ErdControlDiagramView.ConnectorDragStart += new ConnDragChangedEventHandler(diagramView_ConnectorDragStart);
            ErdControlDiagramView.ConnectorDragEnd += new ConnDragEndChangedEventHandler(diagramView_ConnectorDragEnd);
            ErdControlDiagramView.HeadNodeChanged += new NodeChangedEventHandler(diagramView_HeadNodeChanged);
            ErdControlDiagramView.TailNodeChanged += new NodeChangedEventHandler(diagramView_TailNodeChanged);
            ErdControlDiagramView.ConnectorDrop += new ConnectorDroppedEventHandler(diagramView_ConnectorDrop);
            ErdControlDiagramView.PreviewConnectorDrop += new PreviewConnectorDropEventHandler(ErdControlDiagramView_PreviewConnectorDrop);
            ErdControlDiagramView.ConnectorDeleting += new ConnectionDeleteEventHandler(diagramView_ConnectorDeleting);
            ErdControlDiagramView.ConnectorDeleted += new ConnectionDeleteEventHandler(diagramView_ConnectorDeleted);
            ErdControlDiagramView.ConnectorClick += new ConnectorRoutedEventHandler(diagramView_ConnectorClick);
            ErdControlDiagramView.ConnectorSelected += new ConnectorSelectedEventHandler(diagramView_ConnectorSelected);
            ErdControlDiagramView.ConnectorUnSelected += new ConnectorUnSelectedEventHandler(diagramView_ConnectorUnSelected);
            ErdControlDiagramView.LineMoved += new LineNudgeEventHandler(diagramView_LineMoved);
            
        }


        #region Enable Connection
        //Creates orthogonal connection
        private void Ortho_click(object sender, RoutedEventArgs e)
        {
            (ErdControlDiagramView.Page as DiagramPage).ConnectorType = ConnectorType.Orthogonal;
            ErdControlDiagramView.EnableConnection = true;
            MyRibbon.HideBackStage();
        }

        //Creates straight connection
        private void Straight_Click(object sender, RoutedEventArgs e)
        {
            (ErdControlDiagramView.Page as DiagramPage).ConnectorType = ConnectorType.Straight;
            ErdControlDiagramView.EnableConnection = true;

            MyRibbon.HideBackStage();
        }

        //Creates bezier connection
        private void Bezier_click(object sender, RoutedEventArgs e)
        {
            (ErdControlDiagramView.Page as DiagramPage).ConnectorType = ConnectorType.Bezier;
            ErdControlDiagramView.EnableConnection = true;
            MyRibbon.HideBackStage();
        }

        //Creates Arc connection
        private void Arc_Click(object sender, RoutedEventArgs e)
        {
            (ErdControlDiagramView.Page as DiagramPage).ConnectorType = ConnectorType.Arc;
            ErdControlDiagramView.EnableConnection = true;
            MyRibbon.HideBackStage();
        }


        //Create PolyLine connection
        private void Poly_Click(object sender, RoutedEventArgs e)
        {
            (this.ErdControlDiagramView.Page as DiagramPage).IsPolyLineEnabled = true;
            MyRibbon.HideBackStage();
        }
        #endregion

        #endregion

        #region Diagram View EventHandlers

        void ErdControlDiagramView_NodeDragging(object sender, NodeRoutedEventArgs evtArgs)
        {
            AddToLog(evtArgs.Node.Name.ToString(), "  Node is Dragging");
        }
        void diagramView_ConnectorUnSelected(object sender, ConnectorRoutedEventArgs evtArgs)
        {
            ErdLineConnector lcc = evtArgs.Connector as ErdLineConnector;
            
            /*if (lcc.HeadNode == null || lcc.TailNode == null)
                ErdDiagramModel.Connections.Remove(lcc);*/
            AddToLog("LineConnector ", "UnSelected");
        }
        void diagramView_ConnectorSelected(object sender, ConnectorRoutedEventArgs evtArgs)
        {
            ErdLineConnector lcc = evtArgs.Connector as ErdLineConnector;
            lcc.IsSelected = false;
            AddToLog("LineConnector ", "Selected");
        }

        void diagramView_ConnectorClick(object sender, ConnectorRoutedEventArgs evtArgs)
        {
            AddToLog("LineConnector ", "Clicked");
        }
        void diagramView_LineMoved(object sender, LineNudgeEventArgs evtArgs)
        {
            AddToLog("LineConnector", " is Moved");
        }

        void diagramView_NodeMoved(object sender, NodeNudgeEventArgs evtArgs)
        {
            AddToLog(evtArgs.Node.Name.ToString(), " is moved");
        }
        void diagramView_Ungrouped(object sender, UnGroupEventArgs evtArgs)
        {
            AddToLog(evtArgs.Group.NodeChildren.Count.ToString(), " Items are UnGrouped");
        }
        void diagramView_Ungrouping(object sender, UnGroupEventArgs evtArgs)
        {
            if (evtArgs.GroupNode != null)
            {
                AddToLog(evtArgs.GroupNode.Name.ToString(), " is UnGrouping");
            }
            if (evtArgs.GroupLineConnector != null)
            {
                AddToLog(evtArgs.GroupLineConnector.Name.ToString(), "LineConnector is UnGrouping");
            }
        }
        void diagramView_Grouping(object sender, GroupEventArgs evtArgs)
        {
            if (evtArgs.GroupNode != null)
            {
                AddToLog(evtArgs.GroupNode.Name.ToString(), " is Grouping");
            }
            if (evtArgs.GroupLineConnector != null)
            {
                AddToLog(evtArgs.GroupLineConnector.Name.ToString(), "LineConnector is Grouping");
            }
        }
        void diagramView_Grouped(object sender, GroupEventArgs evtArgs)
        {
            AddToLog(evtArgs.Group.NodeChildren.Count.ToString(), " Items are Grouped");
        }
        void diagramView_ConnectorDeleted(object sender, ConnectionDeleteRoutedEventArgs evtArgs)
        {
            AddToLog(evtArgs.DeletedLineConnector.Name.ToString(), " ConnectorDeleted event was fired.");
        }

        void diagramView_ConnectorDeleting(object sender, ConnectionDeleteRoutedEventArgs evtArgs)
        {
            ErdLineConnector lc = evtArgs.DeletedLineConnector as ErdLineConnector;

            Structure st = Structure.Struct;
            Model m = st.model;
            List<Connection> contoremove = new List<Connection>();
            foreach (Connection con in m.connections)
            {

                if (con.Name == lc.ConnectorName)
                    contoremove.Add(con);
            }
            foreach (var k in contoremove)
            {
                m.connections.Remove(k);
                ChangeManager.Manager.Add(k, State.Delete);
            }
            AddToLog(evtArgs.DeletedLineConnector.Name.ToString(), " ConnectorDeleting event was fired.");
        }

        void diagramView_NodeDeleted(object sender, NodeDeleteRoutedEventArgs evtArgs)
        {
            AddToLog(evtArgs.DeletedNode.Name.ToString(), " NodeDeleted event was fired.");
        }

        void diagramView_NodeDeleting(object sender, NodeDeleteRoutedEventArgs evtArgs)
        {
            Node n = evtArgs.DeletedNode as Node;
            Header h = ErdDiagramModel.FindHeader(n);
            string tablename = h.ItemName;
            Structure st = Structure.Struct;
            Model m = st.model;
            List<Connection> contoremove = new List<Connection>();
            foreach (Connection con in m.connections)
            {

                if (con.FirstTable == tablename || con.SecondTable == tablename)
                    contoremove.Add(con);
            }
            foreach (var k in contoremove)
            {
                m.connections.Remove(k);
                ChangeManager.Manager.Add(k, State.Delete);
            }
            DiagramContent.Table t =n.Content as DiagramContent.Table;
            m.tables.Remove(t);
            ChangeManager.Manager.Add(t, State.Delete);
            AddToLog(evtArgs.DeletedNode.Name.ToString(), " NodeDeleting event was fired.");
        }

        void diagramView_NodeUnSelected(object sender, NodeRoutedEventArgs evtArgs)
        {
            
            AddToLog(evtArgs.Node.Name.ToString(), " NodeUnSelected event was fired.");
            evtArgs.Node.GripperVisibility = Visibility.Collapsed;
        }
        Node firstSelectedNode;
        Node secondSelectedNode;
        ErdLineConnector AddedConnector;
        void diagramView_NodeSelected(object sender, NodeRoutedEventArgs evtArgs)
        {
            AddToLog(evtArgs.Node.Name.ToString(), " NodeSelected event was fired.");
           evtArgs.Node.GripperVisibility = Visibility.Visible;
            UpdatePreviewStyle(evtArgs.Node as Node);
         /*   if (firstSelectedNode == null)
                firstSelectedNode = evtArgs.Node as Node;
            else
            {
                secondSelectedNode = evtArgs.Node as Node;
                if (AddedConnector!=null)
                {
                    AddedConnector.HeadNode = firstSelectedNode;
                    AddedConnector.TailNode = secondSelectedNode;
                //    AddedConnector.ConnectionHeadPort = firstSelectedNode.Ports[firstSelectedNode.Ports.Count - 1];
                //    AddedConnector.ConnectionTailPort = secondSelectedNode.Ports[secondSelectedNode.Ports.Count - 2];
                    ErdDiagramModel.Connections.Remove(AddedConnector);
                    ErdDiagramModel.Connections.Add(AddedConnector);
                    string headNodeName = ErdDiagramModel.GetNodeName(firstSelectedNode);
                    string tailNodeName = ErdDiagramModel.GetNodeName(secondSelectedNode);
                    Connection cnt = new Connection();
                    cnt.FirstTable = headNodeName;
                    cnt.SecondTable = tailNodeName;
                    cnt.FirstRelation = "One";
                    cnt.SecondRelation = "Many";
                    
                    DiagramContent.Table head = Structure.Struct.model.tables.First(x => x.Name == headNodeName);
                    DiagramContent.Table tail = Structure.Struct.model.tables.First(x => x.Name == tailNodeName);
                    head.ConnectionProperties.Add(new ConnectionProperties() { Name = "Connection" + tailNodeName, Type = tailNodeName });
                    tail.ConnectionProperties.Add(new ConnectionProperties() { Name = "Connection" + headNodeName, Type = headNodeName });
                    firstSelectedNode.Content = head;
                    secondSelectedNode.Content = tail;
                    firstSelectedNode.Height += 40;
                    secondSelectedNode.Height += 40;
                   
                    Structure.Struct.model.connections.Add(cnt);
                    //ErdDiagramModel.SynchronizeConnector();
                    firstSelectedNode = null;
                    secondSelectedNode = null;
                 
                    
                    var connectorInitialTemplateResource = this.TryFindResource("connection-template");
                    if (connectorInitialTemplateResource != null)
                    {
                        DataTemplate dt = connectorInitialTemplateResource as DataTemplate;


                        AddedConnector.DataContext = cnt;
                        AddedConnector.LabelTemplate = dt;
                        bool k = AddedConnector.ApplyTemplate();

                        // VisualTreeHelper.
                    }
                    //  ErdDiagramModel.Connections.Add(AddedConnector);
                    AddedConnector.ApplyTemplate();
                    AddedConnector = null;
                    Node node = new Node();
                   
                }
            }*/

        }

        void diagramView_ConnectorDrop(object sender, ConnectorDroppedRoutedEventArgs evtArgs)
        {
            AddedConnector = evtArgs.DroppedConnector as ErdLineConnector;
            
            MenuItem m = new MenuItem();
            m.Click += LineConnectorUpdate_Click;
            m.Header = "Update";
           // AddedConnector.ContextMenu = new ContextMenu();
          //  AddedConnector.ContextMenu.Items.Add(m);
            ConnectorProperties cp = new ConnectorProperties();
            cp.Closed += Cp_Closed;
            cp.ShowDialog();
           
           /* var connectorInitialTemplateResource=this.TryFindResource("connector-initial-template");
            if (connectorInitialTemplateResource != null)
            {
                DataTemplate dt = connectorInitialTemplateResource as DataTemplate;
                ErdLineConnector newConnector = evtArgs.DroppedConnector as ErdLineConnector;
                var a = newConnector.LabelTemplate.LoadContent();
                newConnector.DataContext = Structure.Struct;
                newConnector.LabelTemplate = dt;
                bool k=newConnector.ApplyTemplate();
              
               // VisualTreeHelper.
            }*/
        }

        private void Cp_Closed(object sender, EventArgs e)
        {
            
           
            ConnectorProperties cp = sender as ConnectorProperties;
            if (!cp.Result)
            {
                ErdDiagramModel.Connections.Remove(ErdDiagramModel.Connections[ErdDiagramModel.Connections.Count - 1]);
                return;
            }
            ErdDiagramModel.SynchronizeConnector();
        }
        private void Cp_Update_Closed(object sender, EventArgs e)
        {


            ConnectorProperties cp = sender as ConnectorProperties;
            if (!cp.Result)
                return;
            string name = cp.GetName();
            ErdLineConnector elc = new ErdLineConnector(); ;
            foreach (var c in ErdDiagramModel.Connections)
            {
                var k = c as ErdLineConnector;
                if (k.ConnectorName==name)
                {
                    elc = k;
                    break;
                }
            }
            var conn=Structure.Struct.model.connections.Where(x => x.Name==name);
            if (conn.Count() == 0)
                return;
            Connection connection = conn.First() ;
            elc.HeadNodeName = connection.FirstTable;
            elc.TailNodeName = connection.SecondTable;
            elc.HeadNodeRelation = connection.FirstRelation;
            elc.TailNodeRelation = connection.SecondRelation;
            foreach(var node in ErdDiagramModel.Nodes)
            {
                var erd = node as Node;
                var nodeName = ErdDiagramModel.GetNodeName(erd);
                if (elc.HeadNodeName == nodeName)
                    elc.HeadNode = erd;
                else if (elc.TailNodeName == nodeName)
                    elc.TailNode = erd;
            }
            //ErdDiagramModel.SynchronizeConnector();
        }
        void ErdControlDiagramView_PreviewConnectorDrop(object sender, PreviewConnectorDropEventRoutedEventArgs evtArgs)
        {

            Label tt = new Label();
            tt.FontSize = 12;
            tt.Content = "PreviewConnectorDrop event was fired";
            //disArea.Children.Add(tt);
            //scroll.//scrollToBottom();

            var e = ErdDiagramModel;
           // ConnectorProperties.Visibility = Visibility.Visible;


        }
        void diagramView_TailNodeChanged(object sender, NodeChangedRoutedEventArgs evtArgs)
        {
            ErdLineConnector lineconnector = evtArgs.Connector as ErdLineConnector;

        /*    if (lineconnector.HeadNode != null && lineconnector.TailNode != null)
            {
                RegularConnectorPositionStrategy r = new RegularConnectorPositionStrategy();
                r.headNodeHeigth = lineconnector.HeadNode.ActualHeight;
                r.headNodeWidth = lineconnector.HeadNode.ActualWidth;
                r.headNodePosition = new Point(lineconnector.HeadNode.Position.X, lineconnector.HeadNode.Position.Y);
                r.tailNodeHeight = lineconnector.TailNode.ActualHeight;
                r.tailNodeWidth = lineconnector.TailNode.ActualWidth;
                r.tailNodePosition = new Point(lineconnector.TailNode.Position.X, lineconnector.TailNode.Position.Y);
                KeyValuePair<Point, Point> kvp = r.GetPosition();
                lineconnector.StartPointPosition = kvp.Key;
                lineconnector.HeadDecoratorPosition = kvp.Key;
                lineconnector.EndPointPosition = kvp.Value;
                lineconnector.TailDecoratorPosition = kvp.Value;
               
            }*/
            if (evtArgs.PreviousNode != null && evtArgs.CurrentNode != null)
                AddToLog(evtArgs.Connector.Name.ToString(), "TailNode Changed event was fired.", " [CurrentNode: ", evtArgs.CurrentNode.Name.ToString(), ", PreviousNode: ", evtArgs.PreviousNode.Name.ToString());
            else if (evtArgs.PreviousNode == null && evtArgs.CurrentNode != null)
                AddToLog(evtArgs.Connector.Name.ToString(), "TailNode Changed event was fired. [CurrentNode: ", evtArgs.CurrentNode.Name.ToString());
            else if (evtArgs.PreviousNode != null && evtArgs.CurrentNode == null)
                AddToLog(evtArgs.Connector.Name.ToString(), "TailNode Changed event was fired. [PreviousNode: ", evtArgs.PreviousNode.Name.ToString());
            else if (evtArgs.PreviousNode == null && evtArgs.CurrentNode == null)
                AddToLog(evtArgs.Connector.Name.ToString(), "TailNode Changed event was fired.");

        }

        void diagramView_HeadNodeChanged(object sender, NodeChangedRoutedEventArgs evtArgs)
        {
            ErdLineConnector lineconnector = evtArgs.Connector as ErdLineConnector;
            
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
            if (evtArgs.PreviousNode != null && evtArgs.CurrentNode != null)
                AddToLog(evtArgs.Connector.Name.ToString(), "HeadNode Changed event was fired.", " [CurrentNode: ", evtArgs.CurrentNode.Name.ToString(), ", PreviousNode: ", evtArgs.PreviousNode.Name.ToString());
            else if (evtArgs.PreviousNode == null && evtArgs.CurrentNode != null)
                AddToLog(evtArgs.Connector.Name.ToString(), "HeadNode Changed event was fired. [CurrentNode: ", evtArgs.CurrentNode.Name.ToString());
            else if (evtArgs.PreviousNode != null && evtArgs.CurrentNode == null)
                AddToLog(evtArgs.Connector.Name.ToString(), "HeadNode Changed event was fired. [PreviousNode: ", evtArgs.PreviousNode.Name.ToString());
            else if (evtArgs.PreviousNode == null && evtArgs.CurrentNode == null)
                AddToLog(evtArgs.Connector.Name.ToString(), "HeadNode Changed event was fired.");
        }
        void diagramView_NodeRotationChanged(object sender, NodeRoutedEventArgs evtArgs)
        {
            AddToLog(evtArgs.Node.Name.ToString(), " RotationChanged event was fired.");
            
        }

        void diagramView_NodeRotationChanging(object sender, NodeRoutedEventArgs evtArgs)
        {
            AddToLog(evtArgs.Node.Name.ToString(), " RotationChanging event was fired.");
        }

        void diagramView_NodeResized(object sender, NodeRoutedEventArgs evtArgs)
        {
            AddToLog(evtArgs.Node.Name.ToString(), " Resized event was fired.");
        }

        void diagramView_NodeResizing(object sender, NodeRoutedEventArgs evtArgs)
        {
            AddToLog(evtArgs.Node.Name.ToString(), " Resizing event was fired.");
        }

        void diagramView_NodeDrop(object sender, NodeDroppedRoutedEventArgs evtArgs)
        {
            var kt = ErdControlDiagramView;
            Label tt = new Label();
            tt.FontSize = 12;
            tt.Content = "NodeDrop event was fired";
            //disArea.Children.Add(tt);
            //scroll.//scrollToBottom();
            Node node = evtArgs.DroppedNode as Node;
            node.Shape = Shapes.Rectangle;
           


            DataTemplate dt=this.FindResource("node-initial-template") as DataTemplate;
            DiagramContent.Table table = new DiagramContent.Table();
            /*table.Properties = new System.Collections.ObjectModel.ObservableCollection<DiagramContent.Properties>();
            DiagramContent.Properties p = new DiagramContent.Properties();
            p.Name = "IdNumber";
            p.Type = "int";
            p.IsPrimaryKey = true;
            table.Properties.Add(p);*/
            node.Content = table;
            node.ContentTemplate = ErdDiagramModel.ItemTemplate;
            node.ApplyTemplate();
            node.Width = 160;
            node.Height = 50;
            node.PortVisibility = PortVisibility.AlwaysHidden;
           // node.Ports.Add(new ConnectionPort(node, new Point(100, 45)) { Name = "1" });
            if (node.Content is System.Windows.Shapes.Path)
            {
                node.Loaded += new RoutedEventHandler(node_Loaded);
            }
            Structure.Struct.model.tables.Add(table);
            ChangeManager.Manager.Add(table, State.Add);
            var k = this.ShapeStyle;
            int i = 5;
            var a = ErdDiagramBuilder.Model;

            int y = 7;
           // node.Ports.Add(new ConnectionPort() { Node=node, PortOffset=0.5, ToolTip="Connection", PortVisibility = PortVisibility.AlwaysVisible, Left = node.Width / 2, Top = node.Height  });
          //  node.Ports.Add(new ConnectionPort() { Node = node, PortOffset = 0.5, ToolTip = "Connection", PortVisibility = PortVisibility.AlwaysVisible, Left = node.Width / 2, Top = 0 });
            
        }

        void ErdControlDiagramView_PreviewNodeDrop(object sender, PreviewNodeDropEventRoutedEventArgs evtArgs)
        {
             Label tt = new Label();
             tt.FontSize = 12;
             tt.Content = "PreviewNodeDrop event was fired";
             //disArea.Children.Add(tt);
             //scroll.//scrollToBottom();
             
        }
        void node_Loaded(object sender, RoutedEventArgs e)
        {
            /* Node node = sender as Node;
             node.Loaded -= node_Loaded;
             node.Shape = Shapes.CustomPath;
             Path p = node.Template.FindName("PART_Shape", node) as Path;
             Path oldPath = node.Content as Path;
             MainWindow.SetData(node, oldPath.GetValue(Path.DataProperty) as Geometry);

             //p.Data = oldPath.GetValue(Path.DataProperty) as Geometry;
             node.Content = null;*/
        }

        void diagramView_NodeDragEnd(object sender, NodeRoutedEventArgs evtArgs)
        {
            AddToLog(evtArgs.Node.Name.ToString(), " DragEnd event was fired.");
        }

        void diagramView_NodeDragStart(object sender, NodeRoutedEventArgs evtArgs)
        {
            AddToLog(evtArgs.Node.Name.ToString(), " DragStart event was fired.");
        }

        void diagramView_NodeDoubleClick(object sender, NodeRoutedEventArgs evtArgs)
        {
            AddToLog(evtArgs.Node.Name.ToString(), " was Double Clicked.");
        }

        void diagramView_NodeClick(object sender, NodeRoutedEventArgs evtArgs)
        {
            AddToLog(evtArgs.Node.Name.ToString(), " was Clicked.");
         /*   if (firstSelectedNode == null)
                firstSelectedNode = evtArgs.Node as Node;
            else
            {
                secondSelectedNode = evtArgs.Node as Node;
                if (AddedConnector != null)
                {
                    
                    AddedConnector.HeadNode = firstSelectedNode;
                    AddedConnector.TailNode = secondSelectedNode;
                    string headNodeName = ErdDiagramModel.GetNodeName(firstSelectedNode);
                    string tailNodeName = ErdDiagramModel.GetNodeName(secondSelectedNode);
                    Connection cnt = new Connection();
                    cnt.FirstTable = headNodeName;
                    cnt.SecondTable = tailNodeName;
                    cnt.FirstRelation = "One";
                    cnt.SecondRelation = "Many";

                     DiagramContent.Table head = Structure.Struct.model.tables.First(x => x.Name == headNodeName);
                     DiagramContent.Table tail = Structure.Struct.model.tables.First(x => x.Name == tailNodeName);
                     head.ConnectionProperties.Add(new ConnectionProperties() { Name = "Connection" + tailNodeName, Type = tailNodeName });
                     tail.ConnectionProperties.Add(new ConnectionProperties() { Name = "Connection" + headNodeName, Type = headNodeName });
                     firstSelectedNode.Content = head;
                     secondSelectedNode.Content = tail;
                     firstSelectedNode.Height += 40;
                     secondSelectedNode.Height += 40;

                    Structure.Struct.model.connections.Add(cnt);
                    //ErdDiagramModel.SynchronizeConnector();
                    firstSelectedNode = null;
                    secondSelectedNode = null;


                    var connectorInitialTemplateResource = this.TryFindResource("connection-template");
                    if (connectorInitialTemplateResource != null)
                    {
                        DataTemplate dt = connectorInitialTemplateResource as DataTemplate;


                        AddedConnector.DataContext = cnt;
                        AddedConnector.LabelTemplate = dt;
                        bool k = AddedConnector.ApplyTemplate();

                        // VisualTreeHelper.
                    }
                    //  ErdDiagramModel.Connections.Add(AddedConnector);
                    AddedConnector.ApplyTemplate();
                    AddedConnector = null;

                }
            }*/
        }

        void diagramView_ConnectorDragEnd(object sender, ConnDragEndRoutedEventArgs evtArgs)
        {
            if (evtArgs.FixedNodeEnd != null && evtArgs.HitNodeEnd != null)
                AddToLog(evtArgs.Connector.Name.ToString(), " DragEnd event was fired." + " [FixedNodeEnd: ", evtArgs.FixedNodeEnd.Name.ToString(), ", HitNodeEnd: ", evtArgs.HitNodeEnd.Name.ToString());
            else if (evtArgs.FixedNodeEnd == null && evtArgs.HitNodeEnd != null)
                AddToLog(evtArgs.Connector.Name.ToString(), " DragEnd event was fired." + ", HitNodeEnd: ", evtArgs.HitNodeEnd.Name.ToString());
            else if (evtArgs.FixedNodeEnd != null && evtArgs.HitNodeEnd == null)
                AddToLog(evtArgs.Connector.Name.ToString(), " DragEnd event was fired." + " [FixedNodeEnd: ", evtArgs.FixedNodeEnd.Name.ToString());
            else if (evtArgs.FixedNodeEnd == null && evtArgs.HitNodeEnd == null)
                AddToLog(evtArgs.Connector.Name.ToString(), " DragEnd event was fired.");
        }

        void diagramView_ConnectorDragStart(object sender, ConnDragRoutedEventArgs evtArgs)
        {
            if (evtArgs.FixedNodeEnd != null && evtArgs.MovableNodeEnd != null)
                AddToLog(evtArgs.Connector.Name.ToString(), " DragStart event was fired." + " [FixedNodeEnd: ", evtArgs.FixedNodeEnd.Name.ToString(), ", MovableNodeEnd: ", evtArgs.MovableNodeEnd.Name.ToString());
            else if (evtArgs.FixedNodeEnd == null && evtArgs.MovableNodeEnd != null)
                AddToLog(evtArgs.Connector.Name.ToString(), " DragStart event was fired." + ", MovableNodeEnd: ", evtArgs.MovableNodeEnd.Name.ToString());
            else if (evtArgs.FixedNodeEnd != null && evtArgs.MovableNodeEnd == null)
                AddToLog(evtArgs.Connector.Name.ToString(), " DragStart event was fired." + " [FixedNodeEnd: ", evtArgs.FixedNodeEnd.Name.ToString());
            else if (evtArgs.FixedNodeEnd == null && evtArgs.MovableNodeEnd == null)
                AddToLog(evtArgs.Connector.Name.ToString(), " DragStart event was fired.");
        }

        void diagramView_ConnectorLabelChanged(object sender, LabelConnRoutedEventArgs evtArgs)
        {
            if (evtArgs.HeadNode != null && evtArgs.TailNode != null)
                AddToLog(evtArgs.Connector.Name.ToString(), " Label Changed event  was fired. [OldLabelValue: ", evtArgs.OldLabelValue.ToString(), ", NewLabelValue: ", evtArgs.NewLabelValue.ToString(), ", HeadNode: ", evtArgs.HeadNode.Name.ToString(), ", TailNode: ", evtArgs.TailNode.Name.ToString());
            else if (evtArgs.HeadNode == null && evtArgs.TailNode != null)
                AddToLog(evtArgs.Connector.Name.ToString(), " Label Changed event  was fired. [OldLabelValue: ", evtArgs.OldLabelValue.ToString(), ", NewLabelValue: ", evtArgs.NewLabelValue.ToString(), ", TailNode: ", evtArgs.TailNode.Name.ToString());
            else if (evtArgs.HeadNode != null && evtArgs.TailNode == null)
                AddToLog(evtArgs.Connector.Name.ToString(), " Label Changed event  was fired. [OldLabelValue: ", evtArgs.OldLabelValue.ToString(), ", NewLabelValue: ", evtArgs.NewLabelValue.ToString(), ", HeadNode: ", evtArgs.HeadNode.Name.ToString());
            else if (evtArgs.HeadNode == null && evtArgs.TailNode == null)
                AddToLog(evtArgs.Connector.Name.ToString(), " Label Changed event  was fired. [OldLabelValue: ", evtArgs.OldLabelValue.ToString(), ", NewLabelValue: ", evtArgs.NewLabelValue.ToString());
        }

        void diagramView_ConnectorDoubleClick(object sender, ConnRoutedEventArgs evtArgs)
        {

            if (evtArgs.HeadNode != null && evtArgs.TailNode != null)
                AddToLog(evtArgs.Connector.Name.ToString(), " was Double Clicked." + " [HeadNode: ", evtArgs.HeadNode.Name.ToString(), ", TailNode: ", evtArgs.TailNode.Name.ToString());
            else if (evtArgs.HeadNode == null && evtArgs.TailNode != null)
                AddToLog(evtArgs.Connector.Name.ToString(), " was Double Clicked." + ", TailNode: ", evtArgs.TailNode.Name.ToString());
            else if (evtArgs.HeadNode != null && evtArgs.TailNode == null)
                AddToLog(evtArgs.Connector.Name.ToString(), " was Double Clicked." + " [HeadNode: ", evtArgs.HeadNode.Name.ToString());
            else if (evtArgs.HeadNode == null && evtArgs.TailNode == null)
                AddToLog(evtArgs.Connector.Name.ToString(), " was Double Clicked.");
        }

        void diagramView_ConnectorStartLabelEdit(object sender, LabelEditConnRoutedEventArgs evtArgs)
        {
            if (evtArgs.HeadNode != null && evtArgs.TailNode != null)
                AddToLog(evtArgs.Connector.Name.ToString(), " StartLabelEdit event  was fired. [OldLabelValue: ", evtArgs.OldLabelValue.ToString(), ", HeadNode: ", evtArgs.HeadNode.Name.ToString(), ", TailNode: ", evtArgs.TailNode.Name.ToString());
            else if (evtArgs.HeadNode == null && evtArgs.TailNode != null)
                AddToLog(evtArgs.Connector.Name.ToString(), " StartLabelEdit event  was fired. [OldLabelValue: ", evtArgs.OldLabelValue.ToString(), ", TailNode: ", evtArgs.TailNode.Name.ToString());
            else if (evtArgs.HeadNode != null && evtArgs.TailNode == null)
                AddToLog(evtArgs.Connector.Name.ToString(), " StartLabelEdit event  was fired. [OldLabelValue: ", evtArgs.OldLabelValue.ToString(), ", HeadNode: ", evtArgs.HeadNode.Name.ToString());
            else if (evtArgs.HeadNode == null && evtArgs.TailNode == null)
                AddToLog(evtArgs.Connector.Name.ToString(), " StartLabelEdit event  was fired. [OldLabelValue: ", evtArgs.OldLabelValue.ToString());
        }

        void diagramView_NodeLabelChanged(object sender, LabelRoutedEventArgs evtArgs)
        {
            AddToLog(evtArgs.Node.Name.ToString(), " LabelChanged event  was fired. [OldLabelValue: ", evtArgs.OldLabelValue.ToString(), ", NewLabelValue: ", evtArgs.NewLabelValue.ToString());
        }

        void diagramView_NodeStartLabelEdit(object sender, LabelRoutedEventArgs evtArgs)
        {
            AddToLog(evtArgs.Node.Name.ToString(), " StartLabelEdit event  was fired. [OldLabelValue: ", evtArgs.OldLabelValue.ToString());
        }

        public void AddToLog(string prop, string oldvalue, string newvalue)
        {
            Label tt = new Label();
            tt.FontSize = 12;
            tt.Content = prop + oldvalue + newvalue + "]";
            //disArea.Children.Add(tt);
            //scroll.//scrollToBottom();
        }
        public void AddToLog(string prop, string str, string oldvalue, string str2, string str3)
        {

            Label tt = new Label();
            tt.FontSize = 12;
            tt.Content = prop + str + oldvalue + str2 + str3 + "]";
            //disArea.Children.Add(tt);
            //scroll.//scrollToBottom();
        }
        public void AddToLog(string prop, string str, string oldvalue, string str2, string str3, string str4)
        {
            /*Label tt = new Label();
            tt.FontSize = 12;
            tt.Content = prop + str + oldvalue + str2 + str3 + str4 + "]";
            //disArea.Children.Add(tt);
            //scroll.//scrollToBottom();*/
        }
        public void AddToLog(string prop, string str, string oldvalue, string str2, string str3, string str4, string str5)
        {

            Label tt = new Label();
            tt.FontSize = 12;
            tt.Content = prop + str + oldvalue + str2 + str3 + str4 + str5 + "]";
            //disArea.Children.Add(tt);
            //scroll.//scrollToBottom();
        }
        public void AddToLog(string prop, string str, string oldvalue, string str2, string str3, string str4, string str5, string str6, string str7)
        {

            Label tt = new Label();
            tt.FontSize = 12;
            tt.Content = prop + str + oldvalue + str2 + str3 + str4 + str5 + str6 + str7 + "]";
            //disArea.Children.Add(tt);
            //scroll.//scrollToBottom();
        }
        public void AddToLog(string prop, string str)
        {

            Label tt = new Label();
            tt.FontSize = 12;
            tt.Content = prop + str;
            //disArea.Children.Add(tt);
            //scroll.//scrollToBottom();
        }

        #endregion

        #region DiagramView


        #region Printing

        //Prints the Page.
        private void P_Click(object sender, RoutedEventArgs e)
        {
            if (cb1.SelectedIndex == 0)
            {
                this.Print(ErdControlDiagramView.Page, Stretch.Fill);
            }
            else if (cb1.SelectedIndex == 1)
            {
                this.Print(ErdControlDiagramView.Page, Stretch.None);
            }
            else if (cb1.SelectedIndex == 2)
            {
                this.Print(ErdControlDiagramView.Page, Stretch.Uniform);
            }
            else if (cb1.SelectedIndex == 3)
            {
                this.Print(ErdControlDiagramView.Page, Stretch.UniformToFill);
            }
        }

        //printing
        internal void Print(FrameworkElement element, Stretch s)
        {
            UpdatePreview(element, s);
            StartPrint();
        }

        //printCapabilities
        private void StartPrint()
        {
            /*System.Printing.PrintCapabilities printCapabilities = m_nativePrintDialog.PrintQueue.GetPrintCapabilities(m_nativePrintDialog.PrintTicket);

             Size pageSize = new Size(m_nativePrintDialog.PrintableAreaWidth, m_nativePrintDialog.PrintableAreaHeight);
             Size pageAreaSize = new Size(printCapabilities.PageImageableArea.ExtentWidth, printCapabilities.PageImageableArea.ExtentHeight);
             Rectangle rect = new Rectangle();

             rect.Fill = m_visualBrush;

             SetViewport(m_visualBrush, pageAreaSize);
             rect.Arrange(new Rect(new Point(0, 0), pageAreaSize));

             XpsDocumentWriter writer = PrintQueue.CreateXpsDocumentWriter(m_nativePrintDialog.PrintQueue);
             writer.Write(rect, m_nativePrintDialog.PrintTicket);

             SetViewport(m_visualBrush, new Size(this.PreviewRect.ActualWidth, this.PreviewRect.ActualHeight));*/
        }

        //Set theview port for printing
        private void SetViewport(VisualBrush brush, Size size)
        {
            if (brush == null)
            {
                throw new ArgumentNullException("brush");
            }

            if (cb1.SelectedIndex == 2) // Stretch.Uniform)
            {
                double coefficientHeight = size.Height / brush.Viewbox.Height;
                double coefficientWidth = size.Width / brush.Viewbox.Width;

                if (coefficientHeight < coefficientWidth)
                {
                    double width = coefficientHeight * brush.Viewbox.Width / size.Width;
                    double x = (1 - width) / 2;
                    brush.Viewport = new Rect(new Point(x, 0), new Size(width, 1));
                }
                else if (coefficientHeight > coefficientWidth)
                {
                    double height = coefficientWidth * brush.Viewbox.Height / size.Height;
                    double y = (1 - height) / 2;
                    brush.Viewport = new Rect(new Point(0, y), new Size(1, height));
                }
            }
            else if (cb1.SelectedIndex == 1) // Stretch.None)
            {
                if (size.Width > brush.Viewbox.Width || size.Height > brush.Viewbox.Height)
                {
                    double coefficientHeight = size.Width - brush.Viewbox.Width;
                    double coefficientWidth = size.Height - brush.Viewbox.Height;
                    double width = brush.Viewbox.Width / size.Width;
                    double height = brush.Viewbox.Height / size.Height;
                    double x = (1 - width) / 2;
                    double y = (1 - height) / 2;
                    brush.Viewport = new Rect(new Point(x, y), new Size(width, height));
                }
                else
                {
                    brush.Viewport = new Rect(0, 0, 1, 1);
                }
            }
            else
            {
                brush.Viewport = new Rect(0, 0, 1, 1);
            }
        }

        //Update the print preview
        private void OnUpdatePrintPreview(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (cb1.SelectedIndex == 0)
            {
                UpdatePreview(ErdControlDiagramView.Page, Stretch.Fill);
            }
            else if (cb1.SelectedIndex == 1)
            {
                UpdatePreview(ErdControlDiagramView.Page, Stretch.None);
            }
            else if (cb1.SelectedIndex == 2)
            {
                UpdatePreview(ErdControlDiagramView.Page, Stretch.Uniform);
            }
            else if (cb1.SelectedIndex == 3)
            {
                UpdatePreview(ErdControlDiagramView.Page, Stretch.UniformToFill);
            }
        }

        //Loaded event
        private void OnPrintLoad(object sender, RoutedEventArgs e)
        {
            cb1.SelectedIndex = 2;
            // UpdatePreview(ErdControlDiagramView.Page, Stretch.Uniform);           
        }

        private void OnPrintStretch(object sender, SelectionChangedEventArgs e)
        {
            if (cb1.SelectedIndex == 0)
            {
                UpdatePreview(ErdControlDiagramView.Page, Stretch.Fill);
            }
            else if (cb1.SelectedIndex == 1)
            {
                UpdatePreview(ErdControlDiagramView.Page, Stretch.None);
            }
            else if (cb1.SelectedIndex == 2)
            {
                UpdatePreview(ErdControlDiagramView.Page, Stretch.Uniform);
            }
            else if (cb1.SelectedIndex == 3)
            {
                UpdatePreview(ErdControlDiagramView.Page, Stretch.UniformToFill);
            }
        }

        private void P1_Click(object sender, RoutedEventArgs e)
        {
            ErdControlDiagramView.Print();
        }
        #endregion


        #region Save and Load

        //Opendialog
        private void openFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            if ((bool)openFileDialog.ShowDialog())
            {
            }
        }
        //Saving the filter
        private void save(String filter)
        {
            string s = string.Empty;

            m_SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            m_SaveFileDialog.Filter = filter;
            m_SaveFileDialog.FileName = "Digram";
            m_SaveFileDialog.ShowDialog();
            if (m_SaveFileDialog.FileName != "" && filter == "XAML File(*.xaml)|*.xaml")
            {
                s = m_SaveFileDialog.FileName;
                ErdDiagramBuilder.Save(m_SaveFileDialog.FileName);
            }
            else if (m_SaveFileDialog.FileName != "")
            {
                ErdControlDiagramView.Save(m_SaveFileDialog.FileName);
            }


            if (!string.IsNullOrEmpty(s))
            {
                System.IO.FileStream fs = new System.IO.FileStream(s, System.IO.FileMode.Open);
                System.IO.StreamReader reader = new System.IO.StreamReader(fs);
                string content = reader.ReadToEnd();
                reader.Close();

                if (!(content.Contains("<Node.Ports>") || content.Contains("<LineConnector.ConnectorPathGeometry>")))
                {
                    content = content.Insert("<DiagramPage".Length + 2, "xmlns:db=\"clr-namespace:DiagramBuilder_2010;assembly=DiagramBuilder_2010\"");
                }

                fs = new System.IO.FileStream(s, System.IO.FileMode.Create);
                System.IO.StreamWriter writer = new System.IO.StreamWriter(fs);
                writer.WriteLine(content);
                writer.Close();
            }


        }

        //Craeting the new one
        private void OnCreate(object sender, RoutedEventArgs e)
        {
            if (!ErdControlDiagramView.IsPageSaved)
            {
                string msg = "Do you want to save changes in Existing Page ? ";
                MessageBoxResult result = MessageBox.Show(msg, "Diagram Builder", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    ErdDiagramBuilder.Save();
                    ErdControlDiagramView.Page.Children.Clear();
                    ErdControlDiagramView.IsPageSaved = true;
                }
                else if (result == MessageBoxResult.No)
                {
                    ErdControlDiagramView.Page.Children.Clear();
                    ErdControlDiagramView.IsPageSaved = true;
                }
                else
                {
                    e.Handled = true;
                }
            }
            else
            {
                ErdControlDiagramView.Page.Children.Clear();
                ErdControlDiagramView.IsPageSaved = true;
            }
            MyRibbon.HideBackStage();
        }
        #endregion

        #region Dirty Flag

        //Saves the page.
        private void S_Click(object sender, RoutedEventArgs e)
        {
            OnSave();
        }

        //Loads the selected page.        
        private void L_Click(object sender, RoutedEventArgs e)
        {
            OnLoad();
        }

        // private System.Windows.Forms.SaveFileDialog m_SaveFileDialog;
        private System.Windows.Forms.OpenFileDialog m_OpenFileDialog;
        private bool IsSaveFirst = true;
        private string s;

        private void OnSave()
        {
            if (IsSaveFirst)
            {
                string filter = "XAML File(*.xaml)|*.xaml";
                m_SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
                m_SaveFileDialog.Filter = filter;
                m_SaveFileDialog.FileName = "Diagram";
                System.Windows.Forms.DialogResult result = m_SaveFileDialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK || result == System.Windows.Forms.DialogResult.Yes)
                {
                    if (m_SaveFileDialog.FileName != "" && m_SaveFileDialog.Filter == "XAML File(*.xaml)|*.xaml")
                    {
                        s = m_SaveFileDialog.FileName;
                        ErdDiagramBuilder.Save(m_SaveFileDialog.FileName);
                        IsSaveFirst = false;
                    }
                }
            }
            else
            {
                if (s != "")
                    ErdDiagramBuilder.Save(s);
            }
            Node n = new Node();
            

            if (!string.IsNullOrEmpty(s))
            {
                System.IO.FileStream fs = new System.IO.FileStream(s, System.IO.FileMode.Open);
                System.IO.StreamReader reader = new System.IO.StreamReader(fs);
                string content = reader.ReadToEnd();
                reader.Close();

                if (!(content.Contains("<Node.Ports>") || content.Contains("<LineConnector.ConnectorPathGeometry>")))
                {
                    content = content.Insert("<DiagramPage".Length + 2, "xmlns:db=\"clr-namespace:DiagramBuilder_2010;assembly=DiagramBuilder_2010\"");
                }

                fs = new System.IO.FileStream(s, System.IO.FileMode.Create);
                System.IO.StreamWriter writer = new System.IO.StreamWriter(fs);
                writer.WriteLine(content);
                writer.Close();
                
            }
        }

        private void OnLoad()
        {
            string filter = "XAML File(*.xaml)|*.xaml";
            m_OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            m_OpenFileDialog.Filter = filter;
            System.Windows.Forms.DialogResult result = m_OpenFileDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK || result == System.Windows.Forms.DialogResult.Yes)
            {
                if (m_OpenFileDialog.FileName != "" && m_OpenFileDialog.Filter == "XAML File(*.xaml)|*.xaml")
                {
                    s = m_OpenFileDialog.FileName;
                    ErdDiagramBuilder.Load(m_OpenFileDialog.FileName);
                    IsSaveFirst = false;
                }
            }
        }

        private void mainwindow_Closing(object sender, CancelEventArgs e)
        {
            bool editstate = ErdDiagramBuilder.EditState;
            if (!editstate)
            {
                bool changes = ChangeManager.Manager.HasChanges;
                if (changes)
                {
                    string msg = "Do you want to save changes in Diagram- to save click 'yes', to exit without saving click 'no', to remain without saving click Cancel ? ";
                    MessageBoxResult result = MessageBox.Show(msg, "Diagram Builder", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                    //  ErdDiagramBuilder.Save();
                    if (result == MessageBoxResult.Yes)
                    {
                        ChangeManager.Manager.Execute();
                        MessageBox.Show("Saved Successfully");
                    }

                     else if (result==MessageBoxResult.Cancel)
                     {
                         e.Cancel = true;
                     }
                }
            }
            else
            {
                string msg = "You have tables\\properties in edit mode, do you want to exit-than click 'Yes' or finish the editing -then click 'No'?  ";
                MessageBoxResult result = MessageBox.Show(msg, "Diagram Builder", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                    e.Cancel = true;
                
            }
            
        }

        private void OnIsPageSavedChanged(object sender, EventArgs args)
        {
            DiagramView d = sender as DiagramView;
            if (!d.IsPageSaved)
            {
                mainwindow.Title = "Diagram Builder *";
            }
            else
            {
                mainwindow.Title = "Diagram Builder";
            }
        }

        #endregion



        #region Export into Image

        private void JPEGbtn_Click(object sender, RoutedEventArgs e)
        {
            save("JPEG File(*.jpg)|*.jpg");
            MyRibbon.HideBackStage();
        }

        private void XAMLbtn_Click(object sender, RoutedEventArgs e)
        {
            save("XAML File(*.xaml)|*.xaml");
            MyRibbon.HideBackStage();
        }

        private void BMPbtn_Click(object sender, RoutedEventArgs e)
        {
            save("BMP File(*.bmp)|*.bmp");
            MyRibbon.HideBackStage();
        }

        private void PNGbtn_Click(object sender, RoutedEventArgs e)
        {
            save("PNG File(*.png)|*.png");
            MyRibbon.HideBackStage();
        }

        private void TIFGbtn_Click(object sender, RoutedEventArgs e)
        {
            save("TIF File(*.tif)|*.tif");
            MyRibbon.HideBackStage();
        }

        private void GIFbtn_Click(object sender, RoutedEventArgs e)
        {
            save("GIF File(*.gif)|*.gif");
            MyRibbon.HideBackStage();
        }

        private void WDPbtn_Click(object sender, RoutedEventArgs e)
        {
            save("WDP File(*.wdp)|*.wdp");
            MyRibbon.HideBackStage();
        }

        #endregion

        private void close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #region Zoom

        //Reset the DiagramPage
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            ErdControlDiagramView.Reset(ErdControlDiagramView);
        }

        //Populate the combobox with Values
        void rib_Click(object sender, RoutedEventArgs e)
        {
            ErdControlDiagramView.ZoomFactor = Double.Parse(((sender as RibbonButton).Label));
            (((sender as RibbonButton).Parent) as DropDownButton).Label = ((sender as RibbonButton).Label);
        }

        //Zoom factor
        private void ComboBoxItem_SelectedCRadius(object sender, RoutedEventArgs e)
        {
            RibbonComboBoxItem item = (RibbonComboBoxItem)sender;
            if (ErdControlDiagramView != null)
                ErdControlDiagramView.ZoomFactor = Convert.ToDouble(item.Content);
        }
        #endregion



        #region Ruler

        //Measurement and Units 
        private void M_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as RibbonButton).Label)
            {
                case "Pixel":
                    (ErdControlDiagramView.Page as DiagramPage).MeasurementUnits = MeasureUnits.Pixel;
                    (((sender as RibbonButton).Parent) as DropDownButton).Label = "Pixel";
                    break;
                case "Point":
                    (ErdControlDiagramView.Page as DiagramPage).MeasurementUnits = MeasureUnits.Point;
                    (((sender as RibbonButton).Parent) as DropDownButton).Label = "Point";
                    break;
                case "Centimeter":
                    (ErdControlDiagramView.Page as DiagramPage).MeasurementUnits = MeasureUnits.Centimeter;
                    (((sender as RibbonButton).Parent) as DropDownButton).Label = "Centimeter";
                    break;
                case "Display":
                    (ErdControlDiagramView.Page as DiagramPage).MeasurementUnits = MeasureUnits.Display;
                    (((sender as RibbonButton).Parent) as DropDownButton).Label = "Display";
                    break;
                case "Document":
                    (ErdControlDiagramView.Page as DiagramPage).MeasurementUnits = MeasureUnits.Document;
                    (((sender as RibbonButton).Parent) as DropDownButton).Label = "Document";
                    break;
                case "Inch":
                    (ErdControlDiagramView.Page as DiagramPage).MeasurementUnits = MeasureUnits.Inch;
                    (((sender as RibbonButton).Parent) as DropDownButton).Label = "Inch";
                    break;
                case "KiloMeter":
                    (ErdControlDiagramView.Page as DiagramPage).MeasurementUnits = MeasureUnits.Kilometer;
                    (((sender as RibbonButton).Parent) as DropDownButton).Label = "Kilometer";
                    break;
                case "Meter":
                    (ErdControlDiagramView.Page as DiagramPage).MeasurementUnits = MeasureUnits.Meter;
                    (((sender as RibbonButton).Parent) as DropDownButton).Label = "Meter";
                    break;
                case "MiliMeter":
                    (ErdControlDiagramView.Page as DiagramPage).MeasurementUnits = MeasureUnits.Millimeter;
                    (((sender as RibbonButton).Parent) as DropDownButton).Label = "Millimeter";
                    break;
                case "HalfInch":
                    (ErdControlDiagramView.Page as DiagramPage).MeasurementUnits = MeasureUnits.HalfInch;
                    (((sender as RibbonButton).Parent) as DropDownButton).Label = "HalfInch";
                    break;
                case "SixteenInch":
                    (ErdControlDiagramView.Page as DiagramPage).MeasurementUnits = MeasureUnits.SixteenthInch;
                    (((sender as RibbonButton).Parent) as DropDownButton).Label = "SixteenthInch";
                    break;
                case "EigthInch":
                    (ErdControlDiagramView.Page as DiagramPage).MeasurementUnits = MeasureUnits.EighthInch;
                    (((sender as RibbonButton).Parent) as DropDownButton).Label = "EighthInch";
                    break;
                case "QuarterInch":
                    (ErdControlDiagramView.Page as DiagramPage).MeasurementUnits = MeasureUnits.QuarterInch;
                    (((sender as RibbonButton).Parent) as DropDownButton).Label = "QuarterInch";
                    break;
                case "Miles":
                    (ErdControlDiagramView.Page as DiagramPage).MeasurementUnits = MeasureUnits.Mile;
                    (((sender as RibbonButton).Parent) as DropDownButton).Label = "Mile";
                    break;
                case "Yard":
                    (ErdControlDiagramView.Page as DiagramPage).MeasurementUnits = MeasureUnits.Yard;
                    (((sender as RibbonButton).Parent) as DropDownButton).Label = "Yard";
                    break;
                case "Foot":
                    (ErdControlDiagramView.Page as DiagramPage).MeasurementUnits = MeasureUnits.Foot;
                    (((sender as RibbonButton).Parent) as DropDownButton).Label = "Foot";
                    break;
            }
        }
        #endregion


        #region DrawingTools

        //Selecting the Drawin g tool
        private void OnDrawingToolsSelected(object sender, RoutedEventArgs e)
        {
            RibbonButton ribbonButton = sender as RibbonButton;
            string buttonContent = (string)ribbonButton.Name;
            ErdControlDiagramView.EnableDrawingTools = true;
            switch (buttonContent)
            {
                case "Ellipsecon":
                    ;
                    ErdControlDiagramView.DrawingTool = DrawingTools.Ellipse;
                    break;
                case "Rectanglecon":
                    ErdControlDiagramView.DrawingTool = DrawingTools.Rectangle;
                    break;
                case "RoundRectangleCon":
                    ErdControlDiagramView.DrawingTool = DrawingTools.RoundedRectangle;
                    break;
                case "polygon":
                    ErdControlDiagramView.DrawingTool = DrawingTools.Polygon;
                    break;
                case "none":
                    ErdControlDiagramView.EnableDrawingTools = false;
                    break;
                case "OrthogonalCon1":
                    (ErdControlDiagramView.Page as DiagramPage).ConnectorType = ConnectorType.Orthogonal;
                    ErdControlDiagramView.DrawingTool = DrawingTools.OrthogonalLine;
                    break;
                case "BezierCon1":
                    (ErdControlDiagramView.Page as DiagramPage).ConnectorType = ConnectorType.Bezier;
                    ErdControlDiagramView.DrawingTool = DrawingTools.BezierLine;
                    break;
                case "StraightCon1":
                    (ErdControlDiagramView.Page as DiagramPage).ConnectorType = ConnectorType.Straight;
                    ErdControlDiagramView.DrawingTool = DrawingTools.StraightLine;
                    break;
                case "PolyCon1":
                    ErdControlDiagramView.DrawingTool = DrawingTools.PolyLine;
                    break;
                case "Arc1":
                    (ErdControlDiagramView.Page as DiagramPage).ConnectorType = ConnectorType.Arc;
                    ErdControlDiagramView.DrawingTool = DrawingTools.Arc;
                    break;
            }
            Select1(buttonContent);
        }

        //Selection 
        private void Select1(string buttonContent)
        {
            Ellipsecon.IsSelected = false;
            Rectanglecon.IsSelected = false;
            RoundRectangleCon.IsSelected = false;
            polygon.IsSelected = false;
            none.IsSelected = false;
            OrthogonalCon1.IsSelected = false;
            BezierCon1.IsSelected = false;
            StraightCon1.IsSelected = false;
            PolyCon1.IsSelected = false;

            switch (buttonContent)
            {
                case "Ellipsecon":
                    Ellipsecon.IsSelected = true;
                    break;
                case "Rectanglecon":
                    Rectanglecon.IsSelected = true;
                    break;
                case "RoundRectangleCon":
                    RoundRectangleCon.IsSelected = true;
                    break;
                case "polygon":
                    polygon.IsSelected = true;
                    break;
                case "none":
                    none.IsSelected = true;
                    break;
                case "OrthogonalCon1":
                    OrthogonalCon1.IsSelected = true;
                    break;
                case "BezierCon1":
                    BezierCon1.IsSelected = true;
                    break;
                case "StraightCon1":
                    StraightCon1.IsSelected = true;
                    break;
                case "PolyCon1":
                    PolyCon1.IsSelected = true;
                    break;
            }
        }
        #endregion

        #endregion

        #region SymbolPalette

        //Customize the SymbolPalette
        private void SymPal_Click(object sender, RoutedEventArgs e)
        {
            SkinStorage.SetVisualStyle(ErdDiagramBuilder, "Default");
            MyRibbon.HideBackStage();
        }

        //Customize the SymbolPalette
        private void SymPalCus_Click(object sender, RoutedEventArgs e)
        {
            SkinStorage.SetVisualStyle(ErdDiagramBuilder, "Office2010Blue");
            MyRibbon.HideBackStage();
        }
        #endregion

        #region DiagramPage

        //Apply the theme styles
        private void ThemeStyle_SelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //RibbonGalleryItem themeStyle = e.NewValue as RibbonGalleryItem;
            //if (themeStyle.CheckOnClick)
            //{
            //string name = (themeStyle.Tag ?? themeStyle.Content).ToString();
            string name = e.NewValue.ToString();
            selectedPageTheme = this.Resources[name + "Style"] as Style;
            MainWindow.SetCustomPathStyle(ErdDiagramBuilder, selectedPageTheme);
            //}
        }

        private void OuterThemeStyles_MouseEnter(object sender, MouseEventArgs e)
        {
        }

        //Mouse enter themes to DiagramPage
        private void ThemeStyles_MouseEnter(object sender, MouseEventArgs e)
        {
            System.Windows.Shapes.Path themeStyle = sender as System.Windows.Shapes.Path;
            {
                MainWindow.SetCustomPathStyle(ErdDiagramBuilder, prepareCustomPathStyle(themeStyle));
            }
        }

        private Style prepareCustomPathStyle(System.Windows.Shapes.Path themeStyle)
        {
            Style newStyle = new System.Windows.Style(typeof(System.Windows.Shapes.Path));
            newStyle.Setters.Add(new Setter(System.Windows.Shapes.Path.FillProperty, themeStyle.Fill));
            newStyle.Setters.Add(new Setter(System.Windows.Shapes.Path.StrokeProperty, themeStyle.Stroke));
            newStyle.Setters.Add(new Setter(System.Windows.Shapes.Path.StrokeThicknessProperty, themeStyle.StrokeThickness));
            newStyle.Setters.Add(new Setter(System.Windows.Shapes.Path.EffectProperty, themeStyle.Effect));
            newStyle.Setters.Add(new Setter(System.Windows.Shapes.Path.StretchProperty, Stretch.Fill));
            return newStyle;
        }

        //Mouse leave themes to DiagramPage
        private void ThemeStyle_MouseLeave(object sender, MouseEventArgs e)
        {
            MainWindow.SetCustomPathStyle(ErdDiagramBuilder, selectedPageTheme);
        }

        private void PageSize_Click(object sender, RoutedEventArgs e)
        {
            page = new ChildWindow(ErdControlDiagramView);
        }
        #endregion

        #region Node and LineCOnnector Label

        //Customize the FontFamily to the Label
        private void OnFontNameChanged(object sender, SelectionChangedEventArgs e)
        {
            RibbonComboBox item = sender as RibbonComboBox;
            RibbonComboBoxItem comboItem = (RibbonComboBoxItem)item.SelectedItem;
            m_actualTextStyles.LabelFontFamily = comboItem.Content as FontFamily;
            if (ErdControlDiagramView != null)
            {
                TypeConverter tc = TypeDescriptor.GetConverter(typeof(FontFamily));
                foreach (object o in ErdControlDiagramView.SelectionList)
                {
                    if (o is LineConnector)
                    {
                        (o as LineConnector).LabelFontFamily = comboItem.Content as FontFamily;
                    }
                }
            }
        }

        ////Customize the FontSize to the Label
        private void OnFontSizeChanged(object sender, SelectionChangedEventArgs e)
        {
            RibbonComboBox item = sender as RibbonComboBox;
            RibbonComboBoxItem comboBoxItem = (RibbonComboBoxItem)item.SelectedItem;
            m_actualTextStyles.LabelFontSize = (int)comboBoxItem.Content;
            try
            {
                foreach (object o in ErdControlDiagramView.SelectionList)
                {
                    if (o is LineConnector)
                    {
                        (o as LineConnector).LabelFontSize = (int)comboBoxItem.Content;
                    }
                }
            }
            catch
            {
            }
        }

        //Customize the LabelTextDecorations
        private void OnFontDecorationChanged(object sender, RoutedEventArgs e)
        {
            if (Underline.IsSelected)
            {
                m_actualTextStyles.LabelTextDecorations = TextDecorations.Underline;
                foreach (object o in ErdControlDiagramView.SelectionList)
                {
                    if (o is LineConnector)
                    {
                        (o as LineConnector).LabelTextDecorations = TextDecorations.Underline;
                    }
                }
            }
            else
            {
                m_actualTextStyles.LabelTextDecorations = null;
                foreach (object o in ErdControlDiagramView.SelectionList)
                {
                    if (o is LineConnector)
                    {
                        (o as LineConnector).LabelTextDecorations = null;
                    }
                }
            }
        }

        //Customize the LabelFontWeight
        private void OnFontWeightChanged(object sender, RoutedEventArgs e)
        {
            if (Bold.IsSelected)
            {
                m_actualTextStyles.LabelFontWeight = FontWeights.Bold;
                foreach (object o in ErdControlDiagramView.SelectionList)
                {
                    if (o is LineConnector)
                    {
                        (o as LineConnector).LabelFontWeight = FontWeights.Bold;
                    }
                }
            }
            else
            {
                m_actualTextStyles.LabelFontWeight = FontWeights.Normal;
                foreach (object o in ErdControlDiagramView.SelectionList)
                {
                    if (o is LineConnector)
                    {
                        (o as LineConnector).LabelFontWeight = FontWeights.Normal;
                    }
                }
            }
        }

        //Customize the LabelFontStyle
        private void OnFontStyleChanged(object sender, RoutedEventArgs e)
        {
            if (Italic.IsSelected)
            {
                m_actualTextStyles.LabelFontStyle = FontStyles.Italic;
                foreach (object o in ErdControlDiagramView.SelectionList)
                {
                    if (o is LineConnector)
                    {
                        (o as LineConnector).LabelFontStyle = FontStyles.Italic;
                    }
                }
            }
            else
            {
                m_actualTextStyles.LabelFontStyle = FontStyles.Normal;
                foreach (object o in ErdControlDiagramView.SelectionList)
                {
                    if (o is LineConnector)
                    {
                        (o as LineConnector).LabelFontStyle = FontStyles.Normal;
                    }
                }
            }
        }

        //Customize the Alignment of the Label
        private void OnAlignmentChanged(object sender, RoutedEventArgs e)
        {
            RibbonButton ribbonButton = sender as RibbonButton;
            string buttonContent = (string)ribbonButton.Tag;
            switch (buttonContent)
            {
                case "TopLeft":
                    m_actualTextStyles.LabelHorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    m_actualTextStyles.LabelVerticalAlignment = System.Windows.VerticalAlignment.Top;
                    foreach (object o in ErdControlDiagramView.SelectionList)
                    {
                        if (o is Node)
                        {
                            (o as Node).LabelHorizontalTextAlignment = System.Windows.HorizontalAlignment.Left;
                            (o as Node).LabelVerticalTextAlignment = System.Windows.VerticalAlignment.Top;
                        }
                        else if (o is LineConnector)
                        {
                            (o as LineConnector).LabelHorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                            (o as LineConnector).LabelVerticalAlignment = System.Windows.VerticalAlignment.Top;
                        }
                    }
                    break;
                case "TopCenter":
                    m_actualTextStyles.LabelHorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    m_actualTextStyles.LabelVerticalAlignment = System.Windows.VerticalAlignment.Top;
                    foreach (object o in ErdControlDiagramView.SelectionList)
                    {
                        if (o is Node)
                        {
                            (o as Node).LabelHorizontalTextAlignment = System.Windows.HorizontalAlignment.Center;
                            (o as Node).LabelVerticalTextAlignment = System.Windows.VerticalAlignment.Top;
                        }
                        else if (o is LineConnector)
                        {
                            (o as LineConnector).LabelHorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                            (o as LineConnector).LabelVerticalAlignment = System.Windows.VerticalAlignment.Top;
                        }
                    }
                    break;
                case "TopRight":
                    m_actualTextStyles.LabelHorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                    m_actualTextStyles.LabelVerticalAlignment = System.Windows.VerticalAlignment.Top;
                    foreach (object o in ErdControlDiagramView.SelectionList)
                    {
                        if (o is Node)
                        {
                            (o as Node).LabelHorizontalTextAlignment = System.Windows.HorizontalAlignment.Right;
                            (o as Node).LabelVerticalTextAlignment = System.Windows.VerticalAlignment.Top;
                        }
                        else if (o is LineConnector)
                        {
                            (o as LineConnector).LabelHorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                            (o as LineConnector).LabelVerticalAlignment = System.Windows.VerticalAlignment.Top;
                        }
                    }
                    break;
                case "CenterLeft":
                    m_actualTextStyles.LabelHorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    m_actualTextStyles.LabelVerticalAlignment = System.Windows.VerticalAlignment.Center;
                    foreach (object o in ErdControlDiagramView.SelectionList)
                    {
                        if (o is Node)
                        {
                            (o as Node).LabelHorizontalTextAlignment = System.Windows.HorizontalAlignment.Left;
                            (o as Node).LabelVerticalTextAlignment = System.Windows.VerticalAlignment.Center;
                        }
                        else if (o is LineConnector)
                        {
                            (o as LineConnector).LabelHorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                            (o as LineConnector).LabelVerticalAlignment = System.Windows.VerticalAlignment.Center;
                        }
                    }
                    break;
                case "CenterCenter":
                    m_actualTextStyles.LabelHorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    m_actualTextStyles.LabelVerticalAlignment = System.Windows.VerticalAlignment.Center;
                    foreach (object o in ErdControlDiagramView.SelectionList)
                    {
                        if (o is Node)
                        {
                            (o as Node).LabelHorizontalTextAlignment = System.Windows.HorizontalAlignment.Center;
                            (o as Node).LabelVerticalTextAlignment = System.Windows.VerticalAlignment.Center;
                        }
                        else if (o is LineConnector)
                        {
                            (o as LineConnector).LabelHorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                            (o as LineConnector).LabelVerticalAlignment = System.Windows.VerticalAlignment.Center;
                        }
                    }
                    break;
                case "CenterRight":
                    m_actualTextStyles.LabelHorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                    m_actualTextStyles.LabelVerticalAlignment = System.Windows.VerticalAlignment.Center;
                    foreach (object o in ErdControlDiagramView.SelectionList)
                    {
                        if (o is Node)
                        {
                            (o as Node).LabelHorizontalTextAlignment = System.Windows.HorizontalAlignment.Right;
                            (o as Node).LabelVerticalTextAlignment = System.Windows.VerticalAlignment.Center;
                        }
                        else if (o is LineConnector)
                        {
                            (o as LineConnector).LabelHorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                            (o as LineConnector).LabelVerticalAlignment = System.Windows.VerticalAlignment.Center;
                        }
                    }
                    break;
                case "BottomLeft":
                    m_actualTextStyles.LabelHorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    m_actualTextStyles.LabelVerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                    foreach (object o in ErdControlDiagramView.SelectionList)
                    {
                        if (o is Node)
                        {
                            (o as Node).LabelHorizontalTextAlignment = System.Windows.HorizontalAlignment.Left;
                            (o as Node).LabelVerticalTextAlignment = System.Windows.VerticalAlignment.Bottom;
                        }
                        else if (o is LineConnector)
                        {
                            (o as LineConnector).LabelHorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                            (o as LineConnector).LabelVerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                        }
                    }
                    break;
                case "BottomCenter":
                    m_actualTextStyles.LabelHorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    m_actualTextStyles.LabelVerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                    foreach (object o in ErdControlDiagramView.SelectionList)
                    {
                        if (o is Node)
                        {
                            (o as Node).LabelHorizontalTextAlignment = System.Windows.HorizontalAlignment.Center;
                            (o as Node).LabelVerticalTextAlignment = System.Windows.VerticalAlignment.Bottom;
                        }
                        else if (o is LineConnector)
                        {
                            (o as LineConnector).LabelHorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                            (o as LineConnector).LabelVerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                        }
                    }
                    break;
                case "BottomRight":
                    m_actualTextStyles.LabelHorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                    m_actualTextStyles.LabelVerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                    foreach (object o in ErdControlDiagramView.SelectionList)
                    {
                        if (o is Node)
                        {
                            (o as Node).LabelHorizontalTextAlignment = System.Windows.HorizontalAlignment.Right;
                            (o as Node).LabelVerticalTextAlignment = System.Windows.VerticalAlignment.Bottom;
                        }
                        else if (o is LineConnector)
                        {
                            (o as LineConnector).LabelHorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                            (o as LineConnector).LabelVerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                        }
                    }
                    break;
            }
            Select(buttonContent);
        }

        //Customize the Selection
        private void Select(string buttonContent)
        {
            topLeft.IsSelected = false;
            topCenter.IsSelected = false;
            topRight.IsSelected = false;

            centerLeft.IsSelected = false;
            Center.IsSelected = false;
            centerRight.IsSelected = false;

            BottomLeft.IsSelected = false;
            BottomCenter.IsSelected = false;
            BottomRight.IsSelected = false;
            switch (buttonContent)
            {
                case "TopLeft":
                    topLeft.IsSelected = true;
                    break;
                case "TopCenter":
                    topCenter.IsSelected = true;
                    break;
                case "TopRight":
                    topRight.IsSelected = true;
                    break;
                case "CenterLeft":
                    centerLeft.IsSelected = true;
                    break;
                case "CenterCenter":
                    Center.IsSelected = true;
                    break;
                case "CenterRight":
                    centerRight.IsSelected = true;
                    break;
                case "BottomLeft":
                    BottomLeft.IsSelected = true;
                    break;
                case "BottomCenter":
                    BottomCenter.IsSelected = true;
                    break;
                case "BottomRight":
                    BottomRight.IsSelected = true;
                    break;
            }
        }

        //Customize the LabelForeground
        private void foreGroundColor_ColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            m_actualTextStyles.LabelForeground = new SolidColorBrush((Color)e.NewValue);
            foreach (object o in ErdControlDiagramView.SelectionList)
            {
                if (o is LineConnector)
                {
                    (o as LineConnector).LabelForeground = new SolidColorBrush((Color)e.NewValue);
                }
            }
        }

        #endregion

        #region Helper Methods

        //Creating the Node
        private Node addNode(String name, double offsetx, double offsety, Int32 level, Shapes shape, String label, String tooltip, double width, double height, string CustomStyle)
        {
            Node node = new Node(Guid.NewGuid(), name);
            node.OffsetX = offsetx;
            node.OffsetY = offsety;
            node.Tag = "IsFirstLoaded";
            node.Level = level;
            node.Shape = shape;
            node.Label = label;
            node.ToolTip = tooltip;
            node.Width = width;
            node.Height = height;
            //SetStyle(CustomStyle, node);
            var h = this.ShapeStyle;
            ErdDiagramModel.Nodes.Add(node);
            return node;
        }

        //Apply style to the Node
        private void SetStyle(string style, Node node)
        {
            Style styles = this.Resources[style] as Style;
            ShapeStyles shape = new ShapeStyles();
            foreach (Setter set in styles.Setters)
            {
                if (set.Property == System.Windows.Shapes.Path.StrokeProperty)
                {
                    shape.Stroke = (Brush)set.Value;

                }
                else if (set.Property == System.Windows.Shapes.Path.FillProperty)
                {
                    shape.Fill = (Brush)set.Value;
                }
                else if (set.Property == System.Windows.Shapes.Path.StrokeThicknessProperty)
                {
                    shape.StrokeThickness = (double)set.Value;
                }
            }
            shape.IsPreview = false;
            SetShapeStyle(node, shape);
        }

        //Create the connections
        private void Connect(Node headnode, Node tailnode, DecoratorShape headdecorator, DecoratorShape taildecorator)
        {
            LineConnector conn = new LineConnector();
            conn.HeadNode = headnode;
            conn.TailNode = tailnode;
            conn.HeadDecoratorShape = headdecorator;
            conn.TailDecoratorShape = taildecorator;
            conn.ConnectorType = ConnectorType.Straight;
            ErdDiagramModel.Connections.Add(conn);
        }
        //Populate the combobox with FontFamilies
        private void InitializeFontComboBox(RibbonComboBox ribbonComboBox)
        {
            ribbonComboBox.Items.Clear();
            foreach (FontFamily fontFamily in Fonts.SystemFontFamilies)
            {
                RibbonComboBoxItem item = new RibbonComboBoxItem();
                item.Content = fontFamily;
                ribbonComboBox.Items.Add(item);
            }
            ribbonComboBox.SelectedIndex = 0;
        }

        //Populate the combobox with FontSIzes
        private void InitializeFontSizeBox(RibbonComboBox ribbonComboBox)
        {
            ribbonComboBox.Items.Clear();
            int[] fontSize = new int[15] { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 28, 36, 48, 72 };
            int count = fontSize.Length;
            for (int i = 0; i < count; i++)
            {
                RibbonComboBoxItem ribbonComboBoxItem = new RibbonComboBoxItem();
                ribbonComboBoxItem.Content = fontSize[i];
                ribbonComboBox.Items.Add(ribbonComboBoxItem);
            }
            ribbonComboBox.SelectedIndex = 3;
        }

        //Apply themes
        private void InitializeGallary()
        {
            List<string> items = getItems();
            items.Add("Default");
            this.ThemeStyle.ItemsSource = items;

            items = getItems();
            items.Reverse();
            items.Add("Default");
            this.ShapeStyle.ItemsSource = items;
        }

        //Discover the color
        private double FindColor(Color findColor)
        {
            double R = findColor.R;
            double G = findColor.G;
            double B = findColor.B;
            double result = R / 255 + G / 255 + B / 255;
            return result;
        }

        //Paint 
        private void PreviewClosing_MouseLeave(object sender, MouseEventArgs e)
        {
            PaintSelection(StyleStatus.Cancel);
        }

        private void PaintSelection(StyleStatus status)
        {
            switch (status)
            {
                case StyleStatus.Preview:
                    if (ErdControlDiagramView.SelectionList != null && ErdControlDiagramView.SelectionList.Count > 0)
                    {
                        foreach (Node n in ErdControlDiagramView.SelectionList.OfType<Node>())
                        {
                            MainWindow.SetPreviewShapeStyle(n as Node, m_previewShapeStyles.Clone());
                        }
                    }
                    break;
                case StyleStatus.Ok:
                    if (ErdControlDiagramView.SelectionList != null && ErdControlDiagramView.SelectionList.Count > 0)
                    {
                        foreach (Node n in ErdControlDiagramView.SelectionList.OfType<Node>())
                        {
                            MainWindow.SetPreviewShapeStyle(n as Node, null);
                            MainWindow.SetShapeStyle(n as Node, m_actualShapeStyles.Clone());
                        }
                    }
                    break;
                case StyleStatus.Cancel:
                    if (ErdControlDiagramView.SelectionList != null && ErdControlDiagramView.SelectionList.Count > 0)
                    {
                        foreach (Node n in ErdControlDiagramView.SelectionList.OfType<Node>())
                        {
                            MainWindow.SetPreviewShapeStyle(n as Node, null);
                            m_previewShapeStyles.SetProperties(n);
                        }
                    }
                    break;
                case StyleStatus.Clear:
                    if (ErdControlDiagramView.SelectionList != null && ErdControlDiagramView.SelectionList.Count > 0)
                    {
                        foreach (Node n in ErdControlDiagramView.SelectionList.OfType<Node>())
                        {
                            MainWindow.SetPreviewShapeStyle(n as Node, null);
                            MainWindow.SetShapeStyle(n as Node, null);
                        }
                    }
                    break;
            }
        }

        //Updating the preview styles
        private void UpdatePreviewStyle(Node n)
        {
            m_previewShapeStyles.SetProperties(n);
            m_actualShapeStyles.SetProperties(n);
            m_actualTextStyles.SetProperties(n);
            if (m_actualTextStyles.LabelFontStyle == FontStyles.Italic)
            {
                Italic.IsSelected = true;
            }
            else
            {
                Italic.IsSelected = false;
            }
            if (m_actualTextStyles.LabelFontWeight == FontWeights.Bold || m_actualTextStyles.LabelFontWeight == FontWeights.SemiBold)
            {
                Bold.IsSelected = true;
            }
            else
            {
                Bold.IsSelected = false;
            }
            foreGroundColor.Color = (m_actualTextStyles.LabelForeground as SolidColorBrush).Color;
            fontNameBox.SelectedItem = fontNameBox.Items.OfType<ComboBoxItem>().Where(item => item.Content.Equals(m_actualTextStyles.LabelFontFamily)).First();
            fontSizeBox.SelectedItem = fontSizeBox.Items.OfType<ComboBoxItem>().Where(item => item.Content.Equals((int)m_actualTextStyles.LabelFontSize)).First();
            Select(m_actualTextStyles.LabelVerticalAlignment.ToString() + m_actualTextStyles.LabelHorizontalAlignment.ToString());
            if (m_actualTextStyles.LabelTextDecorations == TextDecorations.Underline)
            {
                Underline.IsSelected = true;
            }
            else
            {
                Underline.IsSelected = false;
            }
        }

        private List<string> getItems()
        {
            return new List<string>()
            {
                "OutLineBlack", "OutLineBlue", "OutLineRed", "OutLineOliveGreen", "OutLinePurple", "OutLineAqua", "OutLineOrange", "FillBlack", "FillBlue", "FillRed", "FillOliveGreen", "FillPurple","FillAqua", "FillOrange", "OutLineFillBlack", "OutLineFillBlue", "OutLineFillRed", "OutLineFillOliveGreen", "OutLineFillPurple", "OutLineFillAqua", "OutLineFillOrange", "SubtleEffectBlack", "SubtleEffectBlue", "SubtleEffectRed", "SubtleEffectOliveGreen", "SubtleEffectPurple", "SubtleEffectAqua", "SubtleEffectOrange", "ModerateEffectBlack", "ModerateEffectBlue", "ModerateEffectRed", "ModerateEffectOliveGreen", "ModerateEffectPurple", "ModerateEffectAqua", "ModerateEffectOrange"
            };
        }

        #endregion

        #region Customize the Ribbon

        private void ribbonwindow_Loaded(object sender, RoutedEventArgs e)
        {
            SkinStorage.SetVisualStyle(this, "Office2010Blue");
            /* foreach (var Node in ErdDiagramBuilder.Model.Nodes)
             {
                 var N = Node as Node;
                 SetStyle("OutLineBlueStyle", N);
             }*/
             

        }

        private void OnBlackAndWhiteClick(object sender, RoutedEventArgs e)
        {
            m_nativePrintDialog.PrintTicket.OutputColor = OutputColor.Monochrome;
        }

        private void OnColorClick(object sender, RoutedEventArgs e)
        {
            m_nativePrintDialog.PrintTicket.OutputColor = OutputColor.Color;
        }

        internal void UpdatePreview(FrameworkElement element, Stretch s)
        {
            m_elementToPrint = element;
            VisualBrush visualBrush = new VisualBrush(element);
            visualBrush.Stretch = s;
            visualBrush.ViewboxUnits = BrushMappingMode.Absolute;
            visualBrush.Viewbox = new Rect(0, 0, m_elementToPrint.ActualWidth, m_elementToPrint.ActualHeight);
            m_visualBrush = visualBrush;
            this.PreviewRect.Fill = visualBrush;
        }

        private void ContactUs_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.syncfusion.com/Account/Logon");
        }

        //Feature overview
        private void EssentialFeature_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.syncfusion.com/products/user-interface-edition/wpf/diagram/features%20overview");
        }

        //Support
        private void Support_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://help.syncfusion.com/ug_91/User%20Interface/WPF/Diagram/index.htm");
        }



        private void OnBlankDocument(object sender, RoutedEventArgs e)
        {
            blankDocument.IsSelected = true;

        }

        private void OnClearLog(object sender, RoutedEventArgs e)
        {
           //disArea.Children.Clear();
        }
        #endregion

        private void lstFirstTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ErdLineConnector lc = ErdDiagramModel.savedconnector;
            //Node n = ErdDiagramModel.GetNodeByName((lstFirstTable.SelectedItem as ComboBoxItem).Content.ToString());
           // lc.HeadNode = n;
           
        }

        private void lstSecondTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ErdLineConnector lc = ErdDiagramModel.savedconnector;
            //Node n = ErdDiagramModel.GetNodeByName((lstFirstTable.SelectedItem as ComboBoxItem).Content.ToString());
            //lc.TailNode = n;
        }

       

        private  void FillTableInterface(string path)
        {
            StreamWriter swTable = new StreamWriter(System.IO.Path.Combine(path, "ITable.cs"));
            swTable.WriteLine("namespace GeneratedDatabase {");
            swTable.WriteLine("public interface ITable }");
            swTable.Close();
        }
        private void FillCommandClass(string path)
        {
            StreamWriter swTable = new StreamWriter(System.IO.Path.Combine(path, "Command.cs"));
            swTable.WriteLine("namespace GeneratedDatabase {");
            swTable.WriteLine("public abstract class Command");
            swTable.WriteLine("{");
            swTable.WriteLine("protected IXMLManager manager;");
            swTable.WriteLine("public abstract void Execute();");
            swTable.WriteLine("}");
            swTable.WriteLine("}");
            swTable.Close();
        }
        private void FillAddCommandClasses(string path)
        {
            StreamWriter swTable = new StreamWriter(System.IO.Path.Combine(path, "AddCommand.cs"));
            swTable.WriteLine("namespace GeneratedDatabase {");
            swTable.WriteLine("public  class AddCommand:Command");
            swTable.WriteLine("{");
            swTable.WriteLine("public  AddCommand(IXMLManager manager)");
            swTable.WriteLine("{");
            swTable.WriteLine("this.manager=manager");
            swTable.WriteLine("}");
            swTable.WriteLine("}");
            swTable.WriteLine("}");
            swTable.Close();
        }
        private void FillUpdateCommandClasses(string path)
        {
            StreamWriter swTable = new StreamWriter(System.IO.Path.Combine(path, "UpdateCommand.cs"));
            swTable.WriteLine("namespace GeneratedDatabase {");
            swTable.WriteLine("public  class UpdateCommand:Command");
            swTable.WriteLine("{");
            swTable.WriteLine("public  AddCommand(IXMLManager manager)");
            swTable.WriteLine("{");
            swTable.WriteLine("this.manager=manager");
            swTable.WriteLine("}");
            swTable.WriteLine("}");
            swTable.WriteLine("}");
            swTable.Close();
        }
        private void FillDeleteCommandClasses(string path)
        {
            StreamWriter swTable = new StreamWriter(System.IO.Path.Combine(path, "DeleteCommand.cs"));
            swTable.WriteLine("namespace GeneratedDatabase {");
            swTable.WriteLine("public  class DeleteCommand:Command");
            swTable.WriteLine("{");
            swTable.WriteLine("public  DeleteCommand(IXMLManager manager)");
            swTable.WriteLine("{");
            swTable.WriteLine("this.manager=manager");
            swTable.WriteLine("}");
            swTable.WriteLine("}");
            swTable.WriteLine("}");
            swTable.Close();
        }
        private  void FillFiles(string path, StreamWriter swPrimary, List<StringBuilder> lst, List<StringBuilder> lstEvents, DiagramContent.Table t)
        {
            swPrimary.WriteLine(string.Format("public ObservableCollection<{0}> {0}_Values;", t.Name));
            FillPrimaryContent(path, lst, lstEvents, t);
            StreamWriter sw = new StreamWriter(System.IO.Path.Combine(path, t.Name + ".cs"));
            sw.WriteLine("using System.ComponentModel;");
            sw.WriteLine("namespace GeneratedDatabase");
            sw.WriteLine("{");
            sw.WriteLine(string.Format("public class {0}:INotifyPropertyChanged, ITable {{", t.Name));
            FillProperties(t, sw);
            sw.WriteLine("}");
            sw.WriteLine("}");
            sw.Close();
        }

        private void FillPrimaryContent(string path, List<StringBuilder> lst, List<StringBuilder> lstEvents, DiagramContent.Table t)
        {
            StringBuilder sb = new StringBuilder();
            FillFirstPart(path, lstEvents, t, sb);
            sb.AppendFormat("foreach(XElement xeProperty in xee.Elements(\"Property\"))");
            sb.AppendLine();
            sb.AppendFormat("{{");
            sb.AppendLine();
            sb.AppendFormat("string name=xeProperty.Attribute(\"Name\");");
            sb.AppendLine();
            sb.AppendFormat("string value=xeProperty.Attribute(\"Value\");");
            sb.AppendLine();
            sb.AppendFormat(" var property = type.GetProperty(name);");
            sb.AppendLine();
            sb.AppendFormat("property.SetValue(elem, value);");
            sb.AppendLine();
            sb.AppendFormat("}}");
            sb.AppendLine();
            sb.AppendFormat("}}");
            sb.AppendLine();
            sb.AppendFormat("}}");
            sb.AppendLine();
            lst.Add(sb);
        }

        private void FillFirstPart(string path, List<StringBuilder> lstEvents, DiagramContent.Table t, StringBuilder sb)
        {
            sb.AppendFormat("{0}_Values=new ObservableCollection<{0}>();", t.Name);
            sb.AppendLine();
            sb.AppendFormat("{0}_Values.CollectionChanged+={0}_CollectionChanged;", t.Name);
            FillEvents(lstEvents, t);
            sb.AppendLine();
            sb.AppendFormat("XDocument xd=XDocument.Load(\"{0}{1}.xml\");", path.Replace("\\", "\\\\"), t.Name);
            sb.AppendLine();
            sb.AppendFormat("XElement xe=xd.Root.Data;");
            sb.AppendLine();
            sb.AppendFormat("if (xe!=null){{");
            sb.AppendLine();
            sb.AppendFormat("foreach(XElement xee in xe.Elements(\"Element\"))");
            sb.AppendLine();
            sb.AppendFormat("{{");
            sb.AppendLine();
            sb.AppendFormat("{0} elem=new {0}();", t.Name);
            sb.AppendLine();
            sb.AppendFormat(" var type = elem.GetType();");
            sb.AppendLine();
        }

        private static void FillProperties(DiagramContent.Table t, StreamWriter sw)
        {
            foreach (DiagramContent.Properties p in t.Properties)
            {
                switch (p.Type)
                {
                    case "int":
                        sw.WriteLine(string.Format("private int {0};", p.Name.ToLower()));
                        sw.WriteLine(string.Format("public int {1} {{get {{return {0};}} set {{{0}=value;}}}}", p.Name.ToLower(), p.Name.ToUpper()));
                        break;
                    case "string":
                        sw.WriteLine(string.Format("private string {0};", p.Name.ToLower()));
                        sw.WriteLine(string.Format("public string {1} {{get {{return {0};}} set {{{0}=value;}}}}", p.Name.ToLower(), p.Name.ToUpper()));
                        break;
                }
            }
        }

        private void FillEvents(List<StringBuilder> lstEvents, DiagramContent.Table t)
        {
            StringBuilder sbEvent = new StringBuilder();
            sbEvent.AppendFormat("private void {0}_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)", t.Name);
            sbEvent.AppendLine();
            sbEvent.Append("{");
            sbEvent.AppendLine();
            sbEvent.Append("}");
            lstEvents.Add(sbEvent);
        }

        private void F_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ItemsControl_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            var k = e.OriginalSource;
        }

        private void Header_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            var k = e.OriginalSource;
        }
        int i = 0;
        private void Header_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            var k = e.OriginalSource;
            var t=BindingOperations.GetBindingExpression(e.Source as DependencyObject, e.Property);
            DiagramContent.Table table = t.DataItem as DiagramContent.Table;
            Header h = sender as Header;
            string previousTableName= h.PreviousValue;
            if (table.Name != previousTableName)
            {
                ChangeManager.Manager.Add(table, State.Add);
                DiagramContent.Table tableo = new DiagramContent.Table();

                tableo.Name = previousTableName;
                ChangeManager.Manager.Add(tableo, State.Delete);
            }
        }

        private void ErdDiagramBuilder_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            var k = e.OriginalSource;
            var t = BindingOperations.GetBindingExpression(e.Source as DependencyObject, e.Property);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ErdDiagramBuilder.EditState = true;
            ErdDiagramBuilder.NumItemsEdited += 2;
            MenuItem ic = e.OriginalSource as MenuItem;
            var a = ic.Parent as ContextMenu;
            ItemsControl icc = a.PlacementTarget as ItemsControl;
            var k=icc.FindVisualParent<Node>();
            //  k.Height = k.Height + 30;
            var node = a.PlacementTarget as Node;
            node.Height = node.Height + 20;
            
            DiagramContent.Table tt = node.Content as DiagramContent.Table;
            int num = tt.Properties.Count + 1;
            node.Ports.Add(new ConnectionPort(node, new Point(node.Width, node.Height-15)));
            DiagramContent.Properties t = new DiagramContent.Properties();
            t.Name = "";
            t.Type = "";
            ItemsControl ii=WpfUIUtilities.FindVisualChildByName<ItemsControl>(node, "PropertyList");
            (ii.ItemsSource as IList).Add(t);
            //tt.Properties.Add(t);
            ChangeManager.Manager.Add(tt, State.Update);
        }

        private void BodyElement_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            BodyElement be = e.OriginalSource as BodyElement;
            
            Node elc = GetParent<Node>(e.OriginalSource as Visual);
            double left = elc.Width;
            double top = 45;
            DiagramContent.Table tt = elc.Content as DiagramContent.Table;
            for (int i=0; i<tt.Properties.Count; i++)
            {
                top += 30 * i;
                DiagramContent.Properties p = tt.Properties[i];
                var ports=elc.Ports.Where(x => x.Name == (i + 1).ToString());
                if (ports.Count()==0)
                {
                    elc.Ports.Add(new ConnectionPort(elc, new Point(left, top)) { Name = (i + 1).ToString() });
                }
            }
           
            ChangeManager.Manager.Add(elc.Content as DiagramContent.Table, State.Update);
        }
        private bool no_generation = true;
        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            
            FolderBrowser fb = new FolderBrowser();
            fb.DialogTitle = "Choose Folder for generated c# types";
            
          //  fb.SelectedChanged += Fb_SelectedChanged;
            bool ?k=fb.ShowDialog();
            if (no_generation)
            {
                lstGenerationInfo.Items.Clear();
                no_generation = false;
            }
            if (k.Value)
                {
                    Structure st = Structure.Struct;
                    Model m = st.model;
                    if (fb.SelectedDirectory != null)
                    {
                        ClassGenerator.CodeCreator cgm = new ClassGenerator.CodeCreator(fb.SelectedDirectory);
                        cgm.Build();

                        lstGenerationInfo.Items.Add(string.Format("Successfully generated files to directory: {0}", fb.SelectedDirectory));
                    MessageBox.Show("Generation successfully, see in bottom the directory details in last record", "Generation Success", MessageBoxButton.OK);
                }

                }
                else MessageBox.Show("Sorry you haven't selected directory for generation, please click on Generate button again", "Generation No Folder", MessageBoxButton.OK);
         
        }

        private void Fb_SelectedChanged(FolderBrowser sender, Syncfusion.Windows.Tools.FolderBrowserSelectionChangedEventArgs e)
        {
            /*Structure st = Structure.Struct;
            Model m = st.model;
            ClassGenerator.CodeCreator cgm = new ClassGenerator.CodeCreator(sender.SelectedDirectory);
            cgm.Build();*/
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StackPanel spTemplateContainer = VisualTreeHelper.GetParent(e.OriginalSource as DependencyObject) as StackPanel;
            StackPanel spSecondRelationContainer = spTemplateContainer.Children[3] as StackPanel;
            ListBox lstFirstTable = spTemplateContainer.FindName("FirstTable") as ListBox;
            ListBox lstSecondTable = spTemplateContainer.FindName("SecondTable") as ListBox;
            ListBox lstFirstRelation = spTemplateContainer.FindName("FirstRelation") as ListBox;
            ListBox lstSecondRelation = spTemplateContainer.FindName("SecondRelation") as ListBox;
            string firstTableValue = lstFirstTable.SelectedValue.ToString(); ;
            string secondTableValue = lstSecondTable.SelectedValue.ToString();
            string firstRelationValue = lstFirstRelation.SelectedValue.ToString();
            string secondRelationValue = lstSecondRelation.SelectedValue.ToString();
            Structure st = Structure.Struct;
            Model m = st.model;
            Connection c = new Connection();
            c.FirstTable = firstTableValue;
            c.FirstRelation = firstRelationValue;
            c.SecondRelation = secondRelationValue;
            c.SecondTable = secondTableValue;
            m.connections.Add(c);
            Visual v = e.Source as Visual;
            ErdLineConnector elc = GetParent<ErdLineConnector>(v);
            elc.Label = string.Format("{0}({1})-{2}({3})", firstTableValue, firstRelationValue, secondTableValue, secondRelationValue);
            elc.DataContext = c;
            elc.Style = ErdDiagramModel.ConnectorStyle;
            elc.ApplyTemplate();
            ChangeManager.Manager.Add(c, State.Add);
        }

        private T GetParent<T>  (Visual v) where T :class
        {
            while (v != null)
            {
                v = VisualTreeHelper.GetParent(v) as Visual;
                if (v is T)
                    break;
            }
            return v as T;
        }

        private void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            StackPanel sp = sender as StackPanel;
            ComboBox cmbFirstTable=sp.FindName("FirstTable") as ComboBox;
            cmbFirstTable.ItemsSource = Structure.Struct.model.tables;
            ComboBox cmbSecondTable = sp.FindName("SecondTable") as ComboBox;
            cmbSecondTable.ItemsSource = Structure.Struct.model.tables;

        }

        private void Create_Connection(object sender, RoutedEventArgs e)
        {
            MenuItem mnu = sender as MenuItem;
            Node node = null;
            if (mnu != null)
            {
                node = ((ContextMenu)mnu.Parent).PlacementTarget as Node;
                ErdDiagramModel m = ErdDiagramModel;
                string name=m.GetNodeName(node);
                ConnectorProperties cp = new ConnectorProperties(name);

            }
        }

        private void ConnectionPropertyAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnNodeApply_Click(object sender, RoutedEventArgs e)
        {
            StackPanel spTemplateContainer = VisualTreeHelper.GetParent(e.OriginalSource as DependencyObject) as StackPanel;
            TextBox txtTableName = spTemplateContainer.FindName("TableName") as TextBox;
            TextBox txtTableNumFields = spTemplateContainer.FindName("NumProperties") as TextBox;
            Node node = GetParent<Node>(spTemplateContainer);
           
            DiagramContent.Table table = new DiagramContent.Table();
            table.Name = txtTableName.Text;
            table.Properties = new System.Collections.ObjectModel.ObservableCollection<DiagramContent.Properties>();
            int numFields;
            if (!int.TryParse(txtTableNumFields.Text, out numFields))
            {
                return;
            }
            for(int i=0; i<numFields; i++)
            {
                DiagramContent.Properties p = new DiagramContent.Properties();
                table.Properties.Add(p);
            }
            Structure.Struct.model.tables.Add(table);
            ChangeManager.Manager.Add(table, State.Add);
            node.Content = table;
            node.ContentTemplate = ErdDiagramModel.ItemTemplate;
            node.ApplyTemplate();
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void tbFirstTableRelation_MouseDown(object sender, MouseButtonEventArgs e)
        {
           
                TextBlock tb = sender as TextBlock;
                StackPanel sp = tb.Parent as StackPanel;
                ComboBox tbb = sp.FindName("txbFirstTableRelation") as ComboBox;
                tbb.Visibility = Visibility.Visible;
                tb.Visibility = Visibility.Collapsed;
            
        }

        private void txbFirstTableRelation_MouseDown(object sender, MouseButtonEventArgs e)
        {
          
                TextBlock tb = sender as TextBlock;
                StackPanel sp = tb.Parent as StackPanel;
                ComboBox tbb = sp.FindName("txbFirstTableRelation") as ComboBox;
                tb.Visibility = Visibility.Visible;
                tbb.Visibility = Visibility.Collapsed;
            
        }

        private void tbSecondTableRelation_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                TextBlock tb = sender as TextBlock;
                StackPanel sp = tb.Parent as StackPanel;
                ComboBox tbb = sp.FindName("lstSecondTableRelation") as ComboBox;
                tbb.Visibility = Visibility.Visible;
                tb.Visibility = Visibility.Collapsed;
            }
        }

        private void lstFirstTableRelation_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                TextBlock tb = sender as TextBlock;
                StackPanel sp = tb.Parent as StackPanel;
                ComboBox tbb = sp.FindName("lstFirstTableRelation") as ComboBox;
                tbb.Visibility = Visibility.Visible;
                tb.Visibility = Visibility.Collapsed;
            }
        }

        private void lstSecondTableRelation_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                TextBlock tb = sender as TextBlock;
                StackPanel sp = tb.Parent as StackPanel;
                ComboBox tbb = sp.FindName("lstSecondTableRelation") as ComboBox;
                tbb.Visibility = Visibility.Visible;
                tb.Visibility = Visibility.Collapsed;
                
            }
        }

        private void LineConnectorUpdate_Click(object sender, RoutedEventArgs e)
        {
           
            MenuItem ic = e.OriginalSource as MenuItem;
            var a = ic.Parent as ContextMenu;
            var k = a.PlacementTarget;
            DiagramView de = new DiagramView();
           
            if (!(k is ErdLineConnector))
            {
                return;
            }
           ErdLineConnector lineConnector = a.PlacementTarget as ErdLineConnector;
            string name = lineConnector.ConnectorName;
            string firstTable = lineConnector.HeadNodeName;
            string firstRelation = lineConnector.HeadNodeRelation;
            string secondTable = lineConnector.TailNodeName;
            string secondRelation = lineConnector.TailNodeRelation;
            ConnectorProperties cp = new ConnectorProperties(name, firstTable, firstRelation, secondTable, secondRelation);
            cp.Closed += Cp_Update_Closed;
            cp.ShowDialog();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ChangeManager.Manager.Execute();
            MessageBox.Show("Saved Successfully", "Save Result", MessageBoxButton.OK);
        }

        private void TableHeader_Validate(object sender, ValueEventArgs e)
        {
           
            string k = e.Value;
            EditableStackPanel header = sender as EditableStackPanel;
            var n = Structure.Struct.model.tables.Where(x => x.Name == k);
            if (n.Count() > 0 && header.GetCurrentValue() != k)
            {
                string message = string.Format("Sorry, table with name: {0} already exist", k);
                MessageBox.Show(message, "Table Name Validation", MessageBoxButton.OK);
                return;
            }


            header.SaveValue();
        }

        private void TableBody_Validate(object sender, ValueEventArgs e)
        {
            
            string k = e.Value;
            EditableStackPanel header = sender as EditableStackPanel;
            Node elc = GetParent<Node>(sender as Visual);
            double left = elc.Width;
            double top = 45;
            DiagramContent.Table tt = elc.Content as DiagramContent.Table;
           
            var n = tt.Properties.Where(x => x.Name == k);
            if (n.Count() > 0 && header.GetCurrentValue() != k)
            {
                string message = string.Format("Sorry, table with name: {0} already contains property with name: {1}", tt.Name, k);
                MessageBox.Show(message, "Property Name Validation", MessageBoxButton.OK);
                return;
            }


            header.SaveValue();
        }

        private void Edit_Begin(object sender, EventArgs e)
        {
            ErdDiagramBuilder.EditState = true;
            ErdDiagramBuilder.NumItemsEdited++;
        }

        private void Edit_End(object sender, EventArgs e)
        {
            ErdDiagramBuilder.NumItemsEdited--;
            if (ErdDiagramBuilder.NumItemsEdited==0)
            ErdDiagramBuilder.EditState = false;
        }
    }


    #region Utils

    public enum StyleStatus
    {
        Preview,
        Ok,
        Cancel,
        Clear
    }

    //ColorToBrushConverter
    public class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color newBrush = (Color)value;
            return new SolidColorBrush(newBrush);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //BindableStaticResource
    public class BindableStaticResource : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var multiBinding = new MultiBinding();
            multiBinding.Bindings.Add(new Binding() { RelativeSource = new RelativeSource() { Mode = RelativeSourceMode.Self } });
            multiBinding.Bindings.Add(new Binding());
            multiBinding.Converter = new ResourceKeyToResourceConverter();
            return multiBinding.ProvideValue(serviceProvider);
        }
    }

    //ResourceKeyToResourceConverter
    class ResourceKeyToResourceConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length < 2)
            {
                return null;
            }
            var element = values[0] as FrameworkElement;
            var resourceKey = values[1];

            resourceKey = string.Format("{0}Style", resourceKey);
            var resource = element.TryFindResource(resourceKey);
            return resource;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #region Sorting

    //MySortDescription
    public class MySortDescription : IComparer
    {
        public List<string> OrderBy = new List<string>();

        public MySortDescription()
        {
            OrderBy.Add("OutLine");
            OrderBy.Add("Fill");
            OrderBy.Add("OutLineFill");
            OrderBy.Add("Subtle");
            OrderBy.Add("Moderate");
        }

        private int IndexOf(string data, List<string> list)
        {
            List<string> foundList = new List<string>();
            foreach (string o in list)
            {
                if (data.StartsWith(o))
                {
                    foundList.Add(o);
                }
            }
            if (foundList.Count > 0)
            {
                var k = foundList.Where(o => o.Length == foundList.Select(s => s.Length).Max()).Select(o => foundList.IndexOf(o)).First();
                return list.IndexOf(foundList[k]);
            }
            else
            {
                return -1;
            }
        }

        private int SubIndexOf(string data, List<string> list)
        {
            List<string> foundList = new List<string>();
            foreach (string o in list)
            {
                if (data.EndsWith(o))
                {
                    foundList.Add(o);
                }
            }
            if (foundList.Count > 0)
            {
                return list.IndexOf(foundList[foundList.Count - 1]);
            }
            else
            {
                return -1;
            }
        }

        public int Compare(object x, object y)
        {
            return Compare(x.ToString(), y.ToString());
        }

        private int Compare(string x, string y)
        {
            int a = IndexOf(x, OrderBy);
            int b = IndexOf(y, OrderBy);
            if (a < b)
            {
                return -1;
            }
            else if (a > b)
            {
                return +1;
            }
            else
            {
                List<string> subString = new List<string>();
                subString.Add("Black");
                subString.Add("Blue");
                subString.Add("Red");
                subString.Add("OliveGreen");
                subString.Add("Purple");
                subString.Add("Aqua");
                subString.Add("Orange");
                subString.Add("Default");
                subString.Add("Custom");

                int c = SubIndexOf(x, subString);
                int d = SubIndexOf(y, subString);
                if (c < d)
                {
                    return -1;
                }
                else if (c > d)
                {
                    return +1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}

    #endregion

    #endregion



