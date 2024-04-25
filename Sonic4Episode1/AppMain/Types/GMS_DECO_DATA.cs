public partial class AppMain
{
    public class GMS_DECO_DATA
    {
        public readonly AOS_TEXTURE tvx_tex = new AOS_TEXTURE();
        public AMS_AMB_HEADER amb_header;
        public OBS_ACTION3D_NN_WORK obj_3d_list;
        public OBS_ACTION3D_NN_WORK[] obj_3d_list_fall;
        public AMS_AMB_HEADER tvx_model;
        public TVX_FILE[] tvx_model_data;

        public void Clear()
        {
            this.tvx_tex.Clear();
            this.amb_header = null;
            this.obj_3d_list = null;
            this.obj_3d_list_fall = null;
            this.tvx_model = null;
        }
    }
}
