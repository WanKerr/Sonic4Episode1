public partial class AppMain
{
    public class AMS_DRAWSTATE_ENVMAP
    {
        public readonly NNS_MATRIX texmtx = GlobalPool<NNS_MATRIX>.Alloc();
        public int texsrc;

        public AMS_DRAWSTATE_ENVMAP()
        {
        }

        public AMS_DRAWSTATE_ENVMAP(AMS_DRAWSTATE_ENVMAP drawState)
        {
            this.texsrc = drawState.texsrc;
            this.texmtx.Assign(drawState.texmtx);
        }

        public AMS_DRAWSTATE_ENVMAP Assign(AMS_DRAWSTATE_ENVMAP drawState)
        {
            if (this != drawState)
            {
                this.texsrc = drawState.texsrc;
                this.texmtx.Assign(drawState.texmtx);
            }
            return this;
        }

        public void Clear()
        {
            this.texsrc = 0;
            this.texmtx.Clear();
        }
    }
}
