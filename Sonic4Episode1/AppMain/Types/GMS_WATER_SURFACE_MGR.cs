public partial class AppMain
{
    public class GMS_WATER_SURFACE_MGR
    {
        public MTS_TASK_TCB tcb_water;
        public AMS_RENDER_TARGET render_target;

        internal void Clear()
        {
            this.tcb_water = null;
            this.render_target = null;
        }
    }
}
