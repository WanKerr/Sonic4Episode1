public partial class AppMain
{
    public class GMS_EFFECT_CREATE_PARAM
    {
        public int ame_idx;
        public uint pos_type;
        public uint init_flag;
        public readonly NNS_VECTOR disp_ofst;
        public NNS_ROTATE_A16 disp_rot;
        public float scale;
        public MPP_VOID_OBS_OBJECT_WORK main_func;
        public int model_idx;

        public GMS_EFFECT_CREATE_PARAM(
          int ame_idx,
          uint pos_type,
          uint init_flag,
          NNS_VECTOR disp_ofst,
          NNS_ROTATE_A16 disp_rot,
          float scale,
          MPP_VOID_OBS_OBJECT_WORK main_func,
          int model_idx)
        {
            this.ame_idx = ame_idx;
            this.pos_type = pos_type;
            this.init_flag = init_flag;
            //this.disp_ofst = GlobalPool<NNS_VECTOR>.Alloc();
            //this.disp_ofst.Assign(disp_ofst);
            this.disp_ofst = disp_ofst;
            this.disp_rot = disp_rot;
            this.scale = scale;
            this.main_func = main_func;
            this.model_idx = model_idx;
        }

        public GMS_EFFECT_CREATE_PARAM()
        {
            this.disp_ofst = GlobalPool<NNS_VECTOR>.Alloc();
            this.disp_rot = new NNS_ROTATE_A16();
        }

        public void Assign(GMS_EFFECT_CREATE_PARAM param)
        {
            this.ame_idx = param.ame_idx;
            this.pos_type = param.pos_type;
            this.init_flag = param.init_flag;
            this.disp_ofst.Assign(param.disp_ofst);
            this.disp_rot = param.disp_rot;
            this.scale = param.scale;
            this.main_func = param.main_func;
            this.model_idx = param.model_idx;
        }
    }
}
