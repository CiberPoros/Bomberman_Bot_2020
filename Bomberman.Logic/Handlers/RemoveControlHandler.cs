using Bomberman.Api;

namespace Bomberman.Logic.Handlers
{
    public static class RemoveControlHandler
    {
        public static bool[,] CanDamaged(Board board, BoardState boardState)
        {
            var size = board.Size;
            var result = new bool[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    var point = new Point(i, j);
                    var element = board.GetAt(point);

                    if (element == Element.BOMB_TIMER_5 || (element == Element.BOMB_BOMBERMAN && boardState.BombermanState.RemoteState != RemoteState.NONE))
                    {
                        foreach (var direction in Utils.Directions)
                        {
                            var currentPoint = point;
                            result[point.X, point.Y] = true;

                            var radius = 3;
                            if (boardState.IsIncreasedBlast[point.X, point.Y])
                                radius += 2;

                            for (int k = 0; k < radius; k++)
                            {
                                currentPoint = currentPoint.Shift(direction);
                                var currentElement = board.GetAt(currentPoint);

                                if (currentElement == Element.WALL || currentElement == Element.DESTROYABLE_WALL)
                                    break;

                                result[currentPoint.X, currentPoint.Y] = true;
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}
