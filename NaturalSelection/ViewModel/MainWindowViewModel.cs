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

            new Thread(start) { IsBackground = true, Priority = ThreadPriority.AboveNormal }.Start();
        }

        private void RefreshMap()
        {
            WorldMap = new ViewModelSquares[constants.WorldSizeY][];

            for (int y = 0; y < constants.WorldSizeY; y++)                                   //TODO: По моему какая то лабуда
            {
                WorldMap[y] = new ViewModelSquares[constants.WorldSizeX];
                for (int x = 0; x < constants.WorldSizeX; x++)
                {
                    if (engine.WorldMap[y][x].TypeSquare == TypeSquare.ACID)
                    {
                        WorldMap[y][x] = new ViewModelAcid((AcidSquare)engine.WorldMap[y][x]);
                    }
                    if (engine.WorldMap[y][x].TypeSquare == TypeSquare.BIO)
                    {
                        WorldMap[y][x] = new ViewModelBio((BioSquare)engine.WorldMap[y][x]);
                    }
                    if (engine.WorldMap[y][x].TypeSquare == TypeSquare.EMPTY)
                    {
                        WorldMap[y][x] = new ViewModelEmpty((EmptySquare)engine.WorldMap[y][x]);
                    }
                    if (engine.WorldMap[y][x].TypeSquare == TypeSquare.FOOD)
                    {
                        WorldMap[y][x] = new ViewModelFood((FoodSquare)engine.WorldMap[y][x]);
                    }
                    if (engine.WorldMap[y][x].TypeSquare == TypeSquare.WALL)
                    {
                        WorldMap[y][x] = new ViewModelWall((WallSquare)engine.WorldMap[y][x]);
                    }
                }
            }
        }

        private void start()
        {
            engine.test();
        }
    }
}
