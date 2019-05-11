namespace ClassGenerator
{
    /** \ingroup Bll
    <summary>
    Manages generation of c# type in memory.
        Also part of \ref Builder 
    </summary>
    <typeparam name="T"></typeparam>
       \copybrief TypeBuilder
   */
    public abstract class TypeCreatorManager<T>
        where T :CreatedType
    {
        protected TypeBuilder<T> typeBuilder;

        /// <summary>
        /// This method manages creation of c# type in memory
        /// </summary>
        /// <returns>The object that represents type that was created</returns>
        public abstract CreatedType Create();
        
    }
}