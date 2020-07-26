using NaturalSelection.Model.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NaturalSelection.Model
{
    public class BehaviorSquare
    {
        private BaseSquare[][] worldMap;
        private Constants constants = new Constants();
        private bool minCountLive = false;
        private Dictionary<TypeSquare, Action> pointerOffset;
        private BioSquare currentBio;


        public BehaviorSquare(BaseSquare[][] worldMap)
        {
            this.worldMap = worldMap;

            pointerOffset = new Dictionary<TypeSquare, Action>
            {
                { TypeSquare.ACID, () => currentBio.Pointer++},
                { TypeSquare.FOOD, () => currentBio.Pointer += 2 },
                { TypeSquare.BIO, () => currentBio.Pointer += 3 },
                { TypeSquare.EMPTY, () => currentBio.Pointer += 4 },
                { TypeSquare.WALL, () => currentBio.Pointer += 5 }
            };

            StartAction();
        }

        private void StartAction()
        {
            BioSquare[] tmpArrayBio = new BioSquare[Counter.CountLiveBio];
            int count = 0;

            for (int y = 1; y < constants.WorldSizeY - 1; y++)
            {
                for (int x = 0; x < constants.WorldSizeX - 1; x++)
                {
                    if (worldMap[y][x] is BioSquare)
                    {
                        tmpArrayBio[count] = worldMap[y][x] as BioSquare;

                        count++;
                    }
                }
            }

            for (int i = 0; i < count; i++)
            {
                currentBio = tmpArrayBio[i];

                ActionSquare();

                if (minCountLive)
                    return;
            }
        }

        private void ActionSquare()
        {
            for (int i = 0; i < 10; i++)
            {
                if (currentBio.Brain[currentBio.Pointer] < 8) { Move(); break; }

                if (currentBio.Brain[currentBio.Pointer] > 7 && currentBio.Brain[currentBio.Pointer] < 16) { Turn(); continue; }

                if (currentBio.Brain[currentBio.Pointer] > 15 && currentBio.Brain[currentBio.Pointer] < 24) { Convert(); break; }

                if (currentBio.Brain[currentBio.Pointer] > 23 && currentBio.Brain[currentBio.Pointer] < 32) { Check(currentBio.Brain[currentBio.Pointer] - 24); continue; }

                if (currentBio.Brain[currentBio.Pointer] > 31)
                {
                    currentBio.Pointer += currentBio.Brain[currentBio.Pointer];
                    if (currentBio.Pointer >= constants.SizeBrain)
                        currentBio.Pointer -= constants.SizeBrain;
                    continue;
                }
            }

            currentBio.Health--;

            if (currentBio.Health < 0)
                currentBio.Health = 0;
            if (currentBio.Health > 99)
                currentBio.Health = 99;

            if (currentBio.Health <= 0)
            {
                DeleteBio();
                Counter.CountLiveBio--;

                if (Counter.CountLiveBio == (constants.CountBio / 8))
                    minCountLive = true;
            }
        }

        private Point Check(int direction)
        {
            Point newPoint = new Point();
            Direction oldDirection = new Direction();
            oldDirection = currentBio.Direction;
            int x, y;

            x = (int)currentBio.Coordinate.X;
            y = (int)currentBio.Coordinate.Y;

            currentBio.Direction = currentBio.Direction + direction;

            if ((int)currentBio.Direction > 7)
                currentBio.Direction -= 8;

            switch (currentBio.Direction)
            {
                case Direction.UP:
                    y--;
                    break;
                case Direction.UPRIGHT:
                    y--;
                    x++;
                    break;
                case Direction.RIGHT:
                    x++;
                    break;
                case Direction.RIGHTDOWN:
                    y++;
                    x++;
                    break;
                case Direction.DOWN:
                    y++;
                    break;
                case Direction.LEFTDOWN:
                    y++;
                    x--;
                    break;
                case Direction.LEFT:
                    x--;
                    break;
                case Direction.UPLEFT:
                    y--;
                    x--;
                    break;
            }

            newPoint.X = x;
            newPoint.Y = y;

            pointerOffset[currentBio.TypeSquare]();

            if (currentBio.Pointer >= constants.SizeBrain)
                currentBio.Pointer -= constants.SizeBrain;

            currentBio.Direction = oldDirection;

            return newPoint;
        }

        private void Move()
        {
            Point newPoint = new Point();

            newPoint = Check(currentBio.Brain[currentBio.Pointer]);

            if (worldMap[(int)newPoint.Y][(int)newPoint.X] is FoodSquare)
            {
                StepBio((int)newPoint.Y, (int)newPoint.X);

                Counter.CountFood--;

                new CreatorSquares().AddFood(worldMap, 1);
            }

            if (worldMap[(int)newPoint.Y][(int)newPoint.X] is EmptySquare)
            {
                StepBio((int)newPoint.Y, (int)newPoint.X);
            }

            if (worldMap[(int)newPoint.Y][(int)newPoint.X] is AcidSquare)
            {
                DeleteBio();

                worldMap[(int)currentBio.Coordinate.Y][(int)currentBio.Coordinate.X] = new AcidSquare((int)currentBio.Coordinate.X, (int)currentBio.Coordinate.Y);

                Counter.CountAcid++;
                Counter.CountLiveBio--;

                if (Counter.CountLiveBio == (constants.CountBio / 8))
                    minCountLive = true;
            }
        }

        private void Turn()
        {
            int direction = currentBio.Brain[currentBio.Pointer] - 8;

            currentBio.Direction += direction;

            if ((int)currentBio.Direction > 7)
                currentBio.Direction -= 8;

            currentBio.Pointer++;

            if (currentBio.Pointer >= constants.SizeBrain)
                currentBio.Pointer -= constants.SizeBrain;
        }

        public void Convert()
        {
            Point newPoint = new Point();
            newPoint = Check(currentBio.Brain[currentBio.Pointer] - 16);

            if (worldMap[(int)newPoint.Y][(int)newPoint.X] is EmptySquare || worldMap[(int)newPoint.Y][(int)newPoint.X] is WallSquare)
                return;

            if (worldMap[(int)newPoint.Y][(int)newPoint.X] is FoodSquare)
            {
                worldMap[(int)newPoint.Y][(int)newPoint.X] = new EmptySquare((int)newPoint.X, (int)newPoint.Y);

                currentBio.Health += constants.Energy;

                Counter.CountFood--;

                new CreatorSquares().AddFood(worldMap, 1);
            }

            if (worldMap[(int)newPoint.Y][(int)newPoint.X] is AcidSquare)
            {
                worldMap[(int)newPoint.Y][(int)newPoint.X] = new FoodSquare((int)newPoint.X, (int)newPoint.Y);

                Counter.CountAcid--;
                Counter.CountFood++;

                new CreatorSquares().AddAcid(worldMap, 1);
            }
        }

        private void DeleteBio()
        {
            worldMap[(int)currentBio.Coordinate.Y][(int)currentBio.Coordinate.X] = new EmptySquare((int)currentBio.Coordinate.X, (int)currentBio.Coordinate.Y);
        }

        private void StepBio(int pointY, int pointX)
        {
            worldMap[pointY][pointX] = currentBio.Clone() as BioSquare;
            worldMap[pointY][pointX].Coordinate = new Point(pointX, pointY);

            DeleteBio();

            currentBio = worldMap[pointY][pointX] as BioSquare;
        }
    }
}
