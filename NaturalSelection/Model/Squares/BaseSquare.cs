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
        private int pointX;
        private int pointY;

        private void RaisePointX(int value) => ChangePointX?.Invoke(this, value);
        private void RaisePointY(int value) => ChangePointY?.Invoke(this, value);

        public event EventHandler<int> ChangePointX;
        public event EventHandler<int> ChangePointY;

        public int PointX
        {
            get { return pointX; }
            set
            {
                pointX = value;
                RaisePointX(PointX);
            }
        }

        public int PointY
        {
            get { return pointY; }
            set
            {
                pointY = value;
                RaisePointY(PointY);
            }
        }

        public TypeSquare TypeSquare { get; protected set; }

        public BaseSquare (int x, int y)
        {
            PointX = x;
            PointY = y;
        }
    }
}
