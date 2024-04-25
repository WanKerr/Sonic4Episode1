public partial class AppMain
{
    public class DMS_TITLEOP_ROCK_SETTING
    {
        public VecFx32 pos = new VecFx32();
        public VecFx32 scale = new VecFx32();

        public DMS_TITLEOP_ROCK_SETTING(uint x1, uint y1, uint z1, uint x2, uint y2, uint z2)
        {
            this.pos = new VecFx32((int)x1, (int)y1, (int)z1);
            this.scale = new VecFx32((int)x2, (int)y2, (int)z2);
        }
    }
}
