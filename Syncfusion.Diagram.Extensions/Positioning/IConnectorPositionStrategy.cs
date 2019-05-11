using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Syncfusion.Diagram.Extensions.Positioning
{
    public interface IСonnectorPositionStrategy
    {
        Point headNodePosition { get; set; }
        double headNodeWidth { get; set; }
        Point tailNodePosition { get; set; }
        double tailNodeWidth { get; set; }
        KeyValuePair<Point, Point> GetPosition();
    }

}
