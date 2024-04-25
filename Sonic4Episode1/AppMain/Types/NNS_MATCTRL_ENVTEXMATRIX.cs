public partial class AppMain
{
    public class NNS_MATCTRL_ENVTEXMATRIX
    {
        public readonly NNS_MATRIX texmtx = GlobalPool<NNS_MATRIX>.Alloc();
        public int texcoordsrc;

        public NNS_MATCTRL_ENVTEXMATRIX()
        {
        }

        public NNS_MATCTRL_ENVTEXMATRIX(int _texcoordsrc, NNS_MATRIX _texmtx)
        {
            this.texcoordsrc = _texcoordsrc;
            this.texmtx.Assign(_texmtx);
        }
    }
}
