using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagramContent
{
     /** \ingroup Bll
     <summary>
     This class represents a structure of erd model
     and needed to define some configuration on structure of Erd Model
     for now it is only container of Erd Model
     </summary>
    */

    public class Structure
    {
        private static Structure _struct;
        private Model _model;
        public Model model
        {
            get { if (_model == null) _model = Model.MyModel; return _model; }
            set { model = value; }
        }
        private Structure()
        {
            // model = Model.MyModel;
        }
        public static Structure Struct { get { if (_struct == null) _struct = new Structure(); return _struct; } }

    }
}
