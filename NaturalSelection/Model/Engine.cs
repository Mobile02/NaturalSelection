using NaturalSelection.Model.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace NaturalSelection.Model
{
    public class Engine
    {
        private ManualResetEventSlim eventSlim;
        private Constants constants;
        private int timeLife;
        private int generation;
        private int maxTimeLife;
        private Thread MainThread;

        private void RaiseTimeLifeProperty(int value) => ChangeTimeLifeProperty?.Invoke(this, value);
        private void RaiseGenerationProperty(int value) => ChangeGenerationProperty?.Invoke(this, value);
        private void RaiseMaxTimeLifeProperty(int value) => ChangeMaxTimeLifeProperty?.Invoke(this, value);
        private void RaiseOnReset(bool value) => OnReset?.Invoke(this, value);

        public event EventHandler<int> ChangeTimeLifeProperty;
        public event EventHandler<int> ChangeGenerationProperty;
        public event EventHandler<int> ChangeMaxTimeLifeProperty;
        public event EventHandler<bool> OnReset;

        #region Свойства
        public BaseSquare[] WorldMap { get; set; }
        
        public int TimeLife
        {
            get { return timeLife; }
            set
            {
                timeLife = value;
                RaiseTimeLifeProperty(TimeLife);
            }
        }
        public int Generation
        {
            get { return generation; }
            set
            {
                generation = value;
                RaiseGenerationProperty(Generation);
            }
        }
        public int MaxTimeLife
        {
            get { return maxTimeLife; }
            set
            {
                maxTimeLife = value;
                RaiseMaxTimeLifeProperty(MaxTimeLife);
            }
        }
        public int Speed { get; set; }
        public int[] ArrayTimeLife;
        #endregion
        public Engine(bool isLoadingSave = false)
        {
            eventSlim = new ManualResetEventSlim(false);
            constants = new Constants();

            StartNewSelection(isLoadingSave);
        }

        private void StartNewSelection(bool isLoadingSave)
        {
            Counter.CountAcid = 0;
            Counter.CountFood = 0;
            Counter.CountLiveBio = 0;
            Counter.Index = 0;

            WorldMap = new BaseSquare[constants.WorldSizeX * constants.WorldSizeY];

            if (!isLoadingSave)
                new CreatorSquares().InitWorldMap(WorldMap);
            else
            {
                LoadWorldMap();
            }

            ArrayTimeLife = new int[constants.CountCicle];

            MainThread = new Thread(MainAsync)
            {
                IsBackground = true,
                Priority = ThreadPriority.AboveNormal
            };
            MainThread.Start();
        }

        private void MainAsync()
        {
            for (Generation = 0; Generation < constants.CountCicle; Generation++)
            {
                for (int i = 0; i < int.MaxValue; i++)
                {
                    eventSlim.Wait();
                    Thread.Sleep(Speed);

                    new BehaviorSquare(WorldMap);

                    TimeLife = i;
                    if (MaxTimeLife < TimeLife)
                        MaxTimeLife = TimeLife;

                    if (Counter.CountLiveBio == constants.CountBio / 8)
                    {
                        new CreatorSquares().RefreshHealthBio(WorldMap);
                        new CreatorSquares().AddChild(WorldMap);

                        ArrayTimeLife[Generation] = TimeLife;
                        i = int.MaxValue - 1;
                    }
                }
            }
        }

        public void Start()
        {
            eventSlim.Set();
        }

        public void Stop()
        {
            eventSlim.Reset();
        }

        public void Reset()
        {
            if (MainThread != null)
                MainThread.Abort();

            RaiseOnReset(true);
        }

        public void SaveWorldMap()
        {
            new FileOperations().SaveWorldMap(WorldMap);
        }

        public void LoadWorldMap()
        {
            if (MainThread != null)
                MainThread.Abort();

            WorldMap = new FileOperations().LoadWorldMap();

            CalculationIndex();
        }

        private void CalculationIndex()
        {
            for (int i = 0; i < constants.WorldSizeX * constants.WorldSizeY; i++)
            {
                if (WorldMap[i] is BioSquare && WorldMap[i].PointX != -1)
                    Counter.CountLiveBio++;
                if (WorldMap[i] is AcidSquare && WorldMap[i].PointX != -1)
                    Counter.CountAcid++;
                if (WorldMap[i] is FoodSquare && WorldMap[i].PointX != -1)
                    Counter.CountFood++;
                if (WorldMap[i] is null)
                    return;
                Counter.Index++;
            }
        }
    }
}
