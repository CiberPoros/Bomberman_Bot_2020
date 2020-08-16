using Bomberman.Api;

namespace Bomberman.Logic.Handlers
{
    public static class BuffsWeightsHandler
    {
        private const float BuffWeight = 800f;
        private const bool SkipBlastRadius = false;

        public static void UpdateWeights(Board board, BoardState boardState)
        {
            var buffs = board.GetPerks();

            foreach (var buff in buffs)
            {
                if (SkipBlastRadius)
                {
                    var element = board.GetAt(buff);

                    if (element == Element.BOMB_BLAST_RADIUS_INCREASE)
                        continue;
                }


                boardState.Weights[buff.X, buff.Y] += BuffWeight;
            }
        }
    }
}
