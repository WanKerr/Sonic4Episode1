using System.IO;

public partial class AppMain
{
    public class NNS_MOTION_KEY_Class2
    {
        public readonly NNS_MOTION_BEZIER_HANDLE Bhandle = new NNS_MOTION_BEZIER_HANDLE();
        public float Frame;
        public float Value;

        public NNS_MOTION_KEY_Class2()
        {
        }

        public NNS_MOTION_KEY_Class2(NNS_MOTION_KEY_Class2 motionKey)
        {
            this.Frame = motionKey.Frame;
            this.Value = motionKey.Value;
            this.Bhandle.Assign(motionKey.Bhandle);
        }

        public NNS_MOTION_KEY_Class2 Assign(NNS_MOTION_KEY_Class2 motionKey)
        {
            if (this != motionKey)
            {
                this.Frame = motionKey.Frame;
                this.Value = motionKey.Value;
                this.Bhandle.Assign(motionKey.Bhandle);
            }
            return this;
        }

        public static NNS_MOTION_KEY_Class2 Read(BinaryReader reader)
        {
            NNS_MOTION_KEY_Class2 nnsMotionKeyClass2 = new NNS_MOTION_KEY_Class2();
            nnsMotionKeyClass2.Frame = reader.ReadSingle();
            nnsMotionKeyClass2.Value = reader.ReadSingle();
            nnsMotionKeyClass2.Bhandle.Assign(NNS_MOTION_BEZIER_HANDLE.Read(reader));
            return nnsMotionKeyClass2;
        }

        public static NNS_MOTION_KEY_Class2[] Read(BinaryReader reader, int count)
        {
            NNS_MOTION_KEY_Class2[] nnsMotionKeyClass2Array = new NNS_MOTION_KEY_Class2[count];
            for (int index = 0; index < count; ++index)
                nnsMotionKeyClass2Array[index] = Read(reader);
            return nnsMotionKeyClass2Array;
        }
    }
}
