using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalSelection.Model
{
    public class WallSquare : BaseSquare
    {
        public WallSquare(int x, int y) : base(x, y)
        {
            TypeSquare = TypeSquare.WALL;
        }
    }
}
