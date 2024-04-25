public partial class AppMain
{
    public class GMS_SOUND_BGM_FADE_WORK
    {
        public float start_vol;
        public float end_vol;
        public float fade_spd;
        public float now_vol;
        public int frame;
        public GSS_SND_SCB snd_scb;
        public GMS_SOUND_BGM_FADE_WORK next;
        public GMS_SOUND_BGM_FADE_WORK prev;
    }
}
