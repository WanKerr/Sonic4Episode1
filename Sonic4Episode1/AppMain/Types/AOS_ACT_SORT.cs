public partial class AppMain
{
    public struct AOS_ACT_SORT
    {
        public AOS_SPRITE sprite;
        public float z;

        public void Clear()
        {
            this.z = 0.0f;
            this.sprite = null;
        }
    }
}
