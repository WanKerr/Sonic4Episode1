public partial class AppMain
{
    public class GMS_GAMEDAT_LOAD_CONTEXT
    {
        public GME_GAMEDAT_LOAD_STATE state;
        public string file_path;
        public int bb_no;
        public AMS_FS fs_req;
        public GMS_GAMEDAT_LOAD_DATA load_data;
        public ushort char_id;
        public ushort ply_no;
        public ushort stage_id;
        public ushort data_no;

        internal void Clear()
        {
            this.state = GME_GAMEDAT_LOAD_STATE.GMD_GAMEDAT_LOAD_STATE_LOADING;
            this.file_path = "";
            this.bb_no = 0;
            this.fs_req = null;
            this.load_data = null;
            this.char_id = 0;
            this.ply_no = 0;
            this.stage_id = 0;
            this.data_no = 0;
        }
    }
}
