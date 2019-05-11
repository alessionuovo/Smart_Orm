using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassGenerator
{
    /** \ingroup Dal
   
     <summary>
     class CodeCompiler-Compiles all files to dll
    
     </summary> 
    */
    public class CodeCompiler
    {
        protected string directory;
        public CodeCompiler(string directory)
        {
            this.directory = directory;
        }

        /// <summary>
        /// Compiles all files to dll
        /// </summary>
        /// <param name="assemblyName">Assembly Name to create</param>
       
        public void Compile(string assemblyName)
        {
            CodeDomProvider csharpCompiler = new CSharpCodeProvider();
            CompilerParameters cp = new CompilerParameters();
            cp.ReferencedAssemblies.Add("System.dll");
            cp.GenerateExecutable = false;
            cp.CompilerOptions = string.Format(" /out:{0}\\\\{1}.dll", directory, assemblyName);
            // Invoke compilation.
            string [] files=GetFiles(directory);
            CompilerResults crSourceCode = csharpCompiler.CompileAssemblyFromFile(cp, files);
            WriteErrorsToFile(crSourceCode);
        }
        
        /// <summary>
        /// Searches c# files in directory
        /// </summary>
        /// <param name="directory">Directory to search files for compilation</param>
        /// <returns>Array of all c# files in directory</returns>
        private string [] GetFiles(string directory)
        {
            DirectoryInfo di = new DirectoryInfo(directory);
            FileInfo[] fi = di.GetFiles("*.cs");
            string[] files = new string[fi.Count()];
            for(int i=0; i<fi.Count(); i++)
            {
                files[i] = fi[i].FullName;
            }
            return files;
        }

        /// <summary>
        /// Writes errors to file in order that user will know 
        /// why ddl was not created
        /// </summary>
        /// <param name="compilationResults">Result of compilation process</param>
        private void WriteErrorsToFile(CompilerResults compilationResults)
        {
            if (compilationResults.Errors.Count>0)
            {
                
                StreamWriter sw = new StreamWriter(Path.Combine(directory,"errors.txt"));
                sw.WriteLine("Compilation Errors");
                sw.WriteLine();
                foreach (CompilerError error in compilationResults.Errors)
                {
                    sw.WriteLine(string.Format("Error in file {0} line {1} column {2} message: {3}", error.FileName, error.Line, error.Column, error.ErrorText));
                }
                sw.Close();
            }
        }

      
    }
  
}
