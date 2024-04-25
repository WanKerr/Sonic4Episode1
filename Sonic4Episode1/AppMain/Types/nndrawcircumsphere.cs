public partial class AppMain
{
    private static class nndrawcircumsphere
    {
        public static readonly NNS_RGBA[] nnsMsstCircumCol = new NNS_RGBA[8]
        {
      new NNS_RGBA(0.0f, 1f, 0.0f, 0.3f),
      new NNS_RGBA(1f, 0.0f, 1f, 0.3f),
      new NNS_RGBA(1f, 1f, 0.0f, 0.3f),
      new NNS_RGBA(1f, 1f, 1f, 0.3f),
      new NNS_RGBA(0.0f, 1f, 1f, 0.3f),
      new NNS_RGBA(1f, 0.0f, 0.0f, 0.3f),
      new NNS_RGBA(0.0f, 0.0f, 0.0f, 0.3f),
      new NNS_RGBA()
        };
        public static int nnsSubMotIdx;
        public static NNS_MATRIX nnsBaseMtx;
        public static NNS_OBJECT nnsObj;
        public static NNS_NODE nnsNodeList;
        public static NNS_MATRIXSTACK nnsMstk;
        public static NNS_MOTION nnsMot;
        public static NNS_TRS nnsTrsList;
        public static float nnsFrame;
        public static uint nnsDrawCsFlag;
    }
}
