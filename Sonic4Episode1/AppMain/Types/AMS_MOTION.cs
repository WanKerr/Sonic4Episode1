public partial class AppMain
{
    public class AMS_MOTION
    {
        public readonly AMS_MOTION_FILE[] mtnfile = New<AMS_MOTION_FILE>(4);
        public readonly AMS_MOTION_BUF[] mbuf = New<AMS_MOTION_BUF>(2);
        public NNS_OBJECT _object;
        public int node_num;
        public int motion_num;
        public NNS_MOTION[] mtnbuf;
        public NNS_TRS[] data;
        public ArrayPointer<NNS_TRS> mmbuf;
        public NNS_OBJECT mmobject;
        public uint mmobj_size;
        public int mmotion_num;
        public NNS_MOTION[] mmtn;
        public int mmotion_id;
        public float mmotion_frame;
    }
}
