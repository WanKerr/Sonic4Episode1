using System;

public partial class AppMain
{
    public class OBS_TBL_WORK
    {
        public readonly short[] key_timer = new short[4];
        public readonly short[] move_timer = new short[4];
        public VecFx32 spd = new VecFx32();
        public VecFx32 scale = new VecFx32();
        public VecU16 dir = new VecU16();
        public readonly OBS_DATA_WORK[] data_work = new OBS_DATA_WORK[4];
        public readonly short[] repeat_point = new short[4];
        public uint flag;
        public OBS_ACT_TBL act_tbl;
        public OBS_MOVE_TBL move_tbl;
        public OBS_SCALE_TBL scale_tbl;
        public OBS_DIR_TBL dir_tbl;

        public OBS_TBL_WORK()
        {
        }

        public OBS_TBL_WORK(OBS_TBL_WORK tblWork)
        {
            Array.Copy(tblWork.key_timer, key_timer, this.key_timer.Length);
            Array.Copy(tblWork.move_timer, move_timer, this.move_timer.Length);
            this.flag = tblWork.flag;
            this.spd.Assign(tblWork.spd);
            this.scale.Assign(tblWork.scale);
            this.dir.Assign(tblWork.dir);
            this.act_tbl = tblWork.act_tbl;
            this.move_tbl = tblWork.move_tbl;
            this.scale_tbl = tblWork.scale_tbl;
            this.dir_tbl = tblWork.dir_tbl;
            Array.Copy(tblWork.data_work, data_work, this.data_work.Length);
            Array.Copy(tblWork.repeat_point, repeat_point, this.repeat_point.Length);
        }

        public OBS_TBL_WORK Assign(OBS_TBL_WORK tblWork)
        {
            if (this != tblWork)
            {
                Array.Copy(tblWork.key_timer, key_timer, this.key_timer.Length);
                Array.Copy(tblWork.move_timer, move_timer, this.move_timer.Length);
                this.flag = tblWork.flag;
                this.spd.Assign(tblWork.spd);
                this.scale.Assign(tblWork.scale);
                this.dir.Assign(tblWork.dir);
                this.act_tbl = tblWork.act_tbl;
                this.move_tbl = tblWork.move_tbl;
                this.scale_tbl = tblWork.scale_tbl;
                this.dir_tbl = tblWork.dir_tbl;
                Array.Copy(tblWork.data_work, data_work, this.data_work.Length);
                Array.Copy(tblWork.repeat_point, repeat_point, this.repeat_point.Length);
            }
            return this;
        }
    }
}
