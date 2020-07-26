﻿using NaturalSelection.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalSelection.ViewModel
{
    public class ViewModelSquares : ViewModelBase
    {
        private readonly BaseSquare model;
        public ViewModelSquares(BaseSquare model)
        {
            this.model = model;
        }
    }
}
