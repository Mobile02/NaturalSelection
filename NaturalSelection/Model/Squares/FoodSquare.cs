using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalSelection.Model
{
     public class FoodSquare : BaseSquare
    {
        private int pointX;
        private int pointY;

        private void RaisePointX(int value) => ChangePointX?.Invoke(this, value);
        private void RaisePointY(int value) => ChangePointY?.Invoke(this, value);

        public event EventHandler<int> ChangePointX;
        public event EventHandler<int> ChangePointY;

        public override int PointX
        {
            get { return pointX; }
            set
            {
                pointX = value;
                RaisePointX(PointX);
            }
        }

        public override int PointY
        {
            get { return pointY; }
            set
            {
                pointY = value;
                RaisePointY(PointY);
            }
        }

        public int Energy { get; private set; }
        public FoodSquare(int x, int y) : base(x, y)
        {
            TypeSquare = TypeSquare.FOOD;
            Energy = new Constants().Energy;
            pointX = x;
            pointY = y;
        }
    }
}
