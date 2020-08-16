using Bomberman.Api;

namespace Bomberman.Logic.Handlers
{
    public static class AfkPlayersHandler
    {
        public static void UpdateAfkTimes(Board board, BoardState boardState)
        {
            if (boardState.PrevState is null)
                return;

            var size = board.Size;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    var element = board.GetAt(new Point(i, j));

                    if (element != Element.OTHER_BOMBERMAN)
                    {
                        boardState.AfkTimes[i, j] = 0;
                        continue;
                    }

                    boardState.AfkTimes[i, j] = boardState.PrevState.AfkTimes[i, j] + 1;
                }
            }
        }
    }
}
