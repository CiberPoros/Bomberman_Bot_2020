using Bomberman.Api;

namespace Bomberman.Logic.Handlers
{
    public static class TimesToBoomHandler
    {
        private const int BlastRadius = 3;

        public static int[,] GetTimesToBoom(Board board, BoardState boardState)
        {
            var size = board.Size;
            var times = new int[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    times[i, j] = int.MaxValue;
                }
            }

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    var point = new Point(i, j);
                    var element = board.GetAt(point);

                    if (!element.IsBomb())
                        continue;

                    HandleAroundBomb(board, boardState, point, times);
                }
            }

            return times;
        }

        private static void HandleAroundBomb(Board board, BoardState boardState, Point bombPoint, int[,] times)
        {
            int timeToBoom = board.GetAt(bombPoint).GetTimeToBoom();

            if (times[bombPoint.X, bombPoint.Y] > timeToBoom)
                times[bombPoint.X, bombPoint.Y] = timeToBoom;

            foreach (var direction in Utils.Directions)
            {
                var currentPoint = bombPoint;

                var blastRadius = BlastRadius;
                if (boardState.IsIncreasedBlast[bombPoint.X, bombPoint.Y])
                    blastRadius += 2;

                for (int i = 0; i < blastRadius; i++)
                {
                    currentPoint = currentPoint.Shift(direction);
                    var currentElement = board.GetAt(currentPoint);

                    if (currentElement == Element.WALL)
                        break;

                    if (currentElement.IsStaticDestroyableElement())
                    {
                        if (times[currentPoint.X, currentPoint.Y] > timeToBoom)
                        {
                            times[currentPoint.X, currentPoint.Y] = timeToBoom;
                            break;
                        }
                    }

                    if (times[currentPoint.X, currentPoint.Y] > timeToBoom)
                        times[currentPoint.X, currentPoint.Y] = timeToBoom;
                }
            }
        }
    }
}
