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


        private int[] BrainCenerator()
        {
            int[] brain = new int[constants.SizeBrain];

            for (int i = 0; i < constants.SizeBrain; i++)
            {
                brain[i] = random.Next(64);
            }

            return (int[])brain.Clone();
        }

        public void FillWorldMap(BaseSquare[][] worldMap)
        {
            for (int y = 0; y < constants.WorldSizeY; y++)
            {
                worldMap[y] = new BaseSquare[constants.WorldSizeX];

                for (int x = 0; x < constants.WorldSizeX; x++)
                {
                    worldMap[y][x] = new EmptySquare(x, y);

                    if (y == 0 || y == constants.WorldSizeY - 1)
                        worldMap[y][x] = new WallSquare(x, y);

                    if (x == 0 || x == constants.WorldSizeX - 1)
                        worldMap[y][x] = new WallSquare(x, y);
                }
            }
        }

        public void AddWall(BaseSquare[][] worldMap, int count)
        {
            while (count > 0)
            {
                int y = random.Next(1, constants.WorldSizeY - 1);
                int x = random.Next(1, constants.WorldSizeX - 1);

                if (worldMap[y][x] is EmptySquare)
                {
                    worldMap[y][x] = new EmptySquare(x, y);
                    count--;
                }
            }
        }

        public void AddBioSquare(BaseSquare[][] worldMap, int count)
        {
            while (count > 0)
            {
                int y = random.Next(1, constants.WorldSizeY - 1);
                int x = random.Next(1, constants.WorldSizeX - 1);

                if (worldMap[y][x] is EmptySquare)
                {
                    worldMap[y][x] = new BioSquare(x, y)
                    {
                        Brain = BrainCenerator(),
                        Pointer = 0,
                        Health = constants.HealthSquare,
                        Direction = (Direction)random.Next(8)
                    };
                    count--;
                }
            }
        }

        public void AddAcid(BaseSquare[][] worldMap, int count)
        {
            while (count > 0)
            {
                int y = random.Next(1, constants.WorldSizeY - 1);
                int x = random.Next(1, constants.WorldSizeX - 1);

                if (worldMap[y][x] is EmptySquare)
                {
                    worldMap[y][x] = new AcidSquare(x, y);
                    count--;
                }
            }
        }

        public void AddFood(BaseSquare[][] worldMap, int count)
        {
            while (count > 0)
            {
                int y = random.Next(1, constants.WorldSizeY - 1);
                int x = random.Next(1, constants.WorldSizeX - 1);

                if (worldMap[y][x] is EmptySquare)
                {
                    worldMap[y][x] = new FoodSquare(x, y);
                    count--;
                }
            }
        }
    }
}
