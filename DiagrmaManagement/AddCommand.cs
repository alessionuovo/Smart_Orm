using System;
using DiagramContent;

using FileManagement;

namespace DiagramManagement
{
     /** \ingroup Bll
     <summary>
     Represents one add element change in the system.
        Part of \ref Command 
     </summary>
     <typeparam name="DataElementType">The type of the element that was changed</typeparam>
    */
    internal class AddCommand<DataElementType> : IChangeCommand<DataElementType>
    {
       

        public AddCommand(DataElementType dataElement)
        {
            DataElement= dataElement;
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
            DbDefinitionsManager.DbManager.Add(DataElement); 
        }
        public void Show()
        {
            
        }
    }
}