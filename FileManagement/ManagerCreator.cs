using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagement
{
    /** \ingroup Bll
     <summary>
     Represents a class that is part of abstract factory
     and creates classes that know how the manage
     the files that will act as database.
     </summary>
    */
    public class ManagerCreator
    {
        public IManager CreateManager(string file, string format)
        {
            if (format=="xml")
                return new XmlManager(file);
            return CreateOtherFormatsManager(file, format);
           
        }

        /// <summary>
        /// This method needs to be overriden in classes\
        /// that extend this class to create manager for other formats
        /// For now null is return
        /// </summary>
        /// <param name="file">Name of the file</param>
        /// <param name="format">The format of the file</param>
        /// <returns>Null for now</returns>
        protected IManager CreateOtherFormatsManager(string file, string format)
        {
            return null;
        }
    }
}
