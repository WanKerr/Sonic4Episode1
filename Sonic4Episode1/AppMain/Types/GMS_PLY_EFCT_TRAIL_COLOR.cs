public partial class AppMain
{
    public class GMS_PLY_EFCT_TRAIL_COLOR
    {
        public readonly NNS_RGBA start_col;
        public readonly NNS_RGBA end_col;

        public GMS_PLY_EFCT_TRAIL_COLOR(NNS_RGBA s_col, NNS_RGBA e_col)
        {
            this.start_col = s_col;
            this.end_col = e_col;
        }

        public GMS_PLY_EFCT_TRAIL_COLOR()
        {
            this.start_col = new NNS_RGBA();
            this.end_col = new NNS_RGBA();
        }
    }
}
