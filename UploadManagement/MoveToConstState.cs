using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Utilities.Definitions;

namespace UploadManagement
{
    /** \ingroup Bll
     <summary>
     Moves files to const directory
     after success validation.
        Part of \ref State Design Pattern
     </summary>
    */
    public class MoveToConstState : UploadState
    {
        internal string format;

        internal List<string> files;

        public MoveToConstState(List<string> files, string format)
        {
            this.format = format;
            this.files = files;

            this.Result = new ResultState();
        }
        /// <summary>
        /// Manages move of files of upload to upload directory
        /// after successfull upload
        /// </summary>
        public override void Execute()
        {
            foreach(string file in files)
            {
                FileHelper.MoveToConstDirectory(file);
                
            }
            this.Result.Status = "Success";
            if (this.Result.Status == "Success")
                SystemDefinitionsManager.DefinitionsManager.SetUploadStateToConst();
        }
    }
}
