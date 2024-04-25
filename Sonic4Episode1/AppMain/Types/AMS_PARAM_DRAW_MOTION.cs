public partial class AppMain
{
    private class AMS_PARAM_DRAW_MOTION
    {
        public NNS_OBJECT _object;
        public NNS_MATRIX mtx;
        public NNS_TEXLIST texlist;
        public uint sub_obj_type;
        public uint flag;
        public NNS_MATERIALCALLBACK_FUNC material_func;
        public NNS_MOTION motion;
        public float frame;
        public readonly object holder;

        public AMS_PARAM_DRAW_MOTION()
        {
        }

        public AMS_PARAM_DRAW_MOTION(object _holder)
        {
            this.holder = _holder;
        }

        public static explicit operator OBS_DRAW_PARAM_3DNN_DRAW_MOTION(
          AMS_PARAM_DRAW_MOTION mtn)
        {
            return (OBS_DRAW_PARAM_3DNN_DRAW_MOTION)mtn.holder;
        }
    }
}
