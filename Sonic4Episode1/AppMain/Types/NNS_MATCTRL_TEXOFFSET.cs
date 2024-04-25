public partial class AppMain
{
    public class NNS_MATCTRL_TEXOFFSET
    {
        public NNS_TEXCOORD offset = new NNS_TEXCOORD();
        public int mode;

        public NNS_MATCTRL_TEXOFFSET()
        {
        }

        public NNS_MATCTRL_TEXOFFSET(int _mode, float _u, float _v)
        {
            this.mode = _mode;
            this.offset.u = _u;
            this.offset.v = _v;
        }
    }
}
