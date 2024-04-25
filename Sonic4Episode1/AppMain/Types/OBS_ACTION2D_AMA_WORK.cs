public partial class AppMain
{
    public class OBS_ACTION2D_AMA_WORK : IClearable
    {
        public readonly AOS_TEXTURE ao_tex = new AOS_TEXTURE();
        public uint flag;
        public AOS_ACTION act;
        public NNS_TEXLIST texlist;
        public A2S_AMA_HEADER ama;
        public OBS_DATA_WORK ama_data_work;
        public uint act_id;
        public float frame;
        public float speed;
        public int type_node;
        public AOS_ACT_COL color;
        public AOS_ACT_COL fade;

        public void Clear()
        {
            this.flag = 0U;
            this.act = null;
            this.ao_tex.Clear();
            this.texlist = null;
            this.ama = null;
            this.ama_data_work = null;
            this.act_id = 0U;
            this.frame = 0.0f;
            this.speed = 0.0f;
            this.type_node = 0;
            this.color.Clear();
            this.fade.Clear();
        }
    }
}
