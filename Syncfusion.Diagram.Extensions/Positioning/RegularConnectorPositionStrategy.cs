using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Syncfusion.Diagram.Extensions.Positioning
{
    public class RegularConnectorPositionStrategy : IСonnectorPositionStrategy
    {
        protected Point _headnodeposition;
        public Point headNodePosition
        {
            get
            {
                if (_headnodeposition==null)
                {
                    _headnodeposition = new Point();
                }
                return _headnodeposition;
            }

            set
            {
                _headnodeposition = value;
            }
        }
        protected double _headnodewidth;
        public double headNodeWidth
        {
            get
            {
                
                return _headnodewidth;
            }

            set
            {
                _headnodewidth = value;
            }
        }
        protected Point _tailnodeposition;
        public Point tailNodePosition
        {
            get
            {
                if (_tailnodeposition == null)
                {
                    _tailnodeposition = new Point();
                }
                return _tailnodeposition;
            }

            set
            {
                _tailnodeposition = value;
            }
        }
        protected double _headnodeheight;
        public double headNodeHeigth
        {
            get
            {

                return _headnodeheight;
            }

            set
            {
                _headnodeheight = value;
            }
        }
        protected double _tailnodeheight;
        public double tailNodeHeight
        {
            get
            {
              
                return _tailnodeheight;
            }

            set
            {
                _tailnodeheight = value;
            }
        }
        protected double _tailnodewidth;
        public double tailNodeWidth
        {
            get
            {

                return _tailnodewidth;
            }

            set
            {
                _tailnodewidth = value;
            }
        }

        public KeyValuePair<Point, Point> GetPosition()
        {
            List<Point> lstHeadEdges = new List<Point>()
            {
                headNodePosition, new Point(headNodePosition.X+headNodeWidth, headNodePosition.Y),
                new Point(headNodePosition.X+headNodeWidth, headNodePosition.Y+headNodeHeigth), new Point(headNodePosition.X, headNodePosition.Y+headNodeHeigth)
                
            };
            List<Point> lstTailEdges = new List<Point>()
            {
                tailNodePosition,
                new Point(tailNodePosition.X+tailNodeWidth, tailNodePosition.Y),
                new Point(tailNodePosition.X+tailNodeWidth, tailNodePosition.Y+tailNodeHeight), new Point(tailNodePosition.X, tailNodePosition.Y+tailNodeHeight)
            };
            double mindistance = 800;
            Point p1 = new Point();
            Point p2 = new Point();
            foreach (Point headEdge in lstHeadEdges)
            {
                foreach(Point tailEdge in lstTailEdges)
                {
                    double distance = Math.Sqrt(Math.Pow(tailEdge.X - headEdge.X, 2) + Math.Pow(tailEdge.Y - headEdge.Y, 2));
                    if (distance < mindistance - 0.00001)
                    {
                        mindistance = distance;
                        p1 = new Point(headEdge.X+20, headEdge.Y+20);
                        p2= new Point(tailEdge.X+20, tailEdge.Y+20);
                    }
                }
            }
            return new KeyValuePair<Point, Point>(p1, p2);
               
        }
    }
}
