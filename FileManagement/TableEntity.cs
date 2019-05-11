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
     Represents database table as manageable object
     Implements crud operation and Validation for table.
         Part of \ref Strategy 
     </summary>
    */
    public class TableEntity : IConcreteEntity<Table>
    {
        private IManager manager;
        

        public TableEntity(IManager manager)
        {
            this.manager = manager;
        }

        public TableEntity(Table table)
        {
            this.Element = table;
        }

        public Table Element
        {
            get
            ;

            set
           ;
        }

        
        

       

        public void Add(Table element, IManager manager)
        {
            this.manager = manager;
            Update(element);
        }

        public void CreateDiagram()
        {
            Table t = new Table();
            t.Name = manager.GetName();
            IEntityDefinition definition=SystemDefinitionsManager.DefinitionsManager.GetValidationDefinition(EntityTypesDefinition.Table);
            if (!(definition is TableDefinition))
                return;
            TableDefinition tabledefinition = definition as TableDefinition;
            List<EntityElement> e = manager.GetEntityElements(tabledefinition.TablePropertyTag);
            t.Properties = new System.Collections.ObjectModel.ObservableCollection<Properties>();
            foreach (EntityElement ee in e)
            {
                var type=ee.attributes.Where(x => x.Key == tabledefinition.TablePropertyType).First();
                var name= ee.attributes.Where(x => x.Key == tabledefinition.TablePropertyName).First();
                var isprimary= ee.attributes.Where(x => x.Key == tabledefinition.TablePropertyPrimary);
                bool iskey = (isprimary.Count()>0 && isprimary.First().Value.ToUpper() == "TRUE");
                if (string.IsNullOrEmpty(name.Value) || string.IsNullOrEmpty(type.Value))
                    continue;
                t.Properties.Add(new Properties() { Name = name.Value, Type = type.Value, IsPrimaryKey=iskey });
            }
            var m = Structure.Struct.model;
            m.tables.Add(t);
        }

       
        /// <summary>
        /// Manages delete of table
        /// </summary>
        /// <param name="element">the Table</param>
        public void Delete(Table element)
        {
            manager.Delete();
        }

       
        /// <summary>
        /// Manages update of table
        /// </summary>
        /// <param name="element">the table</param>
        public void Update(Table element)
        {
            EntityElement e=SystemDefinitionsManager.DefinitionsManager.GetDefinition(element);
            manager.Update(e);
        }

        /// <summary>
        /// Can provide additional validation to table object
        /// </summary>
        /// <returns>Result of validation-success/failure(key) and messages(the value)</returns>
        public KeyValuePair<bool, Result> Validate()
        {
            return new KeyValuePair<bool, Result>();
        }
    }
}
