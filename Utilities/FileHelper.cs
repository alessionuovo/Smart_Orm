using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Definitions;

namespace Utilities
{
    /* \addtogroup Dal
     @{
     <summary>
     class FileHelper-Helps to work with files
     </summary>
    */
    public class FileHelper
    {
        /// <summary>
        /// Separates full path to file and directory
        /// and puts them in keyValuePair
        /// </summary>
        /// <param name="file">Full path to file</param>
        /// <returns>KeyValuePair-key directory name, value-file name</returns>
        public static KeyValuePair<string,string> SeparateFileAndDirectory(string file)
        {
           
           
            return new KeyValuePair<string, string>(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file));
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename">Full Path to file</param>
        /// <returns>Filename without extension</returns>
        public static string GetFileNameOnly(string filename)
        {

            return Path.GetFileNameWithoutExtension(filename);
        }

        /// <summary>
        /// Copies file to temporary directory(used in upload)
        /// </summary>
        /// <param name="file">Full Path to file</param>
        public static void MoveToTempDirectory(string file)
        {
            string directory = Path.Combine(Directory.GetCurrentDirectory(), SystemDefinitionsManager.DefinitionsManager.GetCurrentUploadDirectory());
            MoveToDirectory(file, directory);
        }
        public static void DeleteFromConstDirectory(string file)
        {

        }
        public static void MoveToConstDirectory(string file)
        {
            string directory =  SystemDefinitionsManager.DefinitionsManager.GetCurrentUploadDirectory();
            file = Path.Combine(directory, file);
            
            string currentdirectory= SystemDefinitionsManager.DefinitionsManager.GetConstUploadDirectory();
            MoveToDirectory(file, currentdirectory);
           
        }

        /// <summary>
        /// Copies file to specified directory
        /// </summary>
        /// <param name="file">Full Path</param>
        /// <param name="directory">To which directory to copy</param>
        private static void MoveToDirectory(string file, string directory)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            File.Copy(file, Path.Combine(directory, file.Substring(file.LastIndexOf('\\') + 1)), true);
        }
    }
    /* @} */
}
