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
        private bool isSelected;
        private int pointer;
        private int[] brain;

        public event EventHandler<int> ChangePointer;
        public event EventHandler<bool> Dead;
        private void RaisePointer(int value) => ChangePointer?.Invoke(this, value);
        private void RaiseDead(bool value) => Dead?.Invoke(this, value);

        #region Свойства
        public override int PointX
        {
            get { return pointX; }
            set
            {
                pointX = value;
                RaisePropertyChanged("PointX");
                if (pointX == -1)
                    RaiseDead(true);
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
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }
        public int Pointer
        {
            get { return pointer; }
            set
            {
                pointer = value;
                if (Pointer < 64)
                    RaisePointer(Pointer);
            }
        }
        public int[] Brain
        {
            get { return brain; }
            set
            {
                brain = value;
                RaisePropertyChanged("Brain");
            }
        }
        #endregion
        public ViewModelBio(BioSquare model) : base(model)
        {
            this.model = model;
            Brain = model.Brain;
            this.model.ChangeHealth += (sender, square) => RaisePropertyChanged(nameof(Health));
            this.model.ChangePointX += (sender, X) => PointX = X;
            this.model.ChangePointY += (sender, Y) => PointY = Y;
            this.model.ChangePointer += (sender, squarePointer) => Pointer = squarePointer;
            IsSelected = false;
        }

        public string Health => model.Health == 0 ? "" : model.Health.ToString();
    }
}
