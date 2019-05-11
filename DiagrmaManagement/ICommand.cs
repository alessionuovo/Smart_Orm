using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagramManagement
{
    /** \ingroup Bll
     <summary>
     This interface represents an abstract change in the system.
         Part of \ref Command 
     </summary>
    */
    public interface ICommand
    {
        /// <summary>
        /// This method manage the store of the change results to file/db
        /// or somethinq other that is permanent
        /// </summary>
        void Execute();
    }
}
