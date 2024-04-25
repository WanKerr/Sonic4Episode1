namespace gs.backup
{
    public class SDebug : SBase
    {
        private Sonic4Save save;

        public SDebug(Sonic4Save save)
        {
            this.save = save;
        }

        public void Init()
        {

        }

        public bool GodMode
        {
            get => save.Debug.GodMode;
            set
            {
                save.Debug.GodMode = value;
                isDirty = true;
            }
        }
    }
}