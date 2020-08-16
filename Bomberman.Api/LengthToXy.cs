namespace Bomberman.Api
{
    public class LengthToXy
    {
        public int Size;

        public LengthToXy(int size) => Size = size;

        public int GetLength(int x, int y)
        {
            int xx = InversionX(x);
            int yy = InversionY(y);
            return yy * Size + xx;
        }

        public Point GetXy(int length)
        {
            int x = InversionX(length % Size);
            int y = InversionY(length / Size);
            return new Point(x, y);
        }

        private static int InversionX(int x) => x;

        private int InversionY(int y) => Size - 1 - y;
    }
}
