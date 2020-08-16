using System;
using Bomberman.Api;

namespace Bomberman.Logic.Handlers.ActHandlers
{
    public interface IActHandler
    {
        public ActType GetActType(Board board, BoardState boardState, Direction direction);

        public static string AddActToDirection(string directionString, ActType actType) =>
            actType switch
            {
                ActType.NONE => directionString,
                ActType.ACT_BEFORE => "ACT " + directionString,
                ActType.ACT_AFTER => directionString + " ACT",
                _ => throw new ArgumentOutOfRangeException(nameof(actType), actType, null)
            };

        public static string AddActToDirection(Direction direction, ActType actType) =>
            actType switch
            {
                ActType.NONE => direction.ToString(),
                ActType.ACT_BEFORE => "ACT " + direction.ToString(),
                ActType.ACT_AFTER => direction.ToString() + " ACT",
                _ => throw new ArgumentOutOfRangeException(nameof(actType), actType, null)
            };
    }
}
