using System;
using DiagramContent;
using System.Reflection.Emit;
using System.Reflection;
using System.CodeDom;
using System.Text;
using System.IO;
using Microsoft.CSharp;
using System.Collections.Generic;

namespace ClassGenerator
{
    /** \ingroup Bll
    <summary>
    Represents a builder of ConnectionProperty
    class that represents Foreign keys and relationships in database
       Also part of \ref Builder 
    </summary>
   */
    public class ConnectionPropertyBuilder:PropertyBuilder 
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="directory">Where to save the file</param>
        /// <param name="namespaceName">name of namespace to which type must belong</param>
        public ConnectionPropertyBuilder(string directory, string namespaceName):base(directory, namespaceName)
        {
            this.directory = directory;
            createdType = new ConnectionPropertyClass(directory, "ConnectionProperty");
            this.namespaceName = namespaceName;
            List<GenericClassParameters> lst = new List<GenericClassParameters>()
            {
               new GenericClassParameters() {BaseType="Property", GenericType="T", Constraint=new List<string>() { "IConvertible"}, IsBaseTypeGeneric=true } };

            createdType.CreateGenericType(lst);
            BuildNameSpace();
            BuildImports();
        }

       

        

        protected  override void СreateProperties()
        {


            createdType.CreateAutomaticProperty("Value", "T");
            createdType.CreateAutomaticProperty("IsPrimary", "T");


        }

        /// <summary>
        /// No properties for now 
        /// because for now connection property and property
        /// work the same
        /// </summary>
        public override void BuildPropertiesAndFields()
        {
           
            
        }

        public override void BuildConstructors()
        {
            createdType.BuildOtherConstructors();
        }

        public override void BuildMethods()
        {
            
        }

        /// <summary>
        /// Using directives for a class
        /// </summary>
        protected override void BuildImports()
        {
            createdType.AddImport("System");
        }
    }
}