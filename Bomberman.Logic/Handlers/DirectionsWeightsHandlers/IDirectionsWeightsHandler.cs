using System.Collections.Generic;
using Bomberman.Api;

namespace Bomberman.Logic.Handlers.DirectionsWeightsHandlers
{
    public interface IDirectionsWeightsHandler
    {
        public IDictionary<Direction, float> GetWeights(Board board, BoardState boardState, IEnumerable<Direction> directions, Point startPoint);
    }
}
