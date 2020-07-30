using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalSelection.Model
{
    public class BioSquare : BaseSquare, ICloneable
    {
        private int health;
        private int pointX;
        private int pointY;

        private void RaiseHealth(int value) => ChangeHealth?.Invoke(this, value);
        private void RaisePointX(int value) => ChangePointX?.Invoke(this, value);
        private void RaisePointY(int value) => ChangePointY?.Invoke(this, value);

        public event EventHandler<int> ChangeHealth;
        public event EventHandler<int> ChangePointX;
        public event EventHandler<int> ChangePointY;

        public int Health
        {
            get { return health; }
            set
            {
                health = value;
                RaiseHealth(Health);
            }
        }
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


        public Direction Direction { get; set; }
        public int[] Brain { get; set; }
        public int Pointer { get; set; }

        public BioSquare(int x, int y, int index) : base(x, y, index)
        {
            TypeSquare = TypeSquare.BIO;
            pointX = x;
            pointY = y;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

    }
}
