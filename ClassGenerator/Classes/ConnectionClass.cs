using DiagramContent;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ClassGenerator
{
    /** \ingroup Bll
    <summary>
    Represents connection in database(the generated class for it).
       Also part of \ref Builder 
    </summary>
   */
    internal class ConnectionClass : CreatedClass
    {
        internal ObservableCollection<Properties> prop;
        internal ObservableCollection<ConnectionProperties> connprop;
        private string secondTable;
        private string secondRelation;
        private string firstTable;
        private string firstRelation;
        public ConnectionClass(string directory, string name, ObservableCollection<Properties> properties, ObservableCollection<ConnectionProperties> connProperties) : base(directory, name)
        {
            // this.table = table;
           
            prop = properties;
            connprop = connProperties;
        }

        public ConnectionClass(string directory, string name, string type, string secondTable, string secondRelation) : base(directory, name, type)
        {
            TheType.BaseTypes.Add("ITable");
            this.secondTable = secondTable;
            this.secondRelation = secondRelation;
        }

        public ConnectionClass(string directory, string name, string firstTable, string firstRelation, string secondTable, string secondRelation) : base(directory, name)
        {
            CreateGenericTypeMultiDimension(new List<GenericClassParameters>() { new GenericClassParameters() { BaseType = "IConnection", GenericTypes = new List<string>() { firstTable, secondTable}, Constraint = new List<string>() { "ITable", "new()" }, IsBaseTypeGeneric = true, GenericImplement=new List<string>() { firstTable, secondTable} } });
            TheType.BaseTypes.Add("ITable");

            this.name = name;
            this.firstTable = firstTable;
            this.firstRelation = firstRelation;
            this.secondTable = secondTable;
            this.secondRelation = secondRelation;
        }

        /// <summary>
        /// Default constructor that creates object
        /// for each of its Property(field in database table) fields
        /// and defines the delegate for property change
        /// 
        /// </summary>
        protected override void BuildDefaultConstructorBody()
        {
            /* foreach(Properties p in prop)
             {
                 CodeStatement ObjectCreateStatement = GetObjectCreateStatement(string.Format("Property<{0}>", p.Type), p.Name);
                 defaultConstructor.Statements.Add(ObjectCreateStatement);

                 CodeStatement attachTableEvent = GetDelegateCreateStatement("PropertyChangedEventHandler", p.Name, "CurrentProperty_Changed", "PropertyChanged", true);

                  defaultConstructor.Statements.Add(attachTableEvent);
             }*/
            CodeStatement cs = GetAssignment("lastIndex", new KeyValuePair<string, bool>("lastIndex+1", true));
            defaultConstructor.Statements.Add(cs);
           
            CodeStatement cs1 =  GetAssignmentWithExpression("IdNumber", GetMethodInvokationExpression("Guid", "NewGuid", new List<CodeExpression>()) );
            defaultConstructor.Statements.Add(cs1);

        }

        /// <summary>
        /// Builds event handler for changes in Property fields in the class
         ///   also SetConnectionTables method
         ///   that receives two table records and stores their indexes
         ///   as connection record.
        /// </summary>
        public override void BuildMethods()
        {
            base.BuildMethods();
            CodeMemberMethod PropertyChangedMethod = new CodeMemberMethod();
            PropertyChangedMethod.Name = "Property_Changed";
            PropertyChangedMethod.Attributes = MemberAttributes.Family;
            CodeStatement cer = GetEventHandlerInvokation("TableElementChanged", "EventArgs");
            PropertyChangedMethod.Statements.Add(cer);
            TheType.Members.Add(PropertyChangedMethod);
            CodeMemberMethod GetAttributeMethod = BuildMethodSignature("GetAttribute", MemberAttributes.Private, new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("System.String", "Name") });
            TheType.Members.Add(GetAttributeMethod);
            CodeMemberMethod SetConnectionTablesMethod = BuildMethodSignature("SetConnectionTables", MemberAttributes.Public, new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>(firstTable, "firstTable"), new KeyValuePair<string, string>(secondTable, "secondTable") });
            
            CodeStatement css = GetAssignment("firstTable_IdNumber", new KeyValuePair<string, bool>("firstTable.IdNumber", true));
            SetConnectionTablesMethod.Statements.Add(css);
            CodeStatement css1 = GetAssignment("secondTable_IdNumber", new KeyValuePair<string, bool>("secondTable.IdNumber", true));
            SetConnectionTablesMethod.Statements.Add(css1);
            TheType.Members.Add(SetConnectionTablesMethod);
            CodeMemberMethod GetFirstTableMethod = BuildMethodSignature("GetFirstTableName", MemberAttributes.Public, new List<KeyValuePair<string, string>>(), "System.String");
            CodeStatement css2 = new CodeMethodReturnStatement(new CodePrimitiveExpression(firstTable));
            GetFirstTableMethod.Statements.Add(css2);
            TheType.Members.Add(GetFirstTableMethod);
            CodeMemberMethod GetSecondTableMethod = BuildMethodSignature("GetSecondTableName", MemberAttributes.Public, new List<KeyValuePair<string, string>>(), "System.String");
            CodeStatement css3 = new CodeMethodReturnStatement(new CodePrimitiveExpression(secondTable));
            GetSecondTableMethod.Statements.Add(css3);
            TheType.Members.Add(GetSecondTableMethod);
        }
        public override void BuildEventHandlers()
        {
            List<KeyValuePair<string, string>> lstMethodParameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("System.Object", "sender"),
                    new KeyValuePair<string, string>("PropertyChangedEventArgs", "e")
                };
            CodeMemberMethod CurrentProperty_ChangedMethod = BuildMethodSignature("CurrentProperty_Changed", MemberAttributes.Public, lstMethodParameters);
            CodeStatement TableElementChangedEventinvokeStatement = GetEventHandlerInvokation("TableElementChanged", "EventArgs");
            CurrentProperty_ChangedMethod.Statements.Add(TableElementChangedEventinvokeStatement);
            TheType.Members.Add(CurrentProperty_ChangedMethod);
        }
        
        
    }
}