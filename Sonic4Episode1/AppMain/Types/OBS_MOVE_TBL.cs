public partial class AppMain
{
    public class OBS_MOVE_TBL
    {
        public VecFx32 spd = new VecFx32();
        public VecFx32 spd_add = new VecFx32();
        public byte time;
        public byte flag;

        public OBS_MOVE_TBL()
        {
        }

        public OBS_MOVE_TBL(OBS_MOVE_TBL obsTbl)
        {
            this.spd.Assign(obsTbl.spd);
            this.spd_add.Assign(obsTbl.spd_add);
            this.time = obsTbl.time;
            this.flag = obsTbl.flag;
        }

        public OBS_MOVE_TBL Assign(OBS_MOVE_TBL obsTbl)
        {
            if (this != obsTbl)
            {
                this.spd.Assign(obsTbl.spd);
                this.spd_add.Assign(obsTbl.spd_add);
                this.time = obsTbl.time;
                this.flag = obsTbl.flag;
            }
            return this;
        }
    }
}
