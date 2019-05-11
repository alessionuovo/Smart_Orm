using DiagramContent;
using System;
using System.CodeDom;
using System.Collections.ObjectModel;

namespace ClassGenerator
{
    /** \ingroup Bll
   <summary>
   Implementation of ClassBuilder for DbManager class.
   DbManager class is responsible for managing all the calls
   to the database(here it's stored in files), so the user calls
   methods and properties of DbManager in order to save something to the db.
       Also part of \ref Builder 
   </summary>
   */
    public class DbManagerBuilder:ClassBuilder
    {
      
        
        public DbManagerBuilder(string directory, string namespaceName, ObservableCollection<Table> tables, ObservableCollection<Connection> connections)
        {
            this.directory = directory;
            createdType = new DbManagerClass(directory, "DbManager", tables, connections);
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

        protected override void BuildImports()
        {
            createdType.AddImport("System");
            createdType.AddImport("System.Collections.Specialized");
            createdType.AddImport("System.Collections.Generic");
            createdType.AddImport("System.Linq");
        }

      
    }
}