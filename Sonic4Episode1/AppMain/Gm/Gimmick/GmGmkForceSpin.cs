public partial class AppMain
{
    private static OBS_OBJECT_WORK GmGmkForceSpinSetInit(
        GMS_EVE_RECORD_EVENT eve_rec,
        int pos_x,
        int pos_y,
        byte type)
    {
        UNREFERENCED_PARAMETER(type);
        OBS_OBJECT_WORK work =
            GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_COM_WORK(), "GMK_FORCE_SPIN_SET");
        ObjRectSet(((GMS_ENEMY_COM_WORK) work).rect_work[2].rect, eve_rec.left, eve_rec.top,
            (short) (eve_rec.width + eve_rec.left), (short) (eve_rec.height + eve_rec.top));
        work.move_flag |= 8480U;
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkForceSpinSetMain);
        return work;
    }

    private static OBS_OBJECT_WORK GmGmkForceSpinResetInit(
        GMS_EVE_RECORD_EVENT eve_rec,
        int pos_x,
        int pos_y,
        byte type)
    {
        UNREFERENCED_PARAMETER(type);
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_COM_WORK(),
            "GMK_FORCE_SPIN_RESET");
        ObjRectSet(((GMS_ENEMY_COM_WORK) work).rect_work[2].rect, eve_rec.left, eve_rec.top,
            (short) (eve_rec.width + eve_rec.left), (short) (eve_rec.height + eve_rec.top));
        work.move_flag |= 8480U;
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkForceSpinResetMain);
        return work;
    }

    private static void gmGmkForceSpinSetMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_COM_WORK gmsEnemyComWork = (GMS_ENEMY_COM_WORK) obj_work;
        GMS_PLAYER_WORK ply_work = g_gm_main_system.ply_work[0];
        if ((ply_work.player_flag & GMD_PLF_DIE) != 0 ||
            ((int) ply_work.obj_work.flag & 2) != 0 ||
            (((int) g_gm_main_system.game_flag & 262656) != 0 ||
             !gmGmkForceSpinRectChk(obj_work, ply_work)) ||
            (ply_work.seq_state == GME_PLY_SEQ_STATE_GMK_FORCESPIN || ply_work.seq_state == GME_PLY_SEQ_STATE_GMK_FORCESPIN_DEC || ply_work.seq_state == GME_PLY_SEQ_STATE_GMK_FORCESPIN_FALL))
            return;
        if ((gmsEnemyComWork.eve_rec.flag & 1) != 0)
            GmPlySeqGmkInitForceSpinDec(ply_work);
        else
            GmPlySeqGmkInitForceSpin(ply_work);
    }

    private static void gmGmkForceSpinResetMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_COM_WORK gmsEnemyComWork = (GMS_ENEMY_COM_WORK) obj_work;
        GMS_PLAYER_WORK ply_work = g_gm_main_system.ply_work[0];
        if (((int) ply_work.player_flag & 1024) != 0 || ((int) ply_work.obj_work.flag & 2) != 0 ||
            (((int) g_gm_main_system.game_flag & 262656) != 0 || !gmGmkForceSpinRectChk(obj_work, ply_work)) ||
            ply_work.seq_state != 51 && ply_work.seq_state != 52 && ply_work.seq_state != 53)
            return;
        if (((int) ply_work.obj_work.move_flag & 1) != 0)
        {
            ply_work.no_spddown_timer = 0;
            if ((gmsEnemyComWork.eve_rec.flag & 1) != 0)
                GmPlySeqChangeSequence(ply_work, 10);
            else
                GmPlySeqInitFw(ply_work);
        }
        else
            GmPlySeqGmkInitSpinFall(ply_work, ply_work.obj_work.spd.x, ply_work.obj_work.spd.y);
    }

    private static bool gmGmkForceSpinRectChk(
        OBS_OBJECT_WORK obj_work,
        GMS_PLAYER_WORK ply_work)
    {
        OBS_RECT_WORK obsRectWork = ((GMS_ENEMY_COM_WORK) obj_work).rect_work[2];
        return ply_work.obj_work.pos.x >= obj_work.pos.x + (obsRectWork.rect.left << 12) &&
               ply_work.obj_work.pos.x <= obj_work.pos.x + (obsRectWork.rect.right << 12) &&
               (ply_work.obj_work.pos.y >= obj_work.pos.y + (obsRectWork.rect.top << 12) &&
                ply_work.obj_work.pos.y <= obj_work.pos.y + (obsRectWork.rect.bottom << 12));
    }
}