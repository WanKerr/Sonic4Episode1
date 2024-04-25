public partial class AppMain
{
    public class NNS_MOTION_KEY_Class4
    {
        public float Frame;
        public NNS_TEXCOORD Value;

        public NNS_MOTION_KEY_Class4()
        {
        }

        public NNS_MOTION_KEY_Class4(NNS_MOTION_KEY_Class4 motionKey)
        {
            this.Frame = motionKey.Frame;
            this.Value = motionKey.Value;
        }

        public NNS_MOTION_KEY_Class4 Assign(NNS_MOTION_KEY_Class4 motionKey)
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
