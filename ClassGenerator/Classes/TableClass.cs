using DiagramContent;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

namespace ClassGenerator
{
    /** \ingroup Bll
    <summary>
    Represents table in database(the generated class for it).
       Also part of \ref Builder 
    </summary>
   */
    internal class TableClass : CreatedClass
    {
        internal ObservableCollection<Properties> prop;
        internal ObservableCollection<ConnectionProperties> connprop;
        public TableClass(string directory, string name, ObservableCollection<Properties> properties, ObservableCollection<ConnectionProperties> connProperties) : base(directory, name)
        {
           // this.table = table;
            TheType.BaseTypes.Add("ITable");
            prop = properties;
            connprop = connProperties;
        }
        
        /// <summary>
        /// Default constructor that creates object
        /// for each of its Property(field in database table) fields
        /// and defines the delegate for property change
        /// 
        /// </summary>
        protected override void BuildDefaultConstructorBody()
        {
            foreach(Properties p in prop)
            {
                if (string.IsNullOrEmpty(p.Name) || string.IsNullOrEmpty(p.Type))
                    continue;
                CodeStatement ObjectCreateStatement = GetObjectCreateStatement(string.Format("Property<{0}>", p.Type), p.Name);
                defaultConstructor.Statements.Add(ObjectCreateStatement);
               
                CodeStatement attachTableEvent = GetDelegateCreateStatement("PropertyChangedEventHandler", p.Name, "CurrentProperty_Changed", "PropertyChanged", true);

                 defaultConstructor.Statements.Add(attachTableEvent);
            }
            CodeStatement cs = GetAssignment("lastIndex", new KeyValuePair<string, bool>("lastIndex+1", true));
            defaultConstructor.Statements.Add(cs);
            CodeStatement cs1 = GetAssignmentWithExpression("IdNumber", GetMethodInvokationExpression("Guid", "NewGuid", new List<CodeExpression>()));
            defaultConstructor.Statements.Add(cs1);
        }

        /// <summary>
        /// Builds event handler for changes in Property fields in the class
       
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

        /// <summary>
        /// Create two fields that exist in each class that represents table record
        /// id number and lastindex to create auto increment mechanism.
        /// </summary>
        public override void BuildPropertiesAndFields()
        {
            base.BuildPropertiesAndFields();
            BuildIndexerProperty("IdNumber", "System.Guid", "_idNumber", "internal", new List<string>() {  "_idNumber" });
            BuildField("lastIndex", "System.Int32", MemberAttributes.Static | MemberAttributes.Private, "0");
        }

       
    }
}