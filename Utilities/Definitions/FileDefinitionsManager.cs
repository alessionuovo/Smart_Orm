using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Definitions
{
    /** \ingroup Bll
     <summary>
     Contains and Manages Definitions
     of formats and extensions of files that can store database objects
     in current ORM system.
         Part of \ref  Facade 
     </summary>
    */
    public class FileDefinitionsManager
    {
        private FileDefinitionsManager()
        {
            CreateDefinitions();
        }

        protected void CreateDefinitions()
        {
            lstFormatDefinitions = new List<FileDefinition>();
            lstFormatDefinitions.AddRange(new FileDefinition[]
            {
                new FileDefinition("xml", "xml"),
                new FileDefinition("json", "json")
            });
        }
       
        private static FileDefinitionsManager _manager;
        public static FileDefinitionsManager Manager { get { if (_manager == null) _manager = new FileDefinitionsManager(); return _manager; } }
        /// <summary>
        /// List of definitions
        /// </summary>
        public List<FileDefinition> lstFormatDefinitions { get; set; }
       
       
    }
}
