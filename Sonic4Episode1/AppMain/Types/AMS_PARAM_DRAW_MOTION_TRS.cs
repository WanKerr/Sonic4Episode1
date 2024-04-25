public partial class AppMain
{
    private class AMS_PARAM_DRAW_MOTION_TRS : IClearable
    {
        public NNS_OBJECT _object;
        public NNS_MATRIX mtx;
        public NNS_TEXLIST texlist;
        public uint sub_obj_type;
        public uint flag;
        public NNS_MATERIALCALLBACK_FUNC material_func;
        public NNS_MOTION motion;
        public float frame;
        public NNS_TRS[] trslist;
        public NNS_MOTION mmotion;
        public float mframe;
        public readonly object holder;

        public void Clear()
        {
            this._object = null;
            this.mtx = null;
            this.texlist = null;
            this.sub_obj_type = 0U;
            this.flag = 0U;
            this.material_func = null;
            this.motion = null;
            this.frame = 0.0f;
            this.trslist = null;
            this.mmotion = null;
            this.mframe = 0.0f;
        }

        public AMS_PARAM_DRAW_MOTION_TRS()
        {
        }

        public AMS_PARAM_DRAW_MOTION_TRS(object _holder)
        {
            this.holder = _holder;
        }

        public static explicit operator OBS_DRAW_PARAM_3DNN_MOTION(
          AMS_PARAM_DRAW_MOTION_TRS param)
        {
            return (OBS_DRAW_PARAM_3DNN_MOTION)param.holder;
        }
    }
}
