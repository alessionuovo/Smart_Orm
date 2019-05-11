using System.Collections.Generic;
using System.Xml.Linq;
using Utilities;
using Utilities.Definitions;

namespace FileManagement
{
    /** \ingroup Dal
   
        
    <summary>
     interface IManager-Executes validation and crud on file that represent db, diagram creation
     operations on db.
         Part of \ref Strategy 
        and part of \ref TemplateMethod 
     </summary>
    */
    public interface IManager
    {
        /// <summary>
        /// This method validates that the file is structured correctly
        /// </summary>
        /// <param name="slDefinitions">Defines the structure of all database object that system accepts, the key is the database object type, the value is structure definition</param>
        /// <returns>KeyValuePair-the key is success or failure, ther value is message/s of error</returns>
        KeyValuePair<bool,Result> Validate(SortedList<string, EntityElement> slDefinitions);
        bool Add(EntityElement dbObjectStructure);
        bool Update(EntityElement dbObjectStructure);
        bool Delete();
        //void CreateDiagramFromFile();
        string GetEntityType();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Name of database object(table, connection, etc.)</param>
        /// <returns>Hierarchical Structure of the element</returns>
        EntityElement GetEntityElement(string name);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Name of database object(table, connection, etc.)</param>
        /// <returns>Returns only the child level</returns>
        List<EntityElement> GetEntityElements(string name);
        string GetName();
        void DeleteItem();
        void UpdateItem(EntityElement connectionElement);
        EntityElement GetEntityElementItem(string connectionTag);
    }
    
}