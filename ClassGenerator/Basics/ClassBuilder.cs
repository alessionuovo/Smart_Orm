using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassGenerator
{
    /** \ingroup Bll
     <summary>
    This class responsible for building(creating)
     c# classes(generation in memory).
        Also is part of \ref Builder.
     </summary>
    */
    public abstract class ClassBuilder:TypeBuilder<CreatedClass>
    {
        /// <summary>
        /// This method is responsible for creating constructors of a class
        /// </summary>
        public abstract void BuildConstructors();
      

        
       

       
    }
}
