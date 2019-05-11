using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using Utilities.Definitions;

namespace ClassGenerator
{
    /** \ingroup Dal
   
     <summary>
     Saves all generated types to specified directory
     </summary>
    */
    public static class ClassSaver
    {
        /// <summary>
        /// Saves all the the generated type
        /// </summary>
        /// <param name="directory">In which directory to store</param>
        /// <param name="lstCreatedTypes">Generated Types to store</param>
        public static void Save(string directory, List<CreatedType> lstCreatedTypes)
        {
            CodeDomProvider cSharpCompiler = new CSharpCodeProvider();
            foreach(CreatedType classcreated in lstCreatedTypes)
            {
                StreamWriter sw = new StreamWriter(Path.Combine(directory, classcreated.GetName() + ".cs"));
                StringBuilder generatedCode = new StringBuilder();
                StringWriter codeWriter = new StringWriter(generatedCode);
                cSharpCompiler.GenerateCodeFromCompileUnit(classcreated, codeWriter, null);
                string classCode = generatedCode.ToString();
                sw.Write(classCode);
                sw.Close();
            }
            string uploaddirectory=SystemDefinitionsManager.DefinitionsManager.GetCurrentUploadDirectory();
            DirectoryInfo di = new DirectoryInfo(uploaddirectory);
            if (!Directory.Exists(Path.Combine(directory, "Files")))
            {
                Directory.CreateDirectory(Path.Combine(directory, "Files"));
            }
            string fileDir = Path.Combine(directory, "Files");
          
            foreach (FileInfo fi in di.GetFiles())
            {
                File.Copy(fi.FullName, Path.Combine(fileDir, fi.Name));
            }
        }
    }
    
}
