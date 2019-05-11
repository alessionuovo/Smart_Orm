using DiagramContent;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Utilities;
using Utilities.Definitions;
using System.Xml.XPath;
namespace FileManagement
{
    /** \ingroup Dal
   
    
    <summary>
    class XmlManager
     Implementation of filemanager to xml format.
         Part of \ref Strategy 
    </summary>
    */
    public class XmlManager : FileManager<XElement>
    {
        
        protected XDocument xd;
       
        public XmlManager(string file)
        {
            this.file = file;
            
            directory = SystemDefinitionsManager.DefinitionsManager.GetCurrentUploadDirectory();
            extension = ".xml";
            filename = string.Format("{0}{1}", file, extension);
        }

       
        /// <summary>
        /// Check that extension is valid
        /// there is support that extension differs from format
        /// </summary>
        /// <returns></returns>
        protected override bool CheckValidExtension()
        {
           try
            {
                xd = XDocument.Load(Path.Combine(directory, filename));
              

            } catch (Exception ee) { return false; }
            return true;
        }

       

      
        
       
        /// <summary>
        /// aaa
        /// </summary>
        /// <param name="element">aaa</param>
        /// <returns>aaa</returns>
        protected override XElement GetParent(XElement element)
        {
            return element.Parent;
        }

        protected override KeyValuePair<bool,Result> GetAttribute(XElement element, string c)
        {
            string attr_value=Utilities.XmlHelper.GetAttributeValue(element, c);
            string result;
            bool res;
            if (!string.IsNullOrEmpty(attr_value))
            {
                res = true;
                result = "Success";
            }
            else
            {
                res = false;
                result = string.Format("Attribute with name {0} NOT EXISTS IN ELEMENT WITH NAME {1}", c, element.Name);
            }
            return new KeyValuePair<bool, Result>(res, new Result(result, result));
        }

        protected override XElement FindChildWithName(XElement element, string name)
        {
            return element.Element(name);
        }

        protected override List<XElement> FindChildrenByName(XElement element, string name)
        {
            return element.Elements(name).ToList();
        }

        /// <summary>
        /// Get xml root
        /// </summary>
        /// <returns>XElement that is a root of document</returns>
        protected override XElement GetFileRoot()
        {
            return xd.Root;
        }

        /// <summary>
        /// Get attributes as collection
        /// of name and value
        /// </summary>
        /// <param name="elem">Collection of attributes</param>
        /// <returns></returns>
        public override SortedList<string, string> GetElementAttributes(XElement elem)
        {
            SortedList<string, string> slAttributes = new SortedList<string, string>();
            foreach (XAttribute attr in elem.Attributes())
                slAttributes.Add(attr.Name.ToString(), attr.Value);
            return slAttributes;
        }

        public override XElement GetElementWithName(string name)
        {
            return xd.Root.XPathSelectElement("//" + name);
        }

        /// <summary>
        /// Searches for xml elements with name
        /// </summary>
        /// <param name="name">Name to search</param>
        /// <returns>Xml elements with name</returns>
        public override List<XElement>  GetElementsWithName(string name)
        {
            return xd.Root.XPathSelectElements("//" + name).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        /// <returns>Name of element</returns>
        protected override string GetElementName(XElement root)
        {
            return root.Name.ToString();
        }

        /// <summary>
        /// Add XElement to other XElement
        /// </summary>
        /// <param name="key">XElement to which to add</param>
        /// <param name="elem">XElement to add</param>
        protected override void AddElement(XElement key, XElement elem)
        {
            key.Add(elem);
        }

        protected override XElement CreateElement(EntityElement ee)
        {
            XElement xe = new XElement(ee.name);
            return xe;
        }

        protected override void CreateAttributes(XElement key, EntityElement value)
        {
            foreach(KeyValuePair<string,string> kvp in value.attributes)
            {
                key.SetAttributeValue(kvp.Key, kvp.Value);
            }
        }

        /// <summary>
        /// Creates xml root
        /// </summary>
        /// <param name="e">Hierarchical structure of this XElement</param>
        /// <returns>Create root XElement</returns>
        protected override XElement CreateRoot(EntityElement e)
        {
            xd = XDocument.Parse(string.Format("<{0}></{0}>", e.name));
            return xd.Root;
        }

        /// <summary>
        /// Saves the xml permanently
        /// </summary>
        protected override void Save()
        {
            xd.Save(Path.Combine(directory, filename));
        }

        /// <summary>
        /// Delete the xml files represented by this class from disk
        /// </summary>
        /// <returns></returns>
        public override bool Delete()
        {
            File.Delete(Path.Combine(directory, filename));
            return true;
        }

        public override void DeleteItem()
        {
            XElement xe = xd.Root.XPathSelectElement(string.Format("[@name=\"{0}\"]", file));
            if (xe == null) return;
           
        }

        public override void UpdateItem(EntityElement connectionElement)
        {
            throw new NotImplementedException();
        }

        public override EntityElement GetEntityElementItem(string connectionTag)
        {
            throw new NotImplementedException();
        }
    }
    
}
