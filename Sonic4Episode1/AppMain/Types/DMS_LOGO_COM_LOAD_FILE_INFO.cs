public partial class AppMain
{
    public class DMS_LOGO_COM_LOAD_FILE_INFO
    {
        public string file_path;
        public post_func_Delegate post_func;

        public DMS_LOGO_COM_LOAD_FILE_INFO(string _file_path, post_func_Delegate _post_func)
        {
            this.file_path = _file_path;
            this.post_func = _post_func;
        }
    }
}
