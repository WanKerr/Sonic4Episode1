public partial class AppMain
{


    private static void GmFadeSetFade(
      GMS_FADE_OBJ_WORK fade_obj,
      uint fade_set_type,
      byte start_col_r,
      byte start_col_g,
      byte start_col_b,
      byte start_col_a,
      byte end_col_r,
      byte end_col_g,
      byte end_col_b,
      byte end_col_a,
      float time,
      int draw_start,
      int conti_state)
    {
        IZS_FADE_WORK fadeWork = fade_obj.fade_work;
        IzFadeSetWork(fadeWork, fadeWork.dt_prio, fadeWork.draw_state, fade_set_type, start_col_r, start_col_g, start_col_b, start_col_a, end_col_r, end_col_g, end_col_b, end_col_a, time, draw_start != 0, conti_state != 0);
    }

    private static int GmFadeIsEnd(GMS_FADE_OBJ_WORK fade_obj)
    {
        return fade_obj.fade_work.count >= (double)fade_obj.fade_work.time ? 1 : 0;
    }

    private static GMS_FADE_OBJ_WORK GmFadeCreateFadeObj(
      ushort prio,
      byte group,
      byte pause_level,
      TaskWorkFactoryDelegate work_size,
      ushort dt_prio,
      uint draw_state)
    {
        OBS_OBJECT_WORK obsObjectWork = OBM_OBJECT_TASK_DETAIL_INIT(prio, group, pause_level, 0, work_size, "FADE_OBJ");
        GMS_FADE_OBJ_WORK gmsFadeObjWork = (GMS_FADE_OBJ_WORK)obsObjectWork;
        obsObjectWork.obj_type = 7;
        obsObjectWork.flag |= 18U;
        obsObjectWork.move_flag |= 8448U;
        gmsFadeObjWork.fade_work.dt_prio = dt_prio;
        gmsFadeObjWork.fade_work.draw_state = draw_state;
        gmsFadeObjWork.fade_work.time = 1f;
        gmsFadeObjWork.obj_work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmFadeDispFunc);
        return gmsFadeObjWork;
    }

    private static void gmFadeDispFunc(OBS_OBJECT_WORK obj_work)
    {
        GMS_FADE_OBJ_WORK gmsFadeObjWork = (GMS_FADE_OBJ_WORK)obj_work;
        if (((int)obj_work.disp_flag & 4112) == 0)
            IzFadeUpdate(gmsFadeObjWork.fade_work);
        if (((int)obj_work.disp_flag & 32) != 0)
            return;
        IzFadeDraw(gmsFadeObjWork.fade_work);
    }
}