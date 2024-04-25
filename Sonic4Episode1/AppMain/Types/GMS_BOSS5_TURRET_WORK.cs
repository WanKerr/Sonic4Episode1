public partial class AppMain
{
    public class GMS_BOSS5_TURRET_WORK : IOBS_OBJECT_WORK
    {
        public NNS_QUATERNION disp_quat = new NNS_QUATERNION();
        public VecFx32 vul_fire_pos = new VecFx32();
        public VecFx32 vul_bullet_pos = new VecFx32();
        public readonly GMS_ENEMY_3D_WORK ene_3d;
        public MPP_VOID_GMS_BOSS5_TURRET_WORK proc_update;
        public uint flag;
        public uint wait_timer;
        public int fire_dir_z;
        public int trt_slide_type;
        public float trt_slide_length;
        public int cvr_slide_type;
        public float cvr_slide_ratio;
        public int vul_shot_remain;
        public int vul_burst_timer;
        public int vul_shot_angle;

        public GMS_BOSS5_TURRET_WORK()
        {
            this.ene_3d = new GMS_ENEMY_3D_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.ene_3d.ene_com.obj_work;
        }
    }
}
