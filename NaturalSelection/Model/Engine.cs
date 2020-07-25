using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
