using FileManagement;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Utilities;
using Utilities.Definitions;

namespace FileManagement
{
    /*internal class JsonManager : FileManager<JObject>
    {
        private string file;
        protected JObject jo;

        public JsonManager(string file)
        {
            
            this.file = file;

            directory = SystemDefinitionsManager.DefinitionsManager.GetCurrentUploadDirectory();
            extension = ".json";
            filename = string.Format("{0}{1}", file, extension);
        }





        public bool Add(EntityElement e)
        {
            throw new NotImplementedException();
        }
         
        

       

        public override bool Delete()
        {
            throw new NotImplementedException();
        }

       

        public bool Delete(string name)
        {
            throw new NotImplementedException();
        }

        public override SortedList<string, string> GetElementAttributes(JObject elem)
        {
            throw new NotImplementedException();
        }

        public override List<JObject> GetElementsWithName(string name)
        {
            throw new NotImplementedException();
        }

        public override JObject GetElementWithName(string name)
        {
            throw new NotImplementedException();
        }

        public EntityElement GetEntityElement(string name)
        {
            throw new NotImplementedException();
        }

        public List<EntityElement> GetEntityElements(string name)
        {
            throw new NotImplementedException();
        }

        public string GetEntityType()
        {
            throw new NotImplementedException();
        }

        public string GetName()
        {
            throw new NotImplementedException();
        }

        public bool Update()
        {
            throw new NotImplementedException();
        }

        public bool Update(EntityElement e)
        {
            throw new NotImplementedException();
        }

        public bool Validate()
        {
            throw new NotImplementedException();
        }

        public KeyValuePair<bool, Result> Validate(SortedList<string, EntityElement> slDefinitions)
        {
            throw new NotImplementedException();
        }

        public bool Validate(SortedList<string, XElement> slDefinitions)
        {
            throw new NotImplementedException();
        }

        protected override void AddElement(JObject key, JObject elem)
        {
            
            throw new NotImplementedException();
        }

        protected override bool CheckValidExtension()
        {
            
          
            try
            {
                jo = JObject.Parse(File.ReadAllText(Path.Combine(directory, filename)));
                

            }
            catch (Exception ee) { return false; }
            return true;
        }

        protected override void CreateAttributes(JObject key, EntityElement value)
        {
            throw new NotImplementedException();
        }

        protected override JObject CreateElement(EntityElement ee)
        {
            jo = JObject.Parse(string.Format("{{\"{0}\":[]}}", ee.name));
            return jo[];
        }

        protected override JObject CreateRoot(EntityElement e)
        {
            jo = new JObject();
            return jo;
        }

        protected override List<JObject> FindChildrenByName(JObject parentelement, string name)
        {
            throw new NotImplementedException();
        }

        protected override JObject FindChildWithName(JObject element, string name)
        {
            JProperty joo = element.Property(name);

        }

        protected override KeyValuePair<bool, Result> GetAttribute(JObject element, string c)
        {

            string result;
            bool res;
            JToken value;
            string attr_value;
            if (element.TryGetValue(c, out value))
            {
                attr_value = value.ToString();
                res = true;
                result = "Success";
            }
            
           
            else
            {
                res = false;
                JProperty parentProp = (JProperty)element.Parent;
                string name = parentProp.Name;
                result = string.Format("Attribute with name {0} NOT EXISTS IN ELEMENT WITH NAME {1}", c, name);
            }
            return new KeyValuePair<bool, Result>(res, new Result(result, result));
        }

        protected override string GetElementName(JObject root)
        {
            throw new NotImplementedException();
        }

        protected override JObject GetFileRoot()
        {
            return jo;
        }

        protected override JObject GetParent(JObject element)
        {
            return (JObject)element.Parent;
        }

        protected override void Save()
        {
            throw new NotImplementedException();
        }
    }*/
}