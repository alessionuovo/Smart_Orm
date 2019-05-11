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
     Interface for Creating modules
     of generated types. Also part of \ref Builder 
     </summary>
    */
    public interface IModuleBuilder
    {
        /// <summary>
        /// Creates, stores and compiles all the generated types
        /// </summary>
        /// <param name="directory">Where to save generated types</param>
        /// <returns>List of generated types</returns>
        List<CreatedType> Create(string directory);
    }
}
