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

        }

        public void test()
        {
            for (int y = 1; y < constants.WorldSizeY - 1; y++)
            {
                for (int x = 1; x < constants.WorldSizeX - 1; x++)
                {
                    if (WorldMap[y][x] is BioSquare)
                    {
                        Thread.Sleep(10);
                        (WorldMap[y][x] as BioSquare).Health--;
                        (WorldMap[y][x] as BioSquare).Coordinate = new Point(x + 1, y);
                        (WorldMap[y][x] as BioSquare).Coordinate = new Point(x, y + 1);
                        (WorldMap[y + 1][x + 1]) = WorldMap[y][x];
                        WorldMap[y][x] = new EmptySquare(x, y);
                    }
                }
            }
        }
    }
}
