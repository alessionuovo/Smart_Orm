using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagramContent
{





    /** \ingroup Bll 


       <summary> 
       class Connection represents a connection in database
    It has this properties all are strings</summary>

 
     
   
   */

    public class Connection
    {
        private static int NumElements=0;
        public Connection()
        {
         
            
            NumElements++;
            this.Name = string.Format("Connector{0}", NumElements);
           
        }
        private string _firsttable;
        private string _secondtable;

        /// <summary>
        /// Table Name
        /// </summary>
        public string FirstTable { get { return _firsttable; } set { _firsttable = value; } }
        /// <summary>
        /// Table Name
        /// </summary>
        public string SecondTable { get { return _secondtable; } set { _secondtable = value; } }
        private string _firstrelation;
        private string _secondrelation;
        /// <summary>
        /// Relation Cardinality
        /// </summary>
        public string FirstRelation { get { return _firstrelation; } set { _firstrelation = value; } }
        /// <summary>
        /// Relation Cardinality
        /// </summary>
        public string SecondRelation { get { return _secondrelation; } set { _secondrelation = value; } }
       
        public string Name { get; set; }

    }
   
}
