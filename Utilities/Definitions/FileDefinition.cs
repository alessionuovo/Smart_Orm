namespace Utilities.Definitions
{
    /** \ingroup Bll
     <summary>
     Definition of legal file types and formats
     that store database objects
     </summary>
    */
    public class FileDefinition
    {
        private string format;
        /// <summary>
        /// 
        /// </summary>
        public string Format { get { return format; } protected set { format = value; } }
        private string extension;
        public string Extension { get { return extension; } protected set { extension = value; } }
        
        public FileDefinition(string format, string extension)
        {
            Format = format;
            Extension = extension;
           
        }
    }
}