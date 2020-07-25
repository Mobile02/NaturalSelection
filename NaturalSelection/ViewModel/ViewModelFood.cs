using NaturalSelection.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalSelection.ViewModel
{
    public class ViewModelFood : ViewModelSquares
    {
        private readonly FoodSquare model;
        public ViewModelFood(FoodSquare model)
        {
            this.model = model;
        }
    }
}
