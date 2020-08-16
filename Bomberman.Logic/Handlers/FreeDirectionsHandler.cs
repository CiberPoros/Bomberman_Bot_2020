using System.Collections.Generic;
using System.Linq;
using Bomberman.Api;

namespace Bomberman.Logic.Handlers
{
    public static class FreeDirectionsHandler
    {
        public static IList<Direction> GetFreeDirections(Board board, IEnumerable<Direction> directions)
        {
            var freeDirections = new List<Direction>(5);
            var bomberman = board.GetBomberman();

            freeDirections.AddRange(directions.Where(direction => !board.GetAt(bomberman.Shift(direction)).IsBarrier()));

            if (board.GetAt(board.GetBomberman()) == Element.BOMB_BOMBERMAN)
                freeDirections.Remove(Direction.NONE);

            return freeDirections;
        }
    }
}
