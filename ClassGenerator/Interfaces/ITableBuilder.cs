using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassGenerator
{
    /** \ingroup Bll
    <summary>
    Implementation of InterfaceBuilder
    for ITable interface which serves as base interface
    for classes that represent tables in database.
       Also part of \ref Builder 
    </summary>
   */
    public class ITableBuilder : InterfaceBuilder
    {
        public ITableBuilder(string directory, string namespaceName)
        {
            this.directory = directory;
            createdType = new ITableInterface(directory, "ITable", "interface");

            this.namespaceName = namespaceName;
            BuildImports();
            BuildNameSpace();
        }

        public override void BuildTypeInitializer()
        {
           
        }

       

        public override void BuildMethods()
        {
          
        }

        public override void BuildPropertiesAndFields()
        {
            createdType.CreateAutomaticReadOnlyProperty("IdNumber", "System.Guid");
        }

        protected override void BuildImports()
        {
            createdType.AddImport("System");
        }
    }
}
