using System.IO;

public partial class AppMain
{
    public class NNS_MOTION_KEY_Class16
    {
        public short Frame;
        public NNS_ROTATE_A16 Value;

        public NNS_MOTION_KEY_Class16()
        {
        }

        public NNS_MOTION_KEY_Class16(NNS_MOTION_KEY_Class16 motionKey)
        {
            this.Frame = motionKey.Frame;
            this.Value = motionKey.Value;
        }

        public NNS_MOTION_KEY_Class16 Assign(NNS_MOTION_KEY_Class16 motionKey)
        {
            if (this != motionKey)
            {
                this.Frame = motionKey.Frame;
                this.Value = motionKey.Value;
            }
            return this;
        }

        public static NNS_MOTION_KEY_Class16 Read(BinaryReader reader)
        {
            return new NNS_MOTION_KEY_Class16()
            {
                Frame = reader.ReadInt16(),
                Value = {
                  x = reader.ReadInt16(),
                  y = reader.ReadInt16(),
                  z = reader.ReadInt16()
                }
            };
        }

        public static NNS_MOTION_KEY_Class16[] Read(BinaryReader reader, int count)
        {
            NNS_MOTION_KEY_Class16[] motionKeyClass16Array = new NNS_MOTION_KEY_Class16[count];
            for (int index = 0; index < count; ++index)
                motionKeyClass16Array[index] = Read(reader);
            return motionKeyClass16Array;
        }
    }
}
