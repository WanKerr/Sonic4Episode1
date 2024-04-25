public partial class AppMain
{
    public class AMS_DRAWSTATE_AMBIENT
    {
        public int mode;
        public float r;
        public float g;
        public float b;

        public AMS_DRAWSTATE_AMBIENT()
        {
        }

        public AMS_DRAWSTATE_AMBIENT(AMS_DRAWSTATE_AMBIENT drawState)
        {
            this.mode = drawState.mode;
            this.r = drawState.r;
            this.g = drawState.g;
            this.b = drawState.b;
        }

        public AMS_DRAWSTATE_AMBIENT Assign(AMS_DRAWSTATE_AMBIENT drawState)
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
