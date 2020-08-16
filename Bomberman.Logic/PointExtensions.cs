using System;
using Bomberman.Api;

namespace Bomberman.Logic
{
    public static class PointExtensions
    {
        public static Point Shift(this Point point, Direction direction) =>
            direction switch
            {
                Direction.LEFT => point.ShiftLeft(),
                Direction.RIGHT => point.ShiftRight(),
                Direction.UP => point.ShiftTop(),
                Direction.DOWN => point.ShiftBottom(),
                Direction.ACT => throw new ArgumentOutOfRangeException(nameof(direction), direction, null),
                Direction.NONE => point,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
    }
}
