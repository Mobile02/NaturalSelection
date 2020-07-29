using NaturalSelection.Model;
using NaturalSelection.Model.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NaturalSelection.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelSquares[] worldMap;
        private Constants constants = new Constants();
        private Engine engine;
        private int countRows;
        private int countColumns;
        ConstructorSquareViewModel constructor = new ConstructorSquareViewModel();


        public ViewModelSquares[] WorldMap
        {
            get { return worldMap; }
            set
            {
                worldMap = value;
                RaisePropertyChanged("WorldMap");
            }
        }

        public int CountRows
        {
            get { return countRows; }
            set
            {
                countRows = value;
                RaisePropertyChanged("CountRows");
            }
        }
        public int CountColumns 
        {
            get { return countColumns; }
            set
            {
                countColumns = value;
                RaisePropertyChanged("CountColumns");
            }
        }


        public MainWindowViewModel()
        {
            CountRows = constants.WorldSizeY;
            CountColumns = constants.WorldSizeX;

            engine = new Engine();

            RefreshMap();
            
            BehaviorSquare.Update += BehaviorSquare_Update;
        }

        private void BehaviorSquare_Update(object sender, UpdateInfo e)
        {
            RefreshMap();
            //WorldMap[e.CurrentIndex] = constructor.ConstructorViewModel(engine.WorldMap[e.CurrentIndex]);
            //WorldMap[e.NewIndex] = constructor.ConstructorViewModel(engine.WorldMap[e.NewIndex]);
        }

        private void RefreshMap()
        {
            WorldMap = new ViewModelSquares[constants.WorldSizeX * constants.WorldSizeY];

            for (int i = 0; i < constants.WorldSizeX * constants.WorldSizeY; i++)
            {
                if (engine.WorldMap[i] != null)
                    WorldMap[i] = constructor.ConstructorViewModel(engine.WorldMap[i]);
            }
        }
    }
}
