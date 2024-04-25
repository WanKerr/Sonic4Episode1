public partial class AppMain
{
    public class OBS_COL_CHK_DATA : IClearable
    {
        public int pos_x;
        public int pos_y;
        public ushort[] dir;
        public uint[] attr;
        public ushort flag;
        public ushort vec;

        public OBS_COL_CHK_DATA Assign(OBS_COL_CHK_DATA data)
        {
            if (this != data)
            {
                this.pos_x = data.pos_x;
                this.pos_y = data.pos_y;
                this.dir = data.dir;
                this.dir = data.dir;
                this.attr = data.attr;
                this.attr = data.attr;
                this.flag = data.flag;
                this.vec = data.vec;
            }
            return this;
        }

        public void Clear()
        {
            this.pos_x = 0;
            this.pos_y = 0;
            this.dir = null;
            this.attr = null;
            this.flag = 0;
            this.vec = 0;
        }
    }
}
