public partial class AppMain
{
    private class _nnDrawPrimitive3DCore
    {
        public static RGBA_U8[] cbuf = new RGBA_U8[6];
        public static NNS_PRIM3D_PCT[] prim_d = New<NNS_PRIM3D_PCT>(2048);
        public static RGBA_U8[] prim_c = New<RGBA_U8>(2048);
        public static NNS_PRIM3D_PCT_VertexData vertexData = new NNS_PRIM3D_PCT_VertexData();
        public static NNS_PRIM3D_PC_VertexData vertexDataPC = new NNS_PRIM3D_PC_VertexData();
        public static NNS_PRIM3D_PCT_TexCoordData texCoordData = new NNS_PRIM3D_PCT_TexCoordData();
        public static RGBA_U8_ColorData colorData = new RGBA_U8_ColorData();
    }
}
