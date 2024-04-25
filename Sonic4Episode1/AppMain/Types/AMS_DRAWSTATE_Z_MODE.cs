public partial class AppMain
{
    public class AMS_DRAWSTATE_Z_MODE
    {
        public uint compare;
        public uint update;
        public int func;

        public AMS_DRAWSTATE_Z_MODE()
        {
        }

        public AMS_DRAWSTATE_Z_MODE(AMS_DRAWSTATE_Z_MODE drawState)
        {
            this.compare = drawState.compare;
            this.update = drawState.update;
            this.func = drawState.func;
        }

        public AMS_DRAWSTATE_Z_MODE Assign(AMS_DRAWSTATE_Z_MODE drawState)
        {
            this.compare = drawState.compare;
            this.update = drawState.update;
            this.func = drawState.func;
            return this;
        }

        public void Clear()
        {
            this.compare = 0U;
            this.update = 0U;
            this.func = 0;
        }
    }
}
