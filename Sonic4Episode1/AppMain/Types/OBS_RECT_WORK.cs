public partial class AppMain
{
    public class OBS_RECT_WORK : IClearable
    {
        public readonly OBS_RECT rect = new OBS_RECT();
        public uint flag;
        public OBS_OBJECT_WORK parent_obj;
        public OBS_RECT_WORK_Delegate1 ppHit;
        public OBS_RECT_WORK_Delegate1 ppDef;
        public OBS_RECT_WORK_Delegate2 ppCheck;
        public short hit_power;
        public short def_power;
        public ushort hit_flag;
        public ushort def_flag;
        public byte group_no;
        public byte target_g_flag;
        public uint attr_flag;
        public uint user_data;
        public object pDataWork;

        public void Clear()
        {
            this.rect.Clear();
            this.flag = 0U;
            this.parent_obj = null;
            this.ppHit = this.ppDef = null;
            this.ppCheck = null;
            this.hit_power = this.def_power = 0;
            this.hit_flag = this.def_flag = 0;
            this.group_no = this.target_g_flag = 0;
            this.attr_flag = this.user_data = 0U;
            this.pDataWork = null;
        }
    }
}
