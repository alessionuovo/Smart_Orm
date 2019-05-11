using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Definitions
{
    /** \ingroup Bll
     <summary>
     This class is wrapper for IEntityDefinition,
     so through this class you can get all entity definitions in system.
         Part of \ref  Facade 
     </summary>
    */
    public class DbValidationDefinition
    {
        private DbValidationDefinition() {
            TDefinition = TableDefinition.TDefinition;
            CDefinition = ConnectionDefinition.CDefinition;
        }
        private static DbValidationDefinition _definition;
        public static DbValidationDefinition Definition { get { if (_definition == null) _definition = new DbValidationDefinition(); return _definition; } }
        public string TableTag = "Table";
        public string ConnectionTag = "Connection";
        /// <summary>
        /// Definition of Table file structure
        /// </summary>
       public TableDefinition TDefinition { get; set; }

        /// <summary>
        /// Definition of Connection file structure
        /// </summary>
        public ConnectionDefinition CDefinition { get; set; }
        public int NumElementsPerFile = 1;
    }
}
