public partial class AppMain
{
    public struct NNS_SHADER_CONFIG
    {
        private int bNormalizeVertexNormal;
        private int bRescaleVertexNormal;
        private int nMaxParallelLight;
        private int nMaxPointLight;
        private int nMaxSpotLight;
        private int bLightAmbient;
        private NNE_ATTEN_FUNC PointLightDistAtten;
        private NNE_ATTEN_FUNC SpotLightDistAtten;
        private NNE_FOG_MODEL FogModel;
        private int bDistanceFog;
        private int bFragmentFog;
        private uint nUserUniform;
        private int bHalfFloat;
        private int bNoScaleEnvelope;
        private int bVertexSpecular;
        private int bCalcBinormal;
    }
}
