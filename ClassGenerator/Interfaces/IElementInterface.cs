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
    public class IElementInterface:CreatedInterface
    {
        public IElementInterface(string directory, string name) : base(directory, name)
        {
            this.directory = directory;
            CreateGenericType(new List<GenericClassParameters>() { new GenericClassParameters() { BaseType = "IList", GenericType = "T", Constraint = new List<string>() { "ITable", "new()" }, IsBaseTypeGeneric = true } });
            TheType.BaseTypes.Add("IRecord");
            
        }

        /// <summary>
        /// Creates signature of Accept method 
        /// </summary>
        public override void BuildMethods()
        {
            base.BuildMethods();
            CodeMemberMethod cmm = BuildMethodSignature("Accept",  new List<KeyValuePair<string, string>>() {new KeyValuePair<string, string>("IChangeVisitor<T>", "visitor") });
            
            TheType.Members.Add(cmm);
          
        }
        /// <summary>
        /// Creates property Name
        /// </summary>
        public override void BuildPropertiesAndFields()
        {
            base.BuildPropertiesAndFields();
            BuildProperty("Name", "string");
           
        }
    }
}
