namespace Bomberman.Api
{
    public readonly struct Point
    {
        public readonly int X;
        public readonly int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool IsOutOf(int size) => X >= size || Y >= size || X < 0 || Y < 0;

        public Point ShiftLeft(int delta = 1) => new Point(X - delta, Y);
        public Point ShiftRight(int delta = 1) => new Point(X + delta, Y);
        public Point ShiftTop(int delta = 1) => new Point(X, Y + delta);
        public Point ShiftBottom(int delta = 1) => new Point(X, Y - delta);

        public static bool operator ==(Point p1, Point p2) => p1.X == p2.X && p1.Y == p2.Y;

        public static bool operator !=(Point p1, Point p2) => !(p1 == p2);

        public override string ToString() => $"[{X},{Y}]";

        public override bool Equals(object obj)
        {
            if (obj == null) 
                return false;

            if (!(obj is Point otherPoint)) 
                return false;

            return otherPoint == this;
        }

        public override int GetHashCode() => (X + 63) ^ Y;
    }
}
