public partial class AppMain
{
    public class AMS_DRAWSTATE_ALPHA
    {
        public int mode;
        public float alpha;

        public AMS_DRAWSTATE_ALPHA()
        {
        }

        public AMS_DRAWSTATE_ALPHA(AMS_DRAWSTATE_ALPHA drawState)
        {
            this.mode = drawState.mode;
            this.alpha = drawState.alpha;
        }

        public AMS_DRAWSTATE_ALPHA Assign(AMS_DRAWSTATE_ALPHA drawState)
        {
            this.mode = drawState.mode;
            this.alpha = drawState.alpha;
            return this;
        }

        public void Clear()
        {
            this.mode = 0;
            this.alpha = 0.0f;
        }
    }
}
