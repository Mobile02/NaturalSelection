using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalSelection.Model
{
    public class AcidSquare : BaseSquare
    {
        public AcidSquare(int x, int y) : base(x, y)
        {
            TypeSquare = TypeSquare.ACID;
            PointX = x;
            PointY = y;
        }
    }
}
