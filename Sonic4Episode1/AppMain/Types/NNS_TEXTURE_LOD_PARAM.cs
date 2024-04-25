public partial class AppMain
{
    public class NNS_TEXTURE_LOD_PARAM
    {
        public int BaseLevel;
        public int MaxLevel;
        public float MinLOD;
        public float MaxLOD;
        public float LODBias;

        public NNS_TEXTURE_LOD_PARAM Assign(NNS_TEXTURE_LOD_PARAM lodParam)
        {
            this.BaseLevel = lodParam.BaseLevel;
            this.MaxLevel = lodParam.MaxLevel;
            this.MinLOD = lodParam.MinLOD;
            this.MaxLOD = lodParam.MaxLOD;
            this.LODBias = lodParam.LODBias;
            return this;
        }
    }
}
