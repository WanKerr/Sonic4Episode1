public partial class AppMain
{
    public class GMS_DECO_MGR : IClearable
    {
        public int[] common_frame_motion = new int[3];
        public int[] motion_frame_loop = new int[12];
        public MTS_TASK_TCB tcb_post;
        public bool flag_render_front;
        public bool flag_render_back;
        public AMS_RENDER_TARGET render_target_front;
        public AMS_RENDER_TARGET render_target_back;
        public int state_loop;
        public GSS_SND_SE_HANDLE se_handle;

        public void Clear()
        {
            this.tcb_post = null;
            this.flag_render_front = this.flag_render_back = false;
            this.render_target_front = this.render_target_back = null;
            this.state_loop = 0;
            this.se_handle = null;
            for (int index = 0; index < this.motion_frame_loop.Length; ++index)
                this.motion_frame_loop[index] = 0;
        }
    }
}
