public partial class AppMain
{
    public class NNS_MOTION_KEY_Class6
    {
        public readonly NNS_RGB Value = new NNS_RGB();
        public float Frame;

        public NNS_MOTION_KEY_Class6()
        {
        }

        public NNS_MOTION_KEY_Class6(NNS_MOTION_KEY_Class6 motionKey)
        {
            this.Frame = motionKey.Frame;
            this.Value.Assign(motionKey.Value);
        }

        public NNS_MOTION_KEY_Class6 Assign(NNS_MOTION_KEY_Class6 motionKey)
        {
            if (this != motionKey)
            {
                this.Frame = motionKey.Frame;
                this.Value.Assign(motionKey.Value);
            }
            return this;
        }
    }
}
