public partial class AppMain
{
    private class GMS_BOSS5_RKT_SEQ_WAITFALL_INFO
    {
        public readonly int[] probability = new int[3];
        public readonly uint[] frame = new uint[3];
        public int life_threshold;

        public GMS_BOSS5_RKT_SEQ_WAITFALL_INFO(int life, int[] prob, uint[] fr)
        {
            this.life_threshold = life;
            this.probability = prob;
            this.frame = fr;
        }
    }
}
