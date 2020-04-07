using System;
using System.Collections.Generic;
using System.Text;

public partial class AppMain
{


    private static void GmFadeSetFade(
      AppMain.GMS_FADE_OBJ_WORK fade_obj,
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
        AppMain.IZS_FADE_WORK fadeWork = fade_obj.fade_work;
        AppMain.IzFadeSetWork(fadeWork, fadeWork.dt_prio, fadeWork.draw_state, fade_set_type, start_col_r, start_col_g, start_col_b, start_col_a, end_col_r, end_col_g, end_col_b, end_col_a, time, draw_start != 0, conti_state != 0);
    }

    private static int GmFadeIsEnd(AppMain.GMS_FADE_OBJ_WORK fade_obj)
    {
        return (double)fade_obj.fade_work.count >= (double)fade_obj.fade_work.time ? 1 : 0;
    }

    private static AppMain.GMS_FADE_OBJ_WORK GmFadeCreateFadeObj(
      ushort prio,
      byte group,
      byte pause_level,
      AppMain.TaskWorkFactoryDelegate work_size,
      ushort dt_prio,
      uint draw_state)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.OBM_OBJECT_TASK_DETAIL_INIT(prio, group, pause_level, (byte)0, work_size, "FADE_OBJ");
        AppMain.GMS_FADE_OBJ_WORK gmsFadeObjWork = (AppMain.GMS_FADE_OBJ_WORK)obsObjectWork;
        obsObjectWork.obj_type = (ushort)7;
        obsObjectWork.flag |= 18U;
        obsObjectWork.move_flag |= 8448U;
        gmsFadeObjWork.fade_work.dt_prio = dt_prio;
        gmsFadeObjWork.fade_work.draw_state = draw_state;
        gmsFadeObjWork.fade_work.time = 1f;
        gmsFadeObjWork.obj_work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmFadeDispFunc);
        return gmsFadeObjWork;
    }

    private static void gmFadeDispFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_FADE_OBJ_WORK gmsFadeObjWork = (AppMain.GMS_FADE_OBJ_WORK)obj_work;
        if (((int)obj_work.disp_flag & 4112) == 0)
            AppMain.IzFadeUpdate(gmsFadeObjWork.fade_work);
        if (((int)obj_work.disp_flag & 32) != 0)
            return;
        AppMain.IzFadeDraw(gmsFadeObjWork.fade_work);
    }
}