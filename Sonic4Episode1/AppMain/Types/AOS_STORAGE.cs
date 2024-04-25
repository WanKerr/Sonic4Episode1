using gs;

public partial class AppMain
{
    public class AOS_STORAGE
    {
        public bool initialized;
        public int state;
        public int error;
        public bool save_success;
        public byte[] save_buf;
        public uint save_size;
        public bool load_success;
        public byte[] load_buf;
        public uint load_size;
        public bool del_success;
        public AMS_TCB tcb;
        internal Sonic4Save save;

        public AOS_STORAGE(bool init, int state, int error)
        {
            this.initialized = init;
            this.state = state;
            this.error = error;
        }
    }
}
