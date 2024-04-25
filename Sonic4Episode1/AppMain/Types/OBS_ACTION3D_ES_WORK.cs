public partial class AppMain
{
    public class OBS_ACTION3D_ES_WORK
    {
        public VecU16 disp_rot = new VecU16();
        public readonly NNS_VECTOR4D disp_ofst = new NNS_VECTOR4D();
        public readonly NNS_VECTOR4D dup_draw_ofst = new NNS_VECTOR4D();
        public NNS_QUATERNION user_dir_quat = new NNS_QUATERNION();
        public AMS_AME_ECB ecb;
        public NNS_TEXLIST texlist;
        public object texlistbuf;
        public OBS_DATA_WORK texlist_data_work;
        public NNS_OBJECT _object;
        public OBS_DATA_WORK object_data_work;
        public object eff;
        public OBS_DATA_WORK eff_data_work;
        public object ambtex;
        public OBS_DATA_WORK ambtex_data_work;
        public object model;
        public OBS_DATA_WORK model_data_work;
        public uint flag;
        public uint command_state;
        public int user_attr;
        public int tex_reg_index;
        public int model_reg_index;
        public float speed;

        public void Clear()
        {
            this.ecb = null;
            this.texlist = null;
            this.texlistbuf = null;
            this.texlist_data_work = null;
            this._object = null;
            this.object_data_work = null;
            this.eff = null;
            this.eff_data_work = null;
            this.ambtex = null;
            this.ambtex_data_work = null;
            this.model = null;
            this.model_data_work = null;
            this.flag = 0U;
            this.command_state = 0U;
            this.disp_rot.Clear();
            this.disp_ofst.Clear();
            this.dup_draw_ofst.Clear();
            this.user_dir_quat.Clear();
            this.user_attr = 0;
            this.tex_reg_index = 0;
            this.model_reg_index = 0;
            this.speed = 0.0f;
        }
    }
}
