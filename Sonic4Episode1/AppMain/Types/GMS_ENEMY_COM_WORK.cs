public partial class AppMain
{
    public class GMS_ENEMY_COM_WORK : IOBS_OBJECT_WORK
    {
        public readonly OBS_RECT_WORK[] rect_work = New<OBS_RECT_WORK>(3);
        public readonly OBS_COLLISION_WORK col_work = new OBS_COLLISION_WORK();
        public VecU16 target_dp_dir = new VecU16();
        public VecFx32 target_dp_pos = new VecFx32();
        public readonly OBS_OBJECT_WORK obj_work;
        public GMS_EVE_RECORD_EVENT eve_rec;
        public byte eve_x;
        public byte vit;
        public int born_pos_x;
        public int born_pos_y;
        public int invincible_timer;
        public uint enemy_flag;
        public ushort act_state;
        public OBS_OBJECT_WORK target_obj;
        public int target_dp_dist;
        public readonly object holder;

        public static explicit operator GMS_GMK_TRUCK_WORK(GMS_ENEMY_COM_WORK p)
        {
            return (GMS_GMK_TRUCK_WORK)(GMS_ENEMY_3D_WORK)p.holder;
        }

        public static explicit operator GMS_BOSS5_CORE_WORK(GMS_ENEMY_COM_WORK p)
        {
            return (GMS_BOSS5_CORE_WORK)p.holder;
        }

        public static explicit operator GMS_ENEMY_3D_WORK(GMS_ENEMY_COM_WORK p)
        {
            return p == null ? null : (GMS_ENEMY_3D_WORK)p.holder;
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.obj_work;
        }

        public static explicit operator OBS_OBJECT_WORK(GMS_ENEMY_COM_WORK work)
        {
            return work.obj_work;
        }

        public GMS_ENEMY_COM_WORK()
        {
            this.obj_work = OBS_OBJECT_WORK.Create(this, null);
        }

        public GMS_ENEMY_COM_WORK(object p)
        {
            this.holder = p;
            this.obj_work = OBS_OBJECT_WORK.Create(this, p);
        }
    }
}
