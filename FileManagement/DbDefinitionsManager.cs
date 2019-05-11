using DiagramContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Utilities.Definitions;

namespace FileManagement
{
   
    /** \ingroup Bll
     <summary>
     This class manages all
     the work with the files
     that contain definitions for
     different type of objects in relational database.
        Also part of \ref Singleton 
     </summary>
        
    */
    public class DbDefinitionsManager
    {
        private DbDefinitionsManager()
        {
            sltEntities = new SortedList<string, IEntityManager>();
        }
        private static DbDefinitionsManager _dbmanager;
        protected string format;
        protected string DefaultFormat = "xml";
        public static DbDefinitionsManager DbManager { get { if (_dbmanager == null) _dbmanager = new DbDefinitionsManager(); return _dbmanager; } }
        protected SortedList<string, IEntityManager> sltEntities;

        /// <summary>
        /// This method get filename and format
        /// and manages its format and structure validation
        /// </summary>
        /// <param name="filename">The name of the file</param>
        /// <param name="format">The of the format-For now format and extension are the same but it can be different</param>
        /// <returns>KeyValuePair element- The key is true for success, false for failure, Result-object that contains error message</returns>
        public KeyValuePair<bool, Result> Validate(string filename, string format)
        {

            this.format = format;
            IEntityManager entityManager = new EntityManager();
            KeyValuePair<bool, Result> validationResult= entityManager.Validate(filename, format);
            if (validationResult.Key)
                sltEntities.Add(filename, entityManager);
            return validationResult;
        }

        public void ClearEntities()
        {
            sltEntities.Clear();
        }

        /// <summary>
        /// Creates object of classes that are binded to the diagram
        /// </summary>
        /// <param name="filename">Name of the file from which to create</param>
        public void CreateDiagram(string filename)
        {
            IEntityManager entityManager;
            if (!sltEntities.TryGetValue(filename, out entityManager))
            {
                entityManager = new EntityManager();
                sltEntities.Add(filename, entityManager);
            }
             
            entityManager.CreateDiagram();
        }
        
        /// <summary>
        /// Creates objects of classes that can be bound to diagram control
        /// </summary>
        /// <param name="filename">Name of file that represents database object</param>
        /// <param name="format">Format of the files-here is also file extension</param>
        /// <returns></returns>
        public bool CreateDiagram(string filename, string format)
        {
            IEntityManager entityManager;
            if (!sltEntities.TryGetValue(filename, out entityManager))
            {
                entityManager = new EntityManager();
                bool result = entityManager.SetDiagram(filename, format);
                if (result)
                    sltEntities.Add(filename, entityManager);
                return result;
            }
            return false;

        }
        public void SetFormat(string format)
        {
            this.format = format;
        }
        public string GetFormat()
        {

            SetFormatToDefault();
            return format.ToUpper();
        }
        /// <summary>
        /// In cases where format not chosen is useful
        /// </summary>
        protected void SetFormatToDefault()
        {
            if (string.IsNullOrEmpty(format))
                format = DefaultFormat;
        }
        /// <summary>
        /// Manages the addition of database object
        /// to database(for now files as xml and json serve as database)
        /// </summary>
        /// <typeparam name="DataElementType"></typeparam>
        /// <param name="element">Database object that was added to diagram(business logic representation of it)</param>
        public void Add<DataElementType>(DataElementType element)
        {
            var nameProperty = element.GetType().GetProperty("Name");
            if (nameProperty == null)
                return;
            IEntityManager entityManager = new EntityManager(); 
                sltEntities.Add(nameProperty.GetValue(element).ToString(), entityManager);
            SetFormatToDefault();
            entityManager.Add(element, format);
        }

        /// <summary>
        /// Manages the update of database object
        /// to database(for now files as xml and json serve as database)
        /// </summary>
        /// <typeparam name="DataElementType"></typeparam>
        /// <param name="element">Database object that was added to diagram(business logic representation of it)</param>
        public void Update<DataElementType>(DataElementType element)
        {
            var nameprop = element.GetType().GetProperty("Name");
            IEntityManager entityManager = sltEntities[nameprop.GetValue(element).ToString()];
            SetFormatToDefault();
            entityManager.Update(element);
        }

        /// <summary>
        /// Manages the addition of database object
        /// to database(for now files as xml and json serve as database)
        /// </summary>
        /// <typeparam name="DataElementType">Type of element </typeparam>
        /// <param name="element">Database object that was added to diagram(business logic representation of it)</param>
        public void Delete<DataElementType>(DataElementType element)
        {
            var nameProperty = element.GetType().GetProperty("Name");
            if (!sltEntities.ContainsKey(nameProperty.GetValue(element).ToString()))
                return;
            IEntityManager em = sltEntities[nameProperty.GetValue(element).ToString()];
            SetFormatToDefault();
            em.Delete(element);
        }
       
    }
}
