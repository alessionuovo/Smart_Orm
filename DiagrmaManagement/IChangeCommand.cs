using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagramManagement
{
    /** \ingroup Bll
     <summary>
     This enum represents the three change types
     available in system
     </summary>
    */
    public enum State { Add, Update, Delete}
    /** \ingroup Bll
     <summary>
     This interface represents a change in specific type of element in the system.
         Part of \ref Command Design Pattern
     </summary>
     <typeparam name="DataElementType">Type of element that changes</typeparam>
       
    */
    public interface IChangeCommand<DataElementType>:ICommand
    {
        /// <summary>
        /// Stores the element change of which is recorded by the system
        /// </summary>
        DataElementType DataElement { get; set; }
    }
}
