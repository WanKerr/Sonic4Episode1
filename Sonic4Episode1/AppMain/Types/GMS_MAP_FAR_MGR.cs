public partial class AppMain
{
    public class GMS_MAP_FAR_MGR
    {
        public readonly GMS_MAP_FAR_CAMERA camera = new GMS_MAP_FAR_CAMERA();
        public MTS_TASK_TCB tcb_pre_draw;
        public MTS_TASK_TCB tcb_draw;
        public MTS_TASK_TCB tcb_post_draw;

        internal void Clear()
        {
            this.tcb_pre_draw = null;
            this.tcb_draw = null;
            this.tcb_post_draw = null;
            this.camera.Clear();
        }
    }
}
