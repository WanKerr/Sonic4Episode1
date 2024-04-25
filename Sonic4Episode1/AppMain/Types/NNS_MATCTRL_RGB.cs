public partial class AppMain
{
    public class NNS_MATCTRL_RGB
    {
        public readonly NNS_RGB col = new NNS_RGB();
        public int mode;

        public NNS_MATCTRL_RGB()
        {
        }

        public NNS_MATCTRL_RGB(int _mode, float r, float g, float b)
        {
            _mode = this.mode;
            this.col.r = r;
            this.col.b = b;
            this.col.g = g;
        }
    }
}
