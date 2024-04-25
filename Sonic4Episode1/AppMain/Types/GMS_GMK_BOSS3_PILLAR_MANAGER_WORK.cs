public partial class AppMain
{
    public class GMS_GMK_BOSS3_PILLAR_MANAGER_WORK : IOBS_OBJECT_WORK
    {
        public readonly OBS_OBJECT_WORK[] obj_work_pillar = new OBS_OBJECT_WORK[26];
        public readonly OBS_OBJECT_WORK[] obj_work_wall = new OBS_OBJECT_WORK[2];
        public readonly GMS_ENEMY_3D_WORK gimmick_work;
        public int pattern_no;

        public GMS_GMK_BOSS3_PILLAR_MANAGER_WORK()
        {
            this.gimmick_work = new GMS_ENEMY_3D_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.gimmick_work.ene_com.obj_work;
        }

        public static explicit operator OBS_OBJECT_WORK(
          GMS_GMK_BOSS3_PILLAR_MANAGER_WORK work)
        {
            return work.gimmick_work.ene_com.obj_work;
        }
    }
}
