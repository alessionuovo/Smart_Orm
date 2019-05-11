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
    Representation of IChangeVisitor generated interface.
       Also part of \ref Builder 
    </summary>
   */
    public class IChangeVisitorInterface:CreatedInterface
    {
        public IChangeVisitorInterface(string directory, string name) : base(directory, name)
        {
            this.directory = directory;
            List<GenericClassParameters> lst = new List<GenericClassParameters>()
            {
               new GenericClassParameters() {BaseType="",  GenericType="T",  Constraint=new List<string>() { "ITable", "new()" }, IsBaseTypeGeneric=true }
            };


            CreateGenericType(lst);

        }
        /// <summary>
        /// Builds method signatures for loaddata and savedata methods
        /// </summary>
        public override void BuildMethods()
        {
            base.BuildMethods();
           
            CodeMemberMethod LoadDataMethod = BuildMethodSignature("LoadData", new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("TableList<T>", "tableList") } );
            CodeMemberMethod SaveDataMethod = BuildMethodSignature("SaveData", new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("TableList<T>", "tableList") });
            
            TheType.Members.Add(LoadDataMethod);
            TheType.Members.Add(SaveDataMethod);
        }
    }
}
