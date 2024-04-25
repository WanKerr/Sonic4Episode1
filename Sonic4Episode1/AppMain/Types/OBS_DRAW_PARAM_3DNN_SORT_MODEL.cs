public partial class AppMain
{
    private class OBS_DRAW_PARAM_3DNN_SORT_MODEL
    {
        public readonly AMS_COMMAND_HEADER cmd_header = new AMS_COMMAND_HEADER();
        public readonly AMS_PARAM_SORT_DRAW_OBJECT param = new AMS_PARAM_SORT_DRAW_OBJECT();
        public readonly AMS_DRAWSTATE state = new AMS_DRAWSTATE();
        public OBF_DRAW_USER_FUNC user_func;
        public object user_param;
        public MPP_BOOL_NNSDRAWCALLBACKVAL_OBJECT_DELEGATE material_cb_func;
        public object material_cb_param;
        public uint use_light_flag;
    }
}
