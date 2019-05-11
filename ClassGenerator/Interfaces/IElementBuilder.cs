using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ClassGenerator
{
    /** \ingroup Bll
    <summary>
    Implementation of InterfaceBuilder for IElement
    which represents interface that inherits Save and Load
    operations to collection from another interface and serves as part of Visitor design pattern
    functioning as IVisitor.
       Also part of \ref Builder 
    </summary>
   */
    public class IElementBuilder : InterfaceBuilder
    {
        public IElementBuilder(string directory, string namespaceName)
        {
            this.directory = directory;
            createdType = new IElementInterface(directory, "IElement");
            this.namespaceName = namespaceName;
           
            createdType.TheType.IsInterface = true;
            BuildNameSpace();
            BuildImports();
            createdType.BuildPropertiesAndFields();

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
            createdType.AddImport("System.Collections.Generic");
            createdType.AddImport("System.Collections.ObjectModel");
            createdType.AddImport("System.Collections.Specialized");
        }
    }
}
