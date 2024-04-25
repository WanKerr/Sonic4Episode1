public partial class AppMain
{
    public class DI_BLOCK
    {
        public byte[][] di;

        public DI_BLOCK()
        {
            this.di = new byte[8][];
            for (int index = 0; index < 8; ++index)
                this.di[index] = new byte[8];
        }
    }
}
