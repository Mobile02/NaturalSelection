using NaturalSelection.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalSelection.ViewModel
{
    public class ConstructorSquareViewModel
    {
        public ViewModelSquares ConstructorViewModel(BaseSquare model)
        {
            if (model is AcidSquare)
                return new ViewModelAcid(model as AcidSquare);
            if (model is BioSquare)
                return new ViewModelBio(model as BioSquare);
            if (model is EmptySquare)
                return new ViewModelEmpty(model as EmptySquare);
            if (model is FoodSquare)
                return new ViewModelFood(model as FoodSquare);
            if (model is WallSquare)
                return new ViewModelWall(model as WallSquare);

            return new ViewModelSquares(model);
        }
    }
}
