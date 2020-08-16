using System;
using System.Collections.Generic;
using System.Linq;

namespace Bomberman.Api
{
    public class Board
    {
        private string BoardString { get; }
        private readonly LengthToXy _lengthXy;

        public Board(string boardString)
        {
            BoardString = boardString.Replace("\n", "");
            _lengthXy = new LengthToXy(Size);
        }

        public int Size => (int)Math.Sqrt(BoardString.Length);

        public Point GetBomberman() =>
            Get(Element.BOMBERMAN)
                .Concat(Get(Element.BOMB_BOMBERMAN))
                .Concat(Get(Element.DEAD_BOMBERMAN))
                .Single();

        public List<Point> GetOtherBombermans() =>
            Get(Element.OTHER_BOMBERMAN)
                .Concat(Get(Element.OTHER_BOMB_BOMBERMAN))
                .Concat(Get(Element.OTHER_DEAD_BOMBERMAN))
                .ToList();

        public bool IsMyBombermanDead => BoardString.Contains((char)Element.DEAD_BOMBERMAN);

        public Element GetAt(Point point)
        {
            if (point.IsOutOf(Size))
            {
                return Element.WALL;
            }

            return (Element)BoardString[_lengthXy.GetLength(point.X, point.Y)];
        }

        public bool IsAt(Point point, Element element)
        {
            if (point.IsOutOf(Size))
            {
                return false;
            }

            return GetAt(point) == element;
        }

        public string BoardAsString()
        {
            string result = string.Empty;

            for (int i = 0; i < Size; i++)
            {
                result += BoardString.Substring(i * Size, Size);
                result += "\n";
            }

            return result;
        }

        public List<Point> GetMeatChoppers() => Get(Element.MEAT_CHOPPER);

        public List<Point> GetBarrier() =>
            GetMeatChoppers()
                .Concat(GetWalls())
                .Concat(GetBombs())
                .Concat(GetDestroyableWalls())
                .Concat(GetOtherBombermans())
                .Distinct()
                .ToList();

        public List<Point> Get(Element element)
        {
            List<Point> result = new List<Point>();

            for (int i = 0; i < Size * Size; i++)
            {
                Point pt = _lengthXy.GetXy(i);

                if (IsAt(pt, element))
                {
                    result.Add(pt);
                }
            }

            return result;
        }

        public List<Point> GetWalls() => Get(Element.WALL);

        public List<Point> GetDestroyableWalls() => Get(Element.DESTROYABLE_WALL);

        public List<Point> GetBombs() =>
            Get(Element.BOMB_TIMER_1)
                .Concat(Get(Element.BOMB_TIMER_2))
                .Concat(Get(Element.BOMB_TIMER_3))
                .Concat(Get(Element.BOMB_TIMER_4))
                .Concat(Get(Element.BOMB_TIMER_5))
                .Concat(Get(Element.BOMB_BOMBERMAN))
                .Concat(Get(Element.OTHER_BOMB_BOMBERMAN))
                .ToList();

        public List<Point> GetPerks() =>
            Get(Element.BOMB_BLAST_RADIUS_INCREASE)
                .Concat(Get(Element.BOMB_COUNT_INCREASE))
                .Concat(Get(Element.BOMB_IMMUNE))
                .Concat(Get(Element.BOMB_REMOTE_CONTROL))
                .ToList();

        public List<Point> GetBlasts() => Get(Element.BOOM);

        public List<Point> GetFutureBlasts()
        {
            var bombs = GetBombs();
            var result = new List<Point>();
            foreach (var bomb in bombs)
            {
                result.Add(bomb);
                result.Add(bomb.ShiftLeft());
                result.Add(bomb.ShiftRight());
                result.Add(bomb.ShiftTop());
                result.Add(bomb.ShiftBottom());
            }

            return result.Where(blast => !blast.IsOutOf(Size) && !GetWalls().Contains(blast)).Distinct().ToList();
        }

        public bool IsAnyOfAt(Point point, params Element[] elements) => elements.Any(elem => IsAt(point, elem));

        public bool IsNear(Point point, Element element)
        {
            if (point.IsOutOf(Size))
                return false;

            return IsAt(point.ShiftLeft(), element) ||
                   IsAt(point.ShiftRight(), element) ||
                   IsAt(point.ShiftTop(), element) ||
                   IsAt(point.ShiftBottom(), element);
        }

        public bool IsBarrierAt(Point point) => GetBarrier().Contains(point);

        public int CountNear(Point point, Element element)
        {
            if (point.IsOutOf(Size))
                return 0;

            int count = 0;
            if (IsAt(point.ShiftLeft(), element)) count++;
            if (IsAt(point.ShiftRight(), element)) count++;
            if (IsAt(point.ShiftTop(), element)) count++;
            if (IsAt(point.ShiftBottom(), element)) count++;
            return count;
        }

        public override string ToString() =>
            $"{BoardAsString()}\n" + $"Bomberman at: {GetBomberman()}\n" +
            $"Other bombermans at: {ListToString(GetOtherBombermans())}\n" +
            $"Meat choppers at: {ListToString(GetMeatChoppers())}\n" +
            $"Destroy walls at: {ListToString(GetDestroyableWalls())}\n" +
            $"Bombs at: {ListToString(GetBombs())}\n" + $"Blasts: {ListToString(GetBlasts())}\n" +
            $"Expected blasts at: {ListToString(GetFutureBlasts())}\n" + $"Perks at: {ListToString(GetPerks())}";

        private static string ListToString(List<Point> list) => string.Join(",", list.ToArray());
    }
}
