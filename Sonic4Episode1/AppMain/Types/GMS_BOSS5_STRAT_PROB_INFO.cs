public partial class AppMain
{
    public class GMS_BOSS5_STRAT_PROB_INFO
    {
        public int strat_state;
        public int probability;
        public int is_rkt;

        public GMS_BOSS5_STRAT_PROB_INFO(int a, int b, int c)
        {
            this.strat_state = a;
            this.probability = b;
            this.is_rkt = c;
        }
    }
}
