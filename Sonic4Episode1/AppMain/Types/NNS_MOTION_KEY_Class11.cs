using System.IO;

public partial class AppMain
{
    public class NNS_MOTION_KEY_Class11
    {
        public float Frame;
        public int Value;

        public NNS_MOTION_KEY_Class11()
        {
        }

        public NNS_MOTION_KEY_Class11(NNS_MOTION_KEY_Class11 motionKey)
        {
            this.Frame = motionKey.Frame;
            this.Value = motionKey.Value;
        }

        public NNS_MOTION_KEY_Class11 Assign(NNS_MOTION_KEY_Class11 motionKey)
        {
            if (this != motionKey)
            {
                this.Frame = motionKey.Frame;
                this.Value = motionKey.Value;
            }
            return this;
        }

        public static NNS_MOTION_KEY_Class11 Read(BinaryReader reader)
        {
            return new NNS_MOTION_KEY_Class11()
            {
                Frame = reader.ReadSingle(),
                Value = reader.ReadInt32()
            };
        }

        public static NNS_MOTION_KEY_Class11[] Read(BinaryReader reader, int count)
        {
            NNS_MOTION_KEY_Class11[] motionKeyClass11Array = new NNS_MOTION_KEY_Class11[count];
            for (int index = 0; index < count; ++index)
                motionKeyClass11Array[index] = Read(reader);
            return motionKeyClass11Array;
        }
    }
}
