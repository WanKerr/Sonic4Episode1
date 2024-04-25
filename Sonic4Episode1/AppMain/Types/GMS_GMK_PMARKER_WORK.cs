public partial class AppMain
{
    public class GMS_GMK_PMARKER_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_ENEMY_3D_WORK gmk_work;
        public int markerdist;
        public int markerdistlast;
        public int hitcounter;
        public ushort marker_prty;

        public GMS_GMK_PMARKER_WORK()
        {
            this.gmk_work = new GMS_ENEMY_3D_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.gmk_work.ene_com.obj_work;
        }

        public OBS_ACTION3D_NN_WORK OBJ_3D => this.gmk_work.obj_3d;

        public GMS_ENEMY_COM_WORK COMWORK => this.gmk_work.ene_com;

        public OBS_OBJECT_WORK OBJWORK => this.COMWORK.obj_work;
    }
}
