using Bomberman.Api;

namespace Bomberman.Logic.Handlers
{
    public static class BuffsHandler
    {
        private const int PerkTime = 27;

        public static void Handle(Board board, BoardState boardState)
        {
            if (boardState.PrevState == null)
                return;

            var element = boardState.PrevState.Board.GetAt(board.GetBomberman());

            switch (element)
            {
                case Element.BOMB_BLAST_RADIUS_INCREASE:
                    boardState.BombermanState.IncreaseRadiusTime += PerkTime;
                    break;
                case Element.BOMB_COUNT_INCREASE:
                    boardState.BombermanState.IncreaseBombCountTime += PerkTime;
                    break;
                case Element.BOMB_IMMUNE:
                    boardState.BombermanState.ImmortalityTime += PerkTime;
                    break;
                case Element.BOMB_REMOTE_CONTROL:
                    boardState.BombermanState.RemoteControlBombsCount += 3;
                    boardState.BombermanState.RemoteState = RemoteState.CAN_ACT;
                    break;
                default:
                    return;
            }
        }
    }
}
