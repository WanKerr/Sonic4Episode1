public partial class AppMain
{
    public class GMS_BOSS5_SCT_PART_INFO
    {
        public int cnm_mode;
        public int is_local_coord;
        public int is_inherit_scale;

        public GMS_BOSS5_SCT_PART_INFO(int a, int b, int c)
        {
            this.cnm_mode = a;
            this.is_local_coord = b;
            this.is_inherit_scale = c;
        }
    }
}
