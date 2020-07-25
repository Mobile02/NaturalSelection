using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalSelection.Model
{
    public abstract class BaseSquare
    {
        public TypeSquare TypeSquare { get; protected set; }
        public int PointX { get; set; }
        public int PointY { get; set; }

        public BaseSquare (int x, int y)
        {
            PointX = x;
            PointY = y;
        }
    }
}
