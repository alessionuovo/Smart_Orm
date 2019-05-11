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
    for Connection class.
    The Connection class represents a specific table in db
    </summary>
   */
    public class ConnectionBuilder:ClassBuilder 
    {

        protected Connection connection;
        public ConnectionBuilder(string directory, string namespaceName,Connection connection)
        {
            this.directory = directory;
            createdType = new ConnectionClass(directory, connection.Name, connection.FirstTable, connection.FirstRelation, connection.SecondTable, connection.SecondRelation);
            this.connection = connection;
            this.namespaceName = namespaceName;
         
            BuildImports();
            BuildNameSpace();

        }

        /// <summary>
        /// Generate class members that represent foreign keys in table
        /// </summary>
        protected void CreateConnectionProperties()
        {
           
        }

        /// <summary>
        /// Generate class members that represent fields in table
        /// </summary>
        protected void СreateProperties()
        {
            string firstTable = connection.FirstTable;
            string firstRelation = connection.FirstRelation;
            string secondTable = connection.SecondTable;
            string secondRelation = connection.SecondRelation;
        
           
            
        }


        /// <summary>
        /// Creates three connection indexes
        /// overall and for tables that it connects
        /// </summary>
        public override void BuildPropertiesAndFields()
        {
            СreateProperties();
            createdType.BuildEvent("TableElementChanged", "EventHandler", MemberAttributes.Public);
            CreateConnectionProperties();
           
            createdType.BuildProperty("firstTable_IdNumber", "System.Guid", "_firstTableIdNumber", "internal");
            createdType.BuildProperty("secondTable_IdNumber", "System.Guid", "_secondTableIdNumber", "internal");
            createdType.BuildField("lastIndex", "System.Int32", MemberAttributes.Private | MemberAttributes.Static, "0");
            createdType.BuildIndexerProperty("IdNumber", "System.Guid", "_idNumber", "internal", new List<string>() {  "_idNumber" });
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