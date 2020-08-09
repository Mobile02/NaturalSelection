using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NaturalSelection.Model
{
    [Serializable]
    public abstract class BaseSquare
    {
        public abstract int PointX { get; set; }
        public abstract int PointY { get; set; }

        public int Index { get; set; }

        public TypeSquare TypeSquare { get; protected set; }

        public BaseSquare (int x, int y, int index)
        {
            PointX = x;
            PointY = y;
            Index = index;
        }
    }
}
