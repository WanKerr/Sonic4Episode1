public partial class AppMain
{
    public class NNS_MOTION_KEY_Class12
    {
        public float Frame;
        public uint Value;

        public NNS_MOTION_KEY_Class12()
        {
        }

        public NNS_MOTION_KEY_Class12(NNS_MOTION_KEY_Class12 motionKey)
        {
            this.Frame = motionKey.Frame;
            this.Value = motionKey.Value;
        }

        public NNS_MOTION_KEY_Class12 Assign(NNS_MOTION_KEY_Class12 motionKey)
        {
            if (this != motionKey)
            {
                this.Frame = motionKey.Frame;
                this.Value = motionKey.Value;
            }
            return this;
        }
    }
}
