public partial class AppMain
{
    public class AOS_ACT_HITP
    {
        public uint flag;
        public AOE_ACT_HIT type;
        public float scale_x;
        public float scale_y;
        public AOS_ACT_RECT rect;

        public void Assign(AOS_ACT_HITP other)
        {
            this.flag = other.flag;
            this.type = other.type;
            this.scale_x = other.scale_x;
            this.scale_y = other.scale_y;
            this.rect = other.rect;
        }

        public void Clear()
        {
            this.flag = 0U;
            this.type = AOE_ACT_HIT.AOD_ACT_HIT_RECT;
            this.scale_x = this.scale_y = 0.0f;
        }

        public void GetCircle(ref AOS_ACT_CIRCLE circle)
        {
            circle.center_x = this.rect.left;
            circle.center_y = this.rect.top;
            circle.radius = this.rect.right;
            circle.pad = (uint)this.rect.bottom;
        }

        public void SetCircle(ref A2S_SUB_CIRCLE circle)
        {
            this.rect.left = circle.center_x;
            this.rect.top = circle.center_y;
            this.rect.right = circle.radius;
            this.rect.bottom = circle.pad;
        }

        public void SetCircle(ref AOS_ACT_CIRCLE circle)
        {
            this.rect.left = circle.center_x;
            this.rect.top = circle.center_y;
            this.rect.right = circle.radius;
            this.rect.bottom = circle.pad;
        }
    }
}
