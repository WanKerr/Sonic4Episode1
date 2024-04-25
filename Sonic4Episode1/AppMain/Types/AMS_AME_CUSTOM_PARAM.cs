public partial class AppMain
{
    public class AMS_AME_CUSTOM_PARAM
    {
        private object _pInitFieldFunc;
        public AmeDelegateFunc pUpdateFunc;
        public AmeDelegateFunc pDrawFunc;

        public AmeDelegateFunc pInitFunc
        {
            get => (AmeDelegateFunc)this._pInitFieldFunc;
            set => this._pInitFieldFunc = value;
        }

        public AmeFieldFunc pFieldFunc
        {
            get => (AmeFieldFunc)this._pInitFieldFunc;
            set => this._pInitFieldFunc = value;
        }
    }
}
