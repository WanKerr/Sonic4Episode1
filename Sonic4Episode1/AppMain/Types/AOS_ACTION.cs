public partial class AppMain
{
    public class AOS_ACTION
    {
        public object data;
        public uint flag;
        public uint state;
        public AOE_ACT_TYPE type;
        public float frame;
        public _last_key last_key;
        public AOS_ACTION child;
        public AOS_ACTION sibling;
        public AOS_SPRITE sprite;

        public void Clear()
        {
            this.data = 0;
            this.flag = 0U;
            this.state = 0U;
            this.type = AOE_ACT_TYPE.AOD_ACT_TYPE_ACTION;
            this.frame = 0.0f;
            this.last_key.Clear();
            this.child = null;
            this.sibling = null;
            this.sprite = null;
        }
    }
}
