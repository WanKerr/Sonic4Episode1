public partial class AppMain
{
    public class GMS_GAMEDAT_LOAD_WORK
    {
        public GMS_GAMEDAT_LOAD_CONTEXT[] context = New<GMS_GAMEDAT_LOAD_CONTEXT>(GMD_GAMEDAT_LOAD_CONTEXT_MAX);
        public ushort[] char_id = new ushort[1];
        public int context_num;
        public int proc_type;
        public bool load_finish;
        public bool post_finish;
        public ushort stage_id;

        internal void Clear()
        {
            for (int index = 0; index < this.context.Length; ++index)
                this.context[index].Clear();
            this.proc_type = 0;
            this.load_finish = false;
            this.post_finish = false;
            this.stage_id = 0;
            for (int index = 0; index < this.char_id.Length; ++index)
                this.char_id[index] = 0;
        }
    }
}
