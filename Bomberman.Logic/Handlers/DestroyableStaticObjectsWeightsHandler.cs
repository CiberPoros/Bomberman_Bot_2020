using System;
using Bomberman.Api;

namespace Bomberman.Logic.Handlers
{
    public static class DestroyableStaticObjectsWeightsHandler
    {
        private const int BlastRadius = 3;
        private const float DestroyableWallWeight = 100f;
        private const float AfkPlayerWeight = 1500f;

        public static float[,] Handle(Board board, BoardState boardState, float[,] weights)
        {
            var size = board.Size;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    var point = new Point(i, j);

                    if (boardState.TimesToBoom[point.X, point.Y] != int.MaxValue)
                        continue;

                    var element = board.GetAt(point);
                    if (!(element == Element.DESTROYABLE_WALL || boardState.IsAfkPlayer(point)))
                        continue;

                    foreach (var direction in Utils.Directions)
                    {
                        var currentPoint = point;
                        for (var k = 0; k < BlastRadius; k++)
                        {
                            currentPoint = currentPoint.Shift(direction);
                            var currentElement = board.GetAt(currentPoint);

                            if (currentElement == Element.WALL || currentElement == Element.DESTROYABLE_WALL)
                                break;

                            weights[currentPoint.X, currentPoint.Y] += GetWeight(element);
                        }
                    }
                }
            }

            return weights;
        }

        private static float GetWeight(Element element) =>
            element switch
            {
                Element.DESTROYABLE_WALL => DestroyableWallWeight,
                Element.OTHER_BOMBERMAN => AfkPlayerWeight,
                _ => throw new ArgumentOutOfRangeException(nameof(element), element, null)
            };
    }
}
