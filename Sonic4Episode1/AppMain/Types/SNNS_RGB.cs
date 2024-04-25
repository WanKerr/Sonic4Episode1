public partial class AppMain
{
    public struct SNNS_RGB
    {
        public float r;
        public float g;
        public float b;

        public SNNS_RGB(float _r, float _g, float _b)
        {
            this.r = _r;
            this.g = _g;
            this.b = _b;
        }

        public void Assign(NNS_RGB rgb)
        {
            this.r = rgb.r;
            this.g = rgb.g;
            this.b = rgb.b;
        }

        public void Clear()
        {
            this.r = this.g = this.b = 0.0f;
        }
    }
}
