public partial class AppMain
{
    public class A2S_SUB_TRS
    {
        public float trs_x;
        public float trs_y;
        public float trs_z;
        public float trs_accele;

        internal void Assign(A2S_SUB_TRS old)
        {
            this.trs_x = old.trs_x;
            this.trs_y = old.trs_y;
            this.trs_z = old.trs_z;
            this.trs_accele = old.trs_accele;
        }
    }
}
