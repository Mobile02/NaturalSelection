using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalSelection.Model
{
     public class FoodSquare : BaseSquare
    {
        public int Energy { get; private set; }
        public FoodSquare(int x, int y) : base(x, y)
        {
            TypeSquare = TypeSquare.FOOD;
            Energy = new Constants().Energy;
        }
    }
}
