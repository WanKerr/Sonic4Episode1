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
    private static void GmGmkPulleyBuild()
    {
        AppMain.gm_gmk_pulley_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(819), AppMain.GmGameDatGetGimmickData(820), 0U);
    }

    private static void GmGmkPulleyFlush()
    {
        AppMain.AMS_AMB_HEADER gimmickData = AppMain.GmGameDatGetGimmickData(819);
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_pulley_obj_3d_list, gimmickData.file_num);
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkPulleyBaseInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        pos_y -= 24576;
        AppMain.OBS_OBJECT_WORK rideWork = AppMain.GMM_ENEMY_CREATE_RIDE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_PULLEY_WORK()), "GMK_PULLEY_BASE");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)rideWork;
        ((AppMain.GMS_GMK_PULLEY_WORK)rideWork).se_handle = (AppMain.GSS_SND_SE_HANDLE)null;
        AppMain.mtTaskChangeTcbDestructor(rideWork.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmGmkPulleyBaseExit));
        AppMain.ObjObjectCopyAction3dNNModel(rideWork, AppMain.gm_gmk_pulley_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        AppMain.ObjObjectAction3dNNMotionLoad(rideWork, 0, false, AppMain.ObjDataGet(821), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        AppMain.ObjDrawObjectActionSet(rideWork, 0);
        rideWork.move_flag |= 8448U;
        rideWork.disp_flag |= 4194308U;
        gmsEnemy3DWork.ene_com.col_work.obj_col.flag |= 134217728U;
        gmsEnemy3DWork.ene_com.rect_work[0].flag &= 4294967291U;
        gmsEnemy3DWork.ene_com.rect_work[1].flag &= 4294967291U;
        AppMain.OBS_RECT_WORK pRec = gmsEnemy3DWork.ene_com.rect_work[2];
        pRec.ppHit = (AppMain.OBS_RECT_WORK_Delegate1)null;
        pRec.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkPulleyDefFunc);
        AppMain.ObjRectAtkSet(pRec, (ushort)0, (short)0);
        AppMain.ObjRectDefSet(pRec, (ushort)65534, (short)0);
        AppMain.ObjRectWorkSet(pRec, (short)-4, (short)9, (short)4, (short)24);
        pRec.flag |= 1024U;
        rideWork.pos.z = 0;
        rideWork.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_EFFECT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_EFFECT_3DNN_WORK()), rideWork, (ushort)0, "GMK_PULLEY_ROT");
        AppMain.GMS_EFFECT_3DNN_WORK gmsEffect3DnnWork = (AppMain.GMS_EFFECT_3DNN_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_gmk_pulley_obj_3d_list[1], gmsEffect3DnnWork.obj_3d);
        work.move_flag |= 8448U;
        work.disp_flag |= 4194304U;
        work.disp_flag &= 4294967039U;
        work.flag |= 16U;
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPulleyRotMain);
        ((AppMain.GMS_GMK_PULLEY_WORK)rideWork).efct_work = (AppMain.GMS_EFFECT_3DES_WORK)null;
        return rideWork;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkPulleyPoleLInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        pos_y -= 24576;
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "GMK_PULLEY_POLE_L");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_gmk_pulley_obj_3d_list[3], gmsEnemy3DWork.obj_3d);
        work.move_flag |= 8448U;
        work.disp_flag |= 4194304U;
        work.flag |= 2U;
        gmsEnemy3DWork.ene_com.col_work.obj_col.flag |= 134217728U;
        work.pos.z = -131072;
        work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPulleyDrawSetPoleL);
        return work;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkPulleyPoleRInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        pos_y -= 24576;
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "GMK_PULLEY_POLE_R");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_gmk_pulley_obj_3d_list[2], gmsEnemy3DWork.obj_3d);
        work.move_flag |= 8448U;
        work.disp_flag |= 4194304U;
        work.flag |= 2U;
        gmsEnemy3DWork.ene_com.col_work.obj_col.flag |= 134217728U;
        work.pos.z = -131072;
        work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPulleyDrawSetPoleR);
        return work;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkPulleyRopeFInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        pos_y -= 24576;
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "GMK_PULLEY_ROPE_F");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_gmk_pulley_obj_3d_list[4], gmsEnemy3DWork.obj_3d);
        work.move_flag |= 8448U;
        work.disp_flag |= 4194304U;
        work.flag |= 2U;
        gmsEnemy3DWork.ene_com.col_work.obj_col.flag |= 134217728U;
        work.pos.z = -131072;
        work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPulleyDrawSetRopeN);
        return work;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkPulleyRopeTInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        pos_y -= 24576;
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "GMK_PULLEY_ROPE_T");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_gmk_pulley_obj_3d_list[5], gmsEnemy3DWork.obj_3d);
        if (eve_rec.id == (ushort)95)
            work.dir.y = (ushort)32768;
        work.move_flag |= 8448U;
        work.disp_flag |= 4194304U;
        work.flag |= 2U;
        gmsEnemy3DWork.ene_com.col_work.obj_col.flag |= 134217728U;
        work.pos.z = -131072;
        work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        work.ppOut = eve_rec.id != (ushort)95 ? new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPulleyDrawSetRopeTR) : new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPulleyDrawSetRopeTL);
        return work;
    }

    public static void GmGmkPulleyDrawServerMain()
    {
        if (!AppMain.GmMainIsDrawEnable())
            return;
        AppMain.GMS_GMK_PULLEY_MANAGER gmkPulleyManager = AppMain.gm_gmk_pulley_manager;
        if (gmkPulleyManager.num <= 0U)
            return;
        AppMain.AMS_PARAM_DRAW_PRIMITIVE prim = AppMain.GlobalPool<AppMain.AMS_PARAM_DRAW_PRIMITIVE>.Alloc();
        prim.type = 1;
        prim.ablend = 0;
        prim.bldSrc = 768;
        prim.bldDst = 774;
        prim.bldMode = 32774;
        prim.aTest = (short)1;
        prim.zMask = (short)0;
        prim.zTest = (short)1;
        prim.noSort = (short)1;
        prim.texlist = gmkPulleyManager.texlist;
        prim.texId = (int)gmkPulleyManager.tex_id;
        uint lightColor = AppMain.GmMainGetLightColor();
        prim.uwrap = 1;
        prim.vwrap = 1;
        prim.count = (int)gmkPulleyManager.num * 6 - 2;
        prim.vtxPCT3D = AppMain.amDrawAlloc_NNS_PRIM3D_PCT(prim.count);
        AppMain.NNS_PRIM3D_PCT[] buffer = prim.vtxPCT3D.buffer;
        int offset = prim.vtxPCT3D.offset;
        prim.format3D = 4;
        AppMain.NNS_MATRIX nnsMatrix1 = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.NNS_VECTOR dst = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        for (int index1 = 0; (long)index1 < (long)gmkPulleyManager.num; ++index1)
        {
            int index2 = offset + index1 * 6;
            int index3 = offset + index1 * 6 - 1;
            int index4 = offset + index1 * 6 + 4;
            AppMain.GMS_GMK_PULLEY_REGISTER gmkPulleyRegister = gmkPulleyManager.reg[index1];
            Vector3 vector3;
            vector3.X = AppMain.FX_FX32_TO_F32(gmkPulleyRegister.vec.x);
            vector3.Y = AppMain.FX_FX32_TO_F32(gmkPulleyRegister.vec.y);
            vector3.Z = AppMain.FX_FX32_TO_F32(gmkPulleyRegister.vec.z);
            AppMain.NNS_VECTOR[] gmGmkPulleyPo = AppMain.gm_gmk_pulley_pos[(int)gmkPulleyRegister.type];
            AppMain.NNS_TEXCOORD[] nnsTexcoordArray = AppMain.gm_gmk_pulley_tex[(int)gmkPulleyRegister.type];
            if (gmkPulleyRegister.flip != (ushort)0)
                AppMain.nnMakeRotateYMatrix(nnsMatrix1, (int)gmkPulleyRegister.flip);
            float num = 0.0f;
            if (gmkPulleyRegister.type == (ushort)2)
                num = 2f;
            for (int index5 = 0; index5 < 4; ++index5)
            {
                if (gmkPulleyRegister.flip != (ushort)0)
                    AppMain.nnTransformVector(dst, nnsMatrix1, gmGmkPulleyPo[index5]);
                else
                    AppMain.nnCopyVector(dst, gmGmkPulleyPo[index5]);
                int index6 = index2 + index5;
                buffer[index6].Pos.x = dst.x + vector3.X;
                buffer[index6].Pos.y = dst.y - vector3.Y + num;
                buffer[index6].Pos.z = dst.z + vector3.Z;
                buffer[index6].Tex.u = nnsTexcoordArray[index5].u;
                buffer[index6].Tex.v = nnsTexcoordArray[index5].v;
                buffer[index6].Col = lightColor;
            }
            if (index1 != 0)
                buffer[index3] = buffer[index2];
            if ((long)index1 != (long)(gmkPulleyManager.num - 1U))
                buffer[index4] = buffer[index2 + 3];
        }
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(dst);
        AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix1);
        AppMain.NNS_MATRIX nnsMatrix2 = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.nnMakeUnitMatrix(nnsMatrix2);
        AppMain.amMatrixPush(nnsMatrix2);
        AppMain.ObjDraw3DNNDrawPrimitive(prim, 0U, 0, 0);
        AppMain.amMatrixPop();
        AppMain.GlobalPool<AppMain.AMS_PARAM_DRAW_PRIMITIVE>.Release(prim);
        AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix2);
        gmkPulleyManager.num = 0U;
    }

    private static void gmGmkPulleyBaseExit(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GMS_GMK_PULLEY_WORK tcbWork = (AppMain.GMS_GMK_PULLEY_WORK)AppMain.mtTaskGetTcbWork(tcb);
        if (tcbWork.se_handle != null)
        {
            AppMain.GsSoundStopSeHandle(tcbWork.se_handle);
            AppMain.GsSoundFreeSeHandle(tcbWork.se_handle);
            tcbWork.se_handle = (AppMain.GSS_SND_SE_HANDLE)null;
        }
        AppMain.GmEnemyDefaultExit(tcb);
    }

    private static void gmGmkPulleyDefFunc(
      AppMain.OBS_RECT_WORK mine_rect,
      AppMain.OBS_RECT_WORK match_rect)
    {
        AppMain.GMS_ENEMY_COM_WORK parentObj1 = (AppMain.GMS_ENEMY_COM_WORK)mine_rect.parent_obj;
        AppMain.GMS_PLAYER_WORK parentObj2 = (AppMain.GMS_PLAYER_WORK)match_rect.parent_obj;
        if (parentObj1 == null || parentObj2 == null || (parentObj2.obj_work.obj_type != (ushort)1 || parentObj2.gmk_obj == (AppMain.OBS_OBJECT_WORK)parentObj1))
            return;
        AppMain.GmPlySeqInitPulley(parentObj2, parentObj1);
        AppMain.ObjDrawObjectActionSet3DNN(parentObj1.obj_work, 3, 0);
        parentObj1.obj_work.dir.y = ((int)parentObj2.obj_work.disp_flag & 1) == 0 ? (ushort)0 : (ushort)32768;
        parentObj1.obj_work.user_flag = (uint)((ulong)parentObj1.obj_work.user_flag & 18446744073709518847UL);
        ((AppMain.OBS_OBJECT_WORK)parentObj1).ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPulleyMove);
        AppMain.ObjRectWorkSet(parentObj1.rect_work[2], (short)-32, (short)9, (short)32, (short)24);
    }

    private static void gmGmkPulleyMove(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.GMS_PLAYER_WORK ply_work = AppMain.g_gm_main_system.ply_work[0];
        if (ply_work.gmk_obj != obj_work)
        {
            AppMain.gmGmkPulleySecedeSet(obj_work, 0);
            obj_work.flag |= 2U;
            obj_work.user_timer = 36;
        }
        else
        {
            if (obj_work.spd.x > 0)
            {
                obj_work.spd.x -= 64;
                if (obj_work.spd.x < 0)
                    obj_work.spd.x = 0;
            }
            else
            {
                obj_work.spd.x += 64;
                if (obj_work.spd.x > 0)
                    obj_work.spd.x = 0;
            }
            if (((int)gmsEnemy3DWork.ene_com.eve_rec.flag & 2) != 0)
            {
                if (((int)gmsEnemy3DWork.ene_com.eve_rec.flag & 1) != 0)
                    obj_work.spd.x -= 128;
                else
                    obj_work.spd.x += 128;
            }
            if (((int)AppMain.g_gs_main_sys_info.game_flag & 1) != 0)
            {
                int num = AppMain.MTM_MATH_CLIP(AppMain.GmPlayerKeyGetGimmickRotZ(ply_work), -24576, 24576) * 160 / 24576;
                obj_work.spd.x += num;
            }
            else
            {
                int num = AppMain.MTM_MATH_CLIP(ply_work.key_rot_z, -24576, 24576) * 160 / 24576;
                obj_work.spd.x += num;
            }
            if (ply_work.act_state != 59 && obj_work.spd.x > -256 && obj_work.spd.x < 256)
                obj_work.user_flag |= 32768U;
            if (ply_work.act_state != 59 && ply_work.act_state != 66 || ((int)ply_work.obj_work.disp_flag & 8) != 0)
            {
                int act_state;
                int id;
                if (((int)ply_work.obj_work.disp_flag & 1) != 0)
                {
                    act_state = 56;
                    id = 0;
                }
                else
                {
                    act_state = 56;
                    id = 0;
                }
                if ((obj_work.spd.x < -256 || obj_work.spd.x > 256) && ((int)obj_work.user_flag & 32768) != 0)
                {
                    AppMain.GmPlayerActionChange(ply_work, 59);
                    AppMain.ObjDrawObjectActionSet3DNN(obj_work, 4, 0);
                    obj_work.user_flag = (uint)((ulong)obj_work.user_flag & 18446744073709518847UL);
                }
                else if (ply_work.act_state != act_state)
                {
                    AppMain.GmPlayerActionChange(ply_work, act_state);
                    ply_work.obj_work.disp_flag |= 4U;
                    AppMain.ObjDrawObjectActionSet3DNN(obj_work, id, 0);
                    obj_work.disp_flag |= 4U;
                }
            }
            obj_work.dir.z = (ushort)AppMain.MTM_MATH_CLIP((int)(short)(obj_work.spd.x / 4), -10240, 10240);
            gmsEnemy3DWork.ene_com.target_dp_dir.z = obj_work.dir.z;
            int pos_x1;
            int pos_x2;
            if (((int)gmsEnemy3DWork.ene_com.eve_rec.flag & 1) != 0)
            {
                pos_x1 = gmsEnemy3DWork.ene_com.born_pos_x - (int)gmsEnemy3DWork.ene_com.eve_rec.left * 64 * 4096;
                pos_x2 = gmsEnemy3DWork.ene_com.born_pos_x;
            }
            else
            {
                pos_x1 = gmsEnemy3DWork.ene_com.born_pos_x;
                pos_x2 = gmsEnemy3DWork.ene_com.born_pos_x + (int)gmsEnemy3DWork.ene_com.eve_rec.left * 64 * 4096;
            }
            if (obj_work.pos.x < pos_x1)
            {
                if (ply_work.obj_work.pos.x > pos_x1 + 32768)
                    ply_work.obj_work.pos.x = pos_x1 + 32768;
                AppMain.gmGmkPulleySonicTakeOffSet(ply_work, obj_work.spd.x);
                AppMain.gmGmkPulleySecedeSet(obj_work, pos_x1);
                obj_work.user_timer = 0;
            }
            else if (obj_work.pos.x > pos_x2)
            {
                if (ply_work.obj_work.pos.x < pos_x2 - 32768)
                    ply_work.obj_work.pos.x = pos_x2 - 32768;
                AppMain.gmGmkPulleySonicTakeOffSet(ply_work, obj_work.spd.x);
                AppMain.gmGmkPulleySecedeSet(obj_work, pos_x2);
                obj_work.user_timer = 0;
            }
            if (obj_work.ppFunc == new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPulleyMove) && AppMain.MTM_MATH_ABS(obj_work.spd.x) > 4096)
                AppMain.gmGmkPulleySparkInit(obj_work);
            else
                AppMain.gmGmkPulleySparkKill(obj_work);
            AppMain.ObjObjectMove(obj_work);
            if (((int)gmsEnemy3DWork.ene_com.eve_rec.flag & 2) == 0)
                return;
            int num1 = AppMain.MTM_MATH_ABS(gmsEnemy3DWork.ene_com.born_pos_x - obj_work.pos.x) / 2;
            obj_work.pos.y = gmsEnemy3DWork.ene_com.born_pos_y + num1;
        }
    }

    private static void gmGmkPulleySonicTakeOffSet(AppMain.GMS_PLAYER_WORK ply_work, int spd_x)
    {
        ply_work.obj_work.spd.y = 0;
        ply_work.obj_work.spd.z = 0;
        ply_work.obj_work.dir.z = (ushort)0;
        ply_work.obj_work.spd_m = spd_x;
        ply_work.obj_work.spd.y = -12288;
        AppMain.GmPlySeqChangeSequence(ply_work, 17);
        if (spd_x > 0)
        {
            if (ply_work.obj_work.spd.x < 16384)
                ply_work.obj_work.spd.x = 16384;
            else if (ply_work.obj_work.spd.x > 24576)
                ply_work.obj_work.spd.x = 24576;
        }
        else if (ply_work.obj_work.spd.x > -16384)
            ply_work.obj_work.spd.x = -16384;
        else if (ply_work.obj_work.spd.x < -24576)
            ply_work.obj_work.spd.x = -24576;
        ply_work.obj_work.spd.x /= 2;
        ply_work.obj_work.spd.y = -12288;
        AppMain.GmPlySeqSetJumpState(ply_work, 0, 7U);
    }

    private static void gmGmkPulleySecedeSet(AppMain.OBS_OBJECT_WORK obj_work, int pos_x)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        if (pos_x != 0)
            obj_work.pos.x = pos_x;
        obj_work.spd.x = 0;
        obj_work.spd.y = 0;
        obj_work.spd_m = 0;
        obj_work.dir.z = (ushort)0;
        gmsEnemy3DWork.ene_com.target_dp_dir.z = obj_work.dir.z;
        AppMain.ObjDrawObjectActionSet3DNN(obj_work, 5, 0);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPulleySecede);
        AppMain.ObjRectWorkSet(gmsEnemy3DWork.ene_com.rect_work[2], (short)-4, (short)9, (short)4, (short)24);
        AppMain.gmGmkPulleySparkKill(obj_work);
        AppMain.GMS_GMK_PULLEY_WORK gmsGmkPulleyWork = (AppMain.GMS_GMK_PULLEY_WORK)obj_work;
        if (gmsGmkPulleyWork.se_handle == null)
            return;
        AppMain.GsSoundStopSeHandle(gmsGmkPulleyWork.se_handle);
        AppMain.GsSoundFreeSeHandle(gmsGmkPulleyWork.se_handle);
        gmsGmkPulleyWork.se_handle = (AppMain.GSS_SND_SE_HANDLE)null;
    }

    private static void gmGmkPulleySecede(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.user_timer != 0)
        {
            --obj_work.user_timer;
            if (obj_work.user_timer == 0)
                obj_work.flag &= 4294967293U;
        }
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        AppMain.ObjDrawObjectActionSet3DNN(obj_work, 0, 0);
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
    }

    private static void gmGmkPulleyRotMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.OBS_OBJECT_WORK parentObj = obj_work.parent_obj;
        obj_work.pos.Assign(parentObj.pos);
        ushort num = (ushort)(parentObj.spd.x >> 1);
        obj_work.dir.z += num;
    }

    private static void gmGmkPulleySparkInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.GMS_GMK_PULLEY_WORK gmsGmkPulleyWork1 = (AppMain.GMS_GMK_PULLEY_WORK)obj_work;
        if (gmsGmkPulleyWork1.efct_work != null)
            return;
        short dir_y = 0;
        short dir_z = 0;
        gmsGmkPulleyWork1.efct_work = AppMain.GmEfctZoneEsCreate(obj_work, 0, 6);
        if (obj_work.spd.x < 0)
        {
            dir_z = (short)-16384;
            AppMain.GmComEfctAddDispOffsetF(gmsGmkPulleyWork1.efct_work, 3f, 0.0f, 0.0f);
        }
        if (((int)gmsEnemy3DWork.ene_com.eve_rec.flag & 2) != 0)
        {
            if (((int)gmsEnemy3DWork.ene_com.eve_rec.flag & 1) != 0)
                dir_z += (short)4836;
            else
                dir_z += (short)-4836;
        }
        AppMain.GmComEfctAddDispRotationS(gmsGmkPulleyWork1.efct_work, (short)0, dir_y, dir_z);
        AppMain.GMS_GMK_PULLEY_WORK gmsGmkPulleyWork2 = (AppMain.GMS_GMK_PULLEY_WORK)obj_work;
        if (gmsGmkPulleyWork2.se_handle == null || gmsGmkPulleyWork2.se_handle.au_player.sound[0] == null)
        {
            gmsGmkPulleyWork2.se_handle = AppMain.GsSoundAllocSeHandle();
            AppMain.GmSoundPlaySE("Pulley", gmsGmkPulleyWork2.se_handle);
        }
        else
            gmsGmkPulleyWork2.se_handle.snd_ctrl_param.volume = 1f;
    }

    private static void gmGmkPulleySparkKill(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_PULLEY_WORK gmsGmkPulleyWork = (AppMain.GMS_GMK_PULLEY_WORK)obj_work;
        if (gmsGmkPulleyWork.efct_work == null)
            return;
        AppMain.ObjDrawKillAction3DES((AppMain.OBS_OBJECT_WORK)gmsGmkPulleyWork.efct_work);
        gmsGmkPulleyWork.efct_work = (AppMain.GMS_EFFECT_3DES_WORK)null;
        ((AppMain.GMS_GMK_PULLEY_WORK)obj_work).se_handle.snd_ctrl_param.volume = 0.0f;
    }

    private static void gmGmkPulleyDrawSetRopeN(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.gmGmkPulleyDrawSetObject(obj_work, 0);
    }

    private static void gmGmkPulleyDrawSetRopeTL(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.gmGmkPulleyDrawSetObject(obj_work, 1);
    }

    private static void gmGmkPulleyDrawSetRopeTR(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.gmGmkPulleyDrawSetObject(obj_work, 2);
    }

    private static void gmGmkPulleyDrawSetPoleL(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.gmGmkPulleyDrawSetObject(obj_work, 3);
    }

    private static void gmGmkPulleyDrawSetPoleR(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.gmGmkPulleyDrawSetObject(obj_work, 4);
    }

    private static void gmGmkPulleyDrawSetObject(AppMain.OBS_OBJECT_WORK obj_work, int type)
    {
        if (!AppMain.GmMainIsDrawEnable() || ((int)obj_work.disp_flag & 32) != 0)
            return;
        AppMain.GMS_GMK_PULLEY_MANAGER gmkPulleyManager = AppMain.gm_gmk_pulley_manager;
        gmkPulleyManager.texlist = obj_work.obj_3d.texlist;
        gmkPulleyManager.tex_id = 0U;
        AppMain.GMS_GMK_PULLEY_REGISTER gmkPulleyRegister = gmkPulleyManager.reg[(int)gmkPulleyManager.num];
        gmkPulleyRegister.type = (ushort)type;
        gmkPulleyRegister.flip = obj_work.dir.y;
        gmkPulleyRegister.vec.Assign(obj_work.pos);
        ++gmkPulleyManager.num;
    }

}