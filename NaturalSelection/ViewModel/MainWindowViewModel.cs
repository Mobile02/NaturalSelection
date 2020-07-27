using NaturalSelection.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NaturalSelection.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelSquares[][] worldMap;
        private Constants constants = new Constants();
        private Engine engine;

        public ViewModelSquares[][] WorldMap
        {
            get { return worldMap; }
            set
            {
                worldMap = value;
                RaisePropertyChanged("WorldMap");
            }
        }


        public MainWindowViewModel()
        {
            engine = new Engine();

            RefreshMap();

            BaseSquare.ChangeCoordinate += On_ChangeCoordinate;
        }

        private void RefreshMap()
        {
            ConstructorSquareViewModel constructor = new ConstructorSquareViewModel();
            WorldMap = new ViewModelSquares[constants.WorldSizeY][];

            for (int y = 0; y < constants.WorldSizeY; y++)
            {
                WorldMap[y] = new ViewModelSquares[constants.WorldSizeX];
                for (int x = 0; x < constants.WorldSizeX; x++)
                {
                    WorldMap[y][x] = constructor.ConstructorViewModel(engine.WorldMap[y][x]);
                }
            }
        }

        private void On_ChangeCoordinate(object sender, System.Windows.Point e)
        {
            RefreshMap();
        }
    }
}
