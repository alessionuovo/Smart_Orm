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
    Represents a ChangeManager generated class.
       Also part of \ref Builder 
    </summary>
   */
    public class ChangeManagerClass:CreatedClass

    {
        public ChangeManagerClass(string directory, string name) : base(directory, name)
        {
            // this.table = table;

        }

        /// <summary>
        /// Creates the only field in the class- list of changed database objects
        /// </summary>
        public override void BuildPropertiesAndFields()
        {
            base.BuildPropertiesAndFields();
            BuildField("lstTableRecords", "List<IRecord>", MemberAttributes.Private);

        }
        protected override void BuildDefaultConstructorBody()
        {
            base.BuildDefaultConstructorBody();
            CodeStatement CreateRecordsStatement = GetObjectCreateStatement("List<IRecord>", "lstTableRecords");
            defaultConstructor.Statements.Add(CreateRecordsStatement);
        }

        /// <summary>
        /// Creates two methods:
      

      //  Add 
      // 1.Adds IRecord(base interface for database object) to list 
       ///  2.Execute  Takes all elements in list and save them permanently
        /// in file by calling corresponding method)
        /// 
        /// 
       
        /// </summary>
        public override void BuildMethods()
        {
            base.BuildMethods();
            CodeMemberMethod AddRecordsMethod = BuildMethodSignature("Add", MemberAttributes.Assembly, new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("IRecord", "elem")  });
            CodeStatement csCollectionAddInvoke = GetMethodInvokationStatement("lstTableRecords", "Add", new List<string>() { "elem" });
            AddRecordsMethod.Statements.Add(csCollectionAddInvoke);
            CodeMemberMethod ExecuteMethod = BuildMethodSignature("Execute", MemberAttributes.Assembly, new List<KeyValuePair<string, string>>());
            CodeStatement AssignRecordStatement = GetAssignment("IRecord", "t", new CodeVariableReferenceExpression("lstTableRecords[i]"));
            CodeStatement SaveMethodInvokeStatement = GetMethodInvokationStatement("t", "Save", new List<string>());
            CodeCompoundStatement SaveAllCommandsStatement = GetSimpleForInteger("i", new KeyValuePair<int, string>(0, "lstTableRecords.Count"), 1, new List<CodeStatement>() { AssignRecordStatement, SaveMethodInvokeStatement });
            ExecuteMethod.Statements.AddRange(SaveAllCommandsStatement.Statements.ToArray());
            CodeStatement ClearCollectionStatement = GetMethodInvokationStatement("lstTableRecords", "Clear", new List<string>());
            ExecuteMethod.Statements.Add(ClearCollectionStatement);
            TheType.Members.Add(AddRecordsMethod);
            TheType.Members.Add(ExecuteMethod);
            
        }
    }
}
