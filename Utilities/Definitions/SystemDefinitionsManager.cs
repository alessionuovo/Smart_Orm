using DiagramContent;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Utilities.Definitions
{
    /** \ingroup Bll
     <summary>
     This class is manager of system configuration
     and it is part of \ref  Facade 
     </summary>
    */
  
    public class SystemDefinitionsManager
    {
        
        private SystemDefinitionsManager()
        {
            fmanager = FileDefinitionsManager.Manager;
            dDefinition = DirectoriesDefinition.DDefinition;
            dbvdefinition = DbValidationDefinition.Definition;
            uploadstate = "temp";
        }

        public string GetSystemFormat()
        {
            return "xml";
        }

        protected string uploadstate;
        private static SystemDefinitionsManager _definitionmanager;
        public static SystemDefinitionsManager DefinitionsManager { get { if (_definitionmanager == null) _definitionmanager = new SystemDefinitionsManager(); return _definitionmanager; } }

        public void CreateSystemDirectories()
        {
            CreateTempUploadDirectory();
            CreateConstUploadDirectory();
        }

        private void CreateConstUploadDirectory()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), GetConstUploadDirectory());
            if (!Directory.Exists(path))

            Directory.CreateDirectory(path);
        }

        private void CreateTempUploadDirectory()
        {
            string path=Path.Combine(Directory.GetCurrentDirectory(), GetTempUploadDirectory());
            if (!Directory.Exists(path))

                Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Part of \ref  Facade Design Pattern
        /// </summary>
        protected FileDefinitionsManager fmanager;
        /// <summary>
        /// Part of \ref  Facade Design Pattern
        /// </summary>
        protected DbValidationDefinition dbvdefinition;
        protected DirectoriesDefinition dDefinition;
        public EntityTypesDefinition EntitiesDefinition { get; set; }
        public List<string> GetDataTypes()
        {
            List<string> lstDataTypes = new List<string>()
            {
                "int",
                "string",
                "double",
                "float",
                "DateTime"

            };
            return lstDataTypes;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>List of KeyValuePair of format and extension</returns>
        public SortedList<string,string> GetAvailableFormats()
        {
            SortedList<string, string> lst = new SortedList<string, string>();
            foreach(FileDefinition fd in fmanager.lstFormatDefinitions)
            {
                if (!lst.ContainsKey(fd.Format))
                  lst.Add(fd.Format, fd.Extension);
            }
            return lst;
        }
        
        /// <summary>
        /// Get structure of database objects supported in the system
        /// </summary>
        /// <returns>List of database object type (key)- structure (value)</returns>
        public SortedList<string, EntityElement> GetValidationDefinitions()
        {
            SortedList<string, EntityElement> slDefinitions = new SortedList<string, EntityElement>();
            slDefinitions.Add(EntityTypesDefinition.Table, dbvdefinition.TDefinition.Element);
            slDefinitions.Add(EntityTypesDefinition.Connection, dbvdefinition.CDefinition.Element);
            return slDefinitions;
        }
        /// <summary>
        /// Get structure of specific database object(The structure of how it is represented on file)
        /// </summary>
        /// <typeparam name="T">Type of database object</typeparam>
        /// <param name="element">Element on which to show the structure</param>
        /// <returns>Hierarchical Structure of database object in file</returns>
        public EntityElement GetDefinition<T>(T element)
        {
            if (element is Table)
            {
                TableDefinition td = new TableDefinition(element as Table);
                return td.Element;
            }
            else if (element is Connection)
            {
                ConnectionDefinition td = new ConnectionDefinition(element as Connection);
                return td.Element;
            }
            else return GetNewEntityDefinition(element);
        }

        /// <summary>
        /// This method is to override in descendant classes in order 
        /// to get structure of something that is not table or connection 
        /// </summary>
        /// <typeparam name="T">Type of database object</typeparam>
        /// <param name="element">Element on which to show the structure</param>
        /// <returns>Hierarchical Structure of database object in file</returns>
        protected EntityElement GetNewEntityDefinition<T>(T element)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEntityDefinition GetValidationDefinition(string type)
        {
            if (type==EntityTypesDefinition.Table)
            {
                return dbvdefinition.TDefinition;
            }
            if (type==EntityTypesDefinition.Connection)
            {
                return dbvdefinition.CDefinition;
            }
            return null;

        }
       
        /// <summary>
        /// Sets that uploaded files are moved to const directory
        /// </summary>
        public void SetUploadStateToConst()
        {
            uploadstate = "const";
        }
        public void SetUploadStateToTemp()
        {
            uploadstate = "temp";
        }
        /// <summary>
        /// Get the directory where files reside
        /// </summary>
        /// <returns></returns>
        public string GetCurrentUploadDirectory()
        {
            if (uploadstate == "temp")
                return Path.Combine(Directory.GetCurrentDirectory(), GetTempUploadDirectory());
            else return Path.Combine(Directory.GetCurrentDirectory(), GetConstUploadDirectory());
        }
        
        /// <summary>
        /// Get the directory to which all classes are generated
        /// to use afterwards
        /// </summary>
        /// <returns></returns>
        public string GetCurrentGeneratedDirectory()
        {
            string path=Path.Combine(Directory.GetCurrentDirectory(), "Generated");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }

        /// <summary>
        /// Get the directory where files are temporary uploaded
        /// </summary>
        /// <returns></returns>
        private string GetTempUploadDirectory()
        {
            return dDefinition.TEMP_DIR;
        }

        /// <summary>
        /// Get the directory where files are temporary updated
        /// </summary>
        /// <returns></returns>
        public string GetConstUploadDirectory()
        {
            return dDefinition.REAL_DIR;
        }

        public List<string> GetDiagramFileNames(string format)
        {
            string dir = GetConstUploadDirectory();
            DirectoryInfo di = new DirectoryInfo(dir);
            List<string> files = new List<string>();
            format = "." + format;
            foreach(FileInfo fi in di.GetFiles())
            {
                if (fi.Extension ==  format)
                    files.Add(fi.Name.Replace(format, string.Empty));
            }
            return files;
        }

        public void EmptyConstUploadDirectory()
        {
            DirectoryInfo di = new DirectoryInfo(GetConstUploadDirectory());
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
        }
    }
}
