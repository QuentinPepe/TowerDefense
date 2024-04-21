namespace Grid
{
    public struct CellPosition
    {
        public int X { get; set; }
        public int Z { get; set; }

        public CellPosition(int x, int z)
        {
            X = x;
            Z = z;
        }

        public static bool operator !=(CellPosition a, CellPosition b)
        {
            return a.X != b.X || a.Z != b.Z;
        }

        public static bool operator ==(CellPosition a, CellPosition b)
        {
            return a.X == b.X && a.Z == b.Z;
        }
    }
}