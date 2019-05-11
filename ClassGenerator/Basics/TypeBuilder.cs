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
     This class is responsible of building
    c# type(generating it in memory). Also part of \ref Builder 
     </summary>
     <typeparam name="T">Type of class that represents c# type being created</typeparam>
    */
    public abstract class TypeBuilder<T>
        where T :CreatedType
    {
        protected T createdType;
        protected string namespaceName;
        protected string directory;
        /// <summary>
        /// Build property member of class/interface/struct
        /// </summary>
        public abstract void BuildPropertiesAndFields();

        /// <summary>
        /// BuildMethod if class/struct
        /// </summary>
        public abstract void BuildMethods();
        public abstract void BuildTypeInitializer();

        /// <summary>
        /// Create using directives
        /// </summary>
        protected abstract void BuildImports();
        /// <summary>
        /// This function create a namespace block in the type
        /// namespace namespaceName
        /// {
        /// }
        /// </summary>
        protected void BuildNameSpace()
        {
            CodeNamespace cnRoot = new CodeNamespace(namespaceName);
            cnRoot.Types.Add(createdType.TheType);
            createdType.Namespaces.Add(cnRoot);
        }
       
        /// <returns>Object that represents the type being created by a builder</returns>
        public T GetCreatedType()
        {
            return createdType;
        }
    }
}
