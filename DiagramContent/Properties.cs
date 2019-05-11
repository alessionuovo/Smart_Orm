using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagramContent
{
     /** \ingroup Bll
     <summary>
     This class represents a field in database table
     Contains: Property Name, Property Type, and indication if it is primary key or not
     </summary>
    */
    public class Properties
    {
        public string Name
        {
            get; set;
        }
        public string Type { get; set; }
        public bool IsPrimaryKey { get; set; }
    }
}
