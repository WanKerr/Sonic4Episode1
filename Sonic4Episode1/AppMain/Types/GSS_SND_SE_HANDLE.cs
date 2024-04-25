public partial class AppMain
{
    public class GSS_SND_SE_HANDLE : IClearable
    {
        public readonly GSS_SND_CTRL_PARAM snd_ctrl_param = new GSS_SND_CTRL_PARAM();
        public uint flag;
        public CriAuPlayer au_player;
        public uint cur_pause_level;

        public void Clear()
        {
            this.flag = 0U;
            this.snd_ctrl_param.Clear();
            this.cur_pause_level = 0U;
            this.au_player.Destroy();
            this.au_player = null;
        }
    }
}
