public partial class AppMain
{
    private static OBS_OBJECT_WORK GmGmkEndingAnimalInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_3D_WORK(), "GMK_END_ANIMAL");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        uint num = eve_rec.flag & 7U;
        work.user_work = num;
        gmGmkAnimalObjSet(work, gmsEnemy3DWork.obj_3d);
        work.user_flag = (eve_rec.flag & 16) == 0 ? 0U : 2U;
        work.disp_flag |= 4259840U;
        work.move_flag &= 4294952703U;
        work.move_flag |= 1680U;
        work.spd.y = -g_gm_gmk_animal_speed_param[(int)work.user_work].jump;
        work.spd_fall = g_gm_gmk_animal_speed_param[(int)work.user_work].gravity;
        work.pos.z = -131072;
        work.flag |= 512U;
        work.flag |= 2U;
        work.flag &= 4294967279U;
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkEndingAnimalMove);
        return work;
    }
}