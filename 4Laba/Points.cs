using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace _4Laba
{
    class PointsS
    {
        public string name;
        public PointF[] points;

        public PointsS(string name, PointF[] points)
        {
            this.name = name;
            this.points = points;
        }
    }
}
