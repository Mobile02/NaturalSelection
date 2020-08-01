using NaturalSelection.Model.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        private bool CheckIndex(BaseSquare[] worldMap, int pointX, int pointY)
        {
            for (int i = 0; i < Counter.Index; i++)
            {
                if (worldMap[i].PointX == pointX && worldMap[i].PointY == pointY)
                    return true;
            }

            return false;
        }

        private int[] BrainGenerator()
        {
            int[] brain = new int[constants.SizeBrain];

            for (int i = 0; i < constants.SizeBrain; i++)
            {
                brain[i] = random.Next(64);
            }

            return (int[])brain.Clone();
        }

        private int[] BrainGeneratorChild(int[] brainParent)
        {
            int[] brainChild = new int[constants.SizeBrain];
            brainChild = (int[])brainParent.Clone();

            if (random.Next(8) == 2 || random.Next(8) == 7 || random.Next(8) == 4)
                brainChild[random.Next(constants.SizeBrain)] = random.Next(constants.SizeBrain);

            return (int[])brainChild.Clone();
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

                if (!CheckIndex(worldMap, x, y))
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

                if (!CheckIndex(worldMap, x, y))
                {
                    worldMap[Counter.Index] = new BioSquare(x, y, Counter.Index++)
                    {
                        Brain = BrainGenerator(),
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

                if (!CheckIndex(worldMap, x, y))
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

                if (!CheckIndex(worldMap, x, y))
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

        private int FindIndexAdd(BaseSquare[] worldMap, string typeSquare, int startIndex, int endIndex)
        {
            for (int i = startIndex; i < endIndex; i++)
            {
                if (worldMap[i].PointX == -1 && worldMap[i].PointY == -1)
                {
                    if (typeSquare == "Food" && worldMap[i] is FoodSquare)
                        return i;
                    if (typeSquare == "Acid" && worldMap[i] is AcidSquare)
                        return i;
                    if (typeSquare == "Bio" && worldMap[i] is BioSquare)
                        return i;
                }
            }

            return 0;
        }

        public void InitWorldMap(BaseSquare[] worldMap)
        {
            new CreatorSquares().AddBioSquare(worldMap, constants.CountBio);
            new CreatorSquares().AddFood(worldMap, constants.CountFood);
            new CreatorSquares().AddAcid(worldMap, constants.CountAcid);
            new CreatorSquares().AddWall(worldMap, constants.CountWall);
            new CreatorSquares().FillField(worldMap);
        }

        public void AddFoodSquare(BaseSquare[] worldMap, int count, int pointX = 0, int pointY = 0)
        {
            if (Counter.CountFood > constants.CountFood)
                return;

            int index = FindIndexAdd(worldMap, "Food", constants.CountBio, constants.CountBio * 2 + constants.CountFood);

            if (pointX == 0)
            {
                pointY = random.Next(1, constants.WorldSizeY - 1);
                pointX = random.Next(1, constants.WorldSizeX - 1);
            }

            while (count > 0)
            {
                if (!CheckIndex(worldMap, pointX, pointY))
                {
                    worldMap[index].PointX = pointX;
                    worldMap[index].PointY = pointY;

                    count--;
                    Counter.CountFood++;
                }

                pointY = random.Next(1, constants.WorldSizeY - 1);
                pointX = random.Next(1, constants.WorldSizeX - 1);
            }
        }

        public void AddAcidSquare(BaseSquare[] worldMap, int count, int pointX = 0, int pointY = 0)
        {
            if (Counter.CountAcid > constants.CountAcid + constants.CountBio - 10)
                return;

            int index = FindIndexAdd(worldMap, "Acid", constants.CountBio * 2 + constants.CountFood, constants.CountBio * 3 + constants.CountFood + constants.CountAcid);
            
            if (pointX == 0)
            {
                pointY = random.Next(1, constants.WorldSizeY - 1);
                pointX = random.Next(1, constants.WorldSizeX - 1);
            }

            while (count > 0)
            {
                if (!CheckIndex(worldMap, pointX, pointY))
                {
                    worldMap[index].PointX = pointX;
                    worldMap[index].PointY = pointY;

                    count--;
                    Counter.CountAcid++;
                }

                pointY = random.Next(1, constants.WorldSizeY - 1);
                pointX = random.Next(1, constants.WorldSizeX - 1);
            }
        }

        public void AddChild(BaseSquare[] worldMap)
        {
            BioSquare[] parents = new BioSquare[constants.CountBio / 8];
            int indexParent = 0;

            for (int i = 0; i < constants.CountBio; i++)
            {
                if (worldMap[i].PointX != -1 && worldMap[i].PointY != -1)
                {
                    parents[indexParent++] = (BioSquare)(worldMap[i] as BioSquare).Clone();
                }
            }

            for (int i = 0; i < parents.Length; i++)
            {
                int count = 0;

                while (count < 7)
                {
                    int y = random.Next(1, constants.WorldSizeY - 1);
                    int x = random.Next(1, constants.WorldSizeX - 1);

                    if (!CheckIndex(worldMap, x, y))
                    {
                        int index = FindIndexAdd(worldMap, "Bio", 0, constants.CountBio);
                        var children = worldMap[index] as BioSquare;

                        children.PointX = x;
                        children.PointY = y;
                        children.Pointer = 0;
                        children.Health = constants.HealthSquare;
                        children.Direction = (Direction)random.Next(8);
                        children.Brain = BrainGeneratorChild(parents[i].Brain);

                        Counter.CountLiveBio++;
                        count++;
                    }
                }

            }
        }

        public void RefreshHealthBio(BaseSquare[] worldMap)
        {
            for (int i = 0; i < constants.CountBio; i++)
            {
                (worldMap[i] as BioSquare).Health = constants.HealthSquare;
            }
        }
    }
}
