public partial class AppMain
{
    public class GMS_DECOGLARE_PARAM
    {
        public readonly uint color;
        public float size;
        public float sort_z;
        public int ablend;

        public GMS_DECOGLARE_PARAM(uint rgba, float Size, float Sort, int Ablend)
        {
            this.color = rgba;
            this.size = Size;
            this.sort_z = Sort;
            this.ablend = Ablend;
        }
    }
}
