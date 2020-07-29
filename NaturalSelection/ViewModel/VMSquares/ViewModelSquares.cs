using NaturalSelection.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NaturalSelection.ViewModel
{
    public abstract class ViewModelSquares : ViewModelBase
    {
        private readonly BaseSquare model;

        public abstract int PointX { get; set; }
        public abstract int PointY { get; set; }

        public ViewModelSquares(BaseSquare model)
        {
            if (model != null)
            {
                this.model = model;
                PointX = model.PointX;
                PointY = model.PointY;
            }
        }
    }
}
