public partial class AppMain
{
    public class NNS_MOTION_KEY_Class8
    {
        public float Frame;
        public int Value;

        public NNS_MOTION_KEY_Class8()
        {
        }

        public NNS_MOTION_KEY_Class8(NNS_MOTION_KEY_Class8 motionKey)
        {
            this.Frame = motionKey.Frame;
            this.Value = motionKey.Value;
        }

        public NNS_MOTION_KEY_Class8 Assign(NNS_MOTION_KEY_Class8 motionKey)
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
