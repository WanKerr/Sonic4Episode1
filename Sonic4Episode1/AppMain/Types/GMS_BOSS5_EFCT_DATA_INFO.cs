public partial class AppMain
{
    public class GMS_BOSS5_EFCT_DATA_INFO
    {
        public int use_model;
        public int ame_arc_idx;
        public int ame_dwork_no;
        public int tex_amb_arc_idx;
        public int tex_amb_dwork_no;
        public int tex_list_dwork_no;
        public int model_arc_idx;
        public int model_dwork_no;
        public int object_dwork_no;

        public GMS_BOSS5_EFCT_DATA_INFO(
          int z,
          int a,
          int b,
          int c,
          int d,
          int e,
          int f,
          int g,
          int h)
        {
            this.use_model = z;
            this.ame_arc_idx = a;
            this.ame_dwork_no = b;
            this.tex_amb_arc_idx = c;
            this.tex_amb_dwork_no = d;
            this.tex_list_dwork_no = e;
            this.model_arc_idx = f;
            this.model_dwork_no = g;
            this.object_dwork_no = h;
        }
    }
}
