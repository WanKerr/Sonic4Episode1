public partial class AppMain
{
    public class SYS_EVT_DATA
    {
        public _init_func_ init_func;
        public _exit_func_ exit_func;
        public _reset_func_ reset_func;
        public _init_sys_func_ init_sys_func;
        public _exit_sys_func_ exit_sys_func;
        public short[] next_evt_id;
        public uint attr;

        public SYS_EVT_DATA(
          _init_func_ f1,
          _exit_func_ f2,
          _reset_func_ f3,
          _init_sys_func_ f4,
          _exit_sys_func_ f5,
          short[] ar,
          uint at)
        {
            this.init_func = f1;
            this.exit_func = f2;
            this.reset_func = f3;
            this.init_sys_func = f4;
            this.exit_sys_func = f5;
            this.next_evt_id = ar;
            this.attr = at;
        }

        public SYS_EVT_DATA()
        {
        }

        public delegate void _init_func_(object obj);

        public delegate void _exit_func_();

        public delegate void _reset_func_();

        public delegate void _init_sys_func_();

        public delegate void _exit_sys_func_();
    }
}
