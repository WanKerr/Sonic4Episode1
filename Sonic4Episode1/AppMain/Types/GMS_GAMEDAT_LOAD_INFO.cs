public partial class AppMain
{
    public class GMS_GAMEDAT_LOAD_INFO
    {
        public GMS_GAMEDAT_LOAD_DATA[] data_tbl;
        public int num;

        public GMS_GAMEDAT_LOAD_INFO(GMS_GAMEDAT_LOAD_DATA[] _tbl, int _num)
        {
            this.data_tbl = _tbl;
            this.num = _num;
        }
    }
}
