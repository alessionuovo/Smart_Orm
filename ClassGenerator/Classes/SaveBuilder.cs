using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassGenerator
{
    /** \ingroup Bll
    <summary>
    This class represents implementation of ClassBuilder
    for FileManager class.
    This class is responsible for loading database data from files and
    saving data in them.
        Also part of \ref Builder 
    </summary>
   */
    public class SaveBuilder : ClassBuilder
    {
        protected string generateDirectory;
        public SaveBuilder(string directory, string namespaceName, string generateDirectory)
        {
            this.directory = directory;
            createdType = new SaveClass(directory, "FileManager", generateDirectory);
            this.namespaceName = namespaceName;
            this.generateDirectory = generateDirectory;
            BuildNameSpace();
            BuildImports();
        }
        public override void BuildTypeInitializer()
        {
            BuildConstructors();
            BuildPropertiesAndFields();
        }

        public override void BuildConstructors()
        {
            createdType.BuildOtherConstructors();
        }

        public override void BuildMethods()
        {
            createdType.BuildMethods();
        }

        /// <summary>
        /// Builds two properties Name and Format
        /// Name is name of saved table or another db object
        /// Format-tells in what format data is saved
        /// </summary>
        public override void BuildPropertiesAndFields()
        {
            createdType.CreateAutomaticProperty("Format", "System.String", "protected");
            createdType.CreateAutomaticProperty("Name", "System.String", "public");
        }

        /// <summary>
        /// Many imports
        /// for Linq, Collection, Reflection and others.
        /// </summary>
        protected override void BuildImports()
        {
            createdType.AddImport("System");
            createdType.AddImport("System.Linq");
            createdType.AddImport("System.Xml.Linq");
            createdType.AddImport("System.Collections.Generic");
            createdType.AddImport("System.Reflection");
            createdType.AddImport("System.ComponentModel");
            createdType.AddImport("System.Collections.ObjectModel");
            createdType.AddImport("System.Collections.Specialized");
        }
    }
}
