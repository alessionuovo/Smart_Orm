using System.CodeDom;
using System.Collections.Generic;

namespace ClassGenerator
{
    /** \ingroup Bll
    <summary>
    Represents a generated class TableList.
    TableList inherits ObservableCollection.(in this case records of database table)
    The idea is implementation of notification on change on 
    inner item.
       Also part of \ref Builder 
    </summary>
   */
    internal class TableListClass : CreatedClass
    {
        public TableListClass(string directory, string name) : base(directory, name)
        {
            CreateGenericType(new List<GenericClassParameters>() { new GenericClassParameters() { BaseType = "ObservableCollection", Constraint = new List<string>() { "ITable", "new()" }, GenericType = "T", IsBaseTypeGeneric = true }, new GenericClassParameters() { BaseType = "IElement", Constraint = new List<string>() { "ITable", "new()" }, GenericType = "T", IsBaseTypeGeneric = true } } );
        }
        /// <summary>
        /// Create constructor with format variable
        /// (here format means also extension)
        /// </summary>
        public override void BuildOtherConstructors()
        {
            base.BuildOtherConstructors();
           CodeConstructor constructor = new CodeConstructor();
            constructor.Attributes = MemberAttributes.Public;
            constructor.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference("System.string"), "format"));
            CodeStatement csAssignFormat = GetAssignment("Format", new KeyValuePair<string, bool>("format", true));
            constructor.Statements.Add(csAssignFormat);
            CodeDelegateCreateExpression CollectionChangedDelegateStatement = new CodeDelegateCreateExpression(
                new CodeTypeReference("NotifyCollectionChangedEventHandler"), new CodeThisReferenceExpression(), string.Format("List_Changed"));
            // Attaches an EventHandler delegate pointing to TestMethod to the TestEvent event.
            CodeAttachEventStatement attachEventStatement = new CodeAttachEventStatement(new CodeThisReferenceExpression(), "CollectionChanged", CollectionChangedDelegateStatement);
            constructor.Statements.Add(attachEventStatement);
            TheType.Members.Add(constructor);
        }

       
        protected override void BuildDefaultConstructorBody()
        {
            base.BuildDefaultConstructorBody();
            CodeDelegateCreateExpression cdceCollectionChanged = new CodeDelegateCreateExpression(
                new CodeTypeReference("NotifyCollectionChangedEventHandler"), new CodeThisReferenceExpression(), string.Format("List_Changed"));
            // Attaches an EventHandler delegate pointing to TestMethod to the TestEvent event.
            CodeAttachEventStatement CollectionChangeEventAttachStatement = new CodeAttachEventStatement(new CodeThisReferenceExpression(), "CollectionChanged", cdceCollectionChanged);
            CodeStatement IsInitedFalseAssignStatement=GetAssignment("IsInited", new KeyValuePair<string,bool>("false", true));
            CodeStatement LoadInvokationStatement = GetMethodInvokationStatement("this", "Load", new List<string>() );
            CodeStatement IsInitedTrueAssignStatement = GetAssignment("IsInited", new KeyValuePair<string,bool>("true", true));
            defaultConstructor.Statements.Add(CollectionChangeEventAttachStatement);
            defaultConstructor.Statements.Add(IsInitedFalseAssignStatement);
            defaultConstructor.Statements.Add(LoadInvokationStatement);
            defaultConstructor.Statements.Add(IsInitedTrueAssignStatement);
        }
        /// <summary>
        /// Creates event handler on table records list change
        /// The idea is bubbling this change upper to event in db manager
        /// </summary>
        public override void BuildEventHandlers()
        {
            base.BuildEventHandlers();
            CodeMemberMethod cmmListChangeEvent = new CodeMemberMethod();
            cmmListChangeEvent.Name = "List_Changed";
            cmmListChangeEvent.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference("System.Object"), "sender"));
            cmmListChangeEvent.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference("NotifyCollectionChangedEventArgs"), "e"));
            CodeExpression ceCheckNotNull = GetConditionCompareToNull("e.NewItems");

            CodeStatement ITableAssignStatement = GetCastWithAssignment("ITable", "t", "e.NewItems[i]");
            CodeStatement TableRecordsChangeEventStatement = GetDelegateCreateStatement("System.EventHandler", string.Format("t"), string.Format("List_Change"), "TableElementChanged", false);
            List<CodeStatement> lst = new List<CodeStatement>();
            lst.Add(ITableAssignStatement);
            lst.Add(TableRecordsChangeEventStatement);
            CodeStatement TableChangedInvokationStatement = GetEventHandlerInvokation("TableChanged", "EventArgs");
            CodeStatement IsInitedCheckStatement = GetUnoConditionIf("IsInited", CodeBinaryOperatorType.IdentityEquality, "true", new List<CodeStatement> { TableChangedInvokationStatement });
            cmmListChangeEvent.Statements.Add(IsInitedCheckStatement);
            // Creates a for loop that sets testInt to 0 and continues incrementing testInt by 1 each loop until testInt is not less than 10.
            CodeCompoundStatement forLoopNewItemsStatement = GetSimpleForInteger("i", new System.Collections.Generic.KeyValuePair<int, string>(0, "e.NewItems.Count"), 1, lst);
                
            //CodeExpressionStatement expressionStatement;
            //expressionStatement = new CodeExpressionStatement(invoke1);
            CodeStatement[] trueStatements =
                forLoopNewItemsStatement.Statements.ToArray()
        ;
            CodeStatement[] falseStatements = { new CodeCommentStatement("Do this is false") };
            CodeStatement IfHasNewItemsStatement = new CodeConditionStatement(ceCheckNotNull, trueStatements);
            cmmListChangeEvent.Statements.Add(IfHasNewItemsStatement);
            TheType.Members.Add(cmmListChangeEvent);
        }
        public override void BuildPropertiesAndFields()
        {
            base.BuildPropertiesAndFields();
            CreateAutomaticProperty("Name", "string", "public");
            CreateAutomaticProperty("IsInited", "bool", "internal");
            CreateAutomaticProperty("Format", "string", "protected");
            CreateAutomaticProperty("changemanager", "ChangeManager", "private");
            CreateAutomaticProperty("visitor", "IChangeVisitor<T>", "private");
            BuildEvent("TableChanged", "EventHandler", MemberAttributes.Public);
        }

        /// <summary>
        /// Creates list_change method(event handler) for change in inner property(change in proerty value)
        /// also creates SaveMethod and LoadMethod- for save and load data from/to files that store it permanently
        /// and Accept-Accept of visitor part of Visitor design pattern
        /// </summary>
        public override void BuildMethods()
        {
            base.BuildMethods();
            CodeMemberMethod ListChangeMethod = BuildMethodSignature("List_Change", MemberAttributes.Public, new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("System.Object", "sender"), new KeyValuePair<string, string>("EventArgs", "e") });
            CodeStatement TableChangedDelegateInvokeStatement = GetEventHandlerInvokation("TableChanged", "EventArgs");
            CodeStatement IsInitedCheckStatement = GetUnoConditionIf("IsInited", CodeBinaryOperatorType.ValueEquality, "true", false, new List<CodeStatement> { TableChangedDelegateInvokeStatement });
            ListChangeMethod.Statements.Add(IsInitedCheckStatement);
            TheType.Members.Add(ListChangeMethod);
            CodeMemberMethod LoadMethod = BuildMethodSignature("Load", MemberAttributes.Public, new List<KeyValuePair<string, string>>() );
            CodeStatement LoadDataInvokationStatement = GetMethodInvokationStatement("visitor", "LoadData", new List<string>() { "this"});
            CodeMemberMethod SaveMethod = BuildMethodSignature("Save", MemberAttributes.Public, new List<KeyValuePair<string, string>>());
            CodeStatement SaveDataInvokationStatement = GetMethodInvokationStatement("visitor", "SaveData", new List<string>() { "this"});
            LoadMethod.Statements.Add(LoadDataInvokationStatement);
            SaveMethod.Statements.Add(SaveDataInvokationStatement);
            TheType.Members.Add(LoadMethod);
            TheType.Members.Add(SaveMethod);
            CodeMemberMethod AcceptMethod = BuildMethodSignature("Accept", MemberAttributes.Public, new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("IChangeVisitor<T>", "Visitor") });
            CodeStatement VisitorAssignStatement = GetAssignment("visitor", new KeyValuePair<string, bool>("Visitor", true));
            AcceptMethod.Statements.Add(VisitorAssignStatement);
            TheType.Members.Add(AcceptMethod);
        }

    }
}