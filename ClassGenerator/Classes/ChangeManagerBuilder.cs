using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassGenerator
{
    /** \ingroup Bll
    <summary>
    Realization of ClassBuilder
    for ChangeManager Class.
    The class that has list of database objects
    that changed.
    Has method of adding item to this list
    and Executing the save of the changes permanently.
       Also part of \ref Builder 
    </summary>
   */
    public class ChangeManagerBuilder : ClassBuilder
    {
        
        public ChangeManagerBuilder(string directory, string namespaceName)
        {
            this.directory = directory;
            createdType = new ChangeManagerClass(directory, "ChangeManager");
            this.namespaceName = namespaceName;

            BuildImports();
            BuildNameSpace();

        }
        public override void BuildTypeInitializer()
        {
            BuildConstructors();
            BuildPropertiesAndFields();
        }

        public override void BuildConstructors()
        {
            createdType.BuildDefaultConstructor();
        }

        public override void BuildMethods()
        {
            createdType.BuildEventHandlers();
            createdType.BuildMethods();
        }

        public override void BuildPropertiesAndFields()
        {
            createdType.BuildPropertiesAndFields();
        }

        /// <summary>
        /// Using directives creation
        /// here only the generic collection
        /// </summary>
        protected override void BuildImports()
        {
            createdType.AddImport("System.Collections.Generic");
        }
        
    }
}
