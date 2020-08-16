using System.Collections.Generic;
using Bomberman.Api;

namespace Bomberman.Logic.Handlers
{
    public static class AngryGhostHandler
    {
        public static IList<Direction> GetSafeDirections(Board board, BoardState boardState, IEnumerable<Direction> directions)
        {
            var bomberman = board.GetBomberman();
            var result = new List<Direction>();

            foreach (var direction in directions)
            {
                var point = bomberman.Shift(direction);

                bool needAdd = true;

                foreach (var nextDirection in Utils.DirectionsWithStop)
                {
                    var nextPoint = point.Shift(nextDirection);
                    var element = board.GetAt(nextPoint);

                    if (element == Element.DEAD_MEAT_CHOPPER)
                    {
                        foreach (var nextNextDirection in Utils.Directions)
                        {
                            var nextNextPoint = nextPoint.Shift(nextNextDirection);

                            if (boardState.PrevState == null)
                                break;

                            var nextElement = boardState.PrevState.Board.GetAt(nextNextPoint);

                            if (nextElement == Element.DEAD_MEAT_CHOPPER)
                            {
                                needAdd = false;
                                break;
                            }
                        }
                    }
                }

                if (needAdd)
                    result.Add(direction);
            }

            return result;
        }
    }
}
