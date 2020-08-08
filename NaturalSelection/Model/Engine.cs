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
        public Engine()
        {
            eventSlim = new ManualResetEventSlim(false);
            constants = new Constants();

            StartNewSelection();
        }

        private void StartNewSelection()
        {
            Counter.CountAcid = 0;
            Counter.CountFood = 0;
            Counter.CountLiveBio = 0;
            Counter.Index = 0;

            WorldMap = new BaseSquare[constants.WorldSizeX * constants.WorldSizeY];

            new CreatorSquares().InitWorldMap(WorldMap);

            ArrayTimeLife = new int[constants.CountCicle];

            MainThread = new Thread(MainAsync);
            MainThread.IsBackground = true;
            MainThread.Priority = ThreadPriority.AboveNormal;
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
    }
}
