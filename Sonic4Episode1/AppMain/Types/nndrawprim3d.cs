public partial class AppMain
{
    private static class nndrawprim3d
    {
        public static readonly float[] nnsDiffuse = new float[4]
        {
      0.8f,
      0.8f,
      0.8f,
      1f
        };
        public static readonly float[] nnsAmbient = new float[4]
        {
      0.2f,
      0.2f,
      0.2f,
      1f
        };
        public static readonly float[] nnsSpecular = new float[4]
        {
      0.0f,
      0.0f,
      0.0f,
      1f
        };
        public static float nnsShininess = 16f;
        public static readonly float[] nnsEmission = new float[4]
        {
      0.0f,
      0.0f,
      0.0f,
      1f
        };
        public static readonly NNS_MATRIX nnsPrim3DMatrix = GlobalPool<NNS_MATRIX>.Alloc();
        public static uint nnsAlphaFunc = 516;
        public static float nnsAlphaFuncRef = 0.0f;
        public static uint nnsDepthFunc = 515;
        public static bool nnsDepthMask = true;
        public static int nnsFormat;
    }
}
