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
        public ViewModelAcid(AcidSquare model)
        {
            this.model = model;
        }
    }
}
