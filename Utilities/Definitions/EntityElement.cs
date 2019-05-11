using System.Collections.Generic;

namespace Utilities.Definitions
{
    /** \ingroup Bll
     <summary>
     Recursive class that holds hierarchical structure(another cannot hold a table definition 
     with data) that holds structure of Entity(table or Connection)
     </summary>
    */
    public class EntityElement
    {
        /// <summary>
        /// Name-name of entity root element/current element
        /// </summary>
        public string name; 

        /// <summary>
        /// Definitions of inner elements-children of current element
        /// </summary>
        public List<EntityElement> entityelements;
        public bool Required { get; internal set; }
        /// <summary>
        /// Key-name of attribute value-its value
        /// </summary>
        public SortedList<string,string> attributes;
        public EntityElement(string name)
        {
            this.name = name;
            Required = true;
            entityelements = new List<EntityElement>();
            attributes = new SortedList<string,string>();
        }
        public EntityElement(string name, bool required)
        {
            this.name = name;
            Required = required;
            entityelements = new List<EntityElement>();
            attributes = new SortedList<string, string>();
        }
    }
}