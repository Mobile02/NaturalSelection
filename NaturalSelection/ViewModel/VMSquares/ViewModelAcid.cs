using NaturalSelection.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalSelection.ViewModel
{
    public class ViewModelAcid : ViewModelSquares
    {
        private readonly AcidSquare model;
        private int pointX;
        private int pointY;

        public override int PointX
        {
            get { return pointX; }
            set
            {
                pointX = value;
                RaisePropertyChanged("PointX");
            }
        }
        public override int PointY
        {
            get { return pointY; }
            set
            {
                pointY = value;
                RaisePropertyChanged("PointY");
            }
        }
        public ViewModelAcid(AcidSquare model) : base (model)
        {
            this.model = model;
            this.model.ChangePointX += (sender, square) => PointX = square;
            this.model.ChangePointY += (sender, square) => PointY = square;
        }
    }
}
