using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace FileManagement
{
    /** \ingroup Bll
     <summary>
     This interface represents 
     one abstract manageable object(so we do not know its type
     and it is used for manage not for display)
     in a db.
         Part of \ref Strategy 
     </summary>
    */
    public interface IEntity
    {
        /// <summary>
        /// This method checks that database object
        /// structure fits the select format
        /// </summary>
        /// <returns>Validation Result: success/failure and message if failed</returns>
        KeyValuePair<bool,Result> Validate();
        
    }
}
