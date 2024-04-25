using System.IO;

public partial class AppMain
{
    public struct NNS_MOTION_KEY_Class14
    {
        public short Frame;
        public short Value;

        public static NNS_MOTION_KEY_Class14 Read(BinaryReader reader)
        {
            return new NNS_MOTION_KEY_Class14()
            {
                Frame = reader.ReadInt16(),
                Value = reader.ReadInt16()
            };
        }

        public static NNS_MOTION_KEY_Class14[] Read(BinaryReader reader, int count)
        {
            NNS_MOTION_KEY_Class14[] motionKeyClass14Array = new NNS_MOTION_KEY_Class14[count];
            for (int index = 0; index < count; ++index)
                motionKeyClass14Array[index] = Read(reader);
            return motionKeyClass14Array;
        }
    }
}
