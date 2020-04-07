using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using mpp;

public partial class AppMain
{
    private static AppMain.OBS_OBJECT_WORK GmGmkSpringInit(
        AppMain.GMS_EVE_RECORD_EVENT eve_rec,
        int pos_x,
        int pos_y,
        byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "GMK_SPRING");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        int index1 = eve_rec.id > (ushort)79 ? (int)eve_rec.id - 70 + 1 - 21 : (int)eve_rec.id - 70;
        if (eve_rec.id == (ushort)71 || eve_rec.id == (ushort)73 || (eve_rec.id == (ushort)75 || eve_rec.id == (ushort)77))
        {
            if (eve_rec.id == (ushort)71 || eve_rec.id == (ushort)75)
                AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_gmk_spring_obj_3d_list[1], gmsEnemy3DWork.obj_3d);
            else
                AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_gmk_spring_obj_3d_list[2], gmsEnemy3DWork.obj_3d);
            AppMain.ObjObjectAction3dNNMotionLoad(work, 0, false, AppMain.ObjDataGet(793), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
            work.user_timer = 2;
            work.user_work = 3U;
        }
        else
        {
            AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_gmk_spring_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
            AppMain.ObjObjectAction3dNNMotionLoad(work, 0, false, AppMain.ObjDataGet(793), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
            work.user_timer = 0;
            work.user_work = 1U;
        }
        work.pos.z = eve_rec.id == (ushort)78 || eve_rec.id == (ushort)79 ? -655360 : -131072;
        gmsEnemy3DWork.ene_com.rect_work[0].flag &= 4294967291U;
        gmsEnemy3DWork.ene_com.rect_work[1].flag &= 4294967291U;
        AppMain.OBS_RECT_WORK pRec1 = gmsEnemy3DWork.ene_com.rect_work[0];
        pRec1.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkSpringDefFunc);
        pRec1.ppHit = (AppMain.OBS_RECT_WORK_Delegate1)null;
        AppMain.ObjRectAtkSet(pRec1, (ushort)0, (short)0);
        AppMain.ObjRectDefSet(pRec1, (ushort)65534, (short)0);
        AppMain.ObjRectWorkSet(pRec1, AppMain.gm_gmk_spring_rect[index1][0], AppMain.gm_gmk_spring_rect[index1][1], AppMain.gm_gmk_spring_rect[index1][2], AppMain.gm_gmk_spring_rect[index1][3]);
        pRec1.flag |= 1024U;
        AppMain.OBS_RECT_WORK pRec2 = gmsEnemy3DWork.ene_com.rect_work[2];
        AppMain.ObjRectWorkSet(pRec2, AppMain.gm_gmk_spring_rect[index1][0], AppMain.gm_gmk_spring_rect[index1][1], AppMain.gm_gmk_spring_rect[index1][2], AppMain.gm_gmk_spring_rect[index1][3]);
        pRec2.flag &= 4294967291U;
        work.move_flag |= 8448U;
        work.disp_flag |= 4194304U;
        work.dir.z = AppMain.gm_gmk_spring_dir[index1];
        if (((int)eve_rec.flag & 16) != 0)
        {
            if (eve_rec.width >= (byte)64)
                eve_rec.width = (byte)0;
            if (!AppMain.GmGmkSwitchIsOn((uint)eve_rec.width))
                AppMain.gmGmkSpringSwitchOffInit(work);
            else
                AppMain.gmGmkSpringFwInit(work);
        }
        else
            AppMain.gmGmkSpringFwInit(work);
        if (AppMain.GsGetMainSysInfo().stage_id == (ushort)14)
        {
            work.obj_3d.use_light_flag = 0U;
            work.obj_3d.use_light_flag |= 64U;
        }
        else
        {
            int nMaterial = work.obj_3d._object.nMaterial;
            if (nMaterial == 1)
            {
                ((AppMain.NNS_MATERIAL_GLES11_DESC)work.obj_3d._object.pMatPtrList[0].pMaterial).fFlag |= 3U;
            }
            else
            {
                AppMain.NNS_MATERIAL_GLES11_DESC[] pMaterial = (AppMain.NNS_MATERIAL_GLES11_DESC[])work.obj_3d._object.pMatPtrList[0].pMaterial;
                for (int index2 = 0; index2 < nMaterial; ++index2)
                    pMaterial[index2].fFlag |= 3U;
            }
        }
        work.flag |= 1073741824U;
        return work;
    }

    public static void GmGmkSpringBuild()
    {
        AppMain.gm_gmk_spring_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(791), AppMain.GmGameDatGetGimmickData(792), 0U);
    }

    public static void GmGmkSpringFlush()
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(791));
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_spring_obj_3d_list, amsAmbHeader.file_num);
    }

    private static void gmGmkSpringFwInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.ObjDrawObjectActionSet(obj_work, obj_work.user_timer);
        obj_work.ppFunc = ((int)gmsEnemy3DWork.ene_com.eve_rec.flag & 16) == 0 ? new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSpringFwMain) : new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSpringSwitchOnMain);
        if (((int)gmsEnemy3DWork.ene_com.eve_rec.flag & 5) == 0)
            return;
        obj_work.disp_flag |= 32U;
    }

    private static void gmGmkSpringFwMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
    }

    private static void gmGmkSpringActInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.ObjDrawObjectActionSet(obj_work, (int)obj_work.user_work);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSpringActMain);
        if (((int)gmsEnemy3DWork.ene_com.eve_rec.flag & 4) != 0)
            obj_work.disp_flag |= 32U;
        else
            obj_work.disp_flag &= 4294967263U;
    }

    private static void gmGmkSpringActMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        gmsEnemy3DWork.ene_com.rect_work[2].flag &= 4294573823U;
        AppMain.gmGmkSpringFwInit(obj_work);
    }

    private static void gmGmkSpringDefFunc(
      AppMain.OBS_RECT_WORK mine_rect,
      AppMain.OBS_RECT_WORK match_rect)
    {
        AppMain.GMS_ENEMY_COM_WORK parentObj1 = (AppMain.GMS_ENEMY_COM_WORK)mine_rect.parent_obj;
        AppMain.GMS_PLAYER_WORK parentObj2 = (AppMain.GMS_PLAYER_WORK)match_rect.parent_obj;
        int fall_dir = -1;
        if (parentObj1 == null || parentObj2 == null || parentObj2.obj_work.obj_type != (ushort)1)
            return;
        AppMain.gmGmkSpringActInit((AppMain.OBS_OBJECT_WORK)parentObj1);
        int a = AppMain.MTM_MATH_CLIP((int)parentObj1.eve_rec.left, 0, 7);
        if (parentObj1.eve_rec.id == (ushort)76 || parentObj1.eve_rec.id == (ushort)72)
            a = AppMain.MTM_MATH_CLIP(a, 0, 5);
        int spd_x = 30720 + 6144 * a;
        int spd_y = -spd_x;
        if (parentObj1.eve_rec.id == (ushort)74 || parentObj1.eve_rec.id == (ushort)70)
            spd_x = 0;
        else if (parentObj1.eve_rec.id == (ushort)72 || parentObj1.eve_rec.id == (ushort)76)
        {
            spd_y = 0;
        }
        else
        {
            spd_x = spd_x * 181 >> 8;
            spd_y = spd_y * 181 >> 8;
        }
        if ((ushort)73 <= parentObj1.eve_rec.id && parentObj1.eve_rec.id <= (ushort)75)
            spd_y = -spd_y;
        if ((ushort)75 <= parentObj1.eve_rec.id && parentObj1.eve_rec.id <= (ushort)77 || (parentObj1.eve_rec.id == (ushort)79 || parentObj1.eve_rec.id == (ushort)101))
            spd_x = -spd_x;
        if ((parentObj1.eve_rec.id == (ushort)76 || parentObj1.eve_rec.id == (ushort)72) && ((int)parentObj1.eve_rec.flag & 2) == 0)
            parentObj2.obj_work.pos.y += 8192;
        if ((byte)1 <= parentObj1.eve_rec.height && parentObj1.eve_rec.height <= (byte)4)
            fall_dir = ((int)parentObj1.eve_rec.height - 1) * 16384;
        AppMain.GmPlySeqInitSpringJump(parentObj2, spd_x, spd_y, ((int)parentObj1.eve_rec.flag & 8) != 0, parentObj1.eve_rec.top >= (sbyte)0 ? (int)parentObj1.eve_rec.top * 4096 : 0, fall_dir, ((int)parentObj1.eve_rec.flag & 32) != 0);
        AppMain.GmComEfctCreateSpring(parentObj1.obj_work, ((int)mine_rect.rect.left + (int)mine_rect.rect.right) * 4096 / 2, ((int)mine_rect.rect.top + (int)mine_rect.rect.bottom) * 4096 / 2);
        if (((int)parentObj1.eve_rec.flag & 64) == 0 || ((int)AppMain.g_gs_main_sys_info.game_flag & 512) == 0)
            return;
        parentObj2.gmk_flag2 |= 512U;
    }

    private static void gmGmkSpringSwitchOffInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.ObjDrawObjectActionSet(obj_work, obj_work.user_timer);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSpringSwitchOffMain);
        obj_work.disp_flag |= 32U;
        obj_work.flag |= 2U;
    }

    private static void gmGmkSpringSwitchOffMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        if (!AppMain.GmGmkSwitchIsOn((uint)gmsEnemy3DWork.ene_com.eve_rec.width))
            return;
        obj_work.disp_flag &= 4294967263U;
        obj_work.flag &= 4294967293U;
        AppMain.GmComEfctCreateSpring(obj_work, ((int)gmsEnemy3DWork.ene_com.rect_work[2].rect.left + (int)gmsEnemy3DWork.ene_com.rect_work[2].rect.right) * 4096 / 2, ((int)gmsEnemy3DWork.ene_com.rect_work[2].rect.top + (int)gmsEnemy3DWork.ene_com.rect_work[2].rect.bottom) * 4096 / 2);
        AppMain.gmGmkSpringFwInit(obj_work);
    }

    private static void gmGmkSpringSwitchOnMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (AppMain.GmGmkSwitchIsOn((uint)((AppMain.GMS_ENEMY_3D_WORK)obj_work).ene_com.eve_rec.width))
            return;
        AppMain.gmGmkSpringSwitchOffInit(obj_work);
    }


}