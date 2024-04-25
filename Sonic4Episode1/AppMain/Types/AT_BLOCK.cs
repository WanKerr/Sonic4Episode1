public partial class AppMain
{
    public class AT_BLOCK
    {
        public byte[][] at;

        public AT_BLOCK()
        {
            this.at = new byte[8][];
            for (int index = 0; index < 8; ++index)
                this.at[index] = new byte[8];
        }
    }
}
