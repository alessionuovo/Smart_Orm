using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagramContent
{
     /** \ingroup Bll
     <summary>
     This class represents an programmatic
     implementation of foreign key in the database
     </summary>
    */
    public class ConnectionProperties
    {
        public string Name
        {
            get; set;
        }
        public string Type { get; set; }
    }
}
