﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalSelection.Model
{
    [Serializable]
    public class BioSquare : BaseSquare, ICloneable
    {
        private int health;
        private int pointX;
        private int pointY;
        private int pointer;

        private void RaiseHealth(int value) => ChangeHealth?.Invoke(this, value);
        private void RaisePointX(int value) => ChangePointX?.Invoke(this, value);
        private void RaisePointY(int value) => ChangePointY?.Invoke(this, value);
        private void RaisePointer(int value) => ChangePointer?.Invoke(this, value);

        [field: NonSerialized]
        public event EventHandler<int> ChangeHealth;
        [field: NonSerialized]
        public event EventHandler<int> ChangePointX;
        [field: NonSerialized]
        public event EventHandler<int> ChangePointY;
        [field: NonSerialized]
        public event EventHandler<int> ChangePointer;

        public int Health
        {
            get { return health; }
            set
            {
                health = value;
                RaiseHealth(Health);
            }
        }
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

        public int Pointer
        {
            get { return pointer; }
            set
            {
                pointer = value;
                RaisePointer(Pointer);
            }
        }

        public Direction Direction { get; set; }
        public int[] Brain { get; set; }
        

        public BioSquare(int x, int y, int index) : base(x, y, index)
        {
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

    }
}
