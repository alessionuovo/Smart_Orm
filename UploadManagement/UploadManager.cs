using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
namespace UploadManagement
{
    /** \ingroup Bll
     <summary>
     Manages upload of database object definition
     files(xml, json etc) to application
     Part of \ref State Design Pattern
     </summary>
    */
    public class UploadManager
    {
        /// <summary>
        /// State Of the class
        /// works like in state-machine
        /// </summary>
        public UploadState state;
        protected List<string> filenames;
        public UploadManager()
        {
            filenames = new List<string>();
        }

        /// <summary>
        /// Manages all the upload mechanism
        /// and set of states
        /// </summary>
        /// <param name="files">Files to upload</param>
        /// <param name="format">in what format the files, Important primarily for validation</param>
        /// <returns>Result of Execution-success or failure if failure there is posibble to get the error message</returns>
        public bool Execute(List<string> files, string format)
        {
            foreach(string file in files)
            {
                if (!string.IsNullOrEmpty(file))
                {
                    KeyValuePair<string, string> kvpFile = FileHelper.SeparateFileAndDirectory(file);
                    string filename = kvpFile.Value;
                    filenames.Add(filename);
                }
            }
            do
            {
                SetState(files, format);
                state.Execute();
            } while (state.Result.Status == "Success" && !(state is FinishState));
            if (state.Result.Status!= "Success")
            {
                return false;
            }
            return true;
          
        } 

        /// <summary>
        /// 
        /// </summary>
        /// <returns>List of occurred errors, the key is exception value is friendly message</returns>
        public SortedList<string,string> GetErrors()
        {
            return state.Result.error;
        }

        /// <summary>
        /// Sets class state to the next state
        /// </summary>
        /// <param name="files">List of files</param>
        /// <param name="format">in what format for validation</param>
        public void SetState(List<string> files, string format)
        {
            if (state == null)
                state = new InitialState(files, format);
            else if (state is InitialState)
            {
                state = new ValidationState(filenames, format);
            }
            else if (state is ValidationState)
            {
                state = new EmptyState();
               
            }
            else if (state is EmptyState)
                state = new MoveToConstState(files, format);
            else if (state is MoveToConstState)
            {
                state = new CreateDiagram(filenames, format);
            }
            else state = new FinishState();
        }
         
    }
}

