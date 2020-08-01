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
        private Constants constants;
        private int nextGen;
        private void RaiseNextGen(int value) => ChangeNextGen?.Invoke(this, value);


        public event EventHandler<int> ChangeNextGen;
        public BaseSquare[] WorldMap { get; set; }
        public int NextGen { get; set; }

        public Engine()
        {

            constants = new Constants();
            WorldMap = new BaseSquare[constants.WorldSizeX * constants.WorldSizeY];

            new CreatorSquares().InitWorldMap(WorldMap);

            new Thread(MainAsync) { IsBackground = true, Priority = ThreadPriority.AboveNormal }.Start();
        }

        private void MainAsync()
        {
            for (int gen = 0; gen < constants.CountCicle; gen++)
            {
                for (int i = 0; i < int.MaxValue; i++)
                {
                    //Thread.Sleep(5);

                    new BehaviorSquare(WorldMap);

                    if (Counter.CountLiveBio == constants.CountBio / 8)
                    {
                        //new FileOperations().SaveBrain(WorldMap);

                        new CreatorSquares().RefreshHealthBio(WorldMap);
                        new CreatorSquares().AddChild(WorldMap);

                        //RaiseNextGen(NextGen);
                        i = int.MaxValue - 1;
                    }
                }
            }
        }
    }
}
