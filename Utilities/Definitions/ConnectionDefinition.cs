using DiagramContent;
using System;

namespace Utilities.Definitions
{
    /** \ingroup Bll
     <summary>
     This class represents definition of structure of Connection
     in file that stores it.
     Inherits IEntityDefinition
     </summary>
    */
    public class ConnectionDefinition:IEntityDefinition
    {
        private ConnectionDefinition()
        {
            Element = new EntityElement("Connection");

            Element.attributes.Add(ConnectionFirstTableAttribute, "5");
            Element.attributes.Add(ConnectionFirstRelationAttribute, "5");
            Element.attributes.Add(ConnectionSecondTableAttribute, "5");
            Element.attributes.Add(ConnectionSecondRelationAttribute, "5");
            Element.attributes.Add(ConnectionNameAttribute, "5");
        }
        public ConnectionDefinition(Connection connection)
        {
            Element = new EntityElement("Connection");
            Element.attributes.Add(ConnectionFirstTableAttribute, connection.FirstTable);
            Element.attributes.Add(ConnectionFirstRelationAttribute, connection.FirstRelation);
            Element.attributes.Add(ConnectionSecondTableAttribute, connection.SecondTable);
            Element.attributes.Add(ConnectionSecondRelationAttribute, connection.SecondRelation);
            Element.attributes.Add(ConnectionNameAttribute, "5");
        }
        private static ConnectionDefinition _cdefinition;
        public static ConnectionDefinition CDefinition { get { if (_cdefinition == null) _cdefinition = new ConnectionDefinition(); return _cdefinition; } }

        public EntityElement Element
        {
            get
            ;

            set
            ;
        }
        /// <summary>
        /// Name of Attribute(or something else the origin is xml but can be json) that represents ConnectionName
        /// </summary>
        public string ConnectionNameAttribute { get { return connectionnameattribute; } }

        /// <summary>
        /// Name of Attribute(or something else the origin is xml but can be json) that represents Connection table
        /// </summary>
        public string ConnectionFirstTableAttribute { get { return connectionfirsttableattribute; } }
        /// <summary>
        ///   /// Name of Attribute(or something else the origin is xml but can be json) that represents Cardinality of relation
        /// </summary>
        public string ConnectionFirstRelationAttribute { get { return connectionfirstrelationattribute; } }

        /// <summary>
        /// Name of Attribute(or something else the origin is xml but can be json) that represents Connection table
        /// </summary>
        public string ConnectionSecondTableAttribute { get { return connectionsecondtableattribute; } }

        /// <summary>
        ///   /// Name of Attribute(or something else the origin is xml but can be json) that represents Cardinality of relation
        /// </summary>
        public string ConnectionSecondRelationAttribute { get { return connectionsecondrelationattribute; } }


        

        /// <summary>
        /// The tag that represents Connection
        /// </summary>
        public string ConnectionTag { get { return connectiontag; }  }
        protected string connectiontag = "Connection";
        protected string connectionnameattribute = "Name";
        protected string connectionfirsttableattribute= "FirstTable";
        protected string connectionfirstrelationattribute = "FirstRelation";
        protected string connectionsecondtableattribute = "SecondTable";
        protected string connectionsecondrelationattribute = "SecondRelation";
      
    }
}