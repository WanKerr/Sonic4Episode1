using System.IO;

public partial class AppMain
{
    public class NNS_MOTION_KEY_Class3
    {
        public readonly NNS_MOTION_SI_SPLINE_HANDLE Shandle = new NNS_MOTION_SI_SPLINE_HANDLE();
        public float Frame;
        public float Value;

        public NNS_MOTION_KEY_Class3()
        {
        }

        public NNS_MOTION_KEY_Class3(NNS_MOTION_KEY_Class3 motionKey)
        {
            this.Frame = motionKey.Frame;
            this.Value = motionKey.Value;
            this.Shandle.Assign(motionKey.Shandle);
        }

        public NNS_MOTION_KEY_Class3 Assign(NNS_MOTION_KEY_Class3 motionKey)
        {
            if (this != motionKey)
            {
                this.Frame = motionKey.Frame;
                this.Value = motionKey.Value;
                this.Shandle.Assign(motionKey.Shandle);
            }
            return this;
        }

        public static NNS_MOTION_KEY_Class3 Read(BinaryReader reader)
        {
            NNS_MOTION_KEY_Class3 nnsMotionKeyClass3 = new NNS_MOTION_KEY_Class3();
            nnsMotionKeyClass3.Frame = reader.ReadSingle();
            nnsMotionKeyClass3.Value = reader.ReadSingle();
            nnsMotionKeyClass3.Shandle.Assign(NNS_MOTION_SI_SPLINE_HANDLE.Read(reader));
            return nnsMotionKeyClass3;
        }
    }
}
