public partial class AppMain
{
    public class AMS_DRAWSTATE_BLEND
    {
        public int mode;

        public AMS_DRAWSTATE_BLEND()
        {
        }

        public AMS_DRAWSTATE_BLEND(AMS_DRAWSTATE_BLEND drawState)
        {
            this.mode = drawState.mode;
        }

        public AMS_DRAWSTATE_BLEND Assign(AMS_DRAWSTATE_BLEND drawState)
        {
            this.mode = drawState.mode;
            return this;
        }

        public void Clear()
        {
            this.mode = 0;
        }
    }
}
