public partial class AppMain
{
    public class AMS_TRAIL_EFFECT
    {
        public AMS_TRAIL_PARAM Work = new AMS_TRAIL_PARAM();
        public AMS_TRAIL_EFFECT pNext;
        public AMS_TRAIL_EFFECT pPrev;
        public DoubleType<AMTREffectProc, int> Procedure;
        public DoubleType<AMTREffectProc, int> Destractor;
        public float fFrame;
        public float fEndFrame;
        public uint drawState;
        public ushort handleId;
        public short flag;

        public void Clear()
        {
            this.pNext = null;
            this.pPrev = null;
            this.Procedure = null;
            this.Destractor = null;
            this.fFrame = this.fEndFrame = 0.0f;
            this.drawState = 0U;
            this.handleId = 0;
            this.flag = 0;
            this.Work.Clear();
        }
    }
}
