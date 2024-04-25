public partial class AppMain
{
    public class GMS_MAP_FAR_CAMERA
    {
        public int camera_id;
        public int camera_type;
        public float camera_speed_x;
        public float camera_speed_y;

        internal void Clear()
        {
            this.camera_id = 0;
            this.camera_type = 0;
            this.camera_speed_x = 0.0f;
            this.camera_speed_y = 0.0f;
        }
    }
}
