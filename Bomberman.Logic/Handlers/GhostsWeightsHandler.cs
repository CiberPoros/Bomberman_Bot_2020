using Bomberman.Api;

namespace Bomberman.Logic.Handlers
{
    public static class GhostsWeightsHandler
    {
        private const int GhostWeight = 120;
        private const int Deep = 12;

        public static int[,,] GetGhostsWeights (Board board, BoardState boardState)
        {
            var size = board.Size;
            int[,,] result = new int[size, size, Deep];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    var time = boardState.TimesToGhostMove[i, j];

                    if (time == 0)
                        continue;

                    if (time >= Deep)
                        continue;

                    foreach (var direction in Utils.Directions)
                    {
                        var point = new Point(i, j);
                        for (int k = 0; k < 3; k++)
                        {
                            point = point.Shift(direction);

                            var element = board.GetAt(point);

                            if (element == Element.WALL || element == Element.DESTROYABLE_WALL)
                                break;

                            result[i, j, time] += GhostWeight;
                        }
                    }
                }
            }

            return result;
        }
    }
}
