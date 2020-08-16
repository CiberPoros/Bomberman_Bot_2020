using Bomberman.Api;

namespace Bomberman.Logic.Handlers
{
    public static class GhostPositionsHandler
    {
        public static int[,] GetTimeToGhostMove(Board board, BoardState boardState)
        {
            var size = board.Size;

            var timesToGhostMove = new int[size, size];

            if (boardState.PrevState == null)
                return timesToGhostMove;

            var ghosts = board.GetMeatChoppers();
            var prevBoard = boardState.PrevState.Board;

            foreach (var ghost in ghosts)
            {
                foreach (var direction in Utils.Directions)
                {
                    var prevPosition = ghost.Shift(direction);

                    if (prevBoard.GetAt(prevPosition) == Element.MEAT_CHOPPER)
                    {
                        HandleGhost(ghost, direction.Inverse(), timesToGhostMove, board, boardState);
                    }
                }
            }

            return timesToGhostMove;
        }

        private static void HandleGhost(Point position, Direction direction, int[,] timesToGhostMove, Board board, BoardState boardState)
        {
            for (int tickNumber = 1;; tickNumber++)
            {
                position = position.Shift(direction);
                var element = board.GetAt(position);

                if (element == Element.DESTROYABLE_WALL || element == Element.WALL)
                    return;

                if (boardState.TimesToBoom[position.X, position.Y] == tickNumber)
                    return;

                timesToGhostMove[position.X, position.Y] = tickNumber;
            }
        }
    }
}
