public partial class AppMain
{
    public class GMS_EFCT_ZONE_CREATE_PARAM
    {
        public readonly GMS_EFFECT_CREATE_PARAM create_param;
        public int model_dwork_no;
        public int mdl_ambtex_idx;

        public GMS_EFCT_ZONE_CREATE_PARAM()
        {
            this.create_param = new GMS_EFFECT_CREATE_PARAM();
        }

        public GMS_EFCT_ZONE_CREATE_PARAM(
          GMS_EFFECT_CREATE_PARAM create_param,
          int model_dwork_no,
          int mdl_ambtex_idx)
        {
            this.create_param = create_param;
            this.mdl_ambtex_idx = mdl_ambtex_idx;
            this.model_dwork_no = model_dwork_no;
        }
    }
}
