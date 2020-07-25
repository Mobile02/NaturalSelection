using NaturalSelection.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalSelection.ViewModel
{
    public class ViewModelEmpty : ViewModelSquares
    {
        private readonly EmptySquare model;
        public ViewModelEmpty(EmptySquare model)
        {
            this.model = model;
        }
    }
}
