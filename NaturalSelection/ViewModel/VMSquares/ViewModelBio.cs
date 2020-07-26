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
        public ViewModelBio(BioSquare model) : base (model)
        {
            this.model = model;
            this.model.ChangeHealth += (sender, square) => RaisePropertyChanged(nameof(Health));
        }

        public string Health => model.Health == 0 ? "" : model.Health.ToString();
    }
}
