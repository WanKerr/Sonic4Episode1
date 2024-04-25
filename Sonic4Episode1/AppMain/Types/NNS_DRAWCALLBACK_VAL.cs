public partial class AppMain
{
    public class NNS_DRAWCALLBACK_VAL
    {
        public int iMaterial;
        public int iPrevMaterial;
        public int iVtxList;
        public int iPrevVtxList;
        public int iNode;
        public int iMeshset;
        public int iSubobject;
        public NNS_MATERIALPTR pMaterial;
        public NNS_VTXLISTPTR pVtxListPtr;
        public NNS_OBJECT pObject;
        public NNS_MATRIX[] pMatrixPalette;
        public uint[] pNodeStatusList;
        public uint DrawSubobjType;
        public uint DrawFlag;
        public int bModified;
        public int bReDraw;
    }
}
