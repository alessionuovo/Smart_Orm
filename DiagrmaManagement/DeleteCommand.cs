using System;
using DiagramContent;
using FileManagement;

namespace DiagramManagement
{
    /** \ingroup Bll
     <summary>
     Represents one delete element change in the system
     </summary>
     <typeparam name="DataElementType">The type of the element that was changed</typeparam>
     Part of \ref Command 
        */
    internal class DeleteCommand<DataElementType> : IChangeCommand<DataElementType>
    {

        
        public DeleteCommand(DataElementType dataElement)
        {
            DataElement = dataElement;
        }

        /// <summary>
        /// Changed object to store
        /// </summary>
        public DataElementType DataElement
        {
            get
           ;

            set
            ;
        }

       
        /// <summary>
        /// This method  executes the save of the change result permanently
        /// in file
        /// </summary>
        public void Execute()
        {
            DbDefinitionsManager.DbManager.Delete(DataElement);
        }
        public void Show()
        {
           
        }
    }
}