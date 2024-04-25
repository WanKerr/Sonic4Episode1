public partial class AppMain
{
    public class AMS_TCB_THREAD
    {
        public readonly AMS_ALARM alarm_wakeup = new AMS_ALARM();
        public int id;
        public AMS_TASK taskp;
        public AMS_TCB tcbp;
        public NNS_MATRIXSTACK matrix_stack;
        public byte[] matrix_stack_buf;
    }
}
