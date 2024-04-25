public partial class AppMain
{
    private class AMS_PARAM_DRAW_OBJECT
    {
        public NNS_OBJECT _object;
        public NNS_MATRIX mtx;
        public NNS_TEXLIST texlist;
        public uint sub_obj_type;
        public uint flag;
        public NNS_MATERIALCALLBACK_FUNC material_func;
        public float scaleZ;
        public readonly object holder;

        public AMS_PARAM_DRAW_OBJECT()
        {
        }

        public AMS_PARAM_DRAW_OBJECT(object _holder)
        {
            this.holder = _holder;
        }

        public static explicit operator OBS_DRAW_PARAM_3DNN_MODEL(
          AMS_PARAM_DRAW_OBJECT ob)
        {
            return (OBS_DRAW_PARAM_3DNN_MODEL)ob.holder;
        }
    }
}
