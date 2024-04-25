public partial class AppMain
{
    public class A2S_SUB_MAT
    {
        public A2S_SUB_COL base_;
        public A2S_SUB_COL fade;
        public float base_accele;
        public float fade_accele;
        public uint blend;

        internal void Assign(A2S_SUB_MAT old)
        {
            this.base_ = old.base_;
            this.fade = old.fade;
            this.base_accele = old.base_accele;
            this.fade_accele = old.fade_accele;
            this.blend = old.blend;
        }
    }
}
