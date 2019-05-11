using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiagramContent;
using Utilities;

namespace FileManagement
{
    /** \ingroup Bll
     <summary>
     Implements management of crud, 
     validation and diagram creation.
         
     </summary>
    */
    public class EntityManager:IEntityManager
    {

        protected IManager manager;
        protected IEntity entity;
        
        public EntityManager()
        {

        }

        
        /// <summary>
        /// Validation management
        /// </summary>
        /// <param name="filename">FileName to check</param>
        /// <param name="format">Current system format(xml, json)</param>
        /// <returns>KeyValuePair-result in success/failure and messages</returns>
        public KeyValuePair<bool,Result> Validate(string filename, string format)
        {
            
            ManagerCreator mc = new ManagerCreator();
            manager = mc.CreateManager(filename, format);
            entity = new Entity(manager);
            return entity.Validate();
        }
        
        /// <summary>
        /// Manages diagram creation(creation of object that is binded to diagram control or other control
        /// that displays a diagram)
        /// </summary>
        public void CreateDiagram()
        {
            string entitytype = manager.GetEntityType();
            switch (entitytype.ToUpper())
            {
                case "TABLE": entity = new TableEntity(manager);
                    break;
                case "CONNECTION":
                    entity = new ConnectionEntity(manager);
                    break;
                default:
                    entity = new TableEntity(manager);
                    break;
            }
            if (entity is IConcreteEntity<Table>)
            {
                IConcreteEntity<Table> tableEntity = entity as IConcreteEntity<Table>;
                tableEntity.CreateDiagram();
            }
            else if (entity is IConcreteEntity<Connection>)
            {
                IConcreteEntity<Connection> connectionEntity = entity as IConcreteEntity<Connection>;
                connectionEntity.CreateDiagram();
            }
            else CreateDiagramForOtherOptions();
        }

        /// <summary>
        /// Override in descendants if you want 
        /// </summary>
        protected void CreateDiagramForOtherOptions()
        {
           
        }

        public bool Add<T>(T element, string format)
        {
            ManagerCreator mc = new ManagerCreator();
            IManager manager = null;
            string name;
            if (element is Table)
            {
                entity = new TableEntity(element as Table);
                name = (element as Table).Name;
            }
            else if (element is Connection)
            {
                entity = new ConnectionEntity(element as Connection);
                name = (element as Connection).Name;
            }
            else AddAnotherEntity(element, out name);
            IConcreteEntity<T> concreteentity = entity as IConcreteEntity<T>;
            manager = mc.CreateManager(name, format);
            concreteentity.Add(element, manager);
            return true;
        }

        /// <summary>
        /// This method is for adding another database object types
        /// and you can implement it in extend classes
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element">The type of database object</param>
        /// <param name="name">In what file to store it</param>
        protected void AddAnotherEntity<T>(T element, out string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update management
        /// </summary>
        /// <typeparam name="T">Type of the element</typeparam>
        /// <param name="element"></param>
        /// <returns>Update result</returns>
        public bool Update<T>(T element)
        {
            IConcreteEntity<T> concreteentity = entity as IConcreteEntity<T>;
            concreteentity.Update(element);
            return true;
        }

        /// <summary>
        /// Delete management
        /// </summary>
        /// <typeparam name="T">Type of the element</typeparam>
        /// <param name="element"></param>
        /// <returns>Delete result</returns>
        public bool Delete<T>(T element)
        {
            IConcreteEntity<T> concreteentity = entity as IConcreteEntity<T>;
            concreteentity.Delete(element);
            return true;
        }

        /// <summary>
        /// Load existing diagram
        /// </summary>
        /// <param name="filename">File that represent db object</param>
        /// <param name="format">Format -extension of file</param>
        /// <returns></returns>
        public bool SetDiagram(string filename, string format)
        {
           KeyValuePair<bool, Result> result=Validate(filename, format);
            if (result.Key != true)
                return result.Key;
            CreateDiagram();
            return true;
        }
    }
}
