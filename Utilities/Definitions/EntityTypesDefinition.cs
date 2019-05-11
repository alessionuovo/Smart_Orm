using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Definitions
{
    /** \ingroup Bll
     <summary>
     This class is defines legal and illegal types of entities in system
     presented by files.
         Part of \ref  Facade 
     </summary>
    */
    public class EntityTypesDefinition
    {
       /// <summary>
       /// Legal Table entity
       /// </summary>
        public static string Table = "Table";
        /// <summary>
        /// Legal Connection Entity
        /// </summary>
        public static string Connection = "Connection";
        /// <summary>
        /// Illegal two entities in same file
        /// </summary>
        public static string IllegalTable = "TwoEntitiesillegal";
        public static string NoLegalEntity = "NoLegalEntity";
        /// <summary>
        /// Entity is null
        /// </summary>
        public static string NullEntity = "NullEntity";   
    }
}
