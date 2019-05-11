using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassGenerator
{
     /** \ingroup Bll
     <summary>
     Implementation of TypeBuilder to interfaces. Aso part of \ref Builder 
     </summary>
    */
    public abstract class InterfaceBuilder : TypeBuilder<CreatedInterface>
    {
        public abstract override void BuildMethods();
     

        public abstract override void BuildPropertiesAndFields();
       

        protected override void BuildImports()
        {
            
        }
    }
}
