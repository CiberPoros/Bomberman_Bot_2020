using Bomberman.Api;

namespace Bomberman.Logic.Handlers
{
    public static class CanDamageBuffHandler
    {
        public static bool[,] GetCanDamageBuffMass(Board board, BoardState boardState)
        {
            var size = board.Size;

            var result = new bool[size, size];

            var buffs = board.GetPerks();

            foreach (var buff in buffs)
            {
                if (boardState.TimesToBoom[buff.X, buff.Y] <= 4) // will be destroyed
                    continue;

                foreach (var direction in Utils.Directions)
                {
                    var point = buff;

                    for (int i = 0; i < 3; i++)
                    {
                        point = point.Shift(direction);

                        var element = board.GetAt(point);

                        if (element == Element.WALL || element == Element.DESTROYABLE_WALL)
                            break;

                        result[point.X, point.Y] = true;
                    }
                }
            }

            return result;
        }
    }
}
