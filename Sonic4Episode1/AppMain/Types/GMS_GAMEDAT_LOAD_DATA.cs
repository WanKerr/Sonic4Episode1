public partial class AppMain
{
    public class GMS_GAMEDAT_LOAD_DATA
    {
        public string path;
        public _alloc_ alloc;
        public _proc_pre_ proc_pre;
        public _proc_post_ proc_post;
        public int user_data;

        public GMS_GAMEDAT_LOAD_DATA(
          string _path,
          _alloc_ _alloc,
          _proc_pre_ _proc_pre,
          _proc_post_ _proc_post,
          int udata)
        {
            this.path = _path;
            this.alloc = _alloc;
            this.proc_pre = _proc_pre;
            this.proc_post = _proc_post;
            this.user_data = udata;
        }

        public delegate object _alloc_(string s);

        public delegate void _proc_pre_(GMS_GAMEDAT_LOAD_CONTEXT contex);

        public delegate void _proc_post_(GMS_GAMEDAT_LOAD_CONTEXT contex);
    }
}
