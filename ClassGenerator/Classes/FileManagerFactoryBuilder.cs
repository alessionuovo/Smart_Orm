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
    This class is implementation of ClassBuilder
    for Property class.
    Property class represents a field in the table on database.
       Also part of \ref Builder 
    </summary>
   */
    public class FileManagerFactoryBuilder:ClassBuilder 
    {

       
        public FileManagerFactoryBuilder(string directory, string namespaceName)
        {
            this.directory = directory;
            createdType = new FileManagerFactory(directory, "FileManagerFactory");
            this.namespaceName = namespaceName;
            createdType.CreateGenericType(new List<GenericClassParameters>() { new GenericClassParameters() { BaseType = "ChangeVisitorFactory", Constraint = new List<string>() { "ITable", "new()" }, GenericType = "T", IsBaseTypeGeneric = true }, new GenericClassParameters() { BaseType = "IElement", Constraint = new List<string>() { "ITable", "new()" }, GenericType = "T", IsBaseTypeGeneric = true } });
            BuildNameSpace();
            BuildImports();
        }
        

       

        /// <summary>
        /// Create a Value property that stores the data
        /// and PropertyChangeEventHandler for listening to property changes
        /// </summary>

        protected virtual  void СreateProperties()
        {


         


        }

        public override void BuildPropertiesAndFields()
        {
            СreateProperties();
            
        }

        public override void BuildConstructors()
        {
            createdType.BuildDefaultConstructor();
            createdType.BuildOtherConstructors();
        }

        public override void BuildMethods()
        {
            createdType.BuildMethods();
            createdType.BuildEventHandlers();
        }

        /// <summary>
        /// Creates using for System and for System.ComponentModel
        /// </summary>
        protected override void BuildImports()
        {
            createdType.AddImport("System");
            createdType.AddImport("System.ComponentModel");
        }

        public override void BuildTypeInitializer()
        {
            BuildConstructors();
            BuildPropertiesAndFields();
        }
    }
}