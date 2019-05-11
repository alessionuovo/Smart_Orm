using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagramContent
{
     /** \ingroup Bll
     <summary>
     This class represents a table in database
     Contains table name and list of properties
     </summary>
    */
    public class Table
    {
        private static int count=1;
        public Table()
        {
            Name = string.Format("Table{0}", count);
            count++;
            Properties = new ObservableCollection<Properties>();
        }
       
        public string Name { get; set; }
        private ObservableCollection<Properties> _properties;
        /// <summary>
        /// List of fields in table
        /// </summary>
        public ObservableCollection<Properties> Properties { get { if (_properties == null) _properties = new ObservableCollection<Properties>(); return _properties; } set { _properties = value; } }
        private ObservableCollection<ConnectionProperties> _connectionproperties;
        public ObservableCollection<ConnectionProperties> ConnectionProperties { get { if (_connectionproperties == null) _connectionproperties = new ObservableCollection<ConnectionProperties>(); return _connectionproperties; } set { _connectionproperties = value; } }
    }
}
