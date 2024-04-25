public partial class AppMain
{
    private struct SResetLocalTable
    {
        public int act_idx;
        public int act_id;
        public int ama_idx;

        public SResetLocalTable(int a, int b, int c)
        {
            this.act_idx = a;
            this.act_id = b;
            this.ama_idx = c;
        }
    }
}
