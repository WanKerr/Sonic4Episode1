public partial class AppMain
{
    public class GMS_COCKPIT_3DNN_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_COCKPIT_COM_WORK cpit_com = new GMS_COCKPIT_COM_WORK();
        public readonly OBS_ACTION3D_NN_WORK obj_3d = new OBS_ACTION3D_NN_WORK();

        public OBS_OBJECT_WORK Cast()
        {
            return this.cpit_com.obj_work;
        }
    }
}
