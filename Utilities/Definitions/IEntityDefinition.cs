using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Definitions
{
    /** \ingroup Bll
     <summary>
     Definition of structure of storage in file
     of database object like Table, Connection 
     </summary>
    */
    public interface IEntityDefinition
    {
        /// <summary>
        /// Recursive class that contains structure of current Entity
        /// that can be Table, Connection
        /// </summary>
        EntityElement Element { get; set; }
    }
}
