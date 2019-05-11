using DiagramContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Utilities.Definitions;

namespace FileManagement
{
    /** \ingroup Bll
     <summary>
     Represents database connection as manageable object
     Implements crud operations and Validation for connection.
        Part of \ref Strategy 
     </summary>
    */
    public class ConnectionEntity : IConcreteEntity<Connection>
    {
        private Connection connection;
        private IManager manager;

        public ConnectionEntity(Connection connection)
        {
            this.connection = connection;
        }

        public ConnectionEntity(IManager manager)
        {
            this.manager = manager;
        }

        /// <summary>
        /// The connection
        /// </summary>
        public Connection Element
        {
            get
            ;

            set
          ;
        }

        

        

        
        /// <summary>
        /// Manages permanent store of added connection
        /// </summary>
        /// <param name="element">The connection</param>
        /// <param name="manager">Executes io operations for connection</param>
        public void Add(Connection element, IManager manager)
        {
            this.manager = manager;
            Update(element);
        }

        /// <summary>
        /// Creates connection elements that binded to control that displays diagram 
        /// </summary>
        public void CreateDiagram()
        {
            
            Connection c = new Connection();
            c.Name = manager.GetName();
            IEntityDefinition definition = SystemDefinitionsManager.DefinitionsManager.GetValidationDefinition(EntityTypesDefinition.Connection);
            if (!(definition is ConnectionDefinition))
                return;
            ConnectionDefinition connectionDefinition = definition as ConnectionDefinition;
            EntityElement connectionElement = manager.GetEntityElement(connectionDefinition.ConnectionTag);
            c.FirstTable = connectionElement.attributes.Where(x => x.Key == connectionDefinition.ConnectionFirstTableAttribute).First().Value;
            c.FirstRelation = connectionElement.attributes.Where(x => x.Key == connectionDefinition.ConnectionFirstRelationAttribute).First().Value;
            c.SecondTable = connectionElement.attributes.Where(x => x.Key == connectionDefinition.ConnectionSecondTableAttribute).First().Value;
            c.SecondRelation = connectionElement.attributes.Where(x => x.Key == connectionDefinition.ConnectionSecondRelationAttribute).First().Value;
          
            var m = Structure.Struct.model;
            m.connections.Add(c);
        }
    

       
        /// <summary>
        /// Manages the delete element
        /// </summary>
        /// <param name="element">Element to delete</param>
        public void Delete(Connection element)
        {
            manager.Delete();
        }

       
        /// <summary>
        /// Update of connection 
        /// </summary>
        /// <param name="element"></param>
        public void Update(Connection element)
        {
            EntityElement connectionElement = SystemDefinitionsManager.DefinitionsManager.GetDefinition(element);
            manager.Update(connectionElement);
        }

        
        /// <summary>
        /// Additional validation 
        /// can be overriden in descendant classes
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<bool, Result> Validate()
        {
            return new KeyValuePair<bool, Result>(true, new Result("success", "success"));
        }
    }
}
