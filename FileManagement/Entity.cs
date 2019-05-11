using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Utilities.Definitions;

namespace FileManagement
{
    /** \ingroup Bll
     <summary>
     Represents an abstract database object
     and implements IEntity interface operations
     for now it is only validate operation.
         Part of \ref Strategy 
     </summary>
    */
    public class Entity : IEntity
    {
        internal IManager manager;

        public Entity(IManager manager)
        {
            this.manager = manager;
        }

       
        /// <summary>
        /// Manages validation of the entity
        /// </summary>
        /// <returns>Validation result-success/failure(key)-error messages(the value object)</returns>
        public KeyValuePair<bool, Result> Validate()
        {
            SortedList<string, EntityElement> slDefinitions = new SortedList<string, EntityElement>();
            slDefinitions = SystemDefinitionsManager.DefinitionsManager.GetValidationDefinitions();
            return manager.Validate(slDefinitions);

        }
    }
}
