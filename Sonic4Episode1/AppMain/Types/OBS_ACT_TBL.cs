public partial class AppMain
{
    public class OBS_ACT_TBL
    {
        public ushort act_id;
        public byte time;
        public byte flag;

        public OBS_ACT_TBL()
        {
        }

        public OBS_ACT_TBL(OBS_ACT_TBL obsTbl)
        {
            this.act_id = obsTbl.act_id;
            this.time = obsTbl.time;
            this.flag = obsTbl.flag;
        }

        public OBS_ACT_TBL Assign(OBS_ACT_TBL obsTbl)
        {
            this.act_id = obsTbl.act_id;
            this.time = obsTbl.time;
            this.flag = obsTbl.flag;
            return this;
        }
    }
}
