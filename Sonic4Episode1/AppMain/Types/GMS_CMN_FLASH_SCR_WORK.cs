public partial class AppMain
{
    public class GMS_CMN_FLASH_SCR_WORK : IClearable
    {
        public GMS_FADE_OBJ_WORK fade_obj_work;
        public uint active_flag;
        public float duration_frame;
        public float fi_frame;
        public float duration_timer;

        public void Clear()
        {
            this.fade_obj_work = null;
            this.active_flag = 0U;
            this.duration_frame = this.fi_frame = this.duration_timer = 0.0f;
        }
    }
}
