using System.IO;

public partial class AppMain
{
    public class NNS_MOTION_KEY_Class13
    {
        public float Frame;
        public NNS_ROTATE_A32 Value;

        public NNS_MOTION_KEY_Class13()
        {
        }

        public NNS_MOTION_KEY_Class13(NNS_MOTION_KEY_Class13 motionKey)
        {
            this.Frame = motionKey.Frame;
            this.Value = motionKey.Value;
        }

        public NNS_MOTION_KEY_Class13 Assign(NNS_MOTION_KEY_Class13 motionKey)
        {
            if (this != motionKey)
            {
                this.Frame = motionKey.Frame;
                this.Value = motionKey.Value;
            }
            return this;
        }

        public static NNS_MOTION_KEY_Class13 Read(BinaryReader reader)
        {
            NNS_MOTION_KEY_Class13 motion = new NNS_MOTION_KEY_Class13();
            motion.Frame = reader.ReadSingle();
            motion.Value = new NNS_ROTATE_A32
            {
                x = reader.ReadInt32(),
                y = reader.ReadInt32(),
                z = reader.ReadInt32()
            };

            return motion;
        }

        public static NNS_MOTION_KEY_Class13[] Read(BinaryReader reader, int count)
        {
            NNS_MOTION_KEY_Class13[] motionArray = new NNS_MOTION_KEY_Class13[count];
            for (int index = 0; index < count; ++index)
                motionArray[index] = Read(reader);
            return motionArray;
        }
    }
}
