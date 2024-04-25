public partial class AppMain
{
    public class A2S_SUB_HIT
    {
        public uint flag;
        public uint type;
        public float hit_accele;
        public uint pad;
        public A2S_SUB_RECT rect;
        public A2S_SUB_CIRCLE circle;

        internal void Assign(A2S_SUB_HIT old)
        {
            this.flag = old.flag;
            this.type = old.type;
            this.hit_accele = old.hit_accele;
            this.pad = old.pad;
            this.rect = old.rect;
            this.circle = old.circle;
        }
    }
}
