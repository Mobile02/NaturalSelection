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

        public int PointX { get; set; }
        public int PointY { get; set; }

        public ViewModelSquares(BaseSquare model)
        {
            this.model = model;
            this.model.ChangePointX += (sender, square) => RaisePropertyChanged("PointX");
            this.model.ChangePointY += (sender, square) => RaisePropertyChanged("PointY");
        }
    }
}
