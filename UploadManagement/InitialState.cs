using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace UploadManagement
{
    /** \ingroup Bll
     <summary>
     Begin State in the system
     Its job is to move to temp directory.
        Part of \ref State Design Pattern
     </summary>
    */
    public class InitialState:UploadState
    {
        internal List<string> files;
        internal string format;
        public InitialState(List<string> files, string format)
        {
            this.files = files;
            this.format = format;
            this.Result = new ResultState();
        }

        /// <summary>
        /// Manages files move to temp directory
        /// </summary>
        public override void Execute()
        {
            foreach(string file in files)
            {
                if (string.IsNullOrEmpty(file)) continue;
                FileHelper.MoveToTempDirectory(file);
                
            }
            this.Result.Status = "Success";
        }
    }
}
