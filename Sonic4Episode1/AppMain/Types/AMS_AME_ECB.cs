public partial class AppMain
{
    public class AMS_AME_ECB : AMS_AME_LIST
    {
        public readonly NNS_VECTOR4D translate = new NNS_VECTOR4D();
        public readonly AMS_AME_BOUNDING bounding = new AMS_AME_BOUNDING();
        public int attribute;
        public int priority;
        public NNS_QUATERNION rotate;
        public int transparency;
        public float size_rate;
        public NNS_OBJECT pObj;
        public AMS_AME_ENTRY entry_head;
        public AMS_AME_ENTRY entry_tail;
        public int entry_num;
        public uint drawState;
        public uint drawObjState;
        public int skip_update;

        public override void Clear()
        {
            this.next = null;
            this.prev = null;
            this.attribute = 0;
            this.priority = 0;
            this.translate.Clear();
            this.rotate.Clear();
            this.bounding.Clear();
            this.transparency = 0;
            this.size_rate = 0.0f;
            this.pObj = null;
            this.entry_head = null;
            this.entry_tail = null;
            this.entry_num = 0;
            this.drawState = 0U;
            this.drawObjState = 0U;
            this.skip_update = 0;
        }
    }
}
