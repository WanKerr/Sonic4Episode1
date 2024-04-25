public partial class AppMain
{
    public struct NNS_TEXCOORD : IClearable
    {
        public float u;
        public float v;

        public NNS_TEXCOORD(float u, float v)
        {
            this.u = u;
            this.v = v;
        }

        public void Clear()
        {
            this.u = this.v = 0.0f;
        }
    }
}
