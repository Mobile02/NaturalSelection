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
        private ObservableCollection<ViewModelSquares> worldMap;
        private readonly Constants constants = new Constants();
        private Engine engine;
        private int countRows;
        private int countColumns;
        private ConstructorSquareViewModel constructor;
        private int widthGraf;
        private ObservableCollection<int[]> chartTimeLife;
        private int timeLife;
        private int maxTimeLife;
        private int speed;
        private int generation;
        private ViewModelBio selectedBio;
        private int[] pointsY;
        private ChartLife chartLife;
        private BrainViewModel brainViewModel;
        private bool isRuning;
        private string buttonContent;

        private ICommand cStartPause;
        private ICommand cReset;
        private ICommand selectItemCommand;
        private ICommand cSave;
        private ICommand cLoad;


        #region Commands
        public ICommand ComStartPause
        {
            get
            {
                if (cStartPause == null)
                {
                    return cStartPause = new RelayCommand(obj => OnStartStop(), obj => CanStartStop());
                }
                return cStartPause;
            }
        }

        public ICommand ComReset
        {
            get
            {
                if (cReset == null)
                {
                    return cReset = new RelayCommand(obj => Reset());
                }
                return cReset;
            }
        }

        public ICommand SelectItemCommand
        {
            get
            {
                if (selectItemCommand == null)
                    return selectItemCommand = new RelayCommand(obj => SelectedItemCommand(obj));

                return selectItemCommand;
            }
        }

        public ICommand ComSave
        {
            get
            {
                if (cSave == null)
                    return cSave = new RelayCommand(obj => CommandSave());

                return cSave;
            }
        }

        public ICommand ComLoad
        {
            get
            {
                if (cLoad == null)
                    return cLoad = new RelayCommand(obj => CommandLoad());

                return cLoad;
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

        public ViewModelBio SelectedBio
        {
            get { return selectedBio; }
            set
            {
                selectedBio = value;
                RaisePropertyChanged("SelectedBio");
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
        public ObservableCollection<ViewModelSquares> WorldMap
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
        public BrainViewModel BrainViewModel
        {
            get { return brainViewModel; }
            set
            {
                brainViewModel = value;
                RaisePropertyChanged("BrainViewModel");
            }
        }

        public string ButtonContent
        {
            get { return buttonContent; }
            set
            {
                buttonContent = value;
                RaisePropertyChanged("ButtonContent");
            }
        }

        public bool IsRuning
        {
            get { return isRuning; }
            set
            {
                isRuning = value;
                RaisePropertyChanged("IsRuning");
            }
        }

        #endregion


        public MainWindowViewModel()
        {
            brainViewModel = new BrainViewModel();
            engine = new Engine();

            InitialWorldMap();
        }

        private void InitialWorldMap()
        {
            constructor = new ConstructorSquareViewModel();
            ChartTimeLife = new ObservableCollection<int[]>();
            chartLife = new ChartLife();

            CountRows = constants.WorldSizeY;
            CountColumns = constants.WorldSizeX;
            pointsY = new int[constants.CountCicle];
            pointsY = engine.ArrayTimeLife;
            WidthChart = (int)(constants.WorldSizeX * 15 + (constants.WorldSizeX * 1.5));
            Speed = 20;

            engine.ChangeTimeLifeProperty += (sender, e) => TimeLife = e;
            engine.ChangeGenerationProperty += (sender, e) => { Generation = e; UpdateChartLife(); };
            engine.ChangeMaxTimeLifeProperty += (sender, e) => MaxTimeLife = e;
            engine.ChangeWorldMap += (sender, e) => InitialWorldMap();

            WorldMap = new ObservableCollection<ViewModelSquares>();

            for (int i = 0; i < constants.WorldSizeX * constants.WorldSizeY; i++)
            {
                if (engine.WorldMap[i] != null)
                    WorldMap.Add(constructor.ConstructorViewModel(engine.WorldMap[i]));
            }
        }

        private void UpdateChartLife()
        {
            ChartTimeLife.Clear();
            ChartTimeLife = chartLife.UpdateChart(ChartTimeLife, pointsY, Generation);
        }

        private void Reset()
        {
            ResetVariables();
            engine.Reset();
        }

        private void CommandSave()
        {
            engine.SaveWorldMap();
        }
        private void CommandLoad()
        {
            ResetVariables();
            engine.LoadWorldMap();
        }

        private void ResetVariables()
        {
            IsRuning = false;
            SelectedBio = null;
            brainViewModel.SetSelectedBio(null);
        }

        private void SelectedItemCommand(object obj)
        {
            if (SelectedBio != null)
            {
                SelectedBio.IsSelected = false;
            }

            SelectedBio = obj as ViewModelBio;
            SelectedBio.IsSelected = true;

            brainViewModel.SetSelectedBio(SelectedBio);
        }

        private bool CanStartStop()
        {
            if (IsRuning)
                ButtonContent = "Пауза";
            else
                ButtonContent = "Старт";

            return true;
        }
        private void OnStartStop()
        {
            if (IsRuning)
            {
                IsRuning = false;
                engine.Stop();
                return;
            }

            engine.Start();
            IsRuning = true;
        }
    }
}
