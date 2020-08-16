using Bomberman.Api;

namespace Bomberman.Logic
{
    public static class Utils
    {
        public static readonly Direction[] Directions = new Direction[] {Direction.DOWN, Direction.LEFT,Direction.RIGHT, Direction.UP};
        public static readonly Direction[] DirectionsWithStop = new Direction[] { Direction.NONE, Direction.DOWN, Direction.LEFT, Direction.RIGHT, Direction.UP };
    }
}
