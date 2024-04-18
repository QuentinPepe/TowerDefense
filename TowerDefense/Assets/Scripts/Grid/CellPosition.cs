namespace Grid
{
    public struct CellPosition
    {
        public int X { get; set; }
        public int Y { get; set; }

        public CellPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static bool operator !=(CellPosition a, CellPosition b)
        {
            return a.X != b.X || a.Y != b.Y;
        }

        public static bool operator ==(CellPosition a, CellPosition b)
        {
            return a.X == b.X && a.Y == b.Y;
        }
    }
}