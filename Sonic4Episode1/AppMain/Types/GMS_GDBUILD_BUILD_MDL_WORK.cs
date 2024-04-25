public partial class AppMain
{
    public class GMS_GDBUILD_BUILD_MDL_WORK : IClearable
    {
        public GME_GAME_DBUILD_MDL_STATE build_state;
        public OBS_ACTION3D_NN_WORK[] obj_3d_list;
        public int num;
        public int reg_num;
        public AMS_AMB_HEADER mdl_amb;
        public AMS_AMB_HEADER tex_amb;
        public uint draw_flag;
        public TXB_HEADER txb;

        public void Clear()
        {
            this.build_state = GME_GAME_DBUILD_MDL_STATE.GME_GAME_DBUILD_MDL_STATE_REG_WAIT;
            this.obj_3d_list = null;
            this.num = this.reg_num = 0;
            this.mdl_amb = this.tex_amb = null;
            this.draw_flag = 0U;
            this.txb = null;
        }
    }
}
