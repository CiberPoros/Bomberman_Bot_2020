using System;
using Bomberman.Api;

namespace Bomberman.Logic
{
    public static class ElementsExtensions
    {
        public static bool IsBarrier(this Element element) =>
            element == Element.WALL ||
            element == Element.BOMB_BOMBERMAN ||
            element == Element.BOMB_TIMER_1 ||
            element == Element.BOMB_TIMER_2 ||
            element == Element.BOMB_TIMER_3 ||
            element == Element.BOMB_TIMER_4 ||
            element == Element.BOMB_TIMER_5 ||
            element == Element.OTHER_BOMBERMAN ||
            element == Element.OTHER_BOMB_BOMBERMAN ||
            element == Element.DESTROYABLE_WALL ||
            element == Element.MEAT_CHOPPER;

        public static bool IsBomb(this Element element) =>
            element == Element.BOMB_BOMBERMAN ||
            element == Element.BOMB_TIMER_1 ||
            element == Element.BOMB_TIMER_2 ||
            element == Element.BOMB_TIMER_3 ||
            element == Element.BOMB_TIMER_4 || 
            element == Element.BOMB_TIMER_5 ||
            element == Element.OTHER_BOMB_BOMBERMAN;

        public static bool IsPerk(this Element element) =>
            element == Element.BOMB_BLAST_RADIUS_INCREASE ||
            element == Element.BOMB_COUNT_INCREASE ||
            element == Element.BOMB_IMMUNE ||
            element == Element.BOMB_REMOTE_CONTROL;

        public static bool IsStaticDestroyableElement(this Element element) =>
            element == Element.DESTROYABLE_WALL || element.IsPerk();

        public static int GetTimeToBoom(this Element element) =>
            element switch
            {
                Element.BOMB_BOMBERMAN => 4,
                Element.BOMB_TIMER_1 => 1,
                Element.BOMB_TIMER_2 => 2,
                Element.BOMB_TIMER_3 => 3,
                Element.BOMB_TIMER_4 => 4,
                Element.BOMB_TIMER_5 => 5,
                Element.OTHER_BOMB_BOMBERMAN => 4,
                _ => throw new ArgumentOutOfRangeException(nameof(element), "Out of range.")
            };
    }
}
