using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NaturalSelection.Model
{
    public abstract class BaseSquare
    {
        private Point coordinate;

        private void RaiseCoordinate(Point value) => ChangeCoordinate?.Invoke(this, value);

        public static event EventHandler<Point> ChangeCoordinate;

        public Point Coordinate
        {
            get { return coordinate; }
            set
            {
                coordinate = value;
                RaiseCoordinate(Coordinate);
            }
        }

        public TypeSquare TypeSquare { get; protected set; }

        public BaseSquare (int x, int y)
        {
            Coordinate = new Point(x, y);
        }
    }
}
