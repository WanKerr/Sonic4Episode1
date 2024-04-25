public partial class AppMain
{
    private class nnlight
    {
        public static NNS_MATRIX nngLightMtx = GlobalPool<NNS_MATRIX>.Alloc();
        public static NNS_GL_LIGHT nngLight = new NNS_GL_LIGHT();
        public static float[] nngPointLightFallOffEnd = new float[4];
        public static float[] nngPointLightFallOffScale = new float[4];
        public static float[] nngSpotLightFallOffEnd = new float[4];
        public static float[] nngSpotLightFallOffScale = new float[4];
        public static float[] nngSpotLightAngleScale = new float[4];
        public static int nngNumParallelLight;
        public static int nngNumPointLight;
        public static int nngNumSpotLight;
    }
}
