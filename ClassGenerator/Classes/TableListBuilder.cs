using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassGenerator
{
    /** \ingroup Bll
    <summary>
    This class is implementation of ClassBuilder for class TableList
    which inherits ObservableCollection .and represents Collection of records.
       Also part of \ref Builder 
    </summary>
   */
    public class TableListBuilder : ClassBuilder
    {
        public TableListBuilder(string directory, string namespaceName)
        {
            this.directory = directory;
            createdType = new TableListClass(directory, "TableList");
            
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
            createdType.BuildOtherConstructors();
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

        protected override void BuildImports()
        {
            createdType.AddImport("System");
            createdType.AddImport("System.Collections.ObjectModel");
            createdType.AddImport("System.Collections.Specialized");
        }
    }
}
