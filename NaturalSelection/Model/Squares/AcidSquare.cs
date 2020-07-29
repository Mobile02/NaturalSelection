﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalSelection.Model
{
    public class AcidSquare : BaseSquare
    {
        private int pointX;
        private int pointY;

        private void RaisePointX(int value) => ChangePointX?.Invoke(this, value);
        private void RaisePointY(int value) => ChangePointY?.Invoke(this, value);

        public event EventHandler<int> ChangePointX;
        public event EventHandler<int> ChangePointY;

        public override int PointX
        {
            get { return pointX; }
            set
            {
                pointX = value;
                RaisePointX(PointX);
            }
        }

        public override int PointY
        {
            get { return pointY; }
            set
            {
                pointY = value;
                RaisePointY(PointY);
            }
        }
        public AcidSquare(int x, int y) : base(x, y)
        {
            TypeSquare = TypeSquare.ACID;
            pointX = x;
            pointY = y;
        }
    }
}
