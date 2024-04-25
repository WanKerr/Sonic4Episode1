using System;

public partial class AppMain
{
    public class GMS_SMSG_MGR_WORK : IClearable
    {
        public GMS_SMSG_2D_OBJ_WORK[] ama_2d_work = new GMS_SMSG_2D_OBJ_WORK[GMD_SMSG_AMA_ACT_MAX];
        public GMS_SMSG_2D_OBJ_WORK[] ama_2d_work_act = new GMS_SMSG_2D_OBJ_WORK[GMD_SMSG_AMA_ACT_ACTION_MAX];
        public uint flag;
        public int timer;
        public int msg_type;
        public pfnGMS_SMSG_MGR_WORK func;
        public float win_per;

        public void Clear()
        {
            this.flag = 0U;
            this.timer = 0;
            this.msg_type = 0;
            this.func = null;
            Array.Clear(ama_2d_work, 0, this.ama_2d_work.Length);
            Array.Clear(ama_2d_work_act, 0, this.ama_2d_work_act.Length);
        }
    }
}
