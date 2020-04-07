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
    public static void GmGmkSwWallBuild()
    {
        AppMain.gm_gmk_sw_wall_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(934)), AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(935)), 0U);
    }

    public static void GmGmkSwWallFlush()
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(934));
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_sw_wall_obj_3d_list, amsAmbHeader.file_num);
    }

    public static AppMain.OBS_OBJECT_WORK GmGmkSwWallInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_SWWALL_WORK()), "GMK_SW_WALL");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.GMS_GMK_SWWALL_WORK gmsGmkSwwallWork = (AppMain.GMS_GMK_SWWALL_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_gmk_sw_wall_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        if ((ushort)256 <= eve_rec.id && eve_rec.id <= (ushort)259)
        {
            AppMain.ObjCopyAction3dNNModel(AppMain.gm_gmk_sw_wall_obj_3d_list[1], gmsGmkSwwallWork.obj_3d_opt[0]);
            AppMain.ObjCopyAction3dNNModel(AppMain.gm_gmk_sw_wall_obj_3d_list[2], gmsGmkSwwallWork.obj_3d_opt[1]);
            gmsGmkSwwallWork.obj_3d_opt[1].drawflag |= 32U;
            gmsEnemy3DWork.ene_com.enemy_flag |= 1U;
        }
        else
            gmsGmkSwwallWork.h_snd = AppMain.GsSoundAllocSeHandle();
        AppMain.mtTaskChangeTcbDestructor(work.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmGmkSwWallDest));
        work.pos.z = -655360;
        work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSwWallDispFunc);
        gmsGmkSwwallWork.id = (uint)AppMain.MTM_MATH_CLIP((int)eve_rec.left, 0, 64);
        ushort wall_type;
        ushort num;
        if ((ushort)248 <= eve_rec.id && eve_rec.id <= (ushort)251)
        {
            gmsGmkSwwallWork.wall_size = 524288;
            wall_type = (ushort)((uint)eve_rec.id - 248U);
            num = (ushort)32;
        }
        else if ((ushort)252 <= eve_rec.id && eve_rec.id <= (ushort)byte.MaxValue)
        {
            gmsGmkSwwallWork.wall_size = 1048576;
            wall_type = (ushort)((uint)eve_rec.id - 252U);
            num = (ushort)32;
        }
        else
        {
            gmsGmkSwwallWork.wall_size = 393216;
            wall_type = (ushort)((uint)eve_rec.id - 256U);
            num = (ushort)24;
            gmsGmkSwwallWork.gear_pos.Assign(work.pos);
            gmsGmkSwwallWork.gearbase_pos.Assign(work.pos);
            switch (eve_rec.id)
            {
                case 257:
                    gmsGmkSwwallWork.gear_pos.x -= -144179;
                    gmsGmkSwwallWork.gear_pos.y += 68812;
                    gmsGmkSwwallWork.gearbase_pos.x -= -196608;
                    gmsGmkSwwallWork.gearbase_pos.y += 49152;
                    gmsGmkSwwallWork.gear_base_dir = (ushort)2730;
                    gmsEnemy3DWork.ene_com.enemy_flag |= 4U;
                    break;
                case 258:
                    gmsGmkSwwallWork.gear_pos.x += 68812;
                    gmsGmkSwwallWork.gear_pos.y += -144179;
                    gmsGmkSwwallWork.gearbase_pos.x += 49152;
                    gmsGmkSwwallWork.gearbase_pos.y += -196608;
                    gmsGmkSwwallWork.gear_base_dir = (ushort)8192;
                    gmsEnemy3DWork.ene_com.enemy_flag |= 4U;
                    work.dir.z = (ushort)49152;
                    break;
                case 259:
                    gmsGmkSwwallWork.gear_pos.x += 68812;
                    gmsGmkSwwallWork.gear_pos.y -= -144179;
                    gmsGmkSwwallWork.gearbase_pos.x += 49152;
                    gmsGmkSwwallWork.gearbase_pos.y -= -196608;
                    gmsGmkSwwallWork.gear_base_dir = (ushort)2730;
                    work.dir.z = (ushort)49152;
                    break;
                default:
                    gmsGmkSwwallWork.gear_pos.x += -144179;
                    gmsGmkSwwallWork.gear_pos.y += 68812;
                    gmsGmkSwwallWork.gearbase_pos.x += -196608;
                    gmsGmkSwwallWork.gearbase_pos.y += 49152;
                    gmsGmkSwwallWork.gear_base_dir = (ushort)62806;
                    break;
            }
        }
        gmsGmkSwwallWork.wall_type = wall_type;
        if (AppMain.GmGmkSwitchTypeIsGear(gmsGmkSwwallWork.id) && (AppMain.GmGmkSwitchGetPer(gmsGmkSwwallWork.id) != 0 || !AppMain.GmGmkSwitchIsOn(gmsGmkSwwallWork.id)) && (AppMain.GmGmkSwitchGetPer(gmsGmkSwwallWork.id) != 4096 || AppMain.GmGmkSwitchIsOn(gmsGmkSwwallWork.id)))
        {
            int v2 = AppMain.GmGmkSwitchGetPer(gmsGmkSwwallWork.id);
            if (AppMain.GmGmkSwitchIsOn(gmsGmkSwwallWork.id))
            {
                if (((int)eve_rec.flag & 1) != 0)
                    v2 = 4096 - v2;
            }
            else if (((int)eve_rec.flag & 1) == 0)
                v2 = 4096 - v2;
            gmsGmkSwwallWork.wall_draw_size = AppMain.FX_Mul(gmsGmkSwwallWork.wall_size, v2);
        }
        else if (AppMain.GmGmkSwitchIsOn(gmsGmkSwwallWork.id) && ((int)eve_rec.flag & 1) != 0 || !AppMain.GmGmkSwitchIsOn(gmsGmkSwwallWork.id) && ((int)eve_rec.flag & 1) == 0)
            gmsGmkSwwallWork.wall_draw_size = gmsGmkSwwallWork.wall_size;
        AppMain.OBS_COLLISION_WORK colWork = gmsEnemy3DWork.ene_com.col_work;
        colWork.obj_col.obj = work;
        colWork.obj_col.dir_data = gmsGmkSwwallWork.col_dir_buf;
        colWork.obj_col.diff_data = AppMain.g_gm_default_col;
        colWork.obj_col.flag |= 402653216U;
        if (wall_type <= (ushort)1)
        {
            colWork.obj_col.height = num;
            colWork.obj_col.ofst_y = (short)-16;
        }
        else
        {
            colWork.obj_col.width = num;
            colWork.obj_col.ofst_x = (short)-16;
        }
        AppMain.gmGmkSwWallSetCol(colWork, gmsGmkSwwallWork.wall_size, gmsGmkSwwallWork.wall_draw_size, wall_type);
        work.move_flag |= 8448U;
        work.disp_flag |= 4194304U;
        gmsEnemy3DWork.ene_com.enemy_flag |= 16384U;
        if (gmsGmkSwwallWork.wall_draw_size == 0 || gmsGmkSwwallWork.wall_draw_size == gmsGmkSwwallWork.wall_size)
            AppMain.gmGmkSwWallFwInit(work);
        else if (AppMain.GmGmkSwitchIsOn(gmsGmkSwwallWork.id) && ((int)eve_rec.flag & 1) != 0 || !AppMain.GmGmkSwitchIsOn(gmsGmkSwwallWork.id) && ((int)eve_rec.flag & 1) == 0)
            AppMain.gmGmkSwWallCloseInit(work);
        else
            AppMain.gmGmkSwWallOpenInit(work);
        return work;
    }

    public static void gmGmkSwWallDest(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GMS_GMK_SWWALL_WORK tcbWork = (AppMain.GMS_GMK_SWWALL_WORK)AppMain.mtTaskGetTcbWork(tcb);
        if (((int)tcbWork.gmk_work.ene_com.enemy_flag & 2) != 0)
            AppMain.ObjAction3dNNMotionRelease(tcbWork.obj_3d_opt[1]);
        if (tcbWork.h_snd != null)
        {
            AppMain.GmSoundStopSE(tcbWork.h_snd);
            AppMain.GsSoundFreeSeHandle(tcbWork.h_snd);
            tcbWork.h_snd = (AppMain.GSS_SND_SE_HANDLE)null;
        }
        AppMain.GmEnemyDefaultExit(tcb);
    }

    public static void gmGmkSwWallFwInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SWWALL_WORK gmsGmkSwwallWork = (AppMain.GMS_GMK_SWWALL_WORK)obj_work;
        obj_work.flag &= 4294967279U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSwWallFwMain);
        obj_work.col_work.obj_col.obj = obj_work;
        if (gmsGmkSwwallWork.h_snd == null)
            return;
        AppMain.GmSoundStopSE(gmsGmkSwwallWork.h_snd);
    }

    public static void gmGmkSwWallFwMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SWWALL_WORK swwall_work = (AppMain.GMS_GMK_SWWALL_WORK)obj_work;
        AppMain.GMS_EVE_RECORD_EVENT eveRec = swwall_work.gmk_work.ene_com.eve_rec;
        if (swwall_work.wall_draw_size != 0)
        {
            if (((int)eveRec.flag & 1) == 0 && AppMain.GmGmkSwitchIsOn(swwall_work.id) || ((int)eveRec.flag & 1) != 0 && !AppMain.GmGmkSwitchIsOn(swwall_work.id))
                AppMain.gmGmkSwWallOpenInit(obj_work);
        }
        else if (((int)eveRec.flag & 1) != 0 && AppMain.GmGmkSwitchIsOn(swwall_work.id) || ((int)eveRec.flag & 1) == 0 && !AppMain.GmGmkSwitchIsOn(swwall_work.id))
            AppMain.gmGmkSwWallCloseInit(obj_work);
        AppMain.gmGmkSwWallCheckColOff(swwall_work);
    }

    public static void gmGmkSwWallOpenInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SWWALL_WORK gmsGmkSwwallWork = (AppMain.GMS_GMK_SWWALL_WORK)obj_work;
        gmsGmkSwwallWork.wall_spd = !AppMain.GmGmkSwitchTypeIsGear(gmsGmkSwwallWork.id) ? 16384 : 0;
        obj_work.flag |= 16U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSwWallOpenMain);
        obj_work.col_work.obj_col.obj = obj_work;
        if (gmsGmkSwwallWork.h_snd == null)
            return;
        AppMain.GmSoundStopSE(gmsGmkSwwallWork.h_snd);
        AppMain.GmSoundPlaySE("Boss3_01", gmsGmkSwwallWork.h_snd);
    }

    public static void gmGmkSwWallOpenMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SWWALL_WORK gmsGmkSwwallWork = (AppMain.GMS_GMK_SWWALL_WORK)obj_work;
        if (((int)gmsGmkSwwallWork.gmk_work.ene_com.eve_rec.flag & 1) != 0 && AppMain.GmGmkSwitchIsOn(gmsGmkSwwallWork.id) || ((int)gmsGmkSwwallWork.gmk_work.ene_com.eve_rec.flag & 1) == 0 && !AppMain.GmGmkSwitchIsOn(gmsGmkSwwallWork.id))
            AppMain.gmGmkSwWallCloseInit(obj_work);
        if (gmsGmkSwwallWork.wall_spd != 0)
        {
            gmsGmkSwwallWork.wall_draw_size -= gmsGmkSwwallWork.wall_spd;
        }
        else
        {
            int v2 = AppMain.GmGmkSwitchGetPer(gmsGmkSwwallWork.id);
            if (((int)gmsGmkSwwallWork.gmk_work.ene_com.eve_rec.flag & 1) != 0)
                v2 = 4096 - v2;
            gmsGmkSwwallWork.wall_draw_size = AppMain.FX_Mul(gmsGmkSwwallWork.wall_size, v2);
        }
        if (gmsGmkSwwallWork.wall_draw_size <= 0)
        {
            gmsGmkSwwallWork.wall_draw_size = 0;
            AppMain.gmGmkSwWallFwInit(obj_work);
        }
        gmsGmkSwwallWork.gear_dir = (ushort)(gmsGmkSwwallWork.wall_draw_size / 64 * 16);
        if (gmsGmkSwwallWork.gmk_work.ene_com.eve_rec.id == (ushort)257 || gmsGmkSwwallWork.gmk_work.ene_com.eve_rec.id == (ushort)258)
            gmsGmkSwwallWork.gear_dir = (ushort)-gmsGmkSwwallWork.gear_dir;
        AppMain.gmGmkSwWallSetCol(gmsGmkSwwallWork.gmk_work.ene_com.col_work, gmsGmkSwwallWork.wall_size, gmsGmkSwwallWork.wall_draw_size, gmsGmkSwwallWork.wall_type);
    }

    public static void gmGmkSwWallCloseInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SWWALL_WORK gmsGmkSwwallWork = (AppMain.GMS_GMK_SWWALL_WORK)obj_work;
        gmsGmkSwwallWork.wall_spd = !AppMain.GmGmkSwitchTypeIsGear(gmsGmkSwwallWork.id) ? 16384 : 0;
        obj_work.flag |= 16U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSwWallCloseMain);
        obj_work.col_work.obj_col.obj = obj_work;
        if (gmsGmkSwwallWork.h_snd == null)
            return;
        AppMain.GmSoundStopSE(gmsGmkSwwallWork.h_snd);
        AppMain.GmSoundPlaySE("Boss3_01", gmsGmkSwwallWork.h_snd);
    }

    public static void gmGmkSwWallCloseMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SWWALL_WORK swwall_work = (AppMain.GMS_GMK_SWWALL_WORK)obj_work;
        if (((int)swwall_work.gmk_work.ene_com.eve_rec.flag & 1) != 0 && !AppMain.GmGmkSwitchIsOn(swwall_work.id) || ((int)swwall_work.gmk_work.ene_com.eve_rec.flag & 1) == 0 && AppMain.GmGmkSwitchIsOn(swwall_work.id))
            AppMain.gmGmkSwWallOpenInit(obj_work);
        if (swwall_work.wall_spd != 0)
        {
            swwall_work.wall_draw_size += swwall_work.wall_spd;
        }
        else
        {
            int v2 = AppMain.GmGmkSwitchGetPer(swwall_work.id);
            if (((int)swwall_work.gmk_work.ene_com.eve_rec.flag & 1) != 0)
                v2 = 4096 - v2;
            swwall_work.wall_draw_size = AppMain.FX_Mul(swwall_work.wall_size, v2);
        }
        if (swwall_work.wall_draw_size >= swwall_work.wall_size)
        {
            swwall_work.wall_draw_size = swwall_work.wall_size;
            AppMain.gmGmkSwWallFwInit(obj_work);
        }
        swwall_work.gear_dir = (ushort)(swwall_work.wall_draw_size / 64 * 16);
        if (swwall_work.gmk_work.ene_com.eve_rec.id == (ushort)257 || swwall_work.gmk_work.ene_com.eve_rec.id == (ushort)258)
            swwall_work.gear_dir = (ushort)-swwall_work.gear_dir;
        AppMain.gmGmkSwWallSetCol(swwall_work.gmk_work.ene_com.col_work, swwall_work.wall_size, swwall_work.wall_draw_size, swwall_work.wall_type);
        AppMain.gmGmkSwWallCheckColOff(swwall_work);
    }

    public static void gmGmkSwWallDispFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SWWALL_WORK gmsGmkSwwallWork = (AppMain.GMS_GMK_SWWALL_WORK)obj_work;
        AppMain.VecFx32 vecFx32 = new AppMain.VecFx32();
        AppMain.VecFx32 scale = new AppMain.VecFx32();
        AppMain.VecU16 vecU16 = new AppMain.VecU16();
        int num1 = (gmsGmkSwwallWork.wall_draw_size + 131071) / 131072;
        AppMain.OBS_COLLISION_WORK colWork = gmsGmkSwwallWork.gmk_work.ene_com.col_work;
        vecFx32.Assign(obj_work.pos);
        int num2;
        int num3 = num2 = 0;
        if (gmsGmkSwwallWork.wall_type <= (ushort)1)
        {
            if (gmsGmkSwwallWork.wall_type == (ushort)0)
            {
                vecFx32.x += (int)colWork.obj_col.width + (int)colWork.obj_col.ofst_x - 16 << 12;
                num3 = -131072;
            }
            else
            {
                vecFx32.x += (int)colWork.obj_col.ofst_x + 16 << 12;
                num3 = 131072;
            }
        }
        else if (gmsGmkSwwallWork.wall_type == (ushort)2)
        {
            vecFx32.y += (int)colWork.obj_col.height + (int)colWork.obj_col.ofst_y - 16 << 12;
            num2 = -131072;
        }
        else
        {
            vecFx32.y += (int)colWork.obj_col.ofst_y + 16 << 12;
            num2 = 131072;
        }
        uint dispFlag = obj_work.disp_flag;
        if (((int)gmsGmkSwwallWork.gmk_work.ene_com.enemy_flag & 2) != 0)
        {
            while (num1 > 0)
            {
                AppMain.ObjDrawAction3DNN(obj_work.obj_3d, new AppMain.VecFx32?(vecFx32), new AppMain.VecU16?(obj_work.dir), obj_work.scale, ref dispFlag);
                --num1;
                vecFx32.x += num3;
                vecFx32.y += num2;
            }
        }
        else
        {
            while (num1 > 0)
            {
                AppMain.ObjDrawAction3DNN(obj_work.obj_3d, new AppMain.VecFx32?(vecFx32), new AppMain.VecU16?(obj_work.dir), obj_work.scale, ref dispFlag);
                --num1;
                vecFx32.x += num3;
                vecFx32.y += num2;
            }
        }
        if (((int)gmsGmkSwwallWork.gmk_work.ene_com.enemy_flag & 1) == 0)
            return;
        vecU16.x = vecU16.y = (ushort)0;
        vecU16.z = (ushort)((uint)gmsGmkSwwallWork.gear_dir + (uint)gmsGmkSwwallWork.gear_base_dir);
        AppMain.ObjDrawAction3DNN(gmsGmkSwwallWork.obj_3d_opt[0], new AppMain.VecFx32?(gmsGmkSwwallWork.gear_pos), new AppMain.VecU16?(vecU16), obj_work.scale, ref dispFlag);
        scale.x = scale.y = scale.z = 4096;
        if (((int)gmsGmkSwwallWork.gmk_work.ene_com.enemy_flag & 4) != 0)
            scale.x = -scale.x;
        AppMain.ObjDrawAction3DNN(gmsGmkSwwallWork.obj_3d_opt[1], new AppMain.VecFx32?(gmsGmkSwwallWork.gearbase_pos), new AppMain.VecU16?(obj_work.dir), scale, ref dispFlag);
    }

    public static void gmGmkSwWallSetCol(
      AppMain.OBS_COLLISION_WORK col_work,
      int size,
      int draw_size,
      ushort wall_type)
    {
        if (wall_type <= (ushort)1)
        {
            col_work.obj_col.width = (ushort)((ulong)((draw_size >> 12) + 7) & 4294967288UL);
            col_work.obj_col.ofst_x = wall_type != (ushort)0 ? (short)((size >> 1) - draw_size >> 12) : (short)((-size >> 13) - ((int)col_work.obj_col.width - (draw_size >> 12)));
            AppMain.gmGmkSwWallSetColDir(col_work.obj_col.dir_data, (int)col_work.obj_col.width + 7 >> 3, (int)col_work.obj_col.height + 7 >> 3, false);
        }
        else
        {
            col_work.obj_col.height = (ushort)((ulong)((draw_size >> 12) + 7) & 4294967288UL);
            col_work.obj_col.ofst_y = wall_type != (ushort)2 ? (short)((size >> 1) - draw_size >> 12) : (short)((-size >> 13) - ((int)col_work.obj_col.height - (draw_size >> 12)));
            AppMain.gmGmkSwWallSetColDir(col_work.obj_col.dir_data, (int)col_work.obj_col.width + 7 >> 3, (int)col_work.obj_col.height + 7 >> 3, true);
        }
    }

    public static void gmGmkSwWallSetColDir(byte[] buf, int width, int height, bool wall)
    {
        if (AppMain.g_gs_main_sys_info.stage_id != (ushort)9 || width <= 0 || height <= 0)
            return;
        int num1 = width >> 1;
        int num2 = height >> 1;
        if (wall)
        {
            AppMain.ArrayPointer<byte> arrayPointer1 = (AppMain.ArrayPointer<byte>)buf;
            int num3 = 0;
            while (num3 < height)
            {
                AppMain.ArrayPointer<byte> arrayPointer2 = arrayPointer1;
                int num4 = 0;
                while (num4 < num1)
                {
                    int num5 = (int)arrayPointer2.SetPrimitive((byte)192);
                    ++num4;
                    ++arrayPointer2;
                }
                while (num4 < width)
                {
                    int num5 = (int)arrayPointer2.SetPrimitive((byte)64);
                    ++num4;
                    ++arrayPointer2;
                }
                ++num3;
                arrayPointer1 += width;
            }
            arrayPointer1 = new AppMain.ArrayPointer<byte>(buf, 1);
            int num6 = 1;
            while (num6 < width - 1)
            {
                int num4 = (int)arrayPointer1.SetPrimitive((byte)0);
                ++num6;
                ++arrayPointer1;
            }
            if (height <= 1)
                return;
            arrayPointer1 = new AppMain.ArrayPointer<byte>(buf, (height - 1) * width + 1);
            int num7 = 1;
            while (num7 < width - 1)
            {
                int num4 = (int)arrayPointer1.SetPrimitive((byte)128);
                ++num7;
                ++arrayPointer1;
            }
        }
        else
        {
            AppMain.ArrayPointer<byte> arrayPointer = (AppMain.ArrayPointer<byte>)buf;
            int num3;
            for (num3 = 0; num3 < num2; ++num3)
            {
                int num4 = 0;
                while (num4 < width)
                {
                    int num5 = (int)arrayPointer.SetPrimitive((byte)0);
                    ++num4;
                    ++arrayPointer;
                }
            }
            for (; num3 < height; ++num3)
            {
                int num4 = 0;
                while (num4 < width)
                {
                    int num5 = (int)arrayPointer.SetPrimitive((byte)128);
                    ++num4;
                    ++arrayPointer;
                }
            }
            arrayPointer = new AppMain.ArrayPointer<byte>(buf, width);
            int num6 = 1;
            while (num6 < height - 1)
            {
                int num4 = (int)arrayPointer.SetPrimitive((byte)192);
                ++num6;
                arrayPointer += width;
            }
            if (width <= 1)
                return;
            arrayPointer = new AppMain.ArrayPointer<byte>(buf, width + (width - 1));
            int num7 = 1;
            while (num7 < height - 1)
            {
                int num4 = (int)arrayPointer.SetPrimitive((byte)64);
                ++num7;
                arrayPointer += width;
            }
        }
    }

    public static void gmGmkSwWallCheckColOff(AppMain.GMS_GMK_SWWALL_WORK swwall_work)
    {
        if (AppMain.g_gs_main_sys_info.stage_id != (ushort)9)
            return;
        AppMain.OBS_OBJECT_WORK obsObjectWork1 = (AppMain.OBS_OBJECT_WORK)swwall_work;
        AppMain.GMS_EVE_RECORD_EVENT eveRec = swwall_work.gmk_work.ene_com.eve_rec;
        if (eveRec.id != (ushort)248 && eveRec.id != (ushort)249 && (eveRec.id != (ushort)252 && eveRec.id != (ushort)253))
            return;
        AppMain.OBS_OBJECT_WORK obsObjectWork2 = (AppMain.OBS_OBJECT_WORK)AppMain.g_gm_main_system.ply_work[0];
        int num1 = obsObjectWork2.pos.x >> 12;
        int num2 = obsObjectWork2.pos.y >> 12;
        int num3 = obsObjectWork1.pos.x >> 12;
        int num4 = obsObjectWork1.pos.y >> 12;
        int num5 = swwall_work.wall_size >> 12;
        if (num3 - (num5 >> 1) < num1 + (int)obsObjectWork2.field_rect[2] && num3 + (num5 >> 1) > num1 + (int)obsObjectWork2.field_rect[0] && (num4 + (int)obsObjectWork1.col_work.obj_col.ofst_y + 8 <= num2 + (int)obsObjectWork2.field_rect[3] && num4 + (int)obsObjectWork1.col_work.obj_col.ofst_y + (int)obsObjectWork1.col_work.obj_col.height > num2 + (int)obsObjectWork2.field_rect[1]))
            obsObjectWork1.col_work.obj_col.obj = (AppMain.OBS_OBJECT_WORK)null;
        else
            obsObjectWork1.col_work.obj_col.obj = obsObjectWork1;
    }

}