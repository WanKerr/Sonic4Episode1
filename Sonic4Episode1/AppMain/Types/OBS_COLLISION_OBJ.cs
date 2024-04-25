public partial class AppMain
{
    public class OBS_COLLISION_OBJ
    {
        public VecFx32 pos = new VecFx32();
        public VecFx32 check_pos = new VecFx32();
        public OBS_OBJECT_WORK obj;
        public OBS_OBJECT_WORK rider_obj;
        public OBS_OBJECT_WORK toucher_obj;
        public short ofst_x;
        public short ofst_y;
        public uint flag;
        public ushort dir;
        public ushort attr;
        public byte[] diff_data;
        public byte[] dir_data;
        public byte[] attr_data;
        public ushort width;
        public ushort height;
        public short check_ofst_x;
        public short check_ofst_y;
        public int left;
        public int top;
        public int right;
        public int bottom;
        public ushort check_dir;
    }
}
