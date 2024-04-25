public partial class AppMain
{
    public class GMS_ENE_KAMA_FADE_ANIME_PAT
    {
        public readonly NNS_RGB col;
        public float intensity;
        public int frame;

        public GMS_ENE_KAMA_FADE_ANIME_PAT()
        {
            this.col = new NNS_RGB();
        }

        public GMS_ENE_KAMA_FADE_ANIME_PAT(NNS_RGB c, float inten, int fr)
        {
            this.col = c;
            this.intensity = inten;
            this.frame = fr;
        }
    }
}
