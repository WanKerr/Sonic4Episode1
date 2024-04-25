public partial class AppMain
{
    public class AMS_DRAWSTATE_FOG_RANGE
    {
        public float fnear;
        public float ffar;

        public AMS_DRAWSTATE_FOG_RANGE()
        {
        }

        public AMS_DRAWSTATE_FOG_RANGE(AMS_DRAWSTATE_FOG_RANGE drawState)
        {
            this.fnear = drawState.fnear;
            this.ffar = drawState.ffar;
        }

        public AMS_DRAWSTATE_FOG_RANGE Assign(AMS_DRAWSTATE_FOG_RANGE drawState)
        {
            this.fnear = drawState.fnear;
            this.ffar = drawState.ffar;
            return this;
        }

        public void Clear()
        {
            this.fnear = this.ffar = 0.0f;
        }
    }
}
