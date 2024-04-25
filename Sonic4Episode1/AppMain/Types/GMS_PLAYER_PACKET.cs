public partial class AppMain
{
    public class GMS_PLAYER_PACKET
    {
        public VecFx32 pos = new VecFx32();
        public ushort disp_flag;
        public short anime_speed;
        public byte act_state;
        public byte dir_x;
        public byte dir_y;
        public byte dir_z;
        public uint move_flag;
        public uint player_flag;
        public uint gmk_flag;
        public short move_x;
        public short move_y;
        public int camera_pos_x;
        public int camera_pos_y;
        public uint time;
    }
}
