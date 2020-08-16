using System.Collections.Generic;
using Bomberman.Api;

namespace Bomberman.Logic.Handlers.ActHandlers
{
    public class AllPathsActHandler : IActHandler
    {
        private const bool CheckDamageBuff = true;
        private const float WeightBooster = 0.5f;
        private const bool ActAfterOnly = true;

        // Огромная куча костылей
        // Бомбы сейчас ставятся только после хода или не ставятся вообще
        // TODO: fix it
        public ActType GetActType(Board board, BoardState boardState, Direction bombermanDirection)
        {
            if (board.IsMyBombermanDead)
                return ActType.NONE;

            if (!boardState.BombermanState.CanPlant)
                return ActType.NONE;

            var bomberman = boardState.BombermanState.Position;
            var nextBombermanPosition = bomberman.Shift(bombermanDirection);
            var size = board.Size;

            if (boardState.BombermanState.RemoteState == RemoteState.CAN_USE)
            {
                if (!boardState.CanRemoteDamaged[bomberman.X, bomberman.Y] && !ActAfterOnly)
                    return ActType.ACT_BEFORE;
                else if (!boardState.CanRemoteDamaged[nextBombermanPosition.X, nextBombermanPosition.Y])
                    return ActType.ACT_AFTER;
                else
                    return ActType.NONE;
            }

            int[,] distance = new int[size, size];
            Queue<Point> q = new Queue<Point>();

            q.Enqueue(bomberman);
            distance[bomberman.X, bomberman.Y] = 1;

            var actType = ActType.NONE;
            var currentWeight = -1f;

            if (!ActAfterOnly)
            {
                if (board.GetAt(bomberman) != Element.BOMB_BOMBERMAN && !CheckDamageBuff || (!boardState.CanDamageBuff[bomberman.X, bomberman.Y] && board.GetAt(bomberman) != Element.BOMB_BOMBERMAN))
                {
                    if (boardState.Weights[bomberman.X, bomberman.Y] > currentWeight)
                    {
                        currentWeight = boardState.Weights[bomberman.X, bomberman.Y];
                        actType = ActType.ACT_BEFORE;
                    }
                }
            }

            if (board.GetAt(nextBombermanPosition) != Element.BOMB_REMOTE_CONTROL && (!CheckDamageBuff || (!boardState.CanDamageBuff[nextBombermanPosition.X, nextBombermanPosition.Y] && !board.GetAt(nextBombermanPosition).IsPerk())))
            {
                if (boardState.Weights[nextBombermanPosition.X, nextBombermanPosition.Y] > currentWeight)
                {
                    currentWeight = boardState.Weights[nextBombermanPosition.X, nextBombermanPosition.Y];
                    actType = ActType.ACT_AFTER;
                }
            }

            if (boardState.BombermanState.IncreaseBombCountTime > 0)
                return actType;

            while (q.TryDequeue(out var point))
            {
                if (point != bomberman && point != nextBombermanPosition)
                {
                    var weight = boardState.Weights[point.X, point.Y];
                    weight /= 1 + distance[point.X, point.Y] * WeightBooster;

                    if (weight > currentWeight)
                        return ActType.NONE;
                }

                if (distance[point.X, point.Y] > 4)
                    continue;

                foreach (var direction in Utils.Directions)
                {
                    var nextPoint = point.Shift(direction);

                    if (distance[nextPoint.X, nextPoint.Y] != 0)
                        continue;

                    if (boardState.TimesToBoom[nextPoint.X, nextPoint.Y] == distance[point.X, point.Y])
                        continue;

                    var nextElement = board.GetAt(nextPoint);

                    if (nextElement == Element.WALL || nextElement == Element.DESTROYABLE_WALL)
                        continue;

                    distance[nextPoint.X, nextPoint.Y] = distance[point.X, point.Y] + 1;
                    q.Enqueue(nextPoint);
                }
            }

            boardState.BombermanState.TimeToPlant = 5;

            return actType;
        }
    }
}
