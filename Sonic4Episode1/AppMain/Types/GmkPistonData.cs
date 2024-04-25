public partial class AppMain
{
    public class GmkPistonData
    {
        public static readonly short[][] tbl_gm_gmk_piston_col_rect = new short[2][]
        {
      new short[8]
      {
         56,
         32,
         -28,
         0,
         -28,
         32,
         28,
         0
      },
      new short[8]
      {
         56,
         32,
         -28,
         -32,
         -28,
         0,
         28,
         32
      }
        };
        public const int GME_GMK_TYPE_PISTON_UP = 0;
        public const int GME_GMK_TYPE_PISTON_DOWN = 1;
        public const int GME_GMK_TYPE_MAX = 2;
        public const int GMD_GMK_BOBJ1_RECT_WIDTH = 56;
        public const int GMD_GMK_BOBJ1_RECT_HEIGHT = 32;
        public const int GMD_GMK_BOBJ1_RECT_MARGIN = 32;
    }
}
