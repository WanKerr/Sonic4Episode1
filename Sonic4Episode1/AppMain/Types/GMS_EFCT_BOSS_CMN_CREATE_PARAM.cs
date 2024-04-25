public partial class AppMain
{
    public class GMS_EFCT_BOSS_CMN_CREATE_PARAM
    {
        public readonly GMS_EFFECT_CREATE_PARAM create_param;
        public int mdl_ambtex_idx;

        public GMS_EFCT_BOSS_CMN_CREATE_PARAM()
        {
            this.create_param = new GMS_EFFECT_CREATE_PARAM();
        }

        public GMS_EFCT_BOSS_CMN_CREATE_PARAM(
          GMS_EFFECT_CREATE_PARAM create_param,
          int mdl_ambtex_idx)
        {
            this.create_param = create_param;
            this.mdl_ambtex_idx = mdl_ambtex_idx;
        }
    }
}
