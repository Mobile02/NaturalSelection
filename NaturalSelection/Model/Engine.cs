﻿using NaturalSelection.Model.Support;
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

        public BaseSquare[][] WorldMap { get; set; }

        public Engine()
        {
            constants = new Constants();
            WorldMap = new BaseSquare[constants.WorldSizeY][];

            new CreatorSquares().FillWorldMap(WorldMap);
            new CreatorSquares().AddBioSquare(WorldMap, constants.CountBio);
            new CreatorSquares().AddWall(WorldMap, 20);
            new CreatorSquares().AddFood(WorldMap, constants.CountFood);
            new CreatorSquares().AddAcid(WorldMap, constants.CountAcid);

            new Thread(MainAsync) { IsBackground = true, Priority = ThreadPriority.AboveNormal }.Start();
        }

        private void MainAsync()
        {
            for (int gen = 0; gen < constants.CountCicle; gen++)
            {
                for (int i = 0; i < int.MaxValue; i++)
                {
                    Thread.Sleep(100);

                    new BehaviorSquare(WorldMap);

                    if (Counter.CountLiveBio <= (constants.CountBio / 8))
                    {
                        //new CreatorSquares().RefreshSquare(WorldMap);
                        //new CreatorSquares().AddAcid(WorldMap, constants.CountAcid);
                        //new CreatorSquares().AddFood(WorldMap, constants.CountFood);
                        new CreatorSquares().AddBioSquare(WorldMap, constants.CountBio - Counter.CountLiveBio);
                        //Counter.CountLiveBio = constants.CountBio;

                        i = int.MaxValue - 1;
                    }
                }
            }
        }
    }
}
