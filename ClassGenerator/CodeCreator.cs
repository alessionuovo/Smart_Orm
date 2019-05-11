using DiagramContent;
using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utilities.Definitions;

namespace ClassGenerator
{
     /** \ingroup Bll
     <summary>
     <para>This class manages creation and compilation of code -in our case it's
     ORM style db management system
     </para>
     </summary>
    */
    public class CodeCreator
    {
        /// <summary>
        /// <para>Directory: stores a directory</para>
        /// </summary>
        protected string directory;
        private string selectedDirectory;

        public CodeCreator()
        {
            
        }

        public CodeCreator(string selectedDirectory)
        {
            this.selectedDirectory = selectedDirectory;
        }

        /// <summary>
        /// Manages generation and compile of classes
        /// </summary>
        public void Build()
        {
            List<CreatedType> lstSourceCode = new List<CreatedType>();
            string directory = SystemDefinitionsManager.DefinitionsManager.GetCurrentGeneratedDirectory();
            IModuleBuilder dbModuleBuilder = new DbModuleBuilder();
            (dbModuleBuilder as DbModuleBuilder).SetGenerateDirectory(selectedDirectory);
            lstSourceCode.AddRange(dbModuleBuilder.Create(directory));
            ClassSaver.Save(selectedDirectory, lstSourceCode);
            CodeCompiler codecompiler = new CodeCompiler(directory);
            codecompiler.Compile("GeneratedCode");
            
            
        }
        
    }

    
}
