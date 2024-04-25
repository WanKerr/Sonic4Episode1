public partial class AppMain
{
    public class DF_BLOCK
    {
        public readonly DF_CELL[,] df = new DF_CELL[8, 8];

        public struct DF_CELL
        {
            public byte[] Data;
            public int Offset;
        }
    }
}
