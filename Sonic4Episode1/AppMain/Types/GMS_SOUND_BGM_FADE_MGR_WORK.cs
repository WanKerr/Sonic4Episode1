public partial class AppMain
{
    public class GMS_SOUND_BGM_FADE_MGR_WORK
    {
        public int num;
        public GMS_SOUND_BGM_FADE_WORK head;
        public GMS_SOUND_BGM_FADE_WORK tail;

        internal void Clear()
        {
            this.num = 0;
            this.head = null;
            this.tail = null;
        }
    }
}
