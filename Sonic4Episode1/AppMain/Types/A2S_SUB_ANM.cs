public partial class AppMain
{
    public class A2S_SUB_ANM
    {
        public int tex_id;
        public uint clamp;
        public uint filter;
        public float texel_accele;
        public A2S_SUB_RECT texel;

        internal void Assign(A2S_SUB_ANM old)
        {
            this.tex_id = old.tex_id;
            this.clamp = old.clamp;
            this.filter = old.filter;
            this.texel_accele = old.texel_accele;
            this.texel = old.texel;
        }
    }
}
