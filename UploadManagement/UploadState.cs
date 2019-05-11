using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UploadManagement
{
    /** \ingroup Bll
     <summary>
     Base class for all states in state machine.
        Part of \ref State Design Pattern
     </summary>
    */
    public abstract class UploadState
    {
        /// <summary>
        /// Main method that does the job of the state
        /// </summary>
        public abstract void Execute();
        /// <summary>
        /// Link to manager
        /// </summary>
        protected UploadManager manager;
        /// <summary>
        /// Link to result
        /// </summary>
        public ResultState Result;
        protected string directory;
        
    }
}
