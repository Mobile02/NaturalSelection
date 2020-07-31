using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalSelection.Model
{
    public class Constants
    {
        public int WorldSizeX { get; private set; }
        public int WorldSizeY { get; private set; }
        public int CountBio { get; private set; }
        public int CountCicle { get; private set; }
        public int HealthSquare { get; private set; }
        public int Energy { get; private set; }
        public int CountAcid { get; private set; }
        public int CountFood { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int SizeBrain { get; private set; }
        public int ScaleChart { get; private set; }
        public int CountWall { get; private set; }

        public Constants()
        {
            WorldSizeX = 64;
            WorldSizeY = 40;
            CountBio = 64; // кратно 8
            CountCicle = 100000;
            HealthSquare = 50;
            Energy = 10;
            CountAcid = 120;
            CountFood = 120;
            CountWall = 20;
            Height = 15;
            Width = 15;
            SizeBrain = 64;
            ScaleChart = 700;
        }
    }
}
