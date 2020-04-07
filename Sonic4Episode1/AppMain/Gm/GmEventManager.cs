using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

public partial class AppMain
{
    private static void GmEventMgrInit()
    {
        AppMain.gm_eve_mgr_tcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.gmEveMgrMain), new AppMain.GSF_TASK_PROCEDURE(AppMain.gmEveMgrDest), 0U, (ushort)0, 4240U, 5, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_EVE_MGR_WORK()), "GM_EVT_MGR");
        AppMain.GMS_EVE_MGR_WORK work = (AppMain.GMS_EVE_MGR_WORK)AppMain.gm_eve_mgr_tcb.work;
        work.Clear();
        AppMain.g_gm_eve_mgr_work = work;
        AppMain.g_gm_eve_mgr_work.map_size[0] = (ushort)((uint)AppMain.gm_eve_data.width << 8);
        AppMain.g_gm_eve_mgr_work.map_size[1] = (ushort)((uint)AppMain.gm_eve_data.height << 8);
    }

    private static void GmEventMgrStart()
    {
        AppMain.GmEventMgrCreateEventEnforce();
        AppMain.g_gm_eve_mgr_work.sts_proc = new AppMain._sts_proc_(AppMain.gmEveMgrStateFuncSingleScr);
    }

    private static void GmEventMgrCreateEventEnforce()
    {
        int[] r_on = new int[4]
        {
      0,
      0,
      (int) AppMain.g_gm_eve_mgr_work.map_size[0] - 1,
      0
        };
        r_on[1] = 0;
        r_on[3] = (int)AppMain.g_gm_eve_mgr_work.map_size[1] - 1;
        for (ushort by = 0; (int)by < (int)AppMain.gm_eve_data.height; ++by)
        {
            for (ushort bx = 0; (int)bx < (int)AppMain.gm_eve_data.width; ++bx)
                AppMain.gmEveMgrCreateEventBlkEvent(4U, bx, by, r_on, (int[])null);
        }
    }

    public static void GmEventMgrExit()
    {
        if (AppMain.gm_eve_mgr_tcb == null)
            return;
        AppMain.mtTaskClearTcb(AppMain.gm_eve_mgr_tcb);
    }

    private static void gmEveMgrMain(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GMS_EVE_MGR_WORK work = (AppMain.GMS_EVE_MGR_WORK)tcb.work;
        if (work.sts_proc != null)
            work.sts_proc();
        AppMain.OBS_CAMERA obsCamera = AppMain.ObjCameraGet(AppMain.g_obj.glb_camera_id);
        if (obsCamera == null)
            return;
        work.prev_pos[0] = obsCamera.disp_pos.x;
        work.prev_pos[1] = obsCamera.disp_pos.y;
    }

    private static void gmEveMgrDest(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.g_gm_eve_mgr_work = (AppMain.GMS_EVE_MGR_WORK)null;
        AppMain.gm_eve_mgr_tcb = (AppMain.MTS_TASK_TCB)null;
    }

    private static void gmEveMgrCreateEventBlkEvent(
      uint flag,
      ushort bx,
      ushort by,
      int[] r_on,
      int[] r_off)
    {
        uint num1 = (uint)bx + (uint)AppMain.gm_eve_data.width * (uint)by;
        AppMain.GMS_EVE_DATA_EV_LIST ev = AppMain.gm_eve_data.ev_list[(int)num1];
        ushort eveNum = ev.eve_num;
        AppMain.ArrayPointer<AppMain.GMS_EVE_RECORD_EVENT> arrayPointer = new AppMain.ArrayPointer<AppMain.GMS_EVE_RECORD_EVENT>(ev.eve_rec);
        int num2 = (int)bx << 8;
        int num3 = (int)by << 8;
        int num4 = 0;
        while (num4 < (int)eveNum)
        {
            if ((((int)flag & 4) == 0 || ((int)arrayPointer[0].flag & 32768) != 0) && arrayPointer[0].pos_x != byte.MaxValue)
            {
                int num5 = (int)arrayPointer[0].pos_x + num2;
                int num6 = (int)arrayPointer[0].pos_y + num3;
                int num7 = (int)AppMain.g_gm_event_size_tbl[(int)arrayPointer[0].id];
                int num8 = num7 + 16 + 32;
                if (((int)flag & 1) == 0 || num5 >= r_on[0] - num8 && num5 <= r_on[2] + num8 && (num6 >= r_on[1] - num8 && num6 <= r_on[3] + num8))
                {
                    int num9 = num7 + 16;
                    if ((((int)flag & 2) == 0 || num5 <= r_off[0] - num9 || (num5 >= r_off[2] + num9 || num6 <= r_off[1] - num9) || num6 >= r_off[3] + num9) && (arrayPointer[0].id < (ushort)346 && AppMain.g_gm_event_tbl[(int)arrayPointer[0].id] != null))
                    {
                        if (((int)arrayPointer[0].flag & 4096 << (int)(ushort)AppMain.GsGetGameLevel()) != 0)
                        {
                            arrayPointer[0].pos_x = byte.MaxValue;
                        }
                        else
                        {
                            AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.g_gm_event_tbl[(int)arrayPointer[0].id](arrayPointer[0], num5 << 12, num6 << 12, (byte)0);
                        }
                    }
                }
            }
            ++num4;
            ++arrayPointer;
        }
    }

    private static void gmEveMgrStateFuncSingleScr()
    {
        AppMain.OBS_CAMERA obsCamera = AppMain.ObjCameraGet(AppMain.g_obj.glb_camera_id);
        if (obsCamera == null || (double)obsCamera.disp_pos.x == (double)AppMain.g_gm_eve_mgr_work.prev_pos[0] && (double)obsCamera.disp_pos.y == (double)AppMain.g_gm_eve_mgr_work.prev_pos[1])
            return;
        AppMain.GmEveMgrCreateEventLcd(3U);
    }

    private static void GmEveMgrCreateEventLcd(uint flag)
    {
        int num1 = AppMain._g_obj.clip_camera[0] >> 12;
        int num2 = AppMain._g_obj.clip_camera[1] >> 12;
        int num3 = (int)AppMain._g_obj.clip_lcd_size[0];
        int num4 = (int)AppMain._g_obj.clip_lcd_size[1];
        AppMain.lcd_rect[0] = num1;
        AppMain.lcd_rect[2] = num1 + num3;
        AppMain.lcd_rect[1] = num2;
        AppMain.lcd_rect[3] = num2 + num4;
        AppMain.lcd_rect[0] = AppMain.MTM_MATH_CLIP(AppMain.lcd_rect[0], 0, (int)AppMain.g_gm_eve_mgr_work.map_size[0] - 1);
        AppMain.lcd_rect[1] = AppMain.MTM_MATH_CLIP(AppMain.lcd_rect[1], 0, (int)AppMain.g_gm_eve_mgr_work.map_size[1] - 1);
        AppMain.lcd_rect[2] = AppMain.MTM_MATH_CLIP(AppMain.lcd_rect[2], 0, (int)AppMain.g_gm_eve_mgr_work.map_size[0] - 1);
        AppMain.lcd_rect[3] = AppMain.MTM_MATH_CLIP(AppMain.lcd_rect[3], 0, (int)AppMain.g_gm_eve_mgr_work.map_size[1] - 1);
        for (int index = 0; index < 3; ++index)
        {
            AppMain._eve_func_ eveFunc = AppMain.gm_evemgr_create_eve_func_tbl[index];
            int num5 = (int)AppMain.gm_evemgr_create_size_tbl[index];
            AppMain.ev_rect[0] = AppMain.lcd_rect[0];
            AppMain.ev_rect[2] = AppMain.lcd_rect[2];
            AppMain.ev_rect[1] = AppMain.lcd_rect[1];
            AppMain.ev_rect[3] = AppMain.lcd_rect[3];
            AppMain.block_rect[0] = AppMain.ev_rect[0] - 16 - num5 - (int)byte.MaxValue >> 8;
            AppMain.block_rect[2] = AppMain.ev_rect[2] + 16 + num5 + (int)byte.MaxValue >> 8;
            AppMain.block_rect[1] = AppMain.ev_rect[1] - 16 - num5 - (int)byte.MaxValue >> 8;
            AppMain.block_rect[3] = AppMain.ev_rect[3] + 16 + num5 + (int)byte.MaxValue >> 8;
            if (AppMain.block_rect[0] < 0)
                AppMain.block_rect[0] = 0;
            if (AppMain.block_rect[2] >= (int)AppMain.gm_eve_data.width)
                AppMain.block_rect[2] = (int)AppMain.gm_eve_data.width - 1;
            if (AppMain.block_rect[1] < 0)
                AppMain.block_rect[1] = 0;
            if (AppMain.block_rect[3] >= (int)AppMain.gm_eve_data.height)
                AppMain.block_rect[3] = (int)AppMain.gm_eve_data.height - 1;
            for (ushort by = (ushort)AppMain.block_rect[1]; (int)by <= (int)(ushort)AppMain.block_rect[3]; ++by)
            {
                for (ushort bx = (ushort)AppMain.block_rect[0]; (int)bx <= (int)(ushort)AppMain.block_rect[2]; ++bx)
                    eveFunc(flag, bx, by, AppMain.ev_rect, AppMain.lcd_rect);
            }
        }
    }

    private static void gmEveMgrCreateEventBlkRing(
      uint flag,
      ushort bx,
      ushort by,
      int[] r_on,
      int[] r_off)
    {
        uint num1 = (uint)bx + (uint)AppMain.gm_ring_data.width * (uint)by;
        AppMain.GMS_EVE_DATA_RG_LIST gmsEveDataRgList = AppMain.gm_ring_data.ring[(int)num1];
        ushort ringNum = gmsEveDataRgList.ring_num;
        AppMain.ArrayPointer<AppMain.GMS_EVE_RECORD_RING> arrayPointer = new AppMain.ArrayPointer<AppMain.GMS_EVE_RECORD_RING>(gmsEveDataRgList.ring_data);
        int num2 = (int)bx << 8;
        int num3 = (int)by << 8;
        int num4 = AppMain.GMD_RING_SIZE + 16 + 32;
        for (ushort index = 0; (int)index < (int)ringNum; ++index)
        {
            if (arrayPointer[0].pos_x != byte.MaxValue)
            {
                int num5 = (int)arrayPointer[0].pos_x + num2;
                int num6 = (int)arrayPointer[0].pos_y + num3;
                if (num5 >= r_on[0] - num4 && num5 <= r_on[2] + num4 && (num6 >= r_on[1] - num4 && num6 <= r_on[3] + num4))
                    AppMain.GmRingCreate((AppMain.GMS_EVE_RECORD_RING)arrayPointer, ((int)bx << 8) + (int)arrayPointer[0].pos_x << 12, ((int)by << 8) + (int)arrayPointer[0].pos_y << 12, 0);
            }
            ++arrayPointer;
        }
    }

    private static void gmEveMgrCreateEventBlkDecorate(
      uint flag,
      ushort bx,
      ushort by,
      int[] r_on,
      int[] r_off)
    {
        uint num1 = (uint)bx + (uint)AppMain.gm_deco_data.width * (uint)by;
        AppMain.GMS_EVE_DATA_DC_LIST dc = AppMain.gm_deco_data.dc_list[(int)num1];
        ushort decNum = dc.dec_num;
        AppMain.ArrayPointer<AppMain.GMS_EVE_RECORD_DECORATE> arrayPointer = new AppMain.ArrayPointer<AppMain.GMS_EVE_RECORD_DECORATE>(dc.dec_data);
        int num2 = (int)bx << 8;
        int num3 = (int)by << 8;
        int num4 = 0;
        while (num4 < (int)decNum)
        {
            if (arrayPointer[0].pos_x != byte.MaxValue)
            {
                int num5 = (int)arrayPointer[0].pos_x + num2;
                int num6 = (int)arrayPointer[0].pos_y + num3;
                int num7 = (int)AppMain.g_gm_decorate_size_tbl[(int)arrayPointer[0].id];
                int num8 = num7 + 16 + 32;
                if (((int)flag & 1) == 0 || num5 >= r_on[0] - num8 && num5 <= r_on[2] + num8 && (num6 >= r_on[1] - num8 && num6 <= r_on[3] + num8))
                {
                    int num9 = num7 + 16;
                    if ((((int)flag & 2) == 0 || num5 <= r_off[0] - num9 || (num5 >= r_off[2] + num9 || num6 <= r_off[1] - num9) || num6 >= r_off[3] + num9) && (arrayPointer[0].id < (ushort)184 && AppMain.g_gm_decorate_tbl[(int)arrayPointer[0].id] != null))
                    {
                        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.g_gm_decorate_tbl[(int)arrayPointer[0].id]((AppMain.GMS_EVE_RECORD_DECORATE)arrayPointer, num5 << 12, num6 << 12, (byte)0);
                    }
                }
            }
            ++num4;
            ++arrayPointer;
        }
    }

    private static void GmEventDataBuild()
    {
        AppMain.AMS_AMB_HEADER gGmGamedat = (AppMain.AMS_AMB_HEADER)AppMain.g_gm_gamedat_map[0];
        AppMain.gm_eve_data = new AppMain.GMS_EVE_DATA_EV_HEADER((AppMain.AmbChunk)AppMain.amBindGet(gGmGamedat, 6));
        AppMain.gm_ring_data = new AppMain.GMS_EVE_DATA_RG_HEADER((AppMain.AmbChunk)AppMain.amBindGet(gGmGamedat, 8));
        AppMain.gm_deco_data = new AppMain.GMS_EVE_DATA_DC_HEADER((AppMain.AmbChunk)AppMain.amBindGet(gGmGamedat, 7));
    }

    private static void GmEventDataFlush()
    {
        if (AppMain.gm_eve_data != null)
            AppMain.gm_eve_data = (AppMain.GMS_EVE_DATA_EV_HEADER)null;
        if (AppMain.gm_ring_data != null)
            AppMain.gm_ring_data = (AppMain.GMS_EVE_DATA_RG_HEADER)null;
        if (AppMain.gm_deco_data == null)
            return;
        AppMain.gm_deco_data = (AppMain.GMS_EVE_DATA_DC_HEADER)null;
    }

    private static void GmEveMgrCreateStateEvent()
    {
        AppMain.GmEveMgrCreateEventLcd(1U);
    }

    private static void GmEventMgrCreateEventInRect(
      ushort left,
      ushort top,
      ushort right,
      ushort bottom)
    {
        int[] r_on = new int[4];
        ushort[] numArray = new ushort[4];
        r_on[0] = (int)left;
        if (r_on[0] > (int)AppMain.g_gm_eve_mgr_work.map_size[0] - 1)
            r_on[0] = (int)AppMain.g_gm_eve_mgr_work.map_size[0] - 1;
        r_on[1] = (int)top;
        if (r_on[1] > (int)AppMain.g_gm_eve_mgr_work.map_size[1] - 1)
            r_on[1] = (int)AppMain.g_gm_eve_mgr_work.map_size[1] - 1;
        r_on[2] = (int)right;
        if (r_on[2] > (int)AppMain.g_gm_eve_mgr_work.map_size[0] - 1)
            r_on[2] = (int)AppMain.g_gm_eve_mgr_work.map_size[0] - 1;
        r_on[3] = (int)bottom;
        if (r_on[3] > (int)AppMain.g_gm_eve_mgr_work.map_size[1] - 1)
            r_on[3] = (int)AppMain.g_gm_eve_mgr_work.map_size[1] - 1;
        numArray[0] = (ushort)(r_on[0] - (int)byte.MaxValue >> 8);
        numArray[2] = (ushort)(r_on[2] + (int)byte.MaxValue >> 8);
        numArray[1] = (ushort)(r_on[1] - (int)byte.MaxValue >> 8);
        numArray[3] = (ushort)(r_on[3] + (int)byte.MaxValue >> 8);
        for (int index = 0; index < 3; ++index)
        {
            AppMain._eve_func_ eveFunc = AppMain.gm_evemgr_create_eve_func_tbl[index];
            for (ushort by = numArray[1]; (int)by <= (int)numArray[3]; ++by)
            {
                for (ushort bx = numArray[0]; (int)bx <= (int)numArray[2]; ++bx)
                    eveFunc(1U, bx, by, r_on, (int[])null);
            }
        }
    }

    private static void GmEveMgrCreateEventAll()
    {
        int[] r_on = new int[4]
        {
      0,
      0,
      (int) AppMain.g_gm_eve_mgr_work.map_size[0] - 1,
      0
        };
        r_on[1] = 0;
        r_on[3] = (int)AppMain.g_gm_eve_mgr_work.map_size[1] - 1;
        for (int index = 0; index < 3; ++index)
        {
            AppMain._eve_func_ eveFunc = AppMain.gm_evemgr_create_eve_func_tbl[index];
            for (ushort by = 0; (int)by < (int)AppMain.gm_eve_data.height; ++by)
            {
                for (ushort bx = 0; (int)bx < (int)AppMain.gm_eve_data.width; ++bx)
                    eveFunc(0U, bx, by, r_on, (int[])null);
            }
        }
    }

    private static AppMain.OBS_OBJECT_WORK GmEventMgrLocalEventBirth(
      ushort id,
      int pos_x,
      int pos_y,
      ushort flag,
      sbyte left,
      sbyte top,
      byte width,
      byte height,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        short num = AppMain.gmEventMgrLocalEventNoGet(0);
        if (num != (short)-1)
        {
            AppMain.gm_eve_local_evt_record[(int)num].pos_x = byte.MaxValue;
            AppMain.gm_eve_local_evt_record[(int)num].pos_y = byte.MaxValue;
            AppMain.gm_eve_local_evt_record[(int)num].id = id;
            AppMain.gm_eve_local_evt_record[(int)num].flag = flag;
            AppMain.gm_eve_local_evt_record[(int)num].left = left;
            AppMain.gm_eve_local_evt_record[(int)num].top = top;
            AppMain.gm_eve_local_evt_record[(int)num].width = width;
            AppMain.gm_eve_local_evt_record[(int)num].height = height;
            AppMain.gm_eve_local_evt_record[(int)num].word_param = (ushort)0;
            obsObjectWork = AppMain.g_gm_event_tbl[(int)id](AppMain.gm_eve_local_evt_record[(int)num], pos_x, pos_y, type);
            if (obsObjectWork == null)
                AppMain.GmEventMgrLocalEventRelease(AppMain.gm_eve_local_evt_record[(int)num]);
        }
        return obsObjectWork;
    }

    private static void GmEventMgrLocalRingBirth(int pos_x, int pos_y, byte type)
    {
        short num = AppMain.gmEventMgrLocalEventNoGet(1);
        if (num == (short)-1)
            return;
        AppMain.gm_eve_local_ring_record[(int)num].pos_x = byte.MaxValue;
        AppMain.gm_eve_local_ring_record[(int)num].pos_y = byte.MaxValue;
        AppMain.GmRingCreate(AppMain.gm_eve_local_ring_record[(int)num], pos_x, pos_y, (int)type);
    }

    private static AppMain.OBS_OBJECT_WORK GmEventMgrLocalDecoBirth(
      ushort id,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        short num = AppMain.gmEventMgrLocalEventNoGet(2);
        if (num != (short)-1)
        {
            AppMain.gm_eve_local_deco_record[(int)num].pos_x = byte.MaxValue;
            AppMain.gm_eve_local_deco_record[(int)num].pos_y = byte.MaxValue;
            AppMain.gm_eve_local_deco_record[(int)num].id = id;
            obsObjectWork = AppMain.g_gm_decorate_tbl[(int)id](AppMain.gm_eve_local_deco_record[(int)num], pos_x, pos_y, type);
            if (obsObjectWork == null)
                AppMain.GmEventMgrLocalDecoRelease(AppMain.gm_eve_local_deco_record[(int)num]);
        }
        return obsObjectWork;
    }

    private static short gmEventMgrLocalEventNoGet(int eve_type)
    {
        uint[] numArray;
        short num;
        switch (eve_type)
        {
            case 0:
                numArray = AppMain.gm_eve_local_evt_obj_use_flag;
                num = (short)2;
                break;
            case 1:
                numArray = AppMain.gm_eve_local_ring_obj_use_flag;
                num = (short)2;
                break;
            case 2:
                numArray = AppMain.gm_eve_local_deco_obj_use_flag;
                num = (short)2;
                break;
            default:
                return 0;
        }
        for (short index1 = 0; (int)index1 < (int)num; ++index1)
        {
            if (numArray[(int)index1] < uint.MaxValue)
            {
                for (short index2 = 0; index2 < (short)32; ++index2)
                {
                    if (0L == ((long)numArray[(int)index1] & (long)(1 << (int)index2)))
                    {
                        numArray[(int)index1] |= 1U << (int)index2;
                        return (short)((int)index1 * 32 + (int)index2);
                    }
                }
            }
        }
        return -1;
    }

    private static void GmEventMgrLocalEventRelease(AppMain.GMS_EVE_RECORD_EVENT eve_rec)
    {
        int num = Array.IndexOf<AppMain.GMS_EVE_RECORD_EVENT>(AppMain.gm_eve_local_evt_record, eve_rec);
        AppMain.gm_eve_local_evt_obj_use_flag[num >> 5] &= (uint)~(1 << num);
    }

    private static void GmEventMgrLocalRingRelease(AppMain.GMS_EVE_RECORD_RING eve_rec)
    {
        int num = Array.IndexOf<AppMain.GMS_EVE_RECORD_RING>(AppMain.gm_eve_local_ring_record, eve_rec);
        AppMain.gm_eve_local_ring_obj_use_flag[num >> 5] &= (uint)~(1 << num);
    }

    private static void GmEventMgrLocalDecoRelease(AppMain.GMS_EVE_RECORD_DECORATE eve_rec)
    {
        int num = Array.IndexOf<AppMain.GMS_EVE_RECORD_DECORATE>(AppMain.gm_eve_local_deco_record, eve_rec);
        AppMain.gm_eve_local_deco_obj_use_flag[num >> 5] &= (uint)~(1 << num);
    }

    private static uint GmEventMgrGetRingNum()
    {
        uint num1 = 0;
        if (AppMain.gm_ring_data != null)
        {
            uint num2 = (uint)AppMain.gm_ring_data.width * (uint)AppMain.gm_ring_data.height;
            for (uint index = 0; index < num2; ++index)
                num1 += (uint)AppMain.gm_ring_data.ring[(int)index].ring_num;
        }
        return num1;
    }
}