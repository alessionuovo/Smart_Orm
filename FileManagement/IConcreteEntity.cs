using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagement
{
    /** \ingroup Bll
     <summary>
     This interface represents 
     one concrete manageable object(so we do not know its type
     and it is used for manage not for display)
     in a db.
         Part of \ref Strategy 
     </summary>
    */
    interface IConcreteEntity<T>:IEntity
    {
        /// <summary>
        /// The object
        /// </summary>
        T Element { get; set; }

        /// <summary>
        /// Adds element to db(stores it permanently)
        /// </summary>
        /// <param name="element"></param>
        /// <param name="manager">Because for now it is file that act as db so it specifies how o save this object to a file</param>
        void Add(T element, IManager manager);

        /// <summary>
        /// Updates element in db
        /// </summary>
        /// <param name="element"></param>
        void Update(T element);

        /// <summary>
        /// Deletes element from db
        /// </summary>
        /// <param name="element"></param>
        void Delete(T element);

        /// <summary>
        /// Creates an object that is displayed on the diagram
        /// </summary>
        void CreateDiagram();
    }
}
