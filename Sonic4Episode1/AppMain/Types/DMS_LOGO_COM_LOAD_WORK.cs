public partial class AppMain
{
    public class DMS_LOGO_COM_LOAD_WORK
    {
        public readonly DMS_LOGO_COM_LOAD_CONTEXT[] context = new DMS_LOGO_COM_LOAD_CONTEXT[10];
        public Reference<MTS_TASK_TCB> load_tcb_addr;
        public int context_num;

        public DMS_LOGO_COM_LOAD_WORK()
        {
            for (int index = 0; index < this.context.Length; ++index)
                this.context[index] = new DMS_LOGO_COM_LOAD_CONTEXT();
        }
    }
}
