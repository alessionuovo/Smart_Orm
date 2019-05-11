using DiagramContent;
using FileManagement;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassGenerator
{
    /** \ingroup Bll
    <summary>
    Represents implementation of CreatedClass
    for DbManager class that is responsible to calls from code
    to database.
       Also part of \ref Builder 
    </summary>
   */
    public class DbManagerClass:CreatedClass
    {
        protected ObservableCollection<Table> tables;
        protected ObservableCollection<Connection> connections;
        public DbManagerClass(string directory, string name, ObservableCollection<Table> tables, ObservableCollection<Connection> connections):base(directory,name)
        {
            this.tables = tables;
            this.connections = connections;
        }

        /// <summary>
        /// Create classes that represents Table
        /// It is Collection of Record Elements
        /// and Create Change Manager
        /// </summary>
        protected override void BuildDefaultConstructorBody()
        {
            string format = DbDefinitionsManager.DbManager.GetFormat();
            //base.BuildDefaultConstructorBody(codeConstructor);
            defaultConstructor.Attributes = MemberAttributes.Family;
            int i = 0;
            foreach (Table table in tables)
            {
                CodeStatement ChangeManagerCreateStatement = GetObjectCreateStatement(string.Format("ChangeManager"), string.Format("changemanager"));
                defaultConstructor.Statements.Add(ChangeManagerCreateStatement);
                CodeStatement TableListCreateStatement = GetObjectCreateStatement(string.Format("TableList<{0}>", table.Name), string.Format("{0}S", table.Name.ToUpper()), new List<string>() { format}); 
                defaultConstructor.Statements.Add(TableListCreateStatement);
                CodeStatement AssignTableNameStatement = GetAssignment(string.Format("{0}S.Name", table.Name.ToUpper()), new KeyValuePair<string,bool>(table.Name, false));
                defaultConstructor.Statements.Add(AssignTableNameStatement);
                CodeStatement VisitorDeclaration = GetDeclaration(string.Format("{0}{1}","visitor", i), string.Format("IChangeVisitor<{0}>", table.Name));
                defaultConstructor.Statements.Add(VisitorDeclaration);
                CodeStatement FileManagerCreateStatement = GetObjectCreateStatement(string.Format("{0}<{1}>", GetConcreteChangeVisitor(), table.Name), string.Format("visitor{0}", i), true, new List<string>() { format});
                defaultConstructor.Statements.Add(FileManagerCreateStatement);
                CodeStatement AcceptVisitorInvokationStatement = GetMethodInvokationStatement(string.Format("{0}S", table.Name.ToUpper()), "Accept", new List<string>() { string.Format("{0}{1}", "visitor", i) });
                defaultConstructor.Statements.Add(AcceptVisitorInvokationStatement);
                CodeStatement IsInitedAssignFalseStatement = GetAssignment(string.Format("{0}S.{1}", table.Name.ToUpper(),"IsInited"), new KeyValuePair<string, bool>("false", true));
                CodeStatement LoadInvokationStatement = GetMethodInvokationStatement(string.Format("{0}S", table.Name.ToUpper()), "Load", new List<string>());
                CodeStatement IsInitedAssignTrueStatement = GetAssignment(string.Format("{0}S.{1}", table.Name.ToUpper(), "IsInited"), new KeyValuePair<string, bool>("true", true));
                defaultConstructor.Statements.Add(IsInitedAssignFalseStatement);
                defaultConstructor.Statements.Add(LoadInvokationStatement);
                defaultConstructor.Statements.Add(IsInitedAssignTrueStatement);
                CodeStatement NotifyDelegateCreateStatement = GetDelegateCreateStatement("NotifyCollectionChangedEventHandler", string.Format("{0}S", table.Name.ToUpper()), string.Format("{0}S_Changed", table.Name.ToUpper()), "CollectionChanged", true); 
                   
                //defaultConstructor.Statements.Add(NotifyDelegateCreateStatement);
                CodeStatement TableChangeDelegateCreateStatement = GetDelegateCreateStatement("EventHandler", string.Format("{0}S", table.Name.ToUpper()), string.Format("{0}S_TableChanged", table.Name.ToUpper()), "TableChanged", true);

                defaultConstructor.Statements.Add(TableChangeDelegateCreateStatement);
                i++;
            }

            foreach (Connection connection in connections)
            {
                CodeStatement ChangeManagerCreateStatement = GetObjectCreateStatement(string.Format("ChangeManager"), string.Format("changemanager"));
                defaultConstructor.Statements.Add(ChangeManagerCreateStatement);
                CodeStatement TableListCreateStatement = GetObjectCreateStatement(string.Format("TableList<{0}>", connection.Name, connection.FirstTable, connection.SecondTable), string.Format("{0}S", connection.Name.ToUpper()), new List<string>() { format });
                defaultConstructor.Statements.Add(TableListCreateStatement);
                CodeStatement AssignTableNameStatement = GetAssignment(string.Format("{0}S.Name", connection.Name.ToUpper()), new KeyValuePair<string, bool>(connection.Name, false));
                defaultConstructor.Statements.Add(AssignTableNameStatement);
                CodeStatement VisitorDeclaration = GetDeclaration(string.Format("{0}{1}", "visitor", i), string.Format("IChangeVisitor<{0}>", connection.Name));
                defaultConstructor.Statements.Add(VisitorDeclaration);
                CodeStatement FileManagerCreateStatement = GetObjectCreateStatement(string.Format("{0}<{1}>", GetConcreteChangeVisitor(), connection.Name, connection.FirstTable, connection.SecondTable), string.Format("visitor{0}", i), true, new List<string>() { format });
                defaultConstructor.Statements.Add(FileManagerCreateStatement);
                CodeStatement AcceptVisitorInvokationStatement = GetMethodInvokationStatement(string.Format("{0}S", connection.Name.ToUpper()), "Accept", new List<string>() { string.Format("{0}{1}", "visitor", i) });
                defaultConstructor.Statements.Add(AcceptVisitorInvokationStatement);
                CodeStatement IsInitedAssignFalseStatement = GetAssignment(string.Format("{0}S.{1}", connection.Name.ToUpper(), "IsInited"), new KeyValuePair<string, bool>("false", true));
                CodeStatement LoadInvokationStatement = GetMethodInvokationStatement(string.Format("{0}S", connection.Name.ToUpper()), "Load", new List<string>());
                CodeStatement IsInitedAssignTrueStatement = GetAssignment(string.Format("{0}S.{1}", connection.Name.ToUpper(), "IsInited"), new KeyValuePair<string, bool>("true", true));
                defaultConstructor.Statements.Add(IsInitedAssignFalseStatement);
                defaultConstructor.Statements.Add(LoadInvokationStatement);
                defaultConstructor.Statements.Add(IsInitedAssignTrueStatement);
                CodeStatement NotifyDelegateCreateStatement = GetDelegateCreateStatement("NotifyCollectionChangedEventHandler", string.Format("{0}S", connection.Name.ToUpper()), string.Format("{0}S_Changed", connection.Name.ToUpper()), "CollectionChanged", true);

                defaultConstructor.Statements.Add(NotifyDelegateCreateStatement);
                CodeStatement TableChangeDelegateCreateStatement = GetDelegateCreateStatement("EventHandler", string.Format("{0}S", connection.Name.ToUpper()), string.Format("{0}S_TableChanged", connection.Name.ToUpper()), "TableChanged", true);
                defaultConstructor.Statements.Add(TableChangeDelegateCreateStatement);
                i++;
            }



        }

        protected virtual string GetConcreteChangeVisitor()
        {
            return "FileManager";
        }

        public override void BuildMethods()
        {
            BuildSaveChangesMethod();
            
        }

       

        
        protected virtual void BuildSaveChangesMethod()
        {
            CodeMemberMethod cmm = BuildMethodSignature("SaveChanges", MemberAttributes.Public, new List<KeyValuePair<string, string>>());
            

            // Creates a statement using a code expression.
            CodeStatement expressionStatement=GetMethodInvokationStatement("changemanager", "Execute", new List<string>());
            cmm.Statements.Add(expressionStatement);
            TheType.Members.Add(cmm);
        }

        /// <summary>
        /// Build Change Event handlers(NotifyCollectionChanged)
        /// for each of database tables
        /// </summary>
        public override void BuildEventHandlers()
        {
            base.BuildEventHandlers();
            foreach (Table table in tables)
            {
               
                List<KeyValuePair<string, string>> lstTableChangedMethodParameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("System.Object", "sender"),
                    new KeyValuePair<string, string>("EventArgs", "e")
                };
                CodeMemberMethod cmmTableChanged = BuildMethodSignature(table.Name.ToUpper() + "S" + "_TableChanged", MemberAttributes.Public, lstTableChangedMethodParameters);
                List<KeyValuePair<string, string>> lstMethodParameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("System.Object", "sender"),
                    new KeyValuePair<string, string>("NotifyCollectionChangedEventArgs", "e")
                };
                CodeMemberMethod cmm = BuildMethodSignature(table.Name.ToUpper() + "S" + "_Changed", MemberAttributes.Public, lstMethodParameters);
                List<string> lstInvokationParameters = new List<string>()
                {
                    table.Name.ToUpper() + "S"
                    
                };
                // Creates a statement using a code expression.
                CodeStatement expressionStatement = GetMethodInvokationStatement("changemanager", "Add", lstInvokationParameters);
                cmm.Statements.Add(expressionStatement);
                cmmTableChanged.Statements.Add(expressionStatement);
                CodeStatement TableNameAssignmentStatement = GetAssignment(table.Name.ToUpper() + "S" + ".Name", new KeyValuePair<string,bool>(table.Name, false));
                cmm.Statements.Add(TableNameAssignmentStatement);
                cmmTableChanged.Statements.Add(TableNameAssignmentStatement);
                TheType.Members.Add(cmm);
                TheType.Members.Add(cmmTableChanged);
            }
            foreach (Connection connection in connections)
            {
                List<KeyValuePair<string, string>> lstTableChangedMethodParameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("System.Object", "sender"),
                    new KeyValuePair<string, string>("EventArgs", "e")
                };
                CodeMemberMethod cmmTableChanged = BuildMethodSignature(connection.Name.ToUpper() + "S" + "_TableChanged", MemberAttributes.Public, lstTableChangedMethodParameters);
                List<KeyValuePair<string, string>> lstMethodParameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("System.Object", "sender"),
                    new KeyValuePair<string, string>("NotifyCollectionChangedEventArgs", "e")
                };
                CodeMemberMethod cmm = BuildMethodSignature(connection.Name.ToUpper() + "S" + "_Changed", MemberAttributes.Public, lstMethodParameters);
                List<string> lstInvokationParameters = new List<string>()
                {
                    connection.Name.ToUpper() + "S"

                };
                // Creates a statement using a code expression.
                CodeStatement expressionStatement = GetMethodInvokationStatement("changemanager", "Add", lstInvokationParameters);
                cmm.Statements.Add(expressionStatement);
                cmmTableChanged.Statements.Add(expressionStatement);
                CodeStatement TableNameAssignmentStatement = GetAssignment(connection.Name.ToUpper() + "S" + ".Name", new KeyValuePair<string, bool>(connection.Name, false));
                cmm.Statements.Add(TableNameAssignmentStatement);
                cmmTableChanged.Statements.Add(TableNameAssignmentStatement);
                TheType.Members.Add(cmm);
                TheType.Members.Add(cmmTableChanged);
            }
        }

        public override void BuildPropertiesAndFields()
        {
            //base.BuildPropertiesAndFields();
            foreach(Table table in tables)
            {
                BuildProperty(table.Name.ToUpper() + "S", string.Format("TableList<{0}>", table.Name), table.Name.ToLower() + "s");
                
            }
            foreach (Connection connection in connections)
            {
                BuildProperty(connection.Name.ToUpper() + "S", string.Format("TableList<{0}>", connection.Name), connection.Name.ToLower() + "s");

            }
            CreateAutomaticProperty("changemanager", "ChangeManager");
            CreateSingletonAccessor("Manager", "_manager", "DbManager");
            
        }

        
    }
}
