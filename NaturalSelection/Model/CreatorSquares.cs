using NaturalSelection.Model.Support;
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

        private bool FindIndex(BaseSquare[] worldMap, int pointX, int pointY)
        {
            for (int i = 0; i < Counter.Index; i++)
            {
                if (worldMap[i].PointX == pointX && worldMap[i].PointY == pointY)
                    return true;
            }

            return false;
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

        private void FillField(BaseSquare[] worldMap)
        {
            int y = 0, x = 0;

            for (int i = 0; i < constants.WorldSizeX; i++)
            {
                worldMap[Counter.Index] = new WallSquare(x, 0, Counter.Index++);
                worldMap[Counter.Index] = new WallSquare(x++, constants.WorldSizeY - 1, Counter.Index++);
            }

            int maxIndex = constants.WorldSizeY + Counter.Index;

            for (int i = Counter.Index; i < maxIndex; i++)
            {
                worldMap[Counter.Index] = new WallSquare(0, y, Counter.Index++);
                worldMap[Counter.Index] = new WallSquare(constants.WorldSizeX - 1, y++, Counter.Index++);
            }
        }

        private void AddWall(BaseSquare[] worldMap, int count)
        {
            while (count > 0)
            {
                int y = random.Next(1, constants.WorldSizeY - 1);
                int x = random.Next(1, constants.WorldSizeX - 1);

                if (!FindIndex(worldMap, x, y))
                {
                    worldMap[Counter.Index] = new WallSquare(x, y, Counter.Index++);
                    count--;
                }
            }
        }

        private void AddBioSquare(BaseSquare[] worldMap, int count)
        {
            while (count > 0)
            {
                int y = random.Next(1, constants.WorldSizeY - 1);
                int x = random.Next(1, constants.WorldSizeX - 1);

                if (!FindIndex(worldMap, x, y))
                {
                    worldMap[Counter.Index] = new BioSquare(x, y, Counter.Index++)
                    {
                        Brain = BrainCenerator(),
                        Pointer = 0,
                        Health = constants.HealthSquare,
                        Direction = (Direction)random.Next(8)
                    };

                    Counter.CountLiveBio++;
                    count--;
                }
            }
        }

        private void AddAcid(BaseSquare[] worldMap, int count)
        {
            while (count > 0)
            {
                int y = random.Next(1, constants.WorldSizeY - 1);
                int x = random.Next(1, constants.WorldSizeX - 1);

                if (!FindIndex(worldMap, x, y))
                {
                    worldMap[Counter.Index] = new AcidSquare(x, y, Counter.Index++);
                    count--;
                    Counter.CountAcid++;
                }
            }

            int maxIndexAcid = Counter.Index + constants.CountBio;

            for (int i = Counter.Index; i < maxIndexAcid; i++)
                worldMap[i] = new AcidSquare(-1, -1, Counter.Index++);
        }

        private void AddFood(BaseSquare[] worldMap, int count)
        {
            while (count > 0)
            {
                int y = random.Next(1, constants.WorldSizeY - 1);
                int x = random.Next(1, constants.WorldSizeX - 1);

                if (!FindIndex(worldMap, x, y))
                {
                    worldMap[Counter.Index] = new FoodSquare(x, y, Counter.Index++);
                    count--;
                    Counter.CountFood++;
                }
            }

            int maxIndexFood = Counter.Index + constants.CountBio;

            for (int i = Counter.Index; i < maxIndexFood; i++)
                worldMap[i] = new FoodSquare(-1, -1, Counter.Index++);
        }

        private int FindIndexAdd(BaseSquare[] worldMap, string typeSquare)
        {
            for (int i = 0; i < constants.WorldSizeX * constants.WorldSizeY; i++) //TODO: Поменять диапазон
            {
                if (worldMap[i].PointX == -1 && worldMap[i].PointY == -1)
                {
                    if (typeSquare == "Food" && worldMap[i] is FoodSquare)
                        return i;
                    if (typeSquare == "Acid" && worldMap[i] is AcidSquare)
                        return i;
                }
            }

            return 0;
        }

        public void InitWorldMap(BaseSquare[] worldMap)
        {
            new CreatorSquares().FillField(worldMap);
            new CreatorSquares().AddWall(worldMap, 20);
            new CreatorSquares().AddFood(worldMap, constants.CountFood);
            new CreatorSquares().AddAcid(worldMap, constants.CountAcid);
            new CreatorSquares().AddBioSquare(worldMap, constants.CountBio);
        }

        public int AddFoodSquare(BaseSquare[] worldMap, int count, int pointX, int pointY)
        {
            int index = FindIndexAdd(worldMap, "Food");

            if(pointX == 0)
            {
                pointY = random.Next(1, constants.WorldSizeY - 1);
                pointX = random.Next(1, constants.WorldSizeX - 1);
            }

            while (count > 0)
            {
                if (!FindIndex(worldMap, pointX, pointY))
                {
                    worldMap[index].PointX = pointX;
                    worldMap[index].PointY = pointY;

                    count--;
                    Counter.CountFood++;
                }

                pointY = random.Next(1, constants.WorldSizeY - 1);
                pointX = random.Next(1, constants.WorldSizeX - 1);
            }

            return index;
        }

        public int AddAcidSquare(BaseSquare[] worldMap, int count, int pointX, int pointY)
        {
            int index = FindIndexAdd(worldMap, "Acid");

            if (pointX == 0)
            {
                pointY = random.Next(1, constants.WorldSizeY - 1);
                pointX = random.Next(1, constants.WorldSizeX - 1);
            }

            while (count > 0)
            {
                if (!FindIndex(worldMap, pointX, pointY))
                {
                    worldMap[index].PointX = pointX;
                    worldMap[index].PointY = pointY;

                    count--;
                    Counter.CountAcid++;
                }

                pointY = random.Next(1, constants.WorldSizeY - 1);
                pointX = random.Next(1, constants.WorldSizeX - 1);
            }

            return index;
        }
    }
}
