public partial class AppMain
{
    public struct AOS_ACT_RECT
    {
        public float left;
        public float top;
        public float right;
        public float bottom;

        public void Assign(ref A2S_SUB_RECT rect)
        {
            this.left = rect.left;
            this.top = rect.top;
            this.right = rect.right;
            this.bottom = rect.bottom;
        }

        public void Clear()
        {
            this.left = this.top = this.right = this.bottom = 0.0f;
        }
    }
}
