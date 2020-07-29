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
        private BaseSquare[] worldMap;
        private Constants constants = new Constants();
        private bool minCountLive = false;
        private Dictionary<TypeSquare, Action> pointerOffset;
        private BioSquare currentBio;
        private int globalIndex;


        public BehaviorSquare(BaseSquare[] worldMap)
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
            for (int i = 0; i < constants.WorldSizeX * constants.WorldSizeY; i++)
            {
                if (worldMap[i] is BioSquare)
                {
                    currentBio = worldMap[i] as BioSquare;
                    globalIndex = i;

                    ActionSquare();

                    if (minCountLive)
                        return;
                }
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

            x = currentBio.PointX;
            y = currentBio.PointY;

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

            pointerOffset[worldMap[SearchIndex(x, y)].TypeSquare]();

            if (currentBio.Pointer >= constants.SizeBrain)
                currentBio.Pointer -= constants.SizeBrain;

            currentBio.Direction = oldDirection;

            return newPoint;
        }

        private void Move()
        {
            Point newPoint = new Point();

            newPoint = Check(currentBio.Brain[currentBio.Pointer]);

            if (worldMap[SearchIndex((int)newPoint.X, (int)newPoint.Y)] is FoodSquare)
            {
                StepBio((int)newPoint.X, (int)newPoint.Y);

                currentBio.Health += constants.Energy;

                Counter.CountFood--;

                new CreatorSquares().AddFood(worldMap, 1);
            }

            if (worldMap[SearchIndex((int)newPoint.X, (int)newPoint.Y)] is null)
            {
                StepBio((int)newPoint.Y, (int)newPoint.X);
            }

            if (worldMap[SearchIndex((int)newPoint.X, (int)newPoint.Y)] is AcidSquare)
            {
                DeleteBio();

                worldMap[globalIndex] = new AcidSquare(currentBio.PointX, currentBio.PointY);

                Counter.CountAcid++;

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

            if (worldMap[SearchIndex((int)newPoint.X, (int)newPoint.Y)] is null || worldMap[SearchIndex((int)newPoint.X, (int)newPoint.Y)] is WallSquare)
                return;

            if (worldMap[SearchIndex((int)newPoint.X, (int)newPoint.Y)] is FoodSquare)
            {
                worldMap[SearchIndex((int)newPoint.X, (int)newPoint.Y)] = new EmptySquare((int)newPoint.X, (int)newPoint.Y);

                currentBio.Health += constants.Energy;

                Counter.CountFood--;

                new CreatorSquares().AddFood(worldMap, 1);
            }

            if (worldMap[SearchIndex((int)newPoint.X, (int)newPoint.Y)] is AcidSquare)
            {
                worldMap[SearchIndex((int)newPoint.X, (int)newPoint.Y)] = new FoodSquare((int)newPoint.X, (int)newPoint.Y);

                Counter.CountAcid--;
                Counter.CountFood++;

                new CreatorSquares().AddAcid(worldMap, 1);
            }
        }

        private void DeleteBio()
        {
            worldMap[globalIndex] = null;
            Counter.CountLiveBio--;
        }

        private void StepBio(int pointX, int pointY)
        {
            worldMap[globalIndex].PointX = pointX;
            worldMap[globalIndex].PointY = pointY;
        }

        private int SearchIndex(int x, int y)
        {
            for (int i = 0; i < constants.WorldSizeX * constants.WorldSizeY; i++)
            {
                if (worldMap[i] != null)
                    if (worldMap[i].PointX == x && worldMap[i].PointY == y)
                        return i;
            }

            return 0;
        }
    }
}
