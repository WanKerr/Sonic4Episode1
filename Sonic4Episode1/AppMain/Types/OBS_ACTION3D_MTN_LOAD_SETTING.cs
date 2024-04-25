public partial class AppMain
{
    public class OBS_ACTION3D_MTN_LOAD_SETTING
    {
        public bool enable;
        public bool marge;
        public OBS_DATA_WORK data_work;
        public string filename;
        public int index;
        public AMS_AMB_HEADER archive;

        public void Clear()
        {
            this.enable = false;
            this.marge = false;
            this.data_work = null;
            this.filename = "";
            this.index = 0;
            this.archive = null;
        }
    }
}
