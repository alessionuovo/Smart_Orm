using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Utilities;
using Utilities.Definitions;

namespace FileManagement
{
    /** \ingroup Dal
  
   
   <summary>
   Abstract class that manages io operations
   on files/database that represent database objects.
   All this depends on the concept is that format that can describe table/connection etc.
   is hierarchical (like xml and json).
   Also this class is part of template method and strategy design patterns
   that is done because the users will add another extensions of this class
   to support other formats.
         Part of \ref Strategy , \ref TemplateMethod
  </summary>
   <typeparam name="T">The type of elements that represents one/record in file
   or database fro example for xml format it is XElement for database table DataRow</typeparam>
   */
    public abstract class FileManager<T> : IManager
    {
        protected string directory;
        protected string entityType;
        protected XDocument xdDefinitions;
        protected EntityElement EntityDefinition;
        protected string extension;
        protected string file;
        protected string filename;
        
        /// <summary>
        /// Stores the update change permanently in the file
        /// </summary>
        /// <param name="e">Data of the table in hierarchical structure</param>
        /// <returns>success/failure</returns>
        public virtual bool Update(EntityElement e)
        {
            Queue<KeyValuePair<T,EntityElement>> definitionEntityElement = new Queue<KeyValuePair<T, EntityElement>>();
            T root = CreateRoot(e);
            KeyValuePair<T, EntityElement> kvp = new KeyValuePair<T, EntityElement>(root, e);
            definitionEntityElement.Enqueue(kvp);
            while (definitionEntityElement.Count != 0)
            {
                kvp = definitionEntityElement.Dequeue();
                CreateAttributes(kvp.Key, kvp.Value);
                
                foreach (EntityElement ee in kvp.Value.entityelements)
                {
                    T elem=CreateElement(ee);
                    AddElement(kvp.Key, elem);
                    kvp = new KeyValuePair<T, EntityElement>(elem, ee);
                    definitionEntityElement.Enqueue(kvp);
                }
            }
            Save();
            return true;
        }

        /// <summary>
        /// In descendant classes saves element permanently
        /// </summary>
        protected abstract void Save();
       
       
        protected abstract void AddElement(T key, T elem);
       

        protected abstract T CreateElement(EntityElement ee);

        /// <summary>
        /// Creates attribute in current element
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected abstract void CreateAttributes(T key, EntityElement value);




        protected abstract T CreateRoot(EntityElement e);


        public abstract bool Delete();
       
       
        

       
        /// <summary>
        /// Validates that file on which this class operates is in acceptable format for the system
        /// and this is what descendant classes must supply.
        /// Also this method acts as template method.
        /// So it calls to other methods that are implemented in descendant classes.
        /// </summary>
        /// <param name="slDefinitions">SortedList- The key is type of database object, value-hierarchical definition of a structure(applicable only when data is stored in files but not in db)</param>
        /// <returns></returns>
        public KeyValuePair<bool,Result> Validate(SortedList<string, EntityElement> slDefinitions)
        {
            
            
            bool checkValidity = CheckValidExtension();
            if (checkValidity)
            {
                entityType = GetEntityType(slDefinitions);
                if (!slDefinitions.ContainsKey(entityType))
                {
                    Result res = new Result("No legal entity", "File not represents table or connection");
                    return new KeyValuePair<bool,Result>(false, res);
                }
                EntityDefinition = slDefinitions[entityType];
                return CheckStructureAndContent();
            }
            Result r = new Result("File is with invalid extension", string.Format("File extension is: {0} but file is not structured like {0}", extension));
            return new KeyValuePair<bool, Result>(false, r);
        }

        /// <summary>
        /// Checks that file is built correctly for entity element
        /// <para>Because the file has hierarchical structure, it executes bfs search on structure tree/para>
        /// </summary>
        /// <returns>Result of check</returns>
        protected  KeyValuePair<bool, Result> CheckStructureAndContent()
        {
            Queue<KeyValuePair<T,EntityElement>> definitionEntityElement = new Queue<KeyValuePair<T,EntityElement>>();
           
            T root = GetFileRoot();
            T element=root;
            KeyValuePair<T, EntityElement> kvp = new KeyValuePair<T, EntityElement>(element, EntityDefinition);
            definitionEntityElement.Enqueue(kvp);
            while (definitionEntityElement.Count!=0)
            {
                kvp = definitionEntityElement.Dequeue();
                element = kvp.Key;
                if (element==null)
                {
                    if (kvp.Value.Required)
                    {
                        Result res = new Result("element is empty", string.Format("The element with name: {0} is empty", kvp.Value.name));
                        return new KeyValuePair<bool, Result>(false, res);
                    }
                    else continue;
                }
                KeyValuePair<bool, Result> result=CheckAttributes(kvp);
                if (!result.Key) return result;
                foreach(EntityElement currentElement in kvp.Value.entityelements)
                {
                    element = GetElement(element, currentElement);
                    if (element == null)
                    {
                        if (currentElement.Required)
                        {
                            Result res = new Result("element not found", string.Format("Element with name: {0} not found, he is mandatory", currentElement.name));
                            return new KeyValuePair<bool, Result>(false, res);
                        }
                        continue;
                    }
                    kvp = new KeyValuePair<T, EntityElement>(element, currentElement);
                    definitionEntityElement.Enqueue(kvp);
                }
            }
            Result r = new Result("success", "success");
            return new KeyValuePair<bool, Result>(true, r);
        }

        /// <summary>
        /// <para>Checks that current element(element is part of representantion of database object in the file) structure fits the definition.</para>
        /// <para></para>
        /// </summary>
        /// <param name="kvp">KeyValuePair in which the key is the current element in hierarchy, and value is definition of its internal structure</param>
        /// <returns>Validation Result -in terms of success/failure and messages</returns>
        protected KeyValuePair<bool, Result> CheckAttributes(KeyValuePair<T, EntityElement> kvp)
        {
            T element = kvp.Key;
            EntityElement e = kvp.Value;
            T parentElement = GetParent(element);
            List<T> elements=new List<T>();
            if (parentElement != null)
                elements = FindChildrenByName(parentElement, e.name);
            else elements.Add(element);
            foreach (T elem in elements)
            {
                foreach (string c in e.attributes.Keys)
                {
                    KeyValuePair<bool,Result> attributeSearchResult = GetAttribute(element, c);
                    if (!attributeSearchResult.Key)
                    {
                        return attributeSearchResult;
                    }
                }
            }
            Result checkResult = new Result("success", "success");
            return new KeyValuePair<bool, Result>(true, checkResult);
        }

      
        

        protected abstract T GetParent(T element);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element">Element in which to search</param>
        /// <param name="deque"></param>
        /// <returns>Child Element that fits the definition</returns>
        protected  T GetElement(T element, EntityElement elementDefinition)
        {
            return FindChildWithName(element, elementDefinition.name);
        }
       
        protected abstract KeyValuePair<bool, Result> GetAttribute(T element, string c);
        protected abstract T FindChildWithName(T element, string name);
        protected abstract List<T> FindChildrenByName(T parentElement, string name);
        protected abstract T GetFileRoot();
        

      
        

       
        

        protected abstract bool CheckValidExtension();
       
        /// <summary>
        /// Identifies type of database object that file represents
        /// </summary>
        /// <param name="slDefinitions">the key is database object type(table, connection), the value is hierarchical structure of the </param>
        /// <returns>Type of database object that file on which this class works represent</returns>
        protected  string GetEntityType(SortedList<string, EntityElement> slDefinitions)
        {
            T root=GetFileRoot();
            string name=GetElementName(root);
            foreach(var kvp in slDefinitions)
            {
                if (kvp.Value.name==name)
                {
                    entityType = kvp.Key;
                    return kvp.Key;
                }
            }
            return "";
        }

        protected abstract string GetElementName(T root);
       

        public string GetEntityType()
        {
            return entityType;
        }
        
        /// <summary>
        /// Get the hierarchical
        /// </summary>
        /// <param name="name">Name of the element</param>
        /// <returns>Hierarchical structure of element by its name</returns>
        public EntityElement GetEntityElement(string name)
        {
            EntityElement e = new EntityElement(name);
            T element=GetElementWithName(name);
            SortedList<string, string> attributes = GetElementAttributes(element);
            e.attributes = attributes;
            return e;
        }
        /// <summary>
        /// Get All entity elements with name(Create a copy of this elements and returns)
        /// </summary>
        /// <param name="name">Name of the elements to search</param>
        /// <returns>List of elements with name</returns>
        public List<EntityElement> GetEntityElements(string name)
        {
            List<EntityElement> ee = new List<EntityElement>();

           
            List<T> elem = GetElementsWithName(name);
            foreach(T e in elem)
            {
                EntityElement eeee = new EntityElement(name);
                SortedList<string, string> attributes = GetElementAttributes(e);
                eeee.attributes = attributes;
                ee.Add(eeee);
            }
            
          
            return ee;
        }
        public abstract SortedList<string, string> GetElementAttributes(T element);
       

        public abstract T GetElementWithName(string name);
        public abstract List<T> GetElementsWithName(string name);

        
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns>filename with which it operates</returns>
        public string GetName()
        {
            return file;
        }

        /// <summary>
        /// This method is needed in some case
        /// in which add and update operations differ
        /// </summary>
        /// <param name="e">The element</param>
        /// <returns></returns>
        public  bool Add(EntityElement e)
        {
            return true;
        }

        /// <summary>
        /// For future use where file can contain multiple database objects
        /// </summary>
        public abstract void DeleteItem();
        /// <summary>
        /// For future use where file can contain multiple database objects
        /// </summary>
        public abstract void UpdateItem(EntityElement connectionElement);
        /// <summary>
        /// For future use where file can contain multiple database objects
        /// </summary>
        public abstract EntityElement GetEntityElementItem(string connectionTag);
    }
   
}
