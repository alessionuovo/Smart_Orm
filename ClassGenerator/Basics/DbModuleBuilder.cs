using DiagramContent;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassGenerator
{
     /** \ingroup Bll
     <summary>
     This class implements IModuleBuilder
     for classes that manage db or help in it
     There are also ChangeManagerClass but put one /two classes in another
     module seems not efficient.
        Also is part of \ref Builder 
     </summary>
    */
    public class DbModuleBuilder : IModuleBuilder
    {
        private string generateDirectory;
        public void SetGenerateDirectory(string directory)
        {
            generateDirectory = directory;
        }
        /// <summary>
        /// Create all types that correlates to db
        /// </summary>
        /// <param name="directory">Directory in which to store them</param>
        /// <returns></returns>
        public List<CreatedType> Create(string directory)
        {
            List<CreatedType> lstcreatedTypees = new List<CreatedType>();
            ClassBuilder cbDbManagerBuilder = new DbManagerBuilder(directory, "GeneratedDb", Structure.Struct.model.tables, Structure.Struct.model.connections);
            ClassCreatorManager ccm = new ClassCreatorManager(cbDbManagerBuilder);
            lstcreatedTypees.Add(ccm.Create());

            foreach(Table table in Structure.Struct.model.tables)
            {
                ClassBuilder cbTableBuilder = new TableBuilder(directory, "GeneratedDb", table);
                ClassCreatorManager TableCreatorManager = new ClassCreatorManager(cbTableBuilder);
                lstcreatedTypees.Add(TableCreatorManager.Create());
            }
            foreach (Connection connection in Structure.Struct.model.connections)
            {
                ClassBuilder cbConnectionBuilder = new ConnectionBuilder(directory, "GeneratedDb", connection);
                ClassCreatorManager ConnectionCreatorManager = new ClassCreatorManager(cbConnectionBuilder);
                lstcreatedTypees.Add(ConnectionCreatorManager.Create());
            }
            ClassBuilder cbTableListBuilder = new TableListBuilder(directory, "GeneratedDb");
              ClassCreatorManager TableListCreatorManager = new ClassCreatorManager(cbTableListBuilder);
              lstcreatedTypees.Add(TableListCreatorManager.Create());
            ClassBuilder cbPropertyBuilder = new PropertyBuilder(directory, "GeneratedDb");
            TypeCreatorManager<CreatedClass> PropertyCreatorManager = new ClassCreatorManager(cbPropertyBuilder);
            lstcreatedTypees.Add(PropertyCreatorManager.Create());
            ClassBuilder cbConnectionPropertyBuilder = new ConnectionPropertyBuilder(directory, "GeneratedDb");
            ClassCreatorManager ConnectionPropertyCreatorManager = new ClassCreatorManager(cbConnectionPropertyBuilder);
            lstcreatedTypees.Add(ConnectionPropertyCreatorManager.Create());
            /*ClassBuilder cbChangeVisitorFactoryBuilder = new ChangeVisitorFactoryBuilder(directory, "GeneratedDb");
           ClassCreatorManager ChangeVisitorFactoryCreatorManager = new ClassCreatorManager(cbChangeVisitorFactoryBuilder);
            lstcreatedTypees.Add(ChangeVisitorFactoryCreatorManager.Create());
            ClassBuilder cbFileManagerFactoryBuilder = new FileManagerFactoryBuilder(directory, "GeneratedDb");
            ClassCreatorManager FileManagerFactoryCreatorManager = new ClassCreatorManager(cbFileManagerFactoryBuilder);
            lstcreatedTypees.Add(FileManagerFactoryCreatorManager.Create());*/
            InterfaceBuilder cbITableBuilder = new ITableBuilder(directory, "GeneratedDb");
            InterfaceCreatorManager ITableCreatorManager = new InterfaceCreatorManager(cbITableBuilder);
            lstcreatedTypees.Add(ITableCreatorManager.Create());
            InterfaceBuilder cbIConnectionBuilder = new IConnectionBuilder(directory, "GeneratedDb");
            InterfaceCreatorManager IConnectionCreatorManager = new InterfaceCreatorManager(cbIConnectionBuilder);
            lstcreatedTypees.Add(IConnectionCreatorManager.Create());
            ClassBuilder cbSaveBuilder = new SaveBuilder(directory, "GeneratedDb", generateDirectory);
            ClassCreatorManager SaveCreatorManager = new ClassCreatorManager(cbSaveBuilder);
            lstcreatedTypees.Add(SaveCreatorManager.Create());
            ClassBuilder cbChangeBuilder = new ChangeManagerBuilder(directory, "GeneratedDb");
            ClassCreatorManager ChangeCreatorManager = new ClassCreatorManager(cbChangeBuilder);
            lstcreatedTypees.Add(ChangeCreatorManager.Create());
             InterfaceBuilder cbIElementBuilder = new IElementBuilder(directory, "GeneratedDb");
            InterfaceCreatorManager IElementCreatorManager = new InterfaceCreatorManager(cbIElementBuilder);
            lstcreatedTypees.Add(IElementCreatorManager.Create());
            InterfaceBuilder cbIChangeVisitorBuilder = new IChangeVisitorBuilder(directory, "GeneratedDb");
            InterfaceCreatorManager IChangeVisitorCreatorManager = new InterfaceCreatorManager(cbIChangeVisitorBuilder);
            lstcreatedTypees.Add(IChangeVisitorCreatorManager.Create());
            InterfaceBuilder cbIRecordBuilder = new IRecordBuilder(directory, "GeneratedDb");
            InterfaceCreatorManager IRecordCreatorManager = new InterfaceCreatorManager(cbIRecordBuilder);
            lstcreatedTypees.Add(IRecordCreatorManager.Create());
            return lstcreatedTypees;

        }

       
    }
}
