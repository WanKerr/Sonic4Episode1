public partial class AppMain
{
    private struct GMS_GMK_CAPSULE_ANIMAL_SET_PARAM
    {
        public short ofs_x;
        public short ofs_y;
        public short ofs_z;
        public byte type;
        public byte vec;
        public ushort time;

        public GMS_GMK_CAPSULE_ANIMAL_SET_PARAM(
          short ofx,
          short ofy,
          short ofz,
          byte tp,
          byte vc,
          ushort tm)
        {
            this.ofs_x = ofx;
            this.ofs_y = ofy;
            this.ofs_z = ofz;
            this.type = tp;
            this.vec = vc;
            this.time = tm;
        }
    }
}
