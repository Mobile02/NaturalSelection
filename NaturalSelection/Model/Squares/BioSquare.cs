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

        private void RaiseHealth(int value) => ChangeHealth?.Invoke(this, value);

        public event EventHandler<int> ChangeHealth;

        public int Health
        {
            get { return health; }
            set
            {
                health = value;
                RaiseHealth(Health);
            }
        }

        public Direction Direction { get; set; }
        public int[] Brain { get; set; }
        public int Pointer { get; set; }

        public BioSquare(int x, int y) : base(x, y)
        {
            TypeSquare = TypeSquare.BIO;
            PointX = x;
            PointY = y;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
