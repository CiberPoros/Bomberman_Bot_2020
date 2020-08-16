using Bomberman.Api;

namespace Bomberman.Logic.Handlers
{
    public static class IncreasedRadiusHandler
    {
        public static bool[,] IsIncreased(Board board, BoardState boardState)
        {
            var size = board.Size;

            var result = new bool[size, size];

            if (boardState.PrevState == null)
                return result;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    var point = new Point(i, j);
                    if (boardState.PrevState.IsIncreasedBlast[i, j] && board.GetAt(point).IsBomb())
                        result[i, j] = true;
                }
            }

            return result;
        }
    }
}
