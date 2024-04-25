public partial class AppMain
{
    public class AMS_DRAWSTATE_TEXOFFSET
    {
        public int mode;
        public float u;
        public float v;

        public AMS_DRAWSTATE_TEXOFFSET()
        {
        }

        public AMS_DRAWSTATE_TEXOFFSET(int mode, float u, float v)
        {
            this.mode = mode;
            this.u = u;
            this.v = v;
        }

        public void Assign(AMS_DRAWSTATE_TEXOFFSET p)
        {
            this.mode = p.mode;
            this.u = p.u;
            this.v = p.v;
        }

        internal void Clear()
        {
            this.mode = 0;
            this.u = 0.0f;
            this.v = 0.0f;
        }
    }
}
