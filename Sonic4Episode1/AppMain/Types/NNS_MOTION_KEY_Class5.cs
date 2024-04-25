using System.IO;

public partial class AppMain
{
    public class NNS_MOTION_KEY_Class5
    {
        public readonly NNS_VECTOR Value = GlobalPool<NNS_VECTOR>.Alloc();
        public float Frame;

        public NNS_MOTION_KEY_Class5()
        {
        }

        public NNS_MOTION_KEY_Class5(NNS_MOTION_KEY_Class5 motionKey)
        {
            this.Frame = motionKey.Frame;
            this.Value.Assign(motionKey.Value);
        }

        public NNS_MOTION_KEY_Class5 Assign(NNS_MOTION_KEY_Class5 motionKey)
        {
            if (this != motionKey)
            {
                this.Frame = motionKey.Frame;
                this.Value.Assign(motionKey.Value);
            }
            return this;
        }

        public static NNS_MOTION_KEY_Class5 Read(BinaryReader reader)
        {
            NNS_MOTION_KEY_Class5 nnsMotionKeyClass5 = new NNS_MOTION_KEY_Class5();
            nnsMotionKeyClass5.Frame = reader.ReadSingle();
            nnsMotionKeyClass5.Value.Assign(NNS_VECTOR.Read(reader));
            return nnsMotionKeyClass5;
        }

        public static NNS_MOTION_KEY_Class5[] Read(BinaryReader reader, int count)
        {
            NNS_MOTION_KEY_Class5[] nnsMotionKeyClass5Array = new NNS_MOTION_KEY_Class5[count];
            for (int index = 0; index < count; ++index)
                nnsMotionKeyClass5Array[index] = Read(reader);
            return nnsMotionKeyClass5Array;
        }
    }
}
