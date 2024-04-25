public partial class AppMain
{
    public class NNS_MOTION_KEY_Class15
    {
        public readonly NNS_MOTION_SI_SPLINE_HANDLE Shandle = new NNS_MOTION_SI_SPLINE_HANDLE();
        public short Frame;
        public short Value;

        public NNS_MOTION_KEY_Class15()
        {
        }

        public NNS_MOTION_KEY_Class15(NNS_MOTION_KEY_Class15 motionKey)
        {
            this.Frame = motionKey.Frame;
            this.Value = motionKey.Value;
            this.Shandle.Assign(motionKey.Shandle);
        }

        public NNS_MOTION_KEY_Class15 Assign(NNS_MOTION_KEY_Class15 motionKey)
        {
            if (this != motionKey)
            {
                this.Frame = motionKey.Frame;
                this.Value = motionKey.Value;
                this.Shandle.Assign(motionKey.Shandle);
            }
            return this;
        }
    }
}
