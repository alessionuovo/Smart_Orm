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
    Represents generated IRecord interface
     which represents
    record in db with operations of Save and Load.
       Also part of \ref Builder 
    </summary>
   */
    public class IRecordInterface:CreatedInterface
    {
        public IRecordInterface(string directory, string name) : base(directory, name)
        {
            this.directory = directory;
           

            
        }

        /// <summary>
        /// Creates signatures for Load and Save methods
        /// that used to Load ansd Save data from db(here from hierachical file formats like xml)
        /// </summary>
        public override void BuildMethods()
        {
            base.BuildMethods();
            CodeMemberMethod LoadMethod = BuildMethodSignature("Load", new List<KeyValuePair<string, string>>() );
            CodeMemberMethod SaveMethod = BuildMethodSignature("Save",  new List<KeyValuePair<string, string>>() );
           
            TheType.Members.Add(LoadMethod);
            TheType.Members.Add(SaveMethod);
        }


        public override void BuildPropertiesAndFields()
        {
            base.BuildPropertiesAndFields();
            BuildProperty ( "Name", "System.String");
           
        }
    }
}
