public partial class AppMain
{
    public class AMS_DRAWSTATE_SPECULAR
    {
        public int mode;
        public float r;
        public float g;
        public float b;

        public AMS_DRAWSTATE_SPECULAR()
        {
        }

        public AMS_DRAWSTATE_SPECULAR(AMS_DRAWSTATE_SPECULAR drawState)
        {
            this.mode = drawState.mode;
            this.r = drawState.r;
            this.g = drawState.g;
            this.b = drawState.b;
        }

        public AMS_DRAWSTATE_SPECULAR Assign(AMS_DRAWSTATE_SPECULAR drawState)
        {
            this.mode = drawState.mode;
            this.r = drawState.r;
            this.g = drawState.g;
            this.b = drawState.b;
            return this;
        }

        public void Clear()
        {
            this.mode = 0;
            this.r = this.g = this.b = 0.0f;
        }
    }
}
