using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassGenerator
{
    /** \ingroup Bll
    <summary>
    Implementation of InterfaceBuilder for generating IChangeVisitor interface.
    This interface is responsible for Loading changes to memory and saving changes to file/db
    Also part of \ref Builder
       </summary>
   */
    public class IChangeVisitorBuilder : InterfaceBuilder
    {
        public IChangeVisitorBuilder(string directory, string namespaceName)
        {
            this.directory = directory;
            createdType = new IChangeVisitorInterface(directory, "IChangeVisitor");
            this.namespaceName = namespaceName;
            createdType.TheType.IsInterface = true;
           
            BuildNameSpace();
            BuildImports();
        }
       

        

        public override void BuildMethods()
        {
            createdType.BuildMethods();
        }

        public override void BuildPropertiesAndFields()
        {
            
        }

        public override void BuildTypeInitializer()
        {
            
        }

        protected override void BuildImports()
        {
            createdType.AddImport("System.Collections.ObjectModel");
            createdType.AddImport("System.Collections.Specialized");
        }
    }
}
