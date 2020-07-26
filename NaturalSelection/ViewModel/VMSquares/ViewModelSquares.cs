using NaturalSelection.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NaturalSelection.ViewModel
{
    public class ViewModelSquares : ViewModelBase
    {
        private readonly BaseSquare model;

        public readonly Point Coordinate;

        public ViewModelSquares(BaseSquare model)
        {
            Coordinate = model.Coordinate;
            this.model = model;
        }
    }
}
