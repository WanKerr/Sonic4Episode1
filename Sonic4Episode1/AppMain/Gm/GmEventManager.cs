using System;

public partial class AppMain
{
    private static void GmEventMgrInit()
    {
        gm_eve_mgr_tcb = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(gmEveMgrMain), new GSF_TASK_PROCEDURE(gmEveMgrDest), 0U, 0, 4240U, 5, () => new GMS_EVE_MGR_WORK(), "GM_EVT_MGR");
        GMS_EVE_MGR_WORK work = (GMS_EVE_MGR_WORK)gm_eve_mgr_tcb.work;
        work.Clear();
        g_gm_eve_mgr_work = work;
        g_gm_eve_mgr_work.map_size[0] = (ushort)((uint)gm_eve_data.width << 8);
        g_gm_eve_mgr_work.map_size[1] = (ushort)((uint)gm_eve_data.height << 8);
    }

    private static void GmEventMgrStart()
    {
        GmEventMgrCreateEventEnforce();
        g_gm_eve_mgr_work.sts_proc = new _sts_proc_(gmEveMgrStateFuncSingleScr);
    }

    private static void GmEventMgrCreateEventEnforce()
    {
        int[] r_on = new int[4]
        {
      0,
      0,
       g_gm_eve_mgr_work.map_size[0] - 1,
      0
        };
        r_on[1] = 0;
        r_on[3] = g_gm_eve_mgr_work.map_size[1] - 1;
        for (ushort by = 0; by < gm_eve_data.height; ++by)
        {
            for (ushort bx = 0; bx < gm_eve_data.width; ++bx)
                gmEveMgrCreateEventBlkEvent(4U, bx, by, r_on, null);
        }
    }

    public static void GmEventMgrExit()
    {
        if (gm_eve_mgr_tcb == null)
            return;
        mtTaskClearTcb(gm_eve_mgr_tcb);
    }

    private static void gmEveMgrMain(MTS_TASK_TCB tcb)
    {
        GMS_EVE_MGR_WORK work = (GMS_EVE_MGR_WORK)tcb.work;
        if (work.sts_proc != null)
            work.sts_proc();
        OBS_CAMERA obsCamera = ObjCameraGet(g_obj.glb_camera_id);
        if (obsCamera == null)
            return;
        work.prev_pos[0] = obsCamera.disp_pos.x;
        work.prev_pos[1] = obsCamera.disp_pos.y;
    }

    private static void gmEveMgrDest(MTS_TASK_TCB tcb)
    {
        g_gm_eve_mgr_work = null;
        gm_eve_mgr_tcb = null;
    }

    private static void gmEveMgrCreateEventBlkEvent(
      uint flag,
      ushort bx,
      ushort by,
      int[] r_on,
      int[] r_off)
    {
        uint num1 = bx + gm_eve_data.width * (uint)by;
        GMS_EVE_DATA_EV_LIST ev = gm_eve_data.ev_list[(int)num1];
        ushort eveNum = ev.eve_num;
        ArrayPointer<GMS_EVE_RECORD_EVENT> arrayPointer = new ArrayPointer<GMS_EVE_RECORD_EVENT>(ev.eve_rec);
        int num2 = bx << 8;
        int num3 = by << 8;
        int num4 = 0;
        while (num4 < eveNum)
        {
            if ((((int)flag & 4) == 0 || (arrayPointer[0].flag & 32768) != 0) && arrayPointer[0].pos_x != byte.MaxValue)
            {
                int num5 = arrayPointer[0].pos_x + num2;
                int num6 = arrayPointer[0].pos_y + num3;
                int num7 = g_gm_event_size_tbl[arrayPointer[0].id];
                int num8 = num7 + 16 + 32;
                if (((int)flag & 1) == 0 || num5 >= r_on[0] - num8 && num5 <= r_on[2] + num8 && (num6 >= r_on[1] - num8 && num6 <= r_on[3] + num8))
                {
                    int num9 = num7 + 16;
                    if ((((int)flag & 2) == 0 || num5 <= r_off[0] - num9 || (num5 >= r_off[2] + num9 || num6 <= r_off[1] - num9) || num6 >= r_off[3] + num9) && (arrayPointer[0].id < 346 && g_gm_event_tbl[arrayPointer[0].id] != null))
                    {
                        if ((arrayPointer[0].flag & 4096 << (ushort)GsGetGameLevel()) != 0)
                        {
                            arrayPointer[0].pos_x = byte.MaxValue;
                        }
                        else
                        {
                            OBS_OBJECT_WORK obsObjectWork = g_gm_event_tbl[arrayPointer[0].id](arrayPointer[0], num5 << 12, num6 << 12, 0);
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
        OBS_CAMERA obsCamera = ObjCameraGet(g_obj.glb_camera_id);
        if (obsCamera == null || obsCamera.disp_pos.x == (double)g_gm_eve_mgr_work.prev_pos[0] && obsCamera.disp_pos.y == (double)g_gm_eve_mgr_work.prev_pos[1])
            return;
        GmEveMgrCreateEventLcd(3U);
    }

    private static void GmEveMgrCreateEventLcd(uint flag)
    {
        int num1 = _g_obj.clip_camera[0] >> 12;
        int num2 = _g_obj.clip_camera[1] >> 12;
        int num3 = _g_obj.clip_lcd_size[0];
        int num4 = _g_obj.clip_lcd_size[1];
        lcd_rect[0] = num1;
        lcd_rect[2] = num1 + num3;
        lcd_rect[1] = num2;
        lcd_rect[3] = num2 + num4;
        lcd_rect[0] = MTM_MATH_CLIP(lcd_rect[0], 0, g_gm_eve_mgr_work.map_size[0] - 1);
        lcd_rect[1] = MTM_MATH_CLIP(lcd_rect[1], 0, g_gm_eve_mgr_work.map_size[1] - 1);
        lcd_rect[2] = MTM_MATH_CLIP(lcd_rect[2], 0, g_gm_eve_mgr_work.map_size[0] - 1);
        lcd_rect[3] = MTM_MATH_CLIP(lcd_rect[3], 0, g_gm_eve_mgr_work.map_size[1] - 1);
        for (int index = 0; index < 3; ++index)
        {
            _eve_func_ eveFunc = gm_evemgr_create_eve_func_tbl[index];
            int num5 = gm_evemgr_create_size_tbl[index];
            ev_rect[0] = lcd_rect[0];
            ev_rect[2] = lcd_rect[2];
            ev_rect[1] = lcd_rect[1];
            ev_rect[3] = lcd_rect[3];
            block_rect[0] = ev_rect[0] - 16 - num5 - byte.MaxValue >> 8;
            block_rect[2] = ev_rect[2] + 16 + num5 + byte.MaxValue >> 8;
            block_rect[1] = ev_rect[1] - 16 - num5 - byte.MaxValue >> 8;
            block_rect[3] = ev_rect[3] + 16 + num5 + byte.MaxValue >> 8;
            if (block_rect[0] < 0)
                block_rect[0] = 0;
            if (block_rect[2] >= gm_eve_data.width)
                block_rect[2] = gm_eve_data.width - 1;
            if (block_rect[1] < 0)
                block_rect[1] = 0;
            if (block_rect[3] >= gm_eve_data.height)
                block_rect[3] = gm_eve_data.height - 1;
            for (ushort by = (ushort)block_rect[1]; by <= (ushort)block_rect[3]; ++by)
            {
                for (ushort bx = (ushort)block_rect[0]; bx <= (ushort)block_rect[2]; ++bx)
                    eveFunc(flag, bx, by, ev_rect, lcd_rect);
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
        var num1 = bx + gm_ring_data.width * (uint)by;
        var gmsEveDataRgList = gm_ring_data.ring[(int)num1];
        var ringNum = gmsEveDataRgList.ring_num;
        var arrayPointer = new ArrayPointer<GMS_EVE_RECORD_RING>(gmsEveDataRgList.ring_data);
        var num2 = bx << 8;
        var num3 = by << 8;
        var num4 = GMD_RING_SIZE + 16 + 32;
        for (ushort index = 0; index < ringNum; ++index)
        {
            if (arrayPointer[0].pos_x != byte.MaxValue)
            {
                var num5 = arrayPointer[0].pos_x + num2;
                var num6 = arrayPointer[0].pos_y + num3;
                if (num5 >= r_on[0] - num4 && num5 <= r_on[2] + num4 && (num6 >= r_on[1] - num4 && num6 <= r_on[3] + num4))
                    GmRingCreate(arrayPointer, (bx << 8) + arrayPointer[0].pos_x << 12, (by << 8) + arrayPointer[0].pos_y << 12, 0);
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
        uint num1 = bx + gm_deco_data.width * (uint)by;
        GMS_EVE_DATA_DC_LIST dc = gm_deco_data.dc_list[(int)num1];
        ushort decNum = dc.dec_num;
        ArrayPointer<GMS_EVE_RECORD_DECORATE> arrayPointer = new ArrayPointer<GMS_EVE_RECORD_DECORATE>(dc.dec_data);
        int num2 = bx << 8;
        int num3 = by << 8;
        int num4 = 0;
        while (num4 < decNum)
        {
            if (arrayPointer[0].pos_x != byte.MaxValue)
            {
                int num5 = arrayPointer[0].pos_x + num2;
                int num6 = arrayPointer[0].pos_y + num3;
                int num7 = g_gm_decorate_size_tbl[arrayPointer[0].id];
                int num8 = num7 + 16 + 32;
                if (((int)flag & 1) == 0 || num5 >= r_on[0] - num8 && num5 <= r_on[2] + num8 && (num6 >= r_on[1] - num8 && num6 <= r_on[3] + num8))
                {
                    int num9 = num7 + 16;
                    if ((((int)flag & 2) == 0 || num5 <= r_off[0] - num9 || (num5 >= r_off[2] + num9 || num6 <= r_off[1] - num9) || num6 >= r_off[3] + num9) && (arrayPointer[0].id < 184 && g_gm_decorate_tbl[arrayPointer[0].id] != null))
                    {
                        OBS_OBJECT_WORK obsObjectWork = g_gm_decorate_tbl[arrayPointer[0].id](arrayPointer, num5 << 12, num6 << 12, 0);
                    }
                }
            }
            ++num4;
            ++arrayPointer;
        }
    }

    private static void GmEventDataBuild()
    {
        AMS_AMB_HEADER gGmGamedat = (AMS_AMB_HEADER)g_gm_gamedat_map[0];
        gm_eve_data = new GMS_EVE_DATA_EV_HEADER((AmbChunk)amBindGet(gGmGamedat, 6));
        gm_ring_data = new GMS_EVE_DATA_RG_HEADER((AmbChunk)amBindGet(gGmGamedat, 8));
        gm_deco_data = new GMS_EVE_DATA_DC_HEADER((AmbChunk)amBindGet(gGmGamedat, 7));
    }

    private static void GmEventDataFlush()
    {
        if (gm_eve_data != null)
            gm_eve_data = null;
        if (gm_ring_data != null)
            gm_ring_data = null;
        if (gm_deco_data == null)
            return;
        gm_deco_data = null;
    }

    private static void GmEveMgrCreateStateEvent()
    {
        GmEveMgrCreateEventLcd(1U);
    }

    private static void GmEventMgrCreateEventInRect(
      ushort left,
      ushort top,
      ushort right,
      ushort bottom)
    {
        int[] r_on = new int[4];
        ushort[] numArray = new ushort[4];
        r_on[0] = left;
        if (r_on[0] > g_gm_eve_mgr_work.map_size[0] - 1)
            r_on[0] = g_gm_eve_mgr_work.map_size[0] - 1;
        r_on[1] = top;
        if (r_on[1] > g_gm_eve_mgr_work.map_size[1] - 1)
            r_on[1] = g_gm_eve_mgr_work.map_size[1] - 1;
        r_on[2] = right;
        if (r_on[2] > g_gm_eve_mgr_work.map_size[0] - 1)
            r_on[2] = g_gm_eve_mgr_work.map_size[0] - 1;
        r_on[3] = bottom;
        if (r_on[3] > g_gm_eve_mgr_work.map_size[1] - 1)
            r_on[3] = g_gm_eve_mgr_work.map_size[1] - 1;
        numArray[0] = (ushort)(r_on[0] - byte.MaxValue >> 8);
        numArray[2] = (ushort)(r_on[2] + byte.MaxValue >> 8);
        numArray[1] = (ushort)(r_on[1] - byte.MaxValue >> 8);
        numArray[3] = (ushort)(r_on[3] + byte.MaxValue >> 8);
        for (int index = 0; index < 3; ++index)
        {
            _eve_func_ eveFunc = gm_evemgr_create_eve_func_tbl[index];
            for (ushort by = numArray[1]; by <= numArray[3]; ++by)
            {
                for (ushort bx = numArray[0]; bx <= numArray[2]; ++bx)
                    eveFunc(1U, bx, by, r_on, null);
            }
        }
    }

    private static void GmEveMgrCreateEventAll()
    {
        int[] r_on = new int[4]
        {
      0,
      0,
       g_gm_eve_mgr_work.map_size[0] - 1,
      0
        };
        r_on[1] = 0;
        r_on[3] = g_gm_eve_mgr_work.map_size[1] - 1;
        for (int index = 0; index < 3; ++index)
        {
            _eve_func_ eveFunc = gm_evemgr_create_eve_func_tbl[index];
            for (ushort by = 0; by < gm_eve_data.height; ++by)
            {
                for (ushort bx = 0; bx < gm_eve_data.width; ++bx)
                    eveFunc(0U, bx, by, r_on, null);
            }
        }
    }

    private static OBS_OBJECT_WORK GmEventMgrLocalEventBirth(
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
        OBS_OBJECT_WORK obsObjectWork = null;
        short num = gmEventMgrLocalEventNoGet(0);
        if (num != -1)
        {
            gm_eve_local_evt_record[num].pos_x = byte.MaxValue;
            gm_eve_local_evt_record[num].pos_y = byte.MaxValue;
            gm_eve_local_evt_record[num].id = id;
            gm_eve_local_evt_record[num].flag = flag;
            gm_eve_local_evt_record[num].left = left;
            gm_eve_local_evt_record[num].top = top;
            gm_eve_local_evt_record[num].width = width;
            gm_eve_local_evt_record[num].height = height;
            gm_eve_local_evt_record[num].word_param = 0;
            obsObjectWork = g_gm_event_tbl[id](gm_eve_local_evt_record[num], pos_x, pos_y, type);
            if (obsObjectWork == null)
                GmEventMgrLocalEventRelease(gm_eve_local_evt_record[num]);
        }
        return obsObjectWork;
    }

    private static void GmEventMgrLocalRingBirth(int pos_x, int pos_y, byte type)
    {
        short num = gmEventMgrLocalEventNoGet(1);
        if (num == -1)
            return;
        gm_eve_local_ring_record[num].pos_x = byte.MaxValue;
        gm_eve_local_ring_record[num].pos_y = byte.MaxValue;
        GmRingCreate(gm_eve_local_ring_record[num], pos_x, pos_y, type);
    }

    private static OBS_OBJECT_WORK GmEventMgrLocalDecoBirth(
      ushort id,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK obsObjectWork = null;
        short num = gmEventMgrLocalEventNoGet(2);
        if (num != -1)
        {
            gm_eve_local_deco_record[num].pos_x = byte.MaxValue;
            gm_eve_local_deco_record[num].pos_y = byte.MaxValue;
            gm_eve_local_deco_record[num].id = id;
            obsObjectWork = g_gm_decorate_tbl[id](gm_eve_local_deco_record[num], pos_x, pos_y, type);
            if (obsObjectWork == null)
                GmEventMgrLocalDecoRelease(gm_eve_local_deco_record[num]);
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
                numArray = gm_eve_local_evt_obj_use_flag;
                num = 2;
                break;
            case 1:
                numArray = gm_eve_local_ring_obj_use_flag;
                num = 2;
                break;
            case 2:
                numArray = gm_eve_local_deco_obj_use_flag;
                num = 2;
                break;
            default:
                return 0;
        }
        for (short index1 = 0; index1 < num; ++index1)
        {
            if (numArray[index1] < uint.MaxValue)
            {
                for (short index2 = 0; index2 < 32; ++index2)
                {
                    if (0 == (numArray[index1] & (uint)(1 << index2)))
                    {
                        numArray[index1] |= (uint)(1U << index2);
                        return (short)(index1 * 32 + index2);
                    }
                }
            }
        }
        return -1;
    }

    private static void GmEventMgrLocalEventRelease(GMS_EVE_RECORD_EVENT eve_rec)
    {
        int num = Array.IndexOf(gm_eve_local_evt_record, eve_rec);
        gm_eve_local_evt_obj_use_flag[num >> 5] &= (uint)~(1 << num);
    }

    private static void GmEventMgrLocalRingRelease(GMS_EVE_RECORD_RING eve_rec)
    {
        int num = Array.IndexOf(gm_eve_local_ring_record, eve_rec);
        gm_eve_local_ring_obj_use_flag[num >> 5] &= (uint)~(1 << num);
    }

    private static void GmEventMgrLocalDecoRelease(GMS_EVE_RECORD_DECORATE eve_rec)
    {
        int num = Array.IndexOf(gm_eve_local_deco_record, eve_rec);
        gm_eve_local_deco_obj_use_flag[num >> 5] &= (uint)~(1 << num);
    }

    private static uint GmEventMgrGetRingNum()
    {
        uint num1 = 0;
        if (gm_ring_data != null)
        {
            uint num2 = gm_ring_data.width * (uint)gm_ring_data.height;
            for (uint index = 0; index < num2; ++index)
                num1 += gm_ring_data.ring[(int)index].ring_num;
        }
        return num1;
    }
}