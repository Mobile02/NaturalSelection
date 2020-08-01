using NaturalSelection.Model;
using NaturalSelection.Model.Support;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NaturalSelection.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelSquares[] worldMap;
        private readonly Constants constants = new Constants();
        private readonly Engine engine;
        private int countRows;
        private int countColumns;
        ConstructorSquareViewModel constructor = new ConstructorSquareViewModel();
        private int widthGraf;
        private ViewModelBio selectedSquare;
        private ObservableCollection<int[]> chartTimeLife;
        private int timeLife;
        private int maxTimeLife;
        private int speed;
        private int generation;

        private ICommand cStart;
        private ICommand cStop;
        private ICommand selectItemCommand;

        #region Commands
        public ICommand ComStart
        {
            get
            {
                if (cStart == null)
                {
                    return cStart = new RelayCommand(obj => Start()) ;
                }
                return cStart;
            }
        }

        public ICommand ComStop
        {
            get
            {
                if (cStop == null)
                {
                    return cStop = new RelayCommand(obj => Stop());
                }
                return cStop;
            }
        }

        #endregion

        #region Свойства
        public int WidthChart
        {
            get { return widthGraf; }
            set
            {
                widthGraf = value;
                RaisePropertyChanged("WidthGraf");
            }
        }

        public ViewModelBio SelectedSquare
        {
            get { return selectedSquare; }
            set
            {
                selectedSquare = value;
                RaisePropertyChanged("SelectedSquare");
            }
        }

        public ObservableCollection<int[]> ChartTimeLife
        {
            get { return chartTimeLife; }
            set
            {
                chartTimeLife = value;
                RaisePropertyChanged("ChartTimeLife");
            }
        }

        public int TimeLife
        {
            get { return timeLife; }
            set
            {
                timeLife = value;
                RaisePropertyChanged("TimeLife");
            }
        }

        public int MaxTimeLife
        {
            get { return maxTimeLife; }
            set
            {
                maxTimeLife = value;
                RaisePropertyChanged("MaxTimeLife");
            }
        }

        public int Speed
        {
            get { return speed; }
            set
            {
                speed = value;
                engine.Speed = value;
                RaisePropertyChanged("Speed");
            }
        }

        public int Generation
        {
            get { return generation; }
            set
            {
                generation = value;
                RaisePropertyChanged("Generation");
            }
        }
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
        #endregion

        public MainWindowViewModel()
        {
            CountRows = constants.WorldSizeY;
            CountColumns = constants.WorldSizeX;

            engine = new Engine();
            Speed = 20;

            RefreshMap();

            engine.ChangeTimeLifeProperty += (sender, e) => TimeLife = e;
            engine.ChangeGenerationProperty += (sender, e) => Generation = e;
            engine.ChangeMaxTimeLifeProperty += (sender, e) => MaxTimeLife = e;
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

        private void Start()
        {
            engine.Start();
        }

        private void Stop()
        {
            engine.Stop();
        }
    }
}
