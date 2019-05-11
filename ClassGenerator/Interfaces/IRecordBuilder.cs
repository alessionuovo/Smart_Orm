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
    Implementation of InterfaceBuilder 
    for IRecord interface which represents
    record in db with operations of Save and Load.
       Also part of \ref Builder 
    </summary>
   */
    public class IRecordBuilder : InterfaceBuilder
    {
        public IRecordBuilder(string directory, string namespaceName)
        {
            this.directory = directory;
            createdType = new IRecordInterface(directory, "IRecord");
            this.namespaceName = namespaceName;
           
            createdType.TheType.IsInterface = true;
            BuildNameSpace();

        }
        public override void BuildTypeInitializer()
        {
            createdType.BuildPropertiesAndFields();
        }

       

        public override void BuildMethods()
        {
            createdType.BuildMethods();
        }

        public override void BuildPropertiesAndFields()
        {
            
        }

        protected override void BuildImports()
        {
            
        }
    }
}
