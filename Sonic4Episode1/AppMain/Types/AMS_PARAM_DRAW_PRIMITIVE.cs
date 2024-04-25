public partial class AppMain
{
    public class AMS_PARAM_DRAW_PRIMITIVE : IClearable
    {
        public NNS_MATRIX mtx;
        public int type;
        public NNS_PRIM3D_PCT_ARRAY vtxPCT3D;
        public NNS_PRIM3D_PC[] vtxPC3D;
        public NNS_PRIM2D_PCT[] vtxPCT2D;
        public NNS_PRIM2D_PC[] vtxPC2D;
        private int formatXD;
        public int count;
        public NNS_TEXLIST texlist;
        public int texId;
        public int ablend;
        public float sortZ;
        public int bldSrc;
        public int bldDst;
        public int bldMode;
        public short aTest;
        public short zMask;
        public short zTest;
        public short noSort;
        public int uwrap;
        public int vwrap;

        public int format3D
        {
            get => this.formatXD;
            set => this.formatXD = value;
        }

        public int format2D
        {
            get => this.formatXD;
            set => this.formatXD = value;
        }

        public float zOffset
        {
            get => this.sortZ;
            set => this.sortZ = value;
        }

        public void Assign(AMS_PARAM_DRAW_PRIMITIVE other)
        {
            this.mtx = other.mtx;
            this.type = other.type;
            this.vtxPCT3D = other.vtxPCT3D;
            this.vtxPC3D = other.vtxPC3D;
            this.vtxPCT2D = other.vtxPCT2D;
            this.vtxPC2D = other.vtxPC2D;
            this.formatXD = other.formatXD;
            this.count = other.count;
            this.texlist = other.texlist;
            this.texId = other.texId;
            this.ablend = other.ablend;
            this.sortZ = other.sortZ;
            this.bldSrc = other.bldSrc;
            this.bldDst = other.bldDst;
            this.bldMode = other.bldMode;
            this.aTest = other.aTest;
            this.zMask = other.zMask;
            this.zTest = other.zTest;
            this.noSort = other.noSort;
            this.uwrap = other.vwrap;
            this.vwrap = other.vwrap;
        }

        public void Clear()
        {
            this.mtx = null;
            this.type = 0;
            this.vtxPCT3D = null;
            this.vtxPC3D = null;
            this.vtxPCT2D = null;
            this.vtxPC2D = null;
            this.formatXD = 0;
            this.count = 0;
            this.texlist = null;
            this.texId = 0;
            this.ablend = 0;
            this.sortZ = 0.0f;
            this.bldSrc = 0;
            this.bldDst = 0;
            this.bldMode = 0;
            this.aTest = 0;
            this.zMask = 0;
            this.zTest = 0;
            this.noSort = 0;
            this.uwrap = 0;
            this.vwrap = 0;
        }
    }
}
