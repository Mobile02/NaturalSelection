using NaturalSelection.Model.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace NaturalSelection.Model
{
    public class BehaviorSquare
    {
        private BaseSquare[] worldMap;
        private readonly Constants constants = new Constants();
        private bool minCountLive = false;
        private Dictionary<Direction, coordinate> CalcNextCoordinate;
        private Dictionary<Type, Action> MovePointer;
        private delegate void coordinate(ref Point newPoint);
        private BioSquare currentBio;
        private int currentIndex = 0;
        private int indexForAction = 1;

        public BehaviorSquare(BaseSquare[] worldMap)
        {
            this.worldMap = worldMap;

            MovePointer = new Dictionary<Type, Action> 
            {
                { typeof(AcidSquare), () => currentBio.Pointer++ },
                { typeof(FoodSquare), () => currentBio.Pointer += 2 },
                { typeof(BioSquare), () => currentBio.Pointer += 3 },
                { typeof(WallSquare), () => currentBio.Pointer += 5 }
            };

            CalcNextCoordinate = new Dictionary<Direction, coordinate>
            {
                {Direction.UP, (ref Point newPoint) => newPoint.Y-- },
                {Direction.UPRIGHT, (ref Point newPoint) => { newPoint.Y--; newPoint.X++; } },
                {Direction.RIGHT, (ref Point newPoint) => newPoint.X++ },
                {Direction.RIGHTDOWN, (ref Point newPoint) => { newPoint.X++; newPoint.Y++; } },
                {Direction.DOWN, (ref Point newPoint) => newPoint.Y++ },
                {Direction.LEFTDOWN, (ref Point newPoint) => { newPoint.Y++; newPoint.X--; } },
                {Direction.LEFT, (ref Point newPoint) => newPoint.X-- },
                {Direction.UPLEFT, (ref Point newPoint) => {newPoint.Y--; newPoint.X--; }}
            };

            StartAction();
        }

        private void StartAction()
        {
            for (int i = 0; i < constants.CountBio; i++)
            {
                if (worldMap[i].PointX == -1 && worldMap[i].PointY == -1)
                    continue;

                currentBio = worldMap[i] as BioSquare;
                currentIndex = i;

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

            if (currentBio.Health > 99)
                currentBio.Health = 99;
            if (currentBio.Health <= 0)
                currentBio.Health = 0;

            if (currentBio.Health <= 0)
            {
                currentBio.PointX = -1;
                currentBio.PointY = -1;
                Counter.CountLiveBio--;

                if (Counter.CountLiveBio == (constants.CountBio / 8))
                {
                    minCountLive = true;
                    currentBio = null;
                }

            }
        }

        private Point Check(int direction)
        {
            Point newPoint = new Point();
            Direction oldDirection = currentBio.Direction;

            newPoint.X = currentBio.PointX;
            newPoint.Y = currentBio.PointY;

            currentBio.Direction += direction;

            if ((int)currentBio.Direction > 7)
                currentBio.Direction -= 8;

            CalcNextCoordinate[currentBio.Direction](ref newPoint);

            indexForAction = SearchIndex((int)newPoint.X, (int)newPoint.Y, 0, constants.CountBio * 3 + constants.CountAcid + constants.CountFood +
                                         constants.WorldSizeX * 2 + constants.WorldSizeY * 2 + constants.CountWall + 10);

            if (worldMap[indexForAction] != null)
                MovePointer[worldMap[indexForAction].GetType()]();
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

                if (Counter.CountFood < constants.CountFood)
                    new CreatorSquares().AddFoodSquare(worldMap, 1);
            }

            if (worldMap[indexForAction] is null)
            {
                StepBio((int)newPoint.X, (int)newPoint.Y);
            }

            if (worldMap[indexForAction] is AcidSquare)
            {
                int x = currentBio.PointX;
                int y = currentBio.PointY;

                currentBio.Health = 50; // исключает двойное вычитание кол-ва живых
                currentBio.PointX = -1;
                currentBio.PointY = -1;
                Counter.CountLiveBio--;

                if (Counter.CountAcid < constants.CountAcid + constants.CountBio - 1)
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
            Point newPoint = Check(currentBio.Brain[currentBio.Pointer] - 16);

            if (worldMap[indexForAction] is null || worldMap[indexForAction] is WallSquare || worldMap[indexForAction] is BioSquare)
                return;

            if (worldMap[indexForAction] is FoodSquare)
            {
                MarkInInactive(indexForAction);

                currentBio.Health += constants.Energy;

                Counter.CountFood--;

                if (Counter.CountFood < constants.CountFood)
                    new CreatorSquares().AddFoodSquare(worldMap, 1);
            }

            if (worldMap[indexForAction] is AcidSquare)
            {
                MarkInInactive(indexForAction);

                new CreatorSquares().AddFoodSquare(worldMap, 1, (int)newPoint.X, (int)newPoint.Y);

                Counter.CountAcid--;

                if (Counter.CountAcid < constants.CountAcid)
                    new CreatorSquares().AddAcidSquare(worldMap, 1);
            }
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
                if (worldMap[i] != null && worldMap[i].PointX == searchPointX && worldMap[i].PointY == searchPointY && worldMap[i].PointX != -1)
                    return worldMap[i].Index;
                else if (worldMap[i] == null)
                    return i;
            }

            throw new Exception("Поиск индекса следующей точки пошел по пизде");
        }
    }
}
