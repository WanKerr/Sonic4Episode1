public partial class AppMain
{
    private class OBS_DRAW_PARAM_3DNN_MODEL
    {
        public readonly NNS_MATRIX mtx = new NNS_MATRIX();
        public readonly AMS_DRAWSTATE draw_state = new AMS_DRAWSTATE();
        public readonly AMS_PARAM_DRAW_OBJECT param;
        public AMS_DRAWSTATE state;
        public MPP_VOID_OBJECT_DELEGATE user_func;
        public object user_param;
        public MPP_BOOL_NNSDRAWCALLBACKVAL_OBJECT_DELEGATE material_cb_func;
        public object material_cb_param;
        public uint use_light_flag;

        public OBS_DRAW_PARAM_3DNN_MODEL()
        {
            this.param = new AMS_PARAM_DRAW_OBJECT(this);
        }
    }
}
