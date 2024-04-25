public partial class AppMain
{
    private class OBS_DRAW_PARAM_3DNN_DRAW_PRIMITIVE
    {
        public readonly AMS_PARAM_DRAW_PRIMITIVE dat = new AMS_PARAM_DRAW_PRIMITIVE();
        public readonly NNS_MATRIX mtx = new NNS_MATRIX();
        public int light;
        public int cull;

        public OBS_DRAW_PARAM_3DNN_DRAW_PRIMITIVE()
        {
        }

        public OBS_DRAW_PARAM_3DNN_DRAW_PRIMITIVE(OBS_DRAW_PARAM_3DNN_DRAW_PRIMITIVE param)
        {
            this.dat.Assign(param.dat);
            this.mtx.Assign(param.mtx);
            this.light = param.light;
            this.cull = param.cull;
        }
    }
}
