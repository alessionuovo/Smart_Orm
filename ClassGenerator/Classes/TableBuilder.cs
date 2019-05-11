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
    This class represents implementation of ClassBuilder
    for Table class.
    The Table class represents a specific table in db.
        Also part of \ref Builder 
    </summary>
   */
    public class TableBuilder:ClassBuilder 
    {

        protected Table table;
        public TableBuilder(string directory, string namespaceName,Table table)
        {
            this.directory = directory;
            createdType = new TableClass(directory, table.Name, table.Properties, table.ConnectionProperties);
            
            this.namespaceName = namespaceName;
            this.table = table;
            BuildImports();
            BuildNameSpace();

        }

        /// <summary>
        /// Generate class members that represent foreign keys in table
        /// </summary>
        protected void CreateConnectionProperties()
        {
            foreach (ConnectionProperties p in table.ConnectionProperties)
            {

                createdType.CreateAutomaticProperty(p.Name, string.Format("ConnectionProperty<{0}>", p.Type));
                


            }
        }

        /// <summary>
        /// Generate class members that represent fields in table
        /// </summary>
        protected void СreateProperties()
        {
            foreach (Properties p in table.Properties)
            {
                if (string.IsNullOrEmpty(p.Name) || string.IsNullOrEmpty(p.Type))
                    continue;
                //createdType.CreateAutomaticReadOnlyProperty(p.Name, string.Format("Property<{0}>", p.Type));
                createdType.CreateAutomaticProperty(p.Name, string.Format("Property<{0}>", p.Type), "public", "internal");


            }
            createdType.BuildPropertiesAndFields();
            
        }



        public override void BuildPropertiesAndFields()
        {
            СreateProperties();
            createdType.BuildEvent("TableElementChanged", "EventHandler", MemberAttributes.Public);
            CreateConnectionProperties();
        }

        public override void BuildConstructors()
        {
            createdType.BuildDefaultConstructor();
        }

        public override void BuildMethods()
        {

            createdType.BuildMethods();
            createdType.BuildEventHandlers();

            
        }

        protected override void BuildImports()
        {
            createdType.AddImport("System");
            createdType.AddImport("System.ComponentModel");
        }

        public override void BuildTypeInitializer()
        {
            BuildPropertiesAndFields();
            BuildConstructors();
           
        }
    }
}