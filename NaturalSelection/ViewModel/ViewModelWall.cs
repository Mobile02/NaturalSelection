using NaturalSelection.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalSelection.ViewModel
{
    public class ViewModelWall : ViewModelSquares
    {
        private readonly WallSquare model;
        public ViewModelWall(WallSquare model)
        {
            this.model = model;
        }
    }
}
