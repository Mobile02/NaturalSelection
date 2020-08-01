using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalSelection.Model.Support
{
    public class FileOperations
    {
        public void SaveBrain(BaseSquare[] worldMap)
        {
            for (int i = 0; i < new Constants().CountBio; i++)
            {
                if (worldMap[i] is BioSquare && worldMap[i].PointX != -1)
                {
                    using (StreamWriter streamWriter = new StreamWriter("brain.txt", true))
                    {
                        string text = "";

                        for (int j = 0; j < new Constants().SizeBrain; j++)
                        {
                            text += (worldMap[i] as BioSquare).Brain[j].ToString() + "  ";
                        }

                        streamWriter.WriteLine(text);
                    }
                }
            }

            using (StreamWriter streamWriter = new StreamWriter("brain.txt", true))
            {
                streamWriter.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            }
        }
    }
}
