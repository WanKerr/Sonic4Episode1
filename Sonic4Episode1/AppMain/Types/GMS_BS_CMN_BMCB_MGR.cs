public partial class AppMain
{
    public class GMS_BS_CMN_BMCB_MGR : IClearable
    {
        public readonly GMS_BS_CMN_BMCB_LINK bmcb_head = new GMS_BS_CMN_BMCB_LINK();
        public readonly GMS_BS_CMN_BMCB_LINK bmcb_tail = new GMS_BS_CMN_BMCB_LINK();

        public void Clear()
        {
            this.bmcb_head.Clear();
            this.bmcb_tail.Clear();
        }
    }
}
