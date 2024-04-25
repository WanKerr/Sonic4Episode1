public partial class AppMain
{
    public class GMS_START_DEMO_WORK
    {
        public GMS_COCKPIT_2D_WORK[] action_obj_work_cmn = new GMS_COCKPIT_2D_WORK[4];
        public GMS_COCKPIT_2D_WORK[] action_obj_work_zone = new GMS_COCKPIT_2D_WORK[1];
        public GMS_COCKPIT_2D_WORK[] action_obj_work_act = new GMS_COCKPIT_2D_WORK[2];
        public uint counter;
        public uint flag;
        public GMS_COCKPIT_2D_WORK action_obj_work_message;
        public _update_ update;

        public delegate void _update_(GMS_START_DEMO_WORK cont);
    }
}
