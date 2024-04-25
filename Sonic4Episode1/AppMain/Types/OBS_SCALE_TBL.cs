public partial class AppMain
{
    public class OBS_SCALE_TBL
    {
        public VecFx32 scale = new VecFx32();
        public byte time;
        public byte flag;

        public OBS_SCALE_TBL()
        {
        }

        public OBS_SCALE_TBL(OBS_SCALE_TBL obsTbl)
        {
            this.scale.Assign(obsTbl.scale);
            this.time = obsTbl.time;
            this.flag = obsTbl.flag;
        }

        public OBS_SCALE_TBL Assign(OBS_SCALE_TBL obsTbl)
        {
            if (this != obsTbl)
            {
                this.scale.Assign(obsTbl.scale);
                this.time = obsTbl.time;
                this.flag = obsTbl.flag;
            }
            return this;
        }
    }
}
