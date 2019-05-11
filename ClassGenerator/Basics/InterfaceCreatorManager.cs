using System;

namespace ClassGenerator
{
     /** \ingroup Bll
     <summary>
     Implementation of TypeCreatedManager on interfaces.
        Also part of \ref Builder 
     </summary>
    */
    internal class InterfaceCreatorManager:TypeCreatorManager<CreatedInterface>
    {
      

        public InterfaceCreatorManager(InterfaceBuilder cbIChangeVisitorBuilder)
        {
            this.typeBuilder = cbIChangeVisitorBuilder;
        }

       
        /// <summary>
        /// Creation of c# interfaces
        /// </summary>
        /// <returns>The Type that was created(generated)</returns>
        public override CreatedType Create()
        {
            typeBuilder.BuildPropertiesAndFields();

            typeBuilder.BuildMethods();
            return typeBuilder.GetCreatedType();
        }


    }
}