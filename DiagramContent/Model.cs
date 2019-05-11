using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DiagramContent
{

    /** \ingroup Bll 



 <summary>
    class Connection is bello
 This class represents ErdModel of database
 Contains list of tables and list of connections between them.
        This class is used in wpf binding.
 </summary>*/
    public class Model
    {
        private static Model _model;
        public static Model MyModel { get { if (_model == null) _model = new Model(); return _model; } }
        /// <summary>
        /// Constructor
        /// </summary>
        private Model()
        {
           
        }

        

        private ObservableCollection<Connection> _connections;
        /// <summary>
        /// List of connections
        /// </summary>
        public ObservableCollection<Connection> connections { get { if (_connections == null) _connections = new ObservableCollection<Connection>(); return _connections; } set { _connections = value; } }
      
        private ObservableCollection<Table> _tables;
        public string name { get { return "aaa"; } }
        /// <summary>
        /// List of tables
        /// </summary>
        public ObservableCollection<Table> tables { get { if (_tables == null) _tables = new ObservableCollection<Table>(); return _tables; } set { _tables = value; } }
    }
   
}

