public partial class AppMain
{
    public struct MD_BLOCK
    {
        public sbyte ofst_x;
        public sbyte ofst_y;

        public MD_BLOCK(sbyte bitFieldValue)
        {
            int num = bitFieldValue & 15;
            this.ofst_x = num >= 8 ? (sbyte)(num - 16) : (sbyte)num;
            this.ofst_y = (sbyte)(bitFieldValue >> 4);
        }
    }
}
