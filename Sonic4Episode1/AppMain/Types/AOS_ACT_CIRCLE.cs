public partial class AppMain
{
    public struct AOS_ACT_CIRCLE
    {
        public float center_x;
        public float center_y;
        public float radius;
        public uint pad;

        public void Assign(ref A2S_SUB_CIRCLE c)
        {
            this.center_x = c.center_x;
            this.center_y = c.center_y;
            this.radius = c.radius;
            this.pad = c.pad;
        }
    }
}
