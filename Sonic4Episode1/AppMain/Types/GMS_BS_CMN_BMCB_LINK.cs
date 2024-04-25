public partial class AppMain
{
    public class GMS_BS_CMN_BMCB_LINK : IClearable
    {
        public GMS_BS_CMN_BMCB_LINK next;
        public GMS_BS_CMN_BMCB_LINK prev;
        public MPP_VOID_MOTION_NSSOBJECT_OBJECT bmcb_func;
        public object bmcb_param;

        public void Clear()
        {
            this.next = this.prev = null;
            this.bmcb_func = null;
            this.bmcb_param = null;
        }
    }
}
