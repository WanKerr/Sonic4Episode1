public partial class AppMain
{
    public class GMS_START_DEMO_MGR : IClearable
    {
        public MTS_TASK_TCB main_tcb;

        public void Clear()
        {
            this.main_tcb = null;
        }
    }
}
