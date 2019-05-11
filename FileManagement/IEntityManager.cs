using DiagramContent;
using System.Collections.Generic;
using Utilities;

namespace FileManagement
{
    /** \ingroup Bll
     <summary>
     Manages crud operations on object
     in database(database in stored as file),
     validation of file format and structure,
     creation of class that is displayed (via bind)
     on diagram.
         Part of \ref Strategy 
     </summary>
    */
    public interface IEntityManager
    {
        /// <summary>
        /// Manages the store of added element
        /// to file/database
        /// </summary>
        /// <typeparam name="T">Type of database object that was added</typeparam>
        /// <param name="element">The database object to store</param>
        /// <param name="format">Format that was chosen</param>
        /// <returns>Success/failure</returns>
        bool Add<T>(T element, string format);
        /// <summary>
        /// Manages the store of added element
        /// to file/database
        /// </summary>
        /// <typeparam name="T">Type of database object that was added</typeparam>
        /// <param name="element">The database object to store</param>
        /// <param name="format">Format that was chosen</param>
        /// <returns>Success/failure</returns>
        bool Update<T>(T element);
        /// <summary>
        /// Deletes the object permanently
        /// </summary>
        /// <typeparam name="T">Type of database object that was added</typeparam>
        /// <param name="element">The database object to store</param>
        /// <param name="format">Format that was chosen</param>
        /// <returns>Success/failure</returns>
        bool Delete<T>(T element);
        /// <summary>
        /// Manages validation of given filename in format
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        KeyValuePair<bool, Result> Validate(string filename, string format);
        void CreateDiagram();
        bool SetDiagram(string filename, string format);
    }
}