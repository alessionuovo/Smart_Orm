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
    public class ChangeVisitorFactoryBuilder:ClassBuilder 
    {

       
        public ChangeVisitorFactoryBuilder(string directory, string namespaceName)
        {
            this.directory = directory;
            createdType = new ChangeVisitorFactory(directory, "ChangeVisitorFactory");
            this.namespaceName = namespaceName;
            
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
            
            BuildPropertiesAndFields();
        }
    }
}