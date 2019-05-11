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
    Represents generated IElement interface
  which represents interface that inherits Save and Load
    operations to collection from another interface and serves as part of Visitor design pattern
    functioning as IVisitableObject. Also this interface is Generic
    and inherits IList generic.
       Also part of \ref Builder 
    </summary>
   */
    public class IConnectionInterface:CreatedInterface
    {
        public IConnectionInterface(string directory, string name) : base(directory, name)
        {
            this.directory = directory;
            CreateGenericType(new List<GenericClassParameters>() { new GenericClassParameters() { BaseType = "", GenericType = "FirstTable", Constraint = new List<string>() { "ITable", "new()" }, IsBaseTypeGeneric = true }, new GenericClassParameters() { BaseType = "", GenericType = "SecondTable", Constraint = new List<string>() { "ITable", "new()" }, IsBaseTypeGeneric = true } });
            TheType.BaseTypes.Add("ITable");
            
        }

        /// <summary>
        /// Creates signature of Accept method 
        /// </summary>
        public override void BuildMethods()
        {
            base.BuildMethods();
            CodeMemberMethod cmmConnectionTables = BuildMethodSignature("SetConnectionTables",  new List<KeyValuePair<string, string>>() {new KeyValuePair<string, string>("FirstTable", "firstTable"), new KeyValuePair<string, string>("SecondTable", "secondTable") });
          
            TheType.Members.Add(cmmConnectionTables);
            CodeMemberMethod cmmFirstTableName = BuildMethodSignature("GetFirstTableName", new List<KeyValuePair<string, string>>() , "System.String");

            TheType.Members.Add(cmmFirstTableName);
            CodeMemberMethod cmmSecondTableName = BuildMethodSignature("GetSecondTableName", new List<KeyValuePair<string, string>>(), "System.String");

            TheType.Members.Add(cmmSecondTableName);
        }

       

        public override void BuildPropertiesAndFields()
        {
            base.BuildPropertiesAndFields();
            CreateAutomaticReadOnlyProperty("firstTable_IdNumber", "System.Guid");
            CreateAutomaticReadOnlyProperty("secondTable_IdNumber", "System.Guid");
        }
    }
}
