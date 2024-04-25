public partial class AppMain
{
    private class AMS_PARAM_DRAW_OBJECT_MATERIAL : AMS_PARAM_DRAW_OBJECT
    {
        public readonly NNS_VECTOR scale = new NNS_VECTOR();
        public NNS_RGBA color;
        public float scroll_u;
        public float scroll_v;
        public int blend;
    }
}
