public partial class AppMain
{
    public class GMS_MAP_SYS_WORK
    {
        public GMS_MAP_OTHER_MAP_STATE[] map_state = New<GMS_MAP_OTHER_MAP_STATE>(5);
        public float[] main_cam_user_disp = new float[2];
        public float[] main_cam_user_target = new float[2];
        public float[] main_cam_user_ofst = new float[2];
        public uint flag;
        public bool auto_resize;
    }
}
