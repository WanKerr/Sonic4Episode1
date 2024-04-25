public partial class AppMain
{
    public class OBS_DIR_TBL
    {
        public VecU16 dir = new VecU16();
        public byte time;
        public byte flag;

        public OBS_DIR_TBL()
        {
        }

        public OBS_DIR_TBL(OBS_DIR_TBL obsTbl)
        {
            this.dir.Assign(obsTbl.dir);
            this.time = obsTbl.time;
            this.flag = obsTbl.flag;
        }

        public OBS_DIR_TBL Assign(OBS_DIR_TBL obsTbl)
        {
            this.dir.Assign(obsTbl.dir);
            this.time = obsTbl.time;
            this.flag = obsTbl.flag;
            return this;
        }
    }
}
