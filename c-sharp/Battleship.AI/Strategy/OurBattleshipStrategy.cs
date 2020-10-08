using Battleship.AI.AITester;

namespace Battleship.AI.Strategy
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
    }

    public class OurBattleshipStrategy : IBattleshipStrategy
    {
        private int targetX;
        private int targetY;
        private int middleIndexY;
        private int middleIndexX;
        private Direction direction = Direction.Right; 
        private const int MaxColumns = 10;

        private FireResult lastLastResult;
        private FireResult lastLastLastResult;

        private int initialHitX = -1;
        private int initialHitY = -1;
        private const int firstMiddleX = MaxColumns / 5;
        private const int firstMiddleY = MaxColumns / 5;
        private const int lastMiddleX = (MaxColumns / 5)  * 4;
        private const int lastMiddleY = (MaxColumns / 5)  * 4;

        public OurBattleshipStrategy()
        {

        }

        public (int x, int y) GetNextMove(FireResult lastResult)
        {
            if (lastResult == FireResult.Hit && lastLastLastResult != FireResult.Hit)
            {
                initialHitX = targetX;
                initialHitY = targetY;
            }
            if (lastResult == FireResult.None)
            {
                targetX = firstMiddleX;
                targetY = firstMiddleY;
            }
            else if (lastResult == FireResult.Sink)
            {
                initialHitX = -1;
                initialHitY = -1;
                if (!MiddleComplete())
                    SearchMiddle();
                else
                    SearchEdge();
            }
            else if (lastResult == FireResult.Hit && lastResult != FireResult.Sink)
            {
                MoveDirection();
            }
            else if (lastResult == FireResult.Miss && lastLastResult == FireResult.Hit && lastLastLastResult == FireResult.Hit)
            {
                targetX = initialHitX;
                targetY = initialHitY;
                InvertDirection();
                MoveDirection();
            }
            else if (lastResult == FireResult.Miss && lastLastResult == FireResult.Hit)
            {
                InvertDirection();
                MoveDirection();
                InvertDirection();
                RotateDirection();
            }
            else
            {
                if (!MiddleComplete())
                    SearchMiddle();
                else
                    SearchEdge();
            }

            lastLastResult = lastResult;
            return (targetX, targetY);
        }

        private bool MiddleComplete() =>
            middleIndexX == lastMiddleX && middleIndexY == lastMiddleY;

        private void SearchEdge()
        {
                targetX += 2;
                if (targetX >= lastMiddleX)
                {
                    targetX = targetY % 2 == firstMiddleY % 2 ? firstMiddleX : firstMiddleX + 1;
                }

                targetY++;
                if (targetY >= lastMiddleY)
                {
                    targetY = firstMiddleY;
                }
                middleIndexX = targetX;
                middleIndexY = targetY;
        } 


        private void SearchMiddle()
        {
                middleIndexX++;
                if (middleIndexX >= lastMiddleX)
                {
                    middleIndexY++;
                    middleIndexX = firstMiddleX;
                }

                targetX = middleIndexX;
                targetY = middleIndexY;
        } 

        private void RotateDirection()
        {
            switch (direction) {
                case Direction.Up:
                    direction = Direction.Right;
                    break;
                case Direction.Down:
                    direction = Direction.Left;
                    break;
                case Direction.Left:
                    direction = Direction.Up;
                    break;
                case Direction.Right:
                    direction = Direction.Down;
                    break;
            }
        }

        private void InvertDirection()
        {
            switch (direction) {
                case Direction.Up:
                    direction = Direction.Down;
                    break;
                case Direction.Down:
                    direction = Direction.Up;
                    break;
                case Direction.Left:
                    direction = Direction.Right;
                    break;
                case Direction.Right:
                    direction = Direction.Left;
                    break;
            }
        }

        private void MoveDirection()
        {
            switch (direction) {
                case Direction.Up:
                    targetY--;
                    break;
                case Direction.Down:
                    targetY++;
                    break;
                case Direction.Left:
                    targetX--;
                    break;
                case Direction.Right:
                    targetX++;
                    break;
            }
        }
    }
}
