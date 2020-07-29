using NaturalSelection.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalSelection.ViewModel
{
    public class ViewModelBio : ViewModelSquares
    {
        private readonly BioSquare model;
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
        public ViewModelBio(BioSquare model) : base (model)
        {
            this.model = model;
            this.model.ChangeHealth += (sender, square) => RaisePropertyChanged(nameof(Health));
            this.model.ChangePointX += (sender, square) => PointX = square;
            this.model.ChangePointY += (sender, square) => PointY = square;
        }

        public string Health => model.Health == 0 ? "" : model.Health.ToString();
    }
}
