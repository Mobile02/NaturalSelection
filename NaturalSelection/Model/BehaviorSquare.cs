﻿using NaturalSelection.Model.Support;
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
        private int currentIndex;
        private int indexForAction = 1;

        public BehaviorSquare(BaseSquare[] worldMap)
        {
            this.worldMap = worldMap;

            pointerOffset = new Dictionary<TypeSquare, Action>
            {
                { TypeSquare.ACID, () => currentBio.Pointer++},
                { TypeSquare.FOOD, () => currentBio.Pointer += 2 },
                { TypeSquare.BIO, () => currentBio.Pointer += 3 },
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
                    currentBio = null;
                    currentBio = worldMap[i] as BioSquare;

                    if (currentBio.PointX < 0)
                        continue;

                    currentIndex = worldMap[i].Index;

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
                DeleteBio(currentIndex);

                if (Counter.CountLiveBio == (constants.CountBio / 8))
                    minCountLive = true;
            }
        }

        private Point Check(int direction)
        {
            Point newPoint = new Point();
            Direction oldDirection = currentBio.Direction;
            int x, y;

            x = currentBio.PointX;
            y = currentBio.PointY;

            currentBio.Direction += direction;

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

            indexForAction = SearchIndex(x, y, 1, 2000); ///TODO: 2000

            if (worldMap[indexForAction] != null)
                pointerOffset[worldMap[indexForAction].TypeSquare]();
            else
                currentBio.Pointer += 4;

            if (currentBio.Pointer >= constants.SizeBrain)
                currentBio.Pointer -= constants.SizeBrain;

            currentBio.Direction = oldDirection;

            return newPoint;
        }

        private void Move()
        {
            Point newPoint = Check(currentBio.Brain[currentBio.Pointer]);

            if (worldMap[indexForAction] is FoodSquare)
            {
                MarkInInactive(indexForAction);

                StepBio((int)newPoint.X, (int)newPoint.Y);

                currentBio.Health += constants.Energy;

                Counter.CountFood--;

                new CreatorSquares().AddFoodSquare(worldMap, 1, 0, 0);
            }

            if (worldMap[indexForAction] is null)
            {
                StepBio((int)newPoint.X, (int)newPoint.Y);
            }

            if (worldMap[indexForAction] is AcidSquare)
            {
                int x, y;
                x = currentBio.PointX;
                y = currentBio.PointY;

                DeleteBio(currentIndex);

                new CreatorSquares().AddAcidSquare(worldMap, 1, x, y);

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

            if (worldMap[indexForAction] is null || worldMap[indexForAction] is WallSquare || worldMap[indexForAction] is BioSquare)
                return;

            if (worldMap[indexForAction] is FoodSquare)
            {
                MarkInInactive(indexForAction);

                currentBio.Health += constants.Energy;

                Counter.CountFood--;

                new CreatorSquares().AddFoodSquare(worldMap, 1, 0, 0);
            }

            if (worldMap[indexForAction] is AcidSquare)
            {
                MarkInInactive(indexForAction);

                new CreatorSquares().AddFoodSquare(worldMap, 1, (int)newPoint.X, (int)newPoint.Y);

                Counter.CountAcid--;

                new CreatorSquares().AddAcidSquare(worldMap, 1, 0, 0);
            }
        }

        private void DeleteBio(int index)
        {
            MarkInInactive(index);
            Counter.CountLiveBio--;
        }

        private void MarkInInactive(int index)
        {
            worldMap[index].PointX = -1;
            worldMap[index].PointY = -1;
        }

        private void StepBio(int pointX, int pointY)
        {
            worldMap[currentIndex].PointX = pointX;
            worldMap[currentIndex].PointY = pointY;
        }

        private int SearchIndex(int searchPointX, int searchPointY, int startIndex, int finishIndex)
        {
            for (int i = startIndex; i < finishIndex; i++)
            {
                if (worldMap[i] != null && worldMap[i].PointX == searchPointX && worldMap[i].PointY == searchPointY)
                    return worldMap[i].Index;
                else if (worldMap[i] == null)
                    return i;
            }

            return 0;
        }
    }
}
