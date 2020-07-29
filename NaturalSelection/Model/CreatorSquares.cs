﻿using NaturalSelection.Model.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalSelection.Model
{
    public class CreatorSquares
    {
        private Random random;
        private Constants constants = new Constants();


        public CreatorSquares()
        {
            DateTimeOffset timeOffset = new DateTimeOffset(DateTime.Now);
            random = new Random((int)timeOffset.Ticks);
        }

        private int FindIndex(BaseSquare[] worldMap)
        {
            int index = 0;
            for (int i = 0; i < constants.WorldSizeX * constants.WorldSizeY; i++)
            {
                if (worldMap[i] is null)
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        private int[] BrainCenerator()
        {
            int[] brain = new int[constants.SizeBrain];

            for (int i = 0; i < constants.SizeBrain; i++)
            {
                brain[i] = random.Next(64);
            }

            return (int[])brain.Clone();
        }

        public void FillField(BaseSquare[] worldMap)
        {
            int y = 0, x = 0, index = 0;

            for (int i = 0; i < constants.WorldSizeX; i++)
            {
                worldMap[index++] = new WallSquare(x, 0);
                worldMap[index++] = new WallSquare(x++, constants.WorldSizeY - 1);
            }

            int maxIndex = constants.WorldSizeY + index;

            for (int i = index; i < maxIndex; i++)
            {
                worldMap[index++] = new WallSquare(0, y);
                worldMap[index++] = new WallSquare(constants.WorldSizeX - 1, y++);
            }
        }

        public void AddWall(BaseSquare[] worldMap, int count)
        {
            int index = FindIndex(worldMap);

            while (count > 0)
            {
                int y = random.Next(1, constants.WorldSizeY - 1);
                int x = random.Next(1, constants.WorldSizeX - 1);

                if (Array.IndexOf(worldMap, new WallSquare(x, y)) == -1)
                {
                    worldMap[index] = new WallSquare(x, y);
                    if (worldMap[++index] != null)
                        index = FindIndex(worldMap);
                    count--;
                }
            }
        }

        public void AddBioSquare(BaseSquare[] worldMap, int count)
        {
            int index = FindIndex(worldMap);

            while (count > 0)
            {
                bool check = false;
                int y = random.Next(1, constants.WorldSizeY - 1);
                int x = random.Next(1, constants.WorldSizeX - 1);

                for (int i = 0; i < constants.WorldSizeX * constants.WorldSizeY; i++)
                {
                    if (!(worldMap[i] is null) && worldMap[i].PointX == x && worldMap[i].PointY == y)
                        check = true;
                }

                if (!check)
                {
                    worldMap[index] = new BioSquare(x, y)
                    {
                        Brain = BrainCenerator(),
                        Pointer = 0,
                        Health = constants.HealthSquare,
                        Direction = (Direction)random.Next(8)
                    };

                    Counter.CountLiveBio++;
                    count--;

                    if (worldMap[++index] != null)
                        index = FindIndex(worldMap);
                }
            }
        }

        public void AddAcid(BaseSquare[] worldMap, int count)
        {
            if (Counter.CountAcid > new Constants().CountAcid)
                return;

            int index = FindIndex(worldMap);

            while (count > 0)
            {
                int y = random.Next(1, constants.WorldSizeY - 1);
                int x = random.Next(1, constants.WorldSizeX - 1);

                if (Array.IndexOf(worldMap, new AcidSquare(x, y)) == -1)
                {
                    worldMap[index] = new AcidSquare(x, y);
                    if (worldMap[++index] != null)
                        index = FindIndex(worldMap);
                    count--;
                    Counter.CountAcid++;
                }
            }
        }

        public void AddFood(BaseSquare[] worldMap, int count)
        {
            if (Counter.CountFood > new Constants().CountFood)
                return;

            int index = FindIndex(worldMap);

            while (count > 0)
            {
                int y = random.Next(1, constants.WorldSizeY - 1);
                int x = random.Next(1, constants.WorldSizeX - 1);

                if (Array.IndexOf(worldMap, new FoodSquare(x, y)) == -1)
                {
                    worldMap[index] = new FoodSquare(x, y);
                    if (worldMap[++index] != null)
                        index = FindIndex(worldMap);
                    count--;
                    Counter.CountFood++;
                }
            }
        }
    }
}
