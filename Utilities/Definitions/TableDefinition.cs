using DiagramContent;
using System;

namespace Utilities.Definitions
{
    /** \ingroup Bll
     <summary>
     This class represents definition of structure of Table
    in file that stores it.
     Inherits IEntityDefinition
     </summary>
    */
    public class TableDefinition:IEntityDefinition
    {
        public TableDefinition() {
            Element = new EntityElement("Table");
            //Element.attributes.Add(TableNameAttribute, "5");
            var PropertiesElement = new EntityElement(TablePropertiesTag);
            var PropertyElement = new EntityElement(TablePropertyTag, false);
            PropertyElement.attributes.Add(TablePropertyName, "5");
            PropertyElement.attributes.Add(TablePropertyType, "5");
            PropertiesElement.entityelements.Add(PropertyElement);
            
            Element.entityelements.Add(PropertiesElement);
        }
        public  TableDefinition(Table table)
        {
            Element = new EntityElement(TableTag);
            var PropertiesElement = new EntityElement(TablePropertiesTag);
            foreach (Properties p in table.Properties)
            {
                if (string.IsNullOrEmpty(p.Name) || string.IsNullOrEmpty(p.Type))
                    continue;
                var PropertyElement = new EntityElement(TablePropertyTag, false);
                PropertyElement.attributes.Add(TablePropertyName, p.Name);
                PropertyElement.attributes.Add(TablePropertyType, p.Type);
                PropertiesElement.entityelements.Add(PropertyElement);
            }
            Element.entityelements.Add(PropertiesElement);
        }
        private static TableDefinition _tdefinition;
        public static TableDefinition TDefinition { get { if (_tdefinition == null) _tdefinition = new TableDefinition(); return _tdefinition; } }

        public EntityElement Element
        {
            get
           ;

            set;
        }
        /// <summary>
        ///   /// <summary>
        /// Name of Attribute(or something else the origin is xml but can be json) that represents Primary key
        /// </summary>
        /// </summary>
        public string TablePropertyPrimary { get { return tablepropertyprimary; } }
       
        ///   /// <summary>
        /// Name of Attribute(or something else the origin is xml but can be json) that represents Table Name
        /// </summary>
        public string TableNameAttribute { get { return tablenameattribute; } }

        /// <summary>
        /// Name of Table Properties tag
        /// </summary>
        public string TablePropertiesTag { get { return tablepropertiestag; } }
        public string TablePropertyTag { get { return tablepropertytag; } }
        /// <summary>
        /// Name of property name attribute
        /// </summary>
        public string TablePropertyName { get { return tablepropertyname; } }
        public string TableTag { get { return tabletag; } }
        /// <summary>
        /// Name of property type attribute
        /// </summary>
        public string TablePropertyType { get { return tablepropertytype; } }
        protected string tablenameattribute = "Name";
        protected string tablepropertiestag = "Properties";
        protected string tablepropertytag = "Property";
        protected string tablepropertyname = "Name";
        protected string tablepropertytype = "Type";
        protected string tablepropertyprimary = "Primary";
        protected string tabletag = "Table";

    }
}