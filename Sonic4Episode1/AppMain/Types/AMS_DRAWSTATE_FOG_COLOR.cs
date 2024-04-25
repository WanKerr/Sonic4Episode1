public partial class AppMain
{
    public class AMS_DRAWSTATE_FOG_COLOR : IClearable
    {
        public float r;
        public float g;
        public float b;

        public AMS_DRAWSTATE_FOG_COLOR()
        {
        }

        public AMS_DRAWSTATE_FOG_COLOR(AMS_DRAWSTATE_FOG_COLOR drawState)
        {
            this.r = drawState.r;
            this.g = drawState.g;
            this.b = drawState.b;
        }

        public AMS_DRAWSTATE_FOG_COLOR Assign(AMS_DRAWSTATE_FOG_COLOR drawState)
        {
            this.r = drawState.r;
            this.g = drawState.g;
            this.b = drawState.b;
            return this;
        }

        public void Clear()
        {
            this.r = this.g = this.b = 0.0f;
        }
    }
}
