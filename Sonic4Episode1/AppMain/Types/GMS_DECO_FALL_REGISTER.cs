public partial class AppMain
{
    public class GMS_DECO_FALL_REGISTER : IClearable
    {
        public VecFx32 vec = new VecFx32();
        public uint num;

        public void Clear()
        {
            this.num = 0U;
            this.vec.Clear();
        }
    }
}
