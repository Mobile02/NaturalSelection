using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalSelection.Model
{
    public class EmptySquare : BaseSquare
    {
        public EmptySquare(int x, int y) : base(x, y)
        {
            TypeSquare = TypeSquare.EMPTY;
        }
    }
}
