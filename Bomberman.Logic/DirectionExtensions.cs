using System;
using Bomberman.Api;

namespace Bomberman.Logic
{
    public static class DirectionExtensions
    {
        public static Direction Inverse(this Direction direction) =>
            direction switch
            {
                Direction.NONE => Direction.NONE,
                Direction.LEFT => Direction.RIGHT,
                Direction.RIGHT => Direction.LEFT,
                Direction.UP => Direction.DOWN,
                Direction.DOWN => Direction.UP,
                Direction.ACT => throw new ArgumentOutOfRangeException(nameof(direction), direction, null),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
    }
}
