public partial class AppMain
{
    public class A2S_SUB_MTN
    {
        public float scl_x;
        public float scl_y;
        public float rot;
        public float scl_accele;
        public float rot_accele;

        internal void Assign(A2S_SUB_MTN old)
        {
            this.scl_x = old.scl_x;
            this.scl_y = old.scl_y;
            this.rot = old.rot;
            this.scl_accele = old.scl_accele;
            this.rot_accele = old.rot_accele;
        }
    }
}
