using System.Collections.Generic;
using System.Linq;
using Bomberman.Api;

namespace Bomberman.Logic.Handlers
{
    public static class GhostHandler
    {
        public static IList<Direction> GetSafeDirections(Board board, IEnumerable<Direction> directions)
        {
            var bomberman = board.GetBomberman();

            return directions.Where(direction => Utils.DirectionsWithStop.
                All(nextDirection => board.GetAt(bomberman.
                    Shift(direction).
                    Shift(nextDirection)) != Element.MEAT_CHOPPER)).
                ToList();
        }
    }
}
