public partial class AppMain
{
    public class GMS_PLY_SEQ_STATE_DATA
    {
        public uint check_attr;
        public uint accept_attr;

        public GMS_PLY_SEQ_STATE_DATA(uint check_attr, uint accept_attr)
        {
            this.check_attr = check_attr;
            this.accept_attr = accept_attr;
        }
    }
}
