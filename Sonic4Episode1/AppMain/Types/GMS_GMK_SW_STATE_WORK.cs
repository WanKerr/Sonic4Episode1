public partial class AppMain
{
    private class GMS_GMK_SW_STATE_WORK : IClearable
    {
        public bool sw;
        public int time;
        public bool gear;
        public int per;

        public void Clear()
        {
            this.sw = this.gear = false;
            this.time = this.per = 0;
        }
    }
}
