using System;

public partial class AppMain
{
    public class DMS_TITLEOP_MGR_WORK
    {
        public OBS_OBJECT_WORK[] obj_work = new OBS_OBJECT_WORK[5];
        public AOS_ACTION[] act = new AOS_ACTION[7];
        public int frame;
        public uint flag;
        public float finger_frame;

        public void Clear()
        {
            this.frame = 0;
            this.flag = 0U;
            Array.Clear(obj_work, 0, this.obj_work.Length);
            Array.Clear(act, 0, this.act.Length);
            this.finger_frame = 0.0f;
        }
    }
}
