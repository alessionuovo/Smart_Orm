using FileManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Utilities.Definitions;

namespace UploadManagement
{
    /** \ingroup Bll
     <summary>
     validates that files fit to format
     (second state) and structure correctly describe
     database object-how is defined in system.
        Part of \ref State Design Pattern
     </summary>
    */
    public class ValidationState:UploadState
    {
        internal string format;
        
        internal List<string> files;
       
        public ValidationState(List<string> files, string format)
        {
            this.format = format;
            this.files = files;
           
            this.Result = new ResultState();
        }

        /// <summary>
        /// Manages files validation for extension
        /// and structure
        /// </summary>
        public override void Execute()
        {
            foreach (string file in files)
            {
                KeyValuePair<bool, Result> result=DbDefinitionsManager.DbManager.Validate(file, format);
               
                if (!result.Key)
                {
                    this.Result.Status = "error";
                    
                    this.Result.error.Add(file, result.Value.DisplayMessage);

                }
                
            }
            if (this.Result.Status != "error")
                this.Result.Status = "Success";
            else DbDefinitionsManager.DbManager.ClearEntities();
        }
    }
}
