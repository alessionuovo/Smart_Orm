using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// This namespace is responsible for generating and populating c#
/// types via \ref Builder 
/// </summary>
namespace ClassGenerator
{

    /// <summary>
    /// This structure is used to cgenerate generic c# type
    /// </summary>
    public struct GenericClassParameters
    {
        private List<string> _genTypes;
        private List<string> _genImplement;
        public List<string> GenericImplement { get { if (_genImplement == null) _genImplement = new List<string>(); return _genImplement;  } internal set { _genImplement = value; } }
        /// <summary>
        /// What is the basetype
        /// </summary>
        public string BaseType { get; set; }
        /// <summary>
        /// The letter or word that represents generic type
        /// </summary>
        public string GenericType { get; set; }
        /// <summary>
        /// Collection of constraints
        /// </summary>
        public List<string> Constraint { get; set; }
        /// <summary>
        /// IDentifies if base type also generic
        /// </summary>
        public bool IsBaseTypeGeneric { get; set; }
        /// <summary>
        /// List of types, for generating something like SortedList that contains
        /// two or more generic params
        /// </summary>
        public List<string> GenericTypes { get { if (_genTypes == null) _genTypes = new List<string>(); return _genTypes; } internal set { _genTypes = value; } }
    }
     /** \ingroup Bll
    <summary>
     Manages creation of class
     
    </summary>
    */
    public class ClassCreatorManager:TypeCreatorManager<CreatedClass>
    {
        
        public ClassCreatorManager(ClassBuilder classBuilder)
        {
            this.typeBuilder = classBuilder;
        }
        /// <summary>
        /// Override of Create method for class c# type.
        /// Create Initializer(constructor and properties and may be other stuff)
        /// and create methods
        /// </summary>
        /// <returns>Created Class</returns>
        public override CreatedType Create()
        {
            typeBuilder.BuildTypeInitializer();
          
            typeBuilder.BuildMethods();
            return typeBuilder.GetCreatedType();
        }
    }
}
