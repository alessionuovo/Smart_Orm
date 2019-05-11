using FileManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UploadManagement
{
    /** \ingroup Bll
     <summary>
     Manages creation of object based on files and can be binded to diagram control.
         Part of \ref State Design Pattern
     </summary>
    */
    public class CreateDiagram : UploadState
    {
        internal List<string> files;
        internal string format;
        public CreateDiagram(List<string> files, string format)
        {
            this.files = files;
            this.format = format;
            this.Result = new ResultState();
        }

        /// <summary>
        /// Manages creation of object based on files and can be binded to diagram control
        /// </summary>
        public override void Execute()
        {
            foreach (string file in files)
            {
                DbDefinitionsManager.DbManager.CreateDiagram(file);
                
            }
            this.Result.Status = "Success";
        }
    }
}
