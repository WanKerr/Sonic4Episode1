public partial class AppMain
{
    private class GMS_GMK_BWALL_PARTS_PARAM_TABLE
    {
        public ushort[][] _params;
        public ushort num;

        public GMS_GMK_BWALL_PARTS_PARAM_TABLE(ushort[][] prm, ushort n)
        {
            this._params = prm;
            this.num = n;
        }

        public GMS_GMK_BWALL_PARTS_PARAM_TABLE(ushort[][] prm, int n)
        {
            this._params = prm;
            this.num = (ushort)n;
        }
    }
}
