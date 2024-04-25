public partial class AppMain
{
    public class GMS_BOSS1_BODY_STATE_ENTER_INFO
    {
        public GMF_BOSS1_BODY_STATE_ENTER_FUNC enter_func;
        public bool is_wrapped;

        public GMS_BOSS1_BODY_STATE_ENTER_INFO(
          GMF_BOSS1_BODY_STATE_ENTER_FUNC _enter_func,
          bool _is_wrapped)
        {
            this.enter_func = _enter_func;
            this.is_wrapped = _is_wrapped;
        }
    }
}
