public partial class AppMain
{
    public class AOS_SPRITE
    {
        public readonly AOS_ACT_HITP hit = new AOS_ACT_HITP();
        public uint flag;
        public uint blend;
        public AOS_ACT_COL color;
        public AOS_ACT_COL fade;
        public float center_x;
        public float center_y;
        public float prio;
        public AOS_ACT_RECT offset;
        public float rotate;
        public NNS_TEXLIST texlist;
        public int tex_id;
        public uint clamp;
        public AOS_ACT_RECT uv;

        public void Assign(AOS_SPRITE from)
        {
            this.flag = from.flag;
            this.blend = from.blend;
            this.color = from.color;
            this.fade = from.fade;
            this.center_x = from.center_x;
            this.center_y = from.center_y;
            this.prio = from.prio;
            this.offset = from.offset;
            this.rotate = from.rotate;
            this.tex_id = from.tex_id;
            this.clamp = from.clamp;
            this.uv = from.uv;
            this.texlist = from.texlist;
            this.hit.Assign(from.hit);
        }

        public void Clear()
        {
            this.flag = 0U;
            this.blend = 0U;
            this.color.Clear();
            this.fade.Clear();
            this.center_x = 0.0f;
            this.center_y = 0.0f;
            this.prio = 0.0f;
            this.offset.Clear();
            this.rotate = 0.0f;
            this.tex_id = 0;
            this.clamp = 0U;
            this.uv.Clear();
            this.texlist = null;
            this.hit.Clear();
        }
    }
}
