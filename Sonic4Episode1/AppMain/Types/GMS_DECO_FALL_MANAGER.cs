public partial class AppMain
{
    public class GMS_DECO_FALL_MANAGER : IClearable
    {
        public GMS_DECO_FALL_REGISTER[] reg = New<GMS_DECO_FALL_REGISTER>(8);
        public uint dec_id;
        public NNS_TEXLIST texlist;
        public ushort all_num;
        public ushort reg_num;
        public float frame;

        public void Clear()
        {
            this.dec_id = 0U;
            this.texlist = null;
            this.all_num = this.reg_num = 0;
            this.frame = 0.0f;
            ClearArray(this.reg);
        }
    }
}
