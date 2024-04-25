public partial class AppMain
{
    public class GMS_BOSS5_BODY_STATE_ENTER_INFO
    {
        public MPP_VOID_GMS_BOSS5_BODY_WORK enter_func;
        public int is_wrapped;

        public GMS_BOSS5_BODY_STATE_ENTER_INFO(MPP_VOID_GMS_BOSS5_BODY_WORK ef, int wrp)
        {
            this.enter_func = ef;
            this.is_wrapped = wrp;
        }
    }
}
