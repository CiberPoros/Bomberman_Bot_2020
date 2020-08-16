using System.Collections.Generic;
using Bomberman.Api;

namespace Bomberman.Logic.Handlers.DirectionsWeightsHandlers
{
    public class DeepDirectionsWeightsHandler : IDirectionsWeightsHandler
    {
        private const int DeepLimit = 11;
        private const float Smoother = 8f;

        public IDictionary<Direction, float> GetWeights(Board board, BoardState boardState, IEnumerable<Direction> directions, Point startPoint)
        {
            var weights = new Dictionary<Direction, float>();
            float maxWeight;

            var startBomberman = board.GetBomberman();
            var bomberman = startPoint;

            foreach (var startDirection in directions)
            {
                maxWeight = 0f;

                bomberman = startPoint.Shift(startDirection);

                Dfs(startPoint.Shift(startDirection));

                weights.Add(startDirection, maxWeight);
            }

            return weights;

            void Dfs(Point prev, int deep = 1, float weight = 0)
            {
                if (boardState.BombermanState.ImmortalityTime < deep)
                {
                    if (boardState.TimesToBoom[prev.X, prev.Y] == deep)
                        return;
                }

                if (boardState.TimesToGhostMove[prev.X, prev.Y] == deep)
                    return;

                weight += boardState.CanRemoteDamaged[prev.X, prev.Y] ? 0 : boardState.Weights[prev.X, prev.Y] / (deep + Smoother);

                if (deep >= DeepLimit)
                {
                    if (weight >= maxWeight)
                        maxWeight = weight;

                    return;
                }

                foreach (var currentDirection in Utils.DirectionsWithStop)
                {
                    var currentPoint = prev.Shift(currentDirection);

                    if (currentPoint == startBomberman && currentDirection != Direction.NONE)
                        continue;

                    if (currentPoint == bomberman && currentDirection != Direction.NONE)
                        continue;

                    var currentElement = board.GetAt(currentPoint);

                    if (currentElement == Element.WALL 
                        || currentElement == Element.DESTROYABLE_WALL 
                        || (currentElement == Element.OTHER_BOMBERMAN && boardState.IsAfkPlayer(currentPoint)))
                        continue;

                    if (currentElement.IsBomb() && currentElement.GetTimeToBoom() <= deep)
                        continue;

                    Dfs(currentPoint, deep + 1, weight);
                }
            }
        }
    }
}
