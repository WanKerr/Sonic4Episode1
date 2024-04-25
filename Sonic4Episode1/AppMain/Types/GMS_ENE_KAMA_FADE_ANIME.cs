public partial class AppMain
{
    public class GMS_ENE_KAMA_FADE_ANIME
    {
        public uint pat_num;
        public GMS_ENE_KAMA_FADE_ANIME_PAT[] anime_pat;

        public GMS_ENE_KAMA_FADE_ANIME(uint num, GMS_ENE_KAMA_FADE_ANIME_PAT[] pat)
        {
            this.pat_num = num;
            this.anime_pat = pat;
        }
    }
}
