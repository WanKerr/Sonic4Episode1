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
    public static void AoActSetTexture(AppMain.NNS_TEXLIST texlist)
    {
        AppMain.g_ao_act_texlist = texlist;
    }

    public static AppMain.NNS_TEXLIST AoActGetTexture()
    {
        return AppMain.g_ao_act_texlist;
    }

    public static AppMain.AOS_ACTION AoActCreate(AppMain.A2S_AMA_HEADER ama, uint id)
    {
        return AppMain.AoActCreate(ama, id, 0.0f);
    }

    public static AppMain.AOS_ACTION AoActCreate(
      AppMain.A2S_AMA_HEADER ama,
      uint id,
      float frame)
    {
        AppMain.A2S_AMA_HEADER a2SAmaHeader = ama;
        if (id >= a2SAmaHeader.act_num)
            return (AppMain.AOS_ACTION)null;
        AppMain.A2S_AMA_ACT a2SAmaAct = a2SAmaHeader.act_tbl[(int)id];
        AppMain.AOS_ACTION act = AppMain.aoActAllocAction();
        if (act == null)
            return (AppMain.AOS_ACTION)null;
        AppMain.AoActAcmPush();
        AppMain.AoActAcmFlagPush(0U, uint.MaxValue);
        act.data = (object)a2SAmaAct;
        act.flag = 0U;
        act.state = 0U;
        act.type = AppMain.AOE_ACT_TYPE.AOD_ACT_TYPE_ACTION;
        act.frame = frame;
        act.last_key.trs = -1;
        act.last_key.mtn = -1;
        act.last_key.anm = -1;
        act.last_key.mat = -1;
        act.last_key.atrs = -1;
        act.last_key.amtn = -1;
        act.last_key.amat = -1;
        act.last_key.usr = -1;
        act.last_key.hit = -1;
        act.child = (AppMain.AOS_ACTION)null;
        act.sibling = (AppMain.AOS_ACTION)null;
        act.sprite = AppMain.AoActSprCreate(ama, id, frame);
        if (act.sprite == null)
        {
            AppMain.AoActDelete(act);
            AppMain.AoActAcmFlagPop();
            AppMain.AoActAcmPop();
            return (AppMain.AOS_ACTION)null;
        }
        AppMain.AoActAcmFlagPop();
        AppMain.AoActAcmPop();
        return act;
    }

    public static AppMain.AOS_ACTION AoActCreateNode(
      AppMain.A2S_AMA_HEADER ama,
      uint id,
      float frame)
    {
        return AppMain.AoActCreateNodeSub(ama, id, frame, false);
    }

    public static AppMain.AOS_ACTION AoActCreateNodeSub(
      AppMain.A2S_AMA_HEADER ama,
      uint id,
      float frame,
      bool sib)
    {
        AppMain.A2S_AMA_HEADER a2SAmaHeader = ama;
        if (id >= a2SAmaHeader.node_num)
            return (AppMain.AOS_ACTION)null;
        AppMain.A2S_AMA_NODE a2SAmaNode = a2SAmaHeader.node_tbl[(int)id];
        AppMain.AOS_ACTION act = AppMain.aoActAllocAction();
        if (act == null)
            return (AppMain.AOS_ACTION)null;
        AppMain.AoActAcmPush();
        AppMain.AoActAcmFlagPush(0U, uint.MaxValue);
        act.data = (object)a2SAmaNode;
        act.flag = 0U;
        act.state = 0U;
        act.type = AppMain.AOE_ACT_TYPE.AOD_ACT_TYPE_NODE;
        act.frame = frame;
        act.last_key.trs = -1;
        act.last_key.mtn = -1;
        act.last_key.anm = -1;
        act.last_key.mat = -1;
        act.last_key.atrs = -1;
        act.last_key.amtn = -1;
        act.last_key.amat = -1;
        act.last_key.usr = -1;
        act.last_key.hit = -1;
        act.child = (AppMain.AOS_ACTION)null;
        act.sibling = (AppMain.AOS_ACTION)null;
        if (a2SAmaNode.act_offset != 0)
        {
            act.sprite = AppMain.AoActSprCreate(ama, a2SAmaNode.act.id, frame);
            if (act.sprite == null)
            {
                AppMain.AoActDelete(act);
                AppMain.AoActAcmFlagPop();
                AppMain.AoActAcmPop();
                return (AppMain.AOS_ACTION)null;
            }
        }
        else
            act.sprite = (AppMain.AOS_SPRITE)null;
        if (a2SAmaNode.child_offset != 0)
        {
            act.child = AppMain.AoActCreateNodeSub(ama, a2SAmaNode.child.id, frame, true);
            if (act.child == null)
            {
                AppMain.AoActAcmFlagPop();
                AppMain.AoActAcmPop();
                return act;
            }
        }
        AppMain.AoActAcmFlagPop();
        AppMain.AoActAcmPop();
        if (sib && a2SAmaNode.sibling_offset != 0)
        {
            act.sibling = AppMain.AoActCreateNodeSub(ama, a2SAmaNode.sibling.id, frame, true);
            if (act.sibling == null)
                return act;
        }
        return act;
    }

    public static void AoActDelete(AppMain.AOS_ACTION act)
    {
        if (act.sibling != null)
        {
            AppMain.AoActDelete(act.sibling);
            act.sibling = (AppMain.AOS_ACTION)null;
        }
        if (act.child != null)
        {
            AppMain.AoActDelete(act.child);
            act.child = (AppMain.AOS_ACTION)null;
        }
        if (act.sprite != null)
        {
            AppMain.AoActSprDelete(act.sprite);
            act.sprite = (AppMain.AOS_SPRITE)null;
        }
        AppMain.aoActFreeAction(act);
    }

    public static void AoActSetFrame(AppMain.AOS_ACTION act, float frame)
    {
        do
        {
            act.frame = frame;
            act.flag |= 1U;
            if (act.child != null)
                AppMain.AoActSetFrame(act.child, frame);
            act = act.sibling;
        }
        while (act != null);
    }

    public static void AoActUpdate(AppMain.AOS_ACTION act)
    {
        AppMain.AoActUpdate(act, 1f);
    }

    public static void AoActUpdate(AppMain.AOS_ACTION act, float frame)
    {
        act.frame += AppMain.g_ao_act_sys_frame_rate * frame;
        if ((double)act.frame < 0.0)
            act.frame = 0.0f;
        act.flag |= 1U;
        if (act.child != null)
            AppMain.AoActSetFrame(act.child, act.frame);
        AppMain.aoActApply(act);
        act.state = AppMain.aoActGetAmaActState(AppMain.aoActGetAmaAct(act), act.frame);
    }

    private static void AoActDraw(AppMain.AOS_ACTION act)
    {
        AppMain.AoActDraw(act, false);
    }

    private static void AoActDraw(AppMain.AOS_ACTION act, bool sort)
    {
        if (AppMain.g_ao_act_sort_num >= AppMain.g_ao_act_sort_buf_size)
            return;
        AppMain.ArrayPointer<AppMain.AOS_ACT_SORT> gAoActSortBuf = AppMain.g_ao_act_sort_buf;
        uint aoActSortBufSize = AppMain.g_ao_act_sort_buf_size;
        uint gAoActSortNum1 = AppMain.g_ao_act_sort_num;
        uint gAoActSortPeak = AppMain.g_ao_act_sort_peak;
        AppMain.g_ao_act_sort_buf += (int)AppMain.g_ao_act_sort_num;
        AppMain.g_ao_act_sort_buf_size -= AppMain.g_ao_act_sort_num;
        AppMain.g_ao_act_sort_num = 0U;
        AppMain.g_ao_act_sort_peak = 0U;
        AppMain.AoActSortRegAction(act);
        uint gAoActSortNum2 = AppMain.g_ao_act_sort_num;
        if (sort)
            AppMain.AoActSortExecute();
        AppMain.AoActSortDraw();
        AppMain.AoActSortUnregAll();
        AppMain.g_ao_act_sort_buf = gAoActSortBuf;
        AppMain.g_ao_act_sort_buf_size = aoActSortBufSize;
        AppMain.g_ao_act_sort_num = gAoActSortNum1;
        AppMain.g_ao_act_sort_peak = gAoActSortPeak;
        if (AppMain.g_ao_act_sort_num + gAoActSortNum2 <= AppMain.g_ao_act_sort_peak)
            return;
        AppMain.g_ao_act_sort_peak = AppMain.g_ao_act_sort_num + gAoActSortNum2;
    }

    public static uint AoActGetState(AppMain.AOS_ACTION act)
    {
        return act != null ? act.state : 1023U;
    }

    public static bool AoActIsEnd(AppMain.AOS_ACTION act)
    {
        return act == null || AppMain.aoActIsAmaActEnd(AppMain.aoActGetAmaAct(act), act.frame);
    }

    public static bool AoActIsEndTrs(AppMain.AOS_ACTION act)
    {
        AppMain.A2S_AMA_ACT amaAct = AppMain.aoActGetAmaAct(act);
        return amaAct == null || AppMain.aoActIsAmaTrsEnd(amaAct.mtn, act.frame);
    }

    public static bool AoActIsEndMtn(AppMain.AOS_ACTION act)
    {
        AppMain.A2S_AMA_ACT amaAct = AppMain.aoActGetAmaAct(act);
        return amaAct == null || AppMain.aoActIsAmaMtnEnd(amaAct.mtn, act.frame);
    }

    public static bool AoActIsEndAnm(AppMain.AOS_ACTION act)
    {
        AppMain.A2S_AMA_ACT amaAct = AppMain.aoActGetAmaAct(act);
        return amaAct == null || AppMain.aoActIsAmaAnmEnd(amaAct.anm, act.frame);
    }

    public static bool AoActIsEndMat(AppMain.AOS_ACTION act)
    {
        AppMain.A2S_AMA_ACT amaAct = AppMain.aoActGetAmaAct(act);
        return amaAct == null || AppMain.aoActIsAmaMatEnd(amaAct.anm, act.frame);
    }

    public static bool AoActIsEndAcmTrs(AppMain.AOS_ACTION act)
    {
        AppMain.A2S_AMA_ACT amaAct = AppMain.aoActGetAmaAct(act);
        return amaAct == null || AppMain.aoActIsAmaAcmTrsEnd(amaAct.acm, act.frame);
    }

    public static bool AoActIsEndAcmMtn(AppMain.AOS_ACTION act)
    {
        AppMain.A2S_AMA_ACT amaAct = AppMain.aoActGetAmaAct(act);
        return amaAct == null || AppMain.aoActIsAmaAcmMtnEnd(amaAct.acm, act.frame);
    }

    public static bool AoActIsEndAcmMat(AppMain.AOS_ACTION act)
    {
        AppMain.A2S_AMA_ACT amaAct = AppMain.aoActGetAmaAct(act);
        return amaAct == null || AppMain.aoActIsAmaAcmMatEnd(amaAct.acm, act.frame);
    }

    public static bool AoActIsEndUsr(AppMain.AOS_ACTION act)
    {
        AppMain.A2S_AMA_ACT amaAct = AppMain.aoActGetAmaAct(act);
        return amaAct == null || AppMain.aoActIsAmaUsrEnd(amaAct.usr, act.frame);
    }

    public static bool AoActIsEndHit(AppMain.AOS_ACTION act)
    {
        AppMain.A2S_AMA_ACT amaAct = AppMain.aoActGetAmaAct(act);
        return amaAct == null || AppMain.aoActIsAmaHitEnd(amaAct.hit, act.frame);
    }

    public static void AoActSortRegSprite(AppMain.AOS_SPRITE spr)
    {
        if (AppMain.g_ao_act_sort_num >= AppMain.g_ao_act_sort_buf_size)
            return;
        AppMain.g_ao_act_sort_buf.array[(int)AppMain.g_ao_act_sort_num + AppMain.g_ao_act_sort_buf.offset].sprite = spr;
        AppMain.g_ao_act_sort_buf.array[(int)AppMain.g_ao_act_sort_num + AppMain.g_ao_act_sort_buf.offset].z = spr.prio;
        ++AppMain.g_ao_act_sort_num;
        if (AppMain.g_ao_act_sort_num <= AppMain.g_ao_act_sort_peak)
            return;
        AppMain.g_ao_act_sort_peak = AppMain.g_ao_act_sort_num;
    }

    public static void AoActSortUnregSprite(AppMain.AOS_SPRITE spr)
    {
        for (int index1 = 0; (long)index1 < (long)AppMain.g_ao_act_sort_num; ++index1)
        {
            if (AppMain.g_ao_act_sort_buf[index1].sprite == spr)
            {
                for (int index2 = 0; (long)index2 < (long)AppMain.g_ao_act_sort_num - (long)index1 - 1L; ++index2)
                    AppMain.g_ao_act_sort_buf.array[index2 + index1 + AppMain.g_ao_act_sort_buf.offset] = AppMain.g_ao_act_sort_buf.array[index2 + index1 + 1 + AppMain.g_ao_act_sort_buf.offset];
                --AppMain.g_ao_act_sort_num;
                if (AppMain.g_ao_act_sort_num <= AppMain.g_ao_act_sort_peak)
                    break;
                AppMain.g_ao_act_sort_peak = AppMain.g_ao_act_sort_num;
                break;
            }
        }
    }

    public static void AoActSortRegAction(AppMain.AOS_ACTION act)
    {
        if (act.sprite != null)
            AppMain.AoActSortRegSprite(act.sprite);
        if (act.child != null)
            AppMain.AoActSortRegAction(act.child);
        if (act.sibling == null)
            return;
        AppMain.AoActSortRegAction(act.sibling);
    }

    public static void AoActSortUnregAction(AppMain.AOS_ACTION act)
    {
        if (act.sprite != null)
            AppMain.AoActSortUnregSprite(act.sprite);
        if (act.child != null)
            AppMain.AoActSortUnregAction(act.child);
        if (act.sibling == null)
            return;
        AppMain.AoActSortUnregAction(act.sibling);
    }

    public static void AoActSortUnregAll()
    {
        AppMain.g_ao_act_sort_num = 0U;
    }

    public static void AoActSortExecute()
    {
        AppMain.AOS_ACT_SORT[] array = AppMain.g_ao_act_sort_buf.array;
        for (int index1 = 0; (long)index1 < (long)AppMain.g_ao_act_sort_num; ++index1)
        {
            bool flag = true;
            for (int index2 = index1 + 1; (long)index2 < (long)AppMain.g_ao_act_sort_num; ++index2)
            {
                if ((double)array[index2].z > (double)array[index2 - 1].z)
                {
                    AppMain.AOS_ACT_SORT aosActSort = array[index2];
                    array[index2] = array[index2 - 1];
                    array[index2 - 1] = aosActSort;
                    flag = false;
                }
            }
            if (flag)
                break;
        }
    }

    public static void AoActSortExecuteFix()
    {
        AppMain.AOS_ACT_SORT[] array = AppMain.g_ao_act_sort_buf.array;
        for (int index1 = 0; (long)index1 < (long)AppMain.g_ao_act_sort_num; ++index1)
        {
            bool flag = true;
            for (int index2 = (int)AppMain.g_ao_act_sort_num - 1; index1 < index2; --index2)
            {
                if ((double)array[index2].z > (double)array[index2 - 1].z)
                {
                    AppMain.AOS_ACT_SORT aosActSort = array[index2];
                    array[index2] = array[index2 - 1];
                    array[index2 - 1] = aosActSort;
                    flag = false;
                }
            }
            if (flag)
                break;
        }
    }

    public static void AoActSortDraw()
    {
        if (AppMain.g_ao_act_sort_num == 0U)
            return;
        if (AppMain.g_ao_act_sys_draw_state_enable)
        {
            AppMain.aoActDrawSortState();
        }
        else
        {
            AppMain.AOS_ACT_DRAW aosActDraw = AppMain.amDrawAlloc_AOS_ACT_DRAW();
            aosActDraw.count = AppMain.g_ao_act_sort_num;
            aosActDraw.sprite = new AppMain.AOS_SPRITE[(int)AppMain.g_ao_act_sort_num];
            for (int index = 0; (long)index < (long)AppMain.g_ao_act_sort_num; ++index)
            {
                aosActDraw.sprite[index] = AppMain.AOS_SPRITE_Pool.Alloc();
                aosActDraw.sprite[index].Assign(AppMain.g_ao_act_sort_buf[index].sprite);
            }
            AppMain.amDrawMakeTask(AppMain.aoActDrawTask_TaskProc, (ushort)AppMain.g_ao_act_sys_draw_prio, (object)aosActDraw);
        }
    }

    public static void AoActAcmInit()
    {
        AppMain.AoActAcmInit((AppMain.AOS_ACT_ACM)null);
    }

    public static void AoActAcmInit(AppMain.AOS_ACT_ACM acm)
    {
        if (acm == null)
            acm = (AppMain.AOS_ACT_ACM)AppMain.g_ao_act_acm_cur;
        acm.trans_x = 0.0f;
        acm.trans_y = 0.0f;
        acm.trans_z = 0.0f;
        acm.color.r = byte.MaxValue;
        acm.color.g = byte.MaxValue;
        acm.color.b = byte.MaxValue;
        acm.color.a = byte.MaxValue;
        acm.fade.r = (byte)0;
        acm.fade.g = (byte)0;
        acm.fade.b = (byte)0;
        acm.fade.a = (byte)0;
        acm.trans_scale_x = 1f;
        acm.trans_scale_y = 1f;
        acm.scale_x = 1f;
        acm.scale_y = 1f;
        acm.rotate = 0.0f;
    }

    public static void AoActAcmPush()
    {
        AppMain.AoActAcmPush((AppMain.AOS_ACT_ACM)null);
    }

    public static void AoActAcmPush(AppMain.AOS_ACT_ACM acm)
    {
        if (AppMain.g_ao_act_acm_num >= AppMain.g_ao_act_acm_buf_size)
            return;
        if (acm == null)
            acm = AppMain.g_ao_act_acm_cur.array[AppMain.g_ao_act_acm_cur.offset];
        ++AppMain.g_ao_act_acm_cur.offset;
        AppMain.g_ao_act_acm_cur.array[AppMain.g_ao_act_acm_cur.offset].Assign(acm);
        ++AppMain.g_ao_act_acm_num;
        if (AppMain.g_ao_act_acm_num <= AppMain.g_ao_act_acm_peak)
            return;
        AppMain.g_ao_act_acm_peak = AppMain.g_ao_act_acm_num;
    }

    public static void AoActAcmPop()
    {
        AppMain.AoActAcmPop(1U);
    }

    public static void AoActAcmPop(uint count)
    {
        for (; count > 0U; --count)
        {
            if (AppMain.g_ao_act_acm_cur == AppMain.g_ao_act_acm_buf)
                return;
            AppMain.g_ao_act_acm_cur -= 1;
            --AppMain.g_ao_act_acm_num;
        }
        if (AppMain.g_ao_act_acm_num <= AppMain.g_ao_act_acm_peak)
            return;
        AppMain.g_ao_act_acm_peak = AppMain.g_ao_act_acm_num;
    }

    private void AoActAcmSet(AppMain.AOS_ACT_ACM acm)
    {
        AppMain.mppAssertNotImpl();
    }

    public static void AoActAcmApply(AppMain.AOS_ACT_ACM acm)
    {
        AppMain.AoActAcmApplyTrans(acm.trans_x, acm.trans_y, acm.trans_z);
        AppMain.AoActAcmApplyColor(acm.color);
        AppMain.AoActAcmApplyFade(acm.fade);
        AppMain.AoActAcmApplyTransScale(acm.trans_scale_x, acm.trans_scale_y);
        AppMain.AoActAcmApplyScale(acm.scale_x, acm.scale_y);
        AppMain.AoActAcmApplyRotate(acm.rotate);
    }

    private void AoActAcmApplyTrans(Vector3 trs)
    {
        AppMain.AoActAcmApplyTrans(trs.X, trs.Y, trs.Z);
    }

    public static void AoActAcmApplyTrans(float x, float y, float z)
    {
        ((AppMain.AOS_ACT_ACM)~AppMain.g_ao_act_acm_cur).trans_x += ((AppMain.AOS_ACT_ACM)~AppMain.g_ao_act_acm_cur).trans_scale_x * x;
        ((AppMain.AOS_ACT_ACM)~AppMain.g_ao_act_acm_cur).trans_y += ((AppMain.AOS_ACT_ACM)~AppMain.g_ao_act_acm_cur).trans_scale_y * y;
        ((AppMain.AOS_ACT_ACM)~AppMain.g_ao_act_acm_cur).trans_z += z;
    }

    public static void AoActAcmApplyColor(AppMain.AOS_ACT_COL col)
    {
        AppMain.AOS_ACT_COL color = ((AppMain.AOS_ACT_ACM)~AppMain.g_ao_act_acm_cur).color;
        color.r = (byte)((uint)color.r * (uint)col.r >> 8);
        color.g = (byte)((uint)color.g * (uint)col.g >> 8);
        color.b = (byte)((uint)color.b * (uint)col.b >> 8);
        color.a = (byte)((uint)color.a * (uint)col.a >> 8);
        ((AppMain.AOS_ACT_ACM)~AppMain.g_ao_act_acm_cur).color = color;
    }

    public static void AoActAcmApplyFade(AppMain.AOS_ACT_COL fade)
    {
        AppMain.AOS_ACT_COL fade1 = ((AppMain.AOS_ACT_ACM)~AppMain.g_ao_act_acm_cur).fade;
        uint num1 = (uint)fade1.r + (uint)fade.r;
        fade1.r = num1 <= (uint)byte.MaxValue ? (byte)num1 : byte.MaxValue;
        uint num2 = (uint)fade1.g + (uint)fade.g;
        fade1.g = num2 <= (uint)byte.MaxValue ? (byte)num2 : byte.MaxValue;
        uint num3 = (uint)fade1.b + (uint)fade.b;
        fade1.b = num3 <= (uint)byte.MaxValue ? (byte)num3 : byte.MaxValue;
        uint num4 = (uint)fade1.a + (uint)fade.a;
        fade1.a = num4 <= (uint)byte.MaxValue ? (byte)num4 : byte.MaxValue;
        ((AppMain.AOS_ACT_ACM)~AppMain.g_ao_act_acm_cur).fade = fade1;
    }

    public static void AoActAcmApplyTransScale(Vector3 tscl)
    {
        AppMain.AoActAcmApplyTransScale(tscl.X, tscl.Y);
    }

    public static void AoActAcmApplyTransScale(float x, float y)
    {
        ((AppMain.AOS_ACT_ACM)~AppMain.g_ao_act_acm_cur).trans_scale_x *= x;
        ((AppMain.AOS_ACT_ACM)~AppMain.g_ao_act_acm_cur).trans_scale_y *= y;
    }

    public static void AoActAcmApplyScale(Vector3 scl)
    {
        AppMain.AoActAcmApplyScale(scl.X, scl.Y);
    }

    public static void AoActAcmApplyScale(float x, float y)
    {
        ((AppMain.AOS_ACT_ACM)~AppMain.g_ao_act_acm_cur).scale_x *= x;
        ((AppMain.AOS_ACT_ACM)~AppMain.g_ao_act_acm_cur).scale_y *= y;
    }

    public static void AoActAcmApplyRotate(float rot)
    {
        ((AppMain.AOS_ACT_ACM)~AppMain.g_ao_act_acm_cur).rotate += rot;
    }

    public static void AoActAcmSetFlag(uint flag)
    {
        int num = (int)AppMain.g_ao_act_acm_flag_cur.SetPrimitive(flag);
    }

    public static void AoActAcmSetFlag()
    {
        AppMain.AoActAcmSetFlag(0U);
    }

    public static uint AoActAcmGetFlag()
    {
        return (uint)AppMain.g_ao_act_acm_flag_cur;
    }

    public static void AoActAcmFlagPush(uint on, uint off)
    {
        if (AppMain.g_ao_act_acm_flag_num >= AppMain.g_ao_act_acm_flag_buf_size)
            return;
        uint gAoActAcmFlagCur = (uint)AppMain.g_ao_act_acm_flag_cur;
        AppMain.g_ao_act_acm_flag_cur += 1;
        uint num1 = (gAoActAcmFlagCur | on) & ~off;
        int num2 = (int)AppMain.g_ao_act_acm_flag_cur.SetPrimitive(num1);
        ++AppMain.g_ao_act_acm_flag_num;
        if (AppMain.g_ao_act_acm_flag_num <= AppMain.g_ao_act_acm_flag_peak)
            return;
        AppMain.g_ao_act_acm_flag_peak = AppMain.g_ao_act_acm_flag_num;
    }

    public static void AoActAcmFlagPop()
    {
        AppMain.AoActAcmFlagPop(1U);
    }

    public static void AoActAcmFlagPop(uint count)
    {
        for (; count > 0U; --count)
        {
            if (AppMain.g_ao_act_acm_flag_cur == AppMain.g_ao_act_acm_flag_buf)
                return;
            AppMain.g_ao_act_acm_flag_cur -= 1;
            --AppMain.g_ao_act_acm_flag_num;
        }
        if (AppMain.g_ao_act_acm_flag_num <= AppMain.g_ao_act_acm_flag_peak)
            return;
        AppMain.g_ao_act_acm_flag_peak = AppMain.g_ao_act_acm_flag_num;
    }

    public static bool AoActGetHitSpr(AppMain.AOS_ACT_HIT hit, AppMain.AOS_SPRITE spr)
    {
        if ((uint)spr.hit.type >= 2U)
        {
            hit.type = AppMain.AOE_ACT_HIT.AOD_ACT_HIT_NONE;
            return false;
        }
        hit.type = spr.hit.type;
        hit.center_x = spr.center_x;
        hit.center_y = spr.center_y;
        hit.scale_x = spr.hit.scale_x;
        hit.scale_y = spr.hit.scale_y;
        hit.rotate = ((int)spr.hit.flag & 2) == 0 ? spr.rotate : 0.0f;
        switch (hit.type)
        {
            case AppMain.AOE_ACT_HIT.AOD_ACT_HIT_RECT:
                hit.rect.left = spr.hit.rect.left;
                hit.rect.top = spr.hit.rect.top;
                hit.rect.right = spr.hit.rect.right;
                hit.rect.bottom = spr.hit.rect.bottom;
                break;
            case AppMain.AOE_ACT_HIT.AOD_ACT_HIT_CIRCLE:
                hit.circle.center_x = spr.hit.rect.left;
                hit.circle.center_y = spr.hit.rect.top;
                hit.circle.radius = spr.hit.rect.right;
                break;
            default:
                hit.type = AppMain.AOE_ACT_HIT.AOD_ACT_HIT_NONE;
                return false;
        }
        return true;
    }

    public static bool AoActGetHitAct(
      AppMain.ArrayPointer<AppMain.AOS_ACT_HIT> hit,
      AppMain.AOS_ACTION act)
    {
        return AppMain.AoActGetHitSpr((AppMain.AOS_ACT_HIT)hit, AppMain.AoActUtilGetSprFromAct(act));
    }

    public static uint AoActGetHitNum(AppMain.AOS_ACTION act)
    {
        uint num = 0;
        if ((uint)AppMain.AoActUtilGetSprFromAct(act).hit.type < 2U)
            ++num;
        if (act.child != null)
            num += AppMain.AoActGetHitNum(act.child);
        if (act.sibling != null)
            num += AppMain.AoActGetHitNum(act.sibling);
        return num;
    }

    public static uint AoActGetHitTbl(
      AppMain.ArrayPointer<AppMain.AOS_ACT_HIT> hit_tbl,
      AppMain.AOS_ACTION act)
    {
        return AppMain.AoActGetHitTbl(hit_tbl, uint.MaxValue, act);
    }

    public static uint AoActGetHitTbl(
      AppMain.ArrayPointer<AppMain.AOS_ACT_HIT> hit_tbl,
      uint size,
      AppMain.AOS_ACTION act)
    {
        uint num = 0;
        if ((uint)AppMain.AoActUtilGetSprFromAct(act).hit.type < 2U)
        {
            if (size < 1U)
                return num;
            AppMain.AoActGetHitSpr(hit_tbl[(int)num], AppMain.AoActUtilGetSprFromAct(act));
            ++num;
            --size;
        }
        if (act.child != null)
        {
            uint hitTbl = AppMain.AoActGetHitTbl((AppMain.ArrayPointer<AppMain.AOS_ACT_HIT>)(hit_tbl + (int)num), size, act.child);
            num += hitTbl;
            if (size <= hitTbl)
                return num;
            size -= hitTbl;
        }
        if (act.sibling != null)
        {
            uint hitTbl = AppMain.AoActGetHitTbl((AppMain.ArrayPointer<AppMain.AOS_ACT_HIT>)(hit_tbl + (int)num), size, act.sibling);
            num += hitTbl;
            if (size <= hitTbl)
                return num;
            size -= hitTbl;
        }
        return num;
    }

    public static bool AoActGetHitActId(AppMain.AOS_ACT_HIT hit, AppMain.AOS_ACTION act, uint id)
    {
        AppMain.AOS_ACTION actFromId = AppMain.AoActUtilGetActFromId(act, id);
        if (actFromId == null)
        {
            hit.type = AppMain.AOE_ACT_HIT.AOD_ACT_HIT_NONE;
            return false;
        }
        return AppMain.AoActGetHitAct((AppMain.ArrayPointer<AppMain.AOS_ACT_HIT>)new AppMain.AOS_ACT_HIT[1]
        {
      hit
        }, actFromId);
    }

    public static bool AoActHitTest(AppMain.AOS_ACT_HIT hit, float x, float y)
    {
        if ((uint)hit.type >= 2U)
            return false;
        x -= hit.center_x;
        y -= hit.center_y;
        float pSn = 0.0f;
        float pCs = 0.0f;
        AppMain.amSinCos(AppMain.NNM_DEGtoA32(-hit.rotate), out pSn, out pCs);
        float num1 = (float)((double)x * (double)pCs - (double)y * (double)pSn);
        float num2 = (float)((double)x * (double)pSn + (double)y * (double)pCs);
        x = num1 / hit.scale_x;
        y = num2 / hit.scale_y;
        switch (hit.type)
        {
            case AppMain.AOE_ACT_HIT.AOD_ACT_HIT_RECT:
                if ((double)x >= (double)hit.rect.left && (double)x <= (double)hit.rect.right && ((double)y >= (double)hit.rect.top && (double)y <= (double)hit.rect.bottom))
                    return true;
                break;
            case AppMain.AOE_ACT_HIT.AOD_ACT_HIT_CIRCLE:
                float num3 = x - hit.circle.center_x;
                float num4 = y - hit.circle.center_y;
                if ((double)num3 * (double)num3 + (double)num4 * (double)num4 <= (double)hit.circle.radius * (double)hit.circle.radius)
                    return true;
                break;
        }
        return false;
    }

    public static bool AoActHitTestCorReverse(AppMain.AOS_ACT_HIT hit, float x, float y)
    {
        AppMain.AoActCorReverse(ref x, ref y);
        return AppMain.AoActHitTest(hit, x, y);
    }

    public static uint AoActUtilGetActNum(AppMain.AOS_ACTION act)
    {
        return act == null ? 0U : 1U + AppMain.AoActUtilGetActNum(act.child) + AppMain.AoActUtilGetActNum(act.sibling);
    }

    public static AppMain.AOS_ACTION AoActUtilGetActFromId(AppMain.AOS_ACTION act, uint id)
    {
        AppMain.AOS_ACTION aosAction = (AppMain.AOS_ACTION)null;
        if ((int)AppMain.aoActGetAmaAct(act).id == (int)id)
        {
            aosAction = act;
        }
        else
        {
            if (act.child != null)
                aosAction = AppMain.AoActUtilGetActFromId(act.child, id);
            if (aosAction == null && act.sibling != null)
                aosAction = AppMain.AoActUtilGetActFromId(act.sibling, id);
        }
        return aosAction;
    }

    public static AppMain.AOS_SPRITE AoActUtilGetSprFromAct(AppMain.AOS_ACTION act)
    {
        return act.sprite;
    }

    public static void AoActDrawCorWide(
      AppMain.ArrayPointer<AppMain.NNS_PRIM3D_PN> v,
      uint v_num,
      AppMain.AOE_ACT_CORW type)
    {
        switch (type)
        {
            case AppMain.AOE_ACT_CORW.AOD_ACT_CORW_NONE:
                for (int index = 0; (long)index < (long)v_num; ++index)
                    v[index].Pos.x *= 1.125f;
                break;
            case AppMain.AOE_ACT_CORW.AOD_ACT_CORW_CENTER:
                for (int index = 0; (long)index < (long)v_num; ++index)
                    v[index].Pos.x += 60f;
                break;
            case AppMain.AOE_ACT_CORW.AOD_ACT_CORW_RIGHT:
                float num1 = 120f;
                for (int index = 0; (long)index < (long)v_num; ++index)
                    v[index].Pos.x += num1;
                break;
            case AppMain.AOE_ACT_CORW.AOD_ACT_CORW_LEFT_S:
                for (int index = 0; (long)index < (long)v_num; ++index)
                    v[index].Pos.x += (float)AppMain.g_ao_act_wide_shift;
                break;
            case AppMain.AOE_ACT_CORW.AOD_ACT_CORW_RIGHT_S:
                float num2 = 120f - (float)AppMain.g_ao_act_wide_shift;
                for (int index = 0; (long)index < (long)v_num; ++index)
                    v[index].Pos.x += num2;
                break;
        }
    }

    public static void AoActDrawCorWide(
      AppMain.NNS_PRIM3D_PC[] v,
      int i0,
      uint v_num,
      AppMain.AOE_ACT_CORW type)
    {
        switch (type)
        {
            case AppMain.AOE_ACT_CORW.AOD_ACT_CORW_NONE:
                for (int index = 0; (long)index < (long)v_num; ++index)
                    v[i0 + index].Pos.x *= 1.125f;
                break;
            case AppMain.AOE_ACT_CORW.AOD_ACT_CORW_CENTER:
                for (int index = 0; (long)index < (long)v_num; ++index)
                    v[i0 + index].Pos.x += 60f;
                break;
            case AppMain.AOE_ACT_CORW.AOD_ACT_CORW_RIGHT:
                float num1 = 120f;
                for (int index = 0; (long)index < (long)v_num; ++index)
                    v[i0 + index].Pos.x += num1;
                break;
            case AppMain.AOE_ACT_CORW.AOD_ACT_CORW_LEFT_S:
                for (int index = 0; (long)index < (long)v_num; ++index)
                    v[i0 + index].Pos.x += (float)AppMain.g_ao_act_wide_shift;
                break;
            case AppMain.AOE_ACT_CORW.AOD_ACT_CORW_RIGHT_S:
                float num2 = 120f - (float)AppMain.g_ao_act_wide_shift;
                for (int index = 0; (long)index < (long)v_num; ++index)
                    v[i0 + index].Pos.x += num2;
                break;
        }
    }

    public static void AoActDrawCorWide(
      AppMain.NNS_PRIM3D_PCT_ARRAY array,
      int i0,
      uint v_num,
      AppMain.AOE_ACT_CORW type)
    {
        AppMain.NNS_PRIM3D_PCT[] buffer = array.buffer;
        i0 += array.offset;
        switch (type)
        {
            case AppMain.AOE_ACT_CORW.AOD_ACT_CORW_NONE:
                for (int index = 0; (long)index < (long)v_num; ++index)
                    buffer[i0 + index].Pos.x *= 1.125f;
                break;
            case AppMain.AOE_ACT_CORW.AOD_ACT_CORW_CENTER:
                for (int index = 0; (long)index < (long)v_num; ++index)
                    buffer[i0 + index].Pos.x += 60f;
                break;
            case AppMain.AOE_ACT_CORW.AOD_ACT_CORW_RIGHT:
                float num1 = 120f;
                for (int index = 0; (long)index < (long)v_num; ++index)
                    buffer[i0 + index].Pos.x += num1;
                break;
            case AppMain.AOE_ACT_CORW.AOD_ACT_CORW_LEFT_S:
                for (int index = 0; (long)index < (long)v_num; ++index)
                    buffer[i0 + index].Pos.x += (float)AppMain.g_ao_act_wide_shift;
                break;
            case AppMain.AOE_ACT_CORW.AOD_ACT_CORW_RIGHT_S:
                float num2 = 120f - (float)AppMain.g_ao_act_wide_shift;
                for (int index = 0; (long)index < (long)v_num; ++index)
                    buffer[i0 + index].Pos.x += num2;
                break;
        }
    }

    private void AoActDrawCorWide(ref float x, ref float y, AppMain.AOE_ACT_CORW type)
    {
        float num1 = x;
        float num2 = y;
        switch (type)
        {
            case AppMain.AOE_ACT_CORW.AOD_ACT_CORW_NONE:
                num1 *= 1.125f;
                break;
            case AppMain.AOE_ACT_CORW.AOD_ACT_CORW_CENTER:
                num1 += 60f;
                break;
            case AppMain.AOE_ACT_CORW.AOD_ACT_CORW_RIGHT:
                num1 += 120f;
                break;
            case AppMain.AOE_ACT_CORW.AOD_ACT_CORW_LEFT_S:
                num1 += (float)AppMain.g_ao_act_wide_shift;
                break;
            case AppMain.AOE_ACT_CORW.AOD_ACT_CORW_RIGHT_S:
                num1 += 120f - (float)AppMain.g_ao_act_wide_shift;
                break;
        }
        x = num1;
        y = num2;
    }

    public static void AoActCorReverse(ref float x, ref float y)
    {
        float num1 = x;
        float num2 = y;
        float num3 = num1 * 2.25f;
        float num4 = num2 * 2.25f;
        float num5 = num3 * 0.8888889f;
        x = num5;
        y = num4;
    }

    public static AppMain.AOS_ACTION AoActUtilGetChild(AppMain.AOS_ACTION act)
    {
        return act.child;
    }

    public static AppMain.AOS_ACTION AoActUtilGetSibling(AppMain.AOS_ACTION act)
    {
        return act.sibling;
    }

    public static bool AoActIsAma(byte[] file, int offset)
    {
        return (int)file[offset + 1] == (int)AppMain.AmaMagicId[1] && (int)file[offset + 2] == (int)AppMain.AmaMagicId[2] && (int)file[offset + 3] == (int)AppMain.AmaMagicId[3];
    }

    public static void AoActDrawPre()
    {
        AppMain.NNS_MATRIX aoActDrawPreMtx = AppMain.AoActDrawPre_mtx;
        aoActDrawPreMtx.Clear();
        AppMain.nnMakeOrthoMatrix(aoActDrawPreMtx, 0.0f, 720f, 1080f, 0.0f, 1f, 3f);
        AppMain.amDrawSetProjection(aoActDrawPreMtx, 1);
        AppMain.nnMakeUnitMatrix(aoActDrawPreMtx);
        AppMain.nnRotateZMatrix(aoActDrawPreMtx, aoActDrawPreMtx, AppMain.NNM_DEGtoA32(90f));
        AppMain.nnTranslateMatrix(aoActDrawPreMtx, aoActDrawPreMtx, 0.0f, -720f, 0.0f);
        AppMain.amDrawSetWorldViewMatrix(aoActDrawPreMtx);
        AppMain.nnSetPrimitive3DMatrix(aoActDrawPreMtx);
    }

    public static void aoActAcmSprite(AppMain.AOS_SPRITE spr)
    {
        AppMain.AOS_ACT_ACM aosActAcm = (AppMain.AOS_ACT_ACM)~AppMain.g_ao_act_acm_cur;
        uint num1 = (uint)(int)~AppMain.g_ao_act_acm_flag_cur;
        if (((int)num1 & 32) == 0)
        {
            spr.offset.left *= aosActAcm.scale_x;
            spr.offset.right *= aosActAcm.scale_x;
            spr.offset.top *= aosActAcm.scale_y;
            spr.offset.bottom *= aosActAcm.scale_y;
            if (((int)spr.hit.flag & 1) == 0)
            {
                spr.hit.scale_x *= aosActAcm.scale_x;
                spr.hit.scale_y *= aosActAcm.scale_y;
            }
            if ((double)aosActAcm.rotate != 0.0)
            {
                float pSn;
                float pCs;
                AppMain.amSinCos(AppMain.NNM_DEGtoA32(aosActAcm.rotate), out pSn, out pCs);
                float num2 = (float)((double)spr.center_x * (double)pCs - (double)spr.center_y * (double)pSn);
                float num3 = (float)((double)spr.center_x * (double)pSn + (double)spr.center_y * (double)pCs);
                spr.center_x = num2;
                spr.center_y = num3;
                spr.rotate += aosActAcm.rotate;
            }
            spr.center_x *= aosActAcm.trans_scale_x;
            spr.center_y *= aosActAcm.trans_scale_y;
        }
        if (((int)num1 & 8) == 0)
        {
            spr.center_x += aosActAcm.trans_x;
            spr.center_y += aosActAcm.trans_y;
            spr.prio += aosActAcm.trans_z;
        }
        if (((int)num1 & 16) != 0)
            return;
        spr.color.r = (byte)((uint)spr.color.r * (uint)aosActAcm.color.r >> 8);
        spr.color.g = (byte)((uint)spr.color.g * (uint)aosActAcm.color.g >> 8);
        spr.color.b = (byte)((uint)spr.color.b * (uint)aosActAcm.color.b >> 8);
        spr.color.a = (byte)((uint)spr.color.a * (uint)aosActAcm.color.a >> 8);
        uint num4 = (uint)spr.fade.r + (uint)aosActAcm.fade.r;
        spr.fade.r = num4 <= (uint)byte.MaxValue ? (byte)num4 : byte.MaxValue;
        uint num5 = (uint)spr.fade.g + (uint)aosActAcm.fade.g;
        spr.fade.g = num5 <= (uint)byte.MaxValue ? (byte)num5 : byte.MaxValue;
        uint num6 = (uint)spr.fade.b + (uint)aosActAcm.fade.b;
        spr.fade.b = num6 <= (uint)byte.MaxValue ? (byte)num6 : byte.MaxValue;
        uint num7 = (uint)spr.fade.a + (uint)aosActAcm.fade.a;
        if (num7 > (uint)byte.MaxValue)
            spr.fade.a = byte.MaxValue;
        else
            spr.fade.a = (byte)num7;
    }

    public static void aoActApply(AppMain.AOS_ACTION act)
    {
        AppMain.A2S_AMA_ACT act1 = AppMain.aoActGetAmaAct(act);
        AppMain.AoActAcmPush();
        if (act1 != null && ((int)act.flag & 1) != 0)
        {
            float frame1;
            for (frame1 = act.frame; act1.next != null && (double)act1.frm_num <= (double)frame1; act1 = act1.next)
                frame1 -= (float)act1.frm_num;
            float frame2 = frame1;
            AppMain.aoActSearchTrsKey(act1, ref frame2, ref act.last_key.trs);
            float frame3 = frame1;
            AppMain.aoActSearchMtnKey(act1, ref frame3, ref act.last_key.mtn);
            float frame4 = frame1;
            AppMain.aoActSearchAnmKey(act1, ref frame4, ref act.last_key.anm);
            float frame5 = frame1;
            AppMain.aoActSearchMatKey(act1, ref frame5, ref act.last_key.mat);
            float frame6 = frame1;
            AppMain.aoActSearchHitKey(act1, ref frame6, ref act.last_key.hit);
            AppMain.AOS_SPRITE sprite = act.sprite;
            sprite.flag = act1.flag;
            sprite.offset.left = act1.ofst.left;
            sprite.offset.right = act1.ofst.right;
            sprite.offset.top = act1.ofst.top;
            sprite.offset.bottom = act1.ofst.bottom;
            float scale_x;
            float scale_y;
            if (act1.mtn == null)
            {
                sprite.center_x = 0.0f;
                sprite.center_y = 0.0f;
                sprite.prio = 0.0f;
                scale_x = 1f;
                scale_y = 1f;
                sprite.rotate = 0.0f;
            }
            else
            {
                AppMain.aoActMakeTrs(act1.mtn.trs_key_num, act1.mtn.trs_frm_num, act1.mtn.trs_key_tbl, act1.mtn.trs_tbl, act.last_key.trs, frame2, ref sprite.center_x, ref sprite.center_y, ref sprite.prio);
                AppMain.aoActMakeMtn(act1.mtn.mtn_key_num, act1.mtn.mtn_frm_num, act1.mtn.mtn_key_tbl, act1.mtn.mtn_tbl, act.last_key.mtn, frame3, out scale_x, out scale_y, out sprite.rotate);
                sprite.offset.left *= scale_x;
                sprite.offset.right *= scale_x;
                sprite.offset.top *= scale_y;
                sprite.offset.bottom *= scale_y;
            }
            if (act1.anm == null)
            {
                sprite.tex_id = -1;
                sprite.color.r = byte.MaxValue;
                sprite.color.g = byte.MaxValue;
                sprite.color.b = byte.MaxValue;
                sprite.color.a = byte.MaxValue;
                sprite.fade.r = (byte)0;
                sprite.fade.g = (byte)0;
                sprite.fade.b = (byte)0;
                sprite.fade.a = (byte)0;
            }
            else
            {
                AppMain.aoActMakeAnm(act1.anm.anm_key_num, act1.anm.anm_frm_num, act1.anm.anm_key_tbl, act1.anm.anm_tbl, act.last_key.anm, frame4, ref sprite.tex_id, ref sprite.uv, ref sprite.clamp);
                AppMain.aoActMakeMat(act1.anm.mat_key_num, act1.anm.mat_frm_num, act1.anm.mat_key_tbl, act1.anm.mat_tbl, act.last_key.mat, frame5, ref sprite.color, ref sprite.fade, out sprite.blend);
            }
            if (act1.hit == null)
            {
                sprite.hit.type = AppMain.AOE_ACT_HIT.AOD_ACT_HIT_NONE;
            }
            else
            {
                AppMain.aoActMakeHit(act1.hit.hit_key_num, act1.hit.hit_frm_num, act1.hit.hit_key_tbl, act1.hit.hit_tbl, act.last_key.hit, frame6, sprite.hit);
                sprite.hit.scale_x = scale_x;
                sprite.hit.scale_y = scale_y;
            }
            sprite.texlist = AppMain.g_ao_act_texlist;
            AppMain.aoActAcmSprite(sprite);
            if (act1.acm != null)
            {
                float frame7 = frame1;
                AppMain.aoActSearchAcmTrsKey(act1, ref frame7, ref act.last_key.atrs);
                float frame8 = frame1;
                AppMain.aoActSearchAcmMtnKey(act1, ref frame8, ref act.last_key.amtn);
                float frame9 = frame1;
                AppMain.aoActSearchAcmMatKey(act1, ref frame9, ref act.last_key.amat);
                AppMain.AOS_ACT_ACM acm = new AppMain.AOS_ACT_ACM();
                if (((int)act1.acm.flag & 16) != 0)
                {
                    acm.trans_x = sprite.center_x;
                    acm.trans_y = sprite.center_y;
                    acm.trans_z = sprite.prio;
                }
                else
                    AppMain.aoActMakeTrs(act1.acm.trs_key_num, act1.acm.trs_frm_num, act1.acm.trs_key_tbl, act1.acm.trs_tbl, act.last_key.atrs, frame7, ref acm.trans_x, ref acm.trans_y, ref acm.trans_z);
                if (((int)act1.acm.flag & 8) != 0)
                {
                    acm.trans_scale_x = 1f;
                    acm.trans_scale_y = 1f;
                    acm.scale_x = scale_x;
                    acm.scale_y = scale_y;
                    acm.rotate = sprite.rotate;
                }
                else
                    AppMain.aoActMakeAcm(act1.acm.acm_key_num, act1.acm.acm_frm_num, act1.acm.acm_key_tbl, act1.acm.acm_tbl, act.last_key.amtn, frame8, ref acm.trans_scale_x, ref acm.trans_scale_y, ref acm.scale_x, ref acm.scale_y, ref acm.rotate);
                if (((int)act1.acm.flag & 32) != 0)
                {
                    acm.color = sprite.color;
                    acm.fade = sprite.fade;
                }
                else
                    AppMain.aoActMakeMat(act1.acm.mat_key_num, act1.acm.mat_frm_num, act1.acm.mat_key_tbl, act1.acm.mat_tbl, act.last_key.amat, frame9, ref acm.color, ref acm.fade, out uint _);
                AppMain.AoActAcmApply(acm);
            }
        }
        act.flag &= 4294967294U;
        if (act.child != null)
            AppMain.aoActApply(act.child);
        AppMain.AoActAcmPop();
        if (act.sibling == null)
            return;
        AppMain.aoActApply(act.sibling);
    }

    public static void aoActSearchTrsKey(AppMain.A2S_AMA_ACT act, ref float frame, ref int key)
    {
        if (act == null || act.mtn == null || (act.mtn.trs_key_tbl == null || act.mtn.trs_frm_num == 0U))
        {
            frame = 0.0f;
            key = -1;
        }
        else
        {
            AppMain.A2S_AMA_MTN mtn = act.mtn;
            if (((int)mtn.flag & 2) != 0)
                frame = AppMain.aoActGetLoopFrame(frame, mtn.trs_frm_num);
            else if ((double)frame >= (double)mtn.trs_frm_num)
                frame = (float)mtn.trs_frm_num;
            AppMain.aoActSerachKey(mtn.trs_key_tbl, mtn.trs_key_num, ref frame, ref key);
        }
    }

    public static void aoActSearchMtnKey(AppMain.A2S_AMA_ACT act, ref float frame, ref int key)
    {
        if (act == null || act.mtn == null || (act.mtn.mtn_key_tbl == null || act.mtn.mtn_frm_num == 0U))
        {
            frame = 0.0f;
            key = -1;
        }
        else
        {
            AppMain.A2S_AMA_MTN mtn = act.mtn;
            if (((int)mtn.flag & 1) != 0)
                frame = AppMain.aoActGetLoopFrame(frame, mtn.mtn_frm_num);
            else if ((double)frame >= (double)mtn.mtn_frm_num)
                frame = (float)mtn.mtn_frm_num;
            AppMain.aoActSerachKey(mtn.mtn_key_tbl, mtn.mtn_key_num, ref frame, ref key);
        }
    }

    public static void aoActSearchAnmKey(AppMain.A2S_AMA_ACT act, ref float frame, ref int key)
    {
        if (act == null || act.anm == null || (act.anm.anm_key_tbl == null || act.anm.anm_frm_num == 0U))
        {
            frame = 0.0f;
            key = -1;
        }
        else
        {
            AppMain.A2S_AMA_ANM anm = act.anm;
            if (((int)anm.flag & 1) != 0)
                frame = AppMain.aoActGetLoopFrame(frame, anm.anm_frm_num);
            else if ((double)frame >= (double)anm.anm_frm_num)
                frame = (float)anm.anm_frm_num;
            AppMain.aoActSerachKey(anm.anm_key_tbl, anm.anm_key_num, ref frame, ref key);
        }
    }

    public static void aoActSearchMatKey(AppMain.A2S_AMA_ACT act, ref float frame, ref int key)
    {
        if (act == null || act.anm == null || (act.anm.mat_key_tbl == null || act.anm.mat_frm_num == 0U))
        {
            frame = 0.0f;
            key = -1;
        }
        else
        {
            AppMain.A2S_AMA_ANM anm = act.anm;
            if (((int)anm.flag & 2) != 0)
                frame = AppMain.aoActGetLoopFrame(frame, anm.mat_frm_num);
            else if ((double)frame >= (double)anm.mat_frm_num)
                frame = (float)anm.mat_frm_num;
            AppMain.aoActSerachKey(anm.mat_key_tbl, anm.mat_key_num, ref frame, ref key);
        }
    }

    public static void aoActSearchAcmTrsKey(AppMain.A2S_AMA_ACT act, ref float frame, ref int key)
    {
        if (act == null || act.acm == null || (act.acm.trs_key_tbl == null || act.acm.trs_frm_num == 0U) || ((int)act.acm.flag & 16) != 0)
        {
            frame = 0.0f;
            key = -1;
        }
        else
        {
            AppMain.A2S_AMA_ACM acm = act.acm;
            if (((int)acm.flag & 2) != 0)
                frame = AppMain.aoActGetLoopFrame(frame, acm.trs_frm_num);
            else if ((double)frame >= (double)acm.trs_frm_num)
                frame = (float)acm.trs_frm_num;
            AppMain.aoActSerachKey(acm.trs_key_tbl, acm.trs_key_num, ref frame, ref key);
        }
    }

    public static void aoActSearchAcmMtnKey(AppMain.A2S_AMA_ACT act, ref float frame, ref int key)
    {
        if (act == null || act.acm == null || (act.acm.acm_key_tbl == null || act.acm.acm_frm_num == 0U) || ((int)act.acm.flag & 8) != 0)
        {
            frame = 0.0f;
            key = -1;
        }
        else
        {
            AppMain.A2S_AMA_ACM acm = act.acm;
            if (((int)acm.flag & 1) != 0)
                frame = AppMain.aoActGetLoopFrame(frame, acm.acm_frm_num);
            else if ((double)frame >= (double)acm.acm_frm_num)
                frame = (float)acm.acm_frm_num;
            AppMain.aoActSerachKey(acm.acm_key_tbl, acm.acm_key_num, ref frame, ref key);
        }
    }

    public static void aoActSearchAcmMatKey(AppMain.A2S_AMA_ACT act, ref float frame, ref int key)
    {
        if (act == null || act.acm == null || (act.acm.mat_key_tbl == null || act.acm.mat_frm_num == 0U) || ((int)act.acm.flag & 32) != 0)
        {
            frame = 0.0f;
            key = -1;
        }
        else
        {
            AppMain.A2S_AMA_ACM acm = act.acm;
            if (((int)acm.flag & 4) != 0)
                frame = AppMain.aoActGetLoopFrame(frame, acm.mat_frm_num);
            else if ((double)frame >= (double)acm.mat_frm_num)
                frame = (float)acm.mat_frm_num;
            AppMain.aoActSerachKey(acm.mat_key_tbl, acm.mat_key_num, ref frame, ref key);
        }
    }

    public static void aoActSearchHitKey(AppMain.A2S_AMA_ACT act, ref float frame, ref int key)
    {
        if (act == null || act.hit == null || (act.hit.hit_key_tbl == null || act.hit.hit_frm_num == 0U))
        {
            frame = 0.0f;
            key = -1;
        }
        else
        {
            AppMain.A2S_AMA_HIT hit = act.hit;
            if (((int)hit.flag & 1) != 0)
                frame = AppMain.aoActGetLoopFrame(frame, hit.hit_frm_num);
            else if ((double)frame >= (double)hit.hit_frm_num)
                frame = (float)hit.hit_frm_num;
            AppMain.aoActSerachKey(hit.hit_key_tbl, hit.hit_key_num, ref frame, ref key);
        }
    }

    public static void aoActMakeTrs(
      uint key_num,
      uint frm_num,
      AppMain.A2S_SUB_KEY[] key_tbl,
      AppMain.A2S_SUB_TRS[] trs_tbl,
      int key,
      float frame,
      ref float trans_x,
      ref float trans_y,
      ref float trans_z)
    {
        if (key < 0 || key_num == 0U)
        {
            trans_x = 0.0f;
            trans_y = 0.0f;
            trans_z = 0.0f;
        }
        else
        {
            uint key1 = (uint)key;
            uint key2 = 0;
            float rate;
            if (!AppMain.aoActGetInterpolInfo(key_tbl, key_num, frm_num, frame, key1, ref key2, out rate))
            {
                AppMain.A2S_SUB_TRS a2SSubTrs = trs_tbl[(int)key1];
                trans_x = a2SSubTrs.trs_x;
                trans_y = a2SSubTrs.trs_y;
                trans_z = a2SSubTrs.trs_z;
            }
            else
            {
                AppMain.A2S_SUB_TRS t1 = trs_tbl[(int)key1];
                AppMain.A2S_SUB_TRS t2 = trs_tbl[(int)key2];
                rate = AppMain.aoActGetAcceleRate(rate, t1.trs_accele);
                if (key_tbl[(int)key1].interpol == 2U && key_num >= 4U)
                {
                    int index1 = (int)key1 - 1;
                    if (index1 < 0)
                        index1 = (int)key_num - 1;
                    int index2 = (int)key2 + 1;
                    if (index2 >= (int)key_num)
                        index2 = 0;
                    AppMain.A2S_SUB_TRS t0 = trs_tbl[index1];
                    AppMain.A2S_SUB_TRS t3 = trs_tbl[index2];
                    AppMain.aoActGetInterpolSpline(t0, t1, t2, t3, rate, ref trans_x, ref trans_y, ref trans_z);
                }
                else
                {
                    trans_x = AppMain.aoActInterpolF32(t1.trs_x, t2.trs_x, rate);
                    trans_y = AppMain.aoActInterpolF32(t1.trs_y, t2.trs_y, rate);
                    trans_z = AppMain.aoActInterpolF32(t1.trs_z, t2.trs_z, rate);
                }
            }
        }
    }

    public static void aoActMakeMtn(
      uint key_num,
      uint frm_num,
      AppMain.A2S_SUB_KEY[] key_tbl,
      AppMain.A2S_SUB_MTN[] mtn_tbl,
      int key,
      float frame,
      out float scale_x,
      out float scale_y,
      out float rotate)
    {
        if (key < 0 || key_num == 0U)
        {
            scale_x = 1f;
            scale_y = 1f;
            rotate = 0.0f;
        }
        else
        {
            uint key1 = (uint)key;
            uint key2 = 0;
            float rate;
            if (!AppMain.aoActGetInterpolInfo(key_tbl, key_num, frm_num, frame, key1, ref key2, out rate))
            {
                AppMain.A2S_SUB_MTN a2SSubMtn = mtn_tbl[(int)key1];
                scale_x = a2SSubMtn.scl_x;
                scale_y = a2SSubMtn.scl_y;
                rotate = a2SSubMtn.rot;
            }
            else
            {
                AppMain.A2S_SUB_MTN a2SSubMtn1 = mtn_tbl[(int)key1];
                AppMain.A2S_SUB_MTN a2SSubMtn2 = mtn_tbl[(int)key2];
                float acceleRate1 = AppMain.aoActGetAcceleRate(rate, a2SSubMtn1.scl_accele);
                float acceleRate2 = AppMain.aoActGetAcceleRate(rate, a2SSubMtn1.rot_accele);
                scale_x = AppMain.aoActInterpolF32(a2SSubMtn1.scl_x, a2SSubMtn2.scl_x, acceleRate1);
                scale_y = AppMain.aoActInterpolF32(a2SSubMtn1.scl_y, a2SSubMtn2.scl_y, acceleRate1);
                rotate = AppMain.aoActInterpolF32(a2SSubMtn1.rot, a2SSubMtn2.rot, acceleRate2);
            }
        }
    }

    public static void aoActMakeAnm(
      uint key_num,
      uint frm_num,
      AppMain.A2S_SUB_KEY[] key_tbl,
      AppMain.A2S_SUB_ANM[] anm_tbl,
      int key,
      float frame,
      ref int tex_id,
      ref AppMain.AOS_ACT_RECT rect,
      ref uint clamp)
    {
        if (key < 0 || key_num == 0U)
        {
            tex_id = -1;
        }
        else
        {
            uint key1 = (uint)key;
            uint key2 = 0;
            float rate;
            if (!AppMain.aoActGetInterpolInfo(key_tbl, key_num, frm_num, frame, key1, ref key2, out rate))
            {
                AppMain.A2S_SUB_ANM a2SSubAnm = anm_tbl[(int)key1];
                tex_id = a2SSubAnm.tex_id;
                rect.left = a2SSubAnm.texel.left;
                rect.top = a2SSubAnm.texel.top;
                rect.right = a2SSubAnm.texel.right;
                rect.bottom = a2SSubAnm.texel.bottom;
                clamp = a2SSubAnm.clamp;
            }
            else
            {
                AppMain.A2S_SUB_ANM a2SSubAnm1 = anm_tbl[(int)key1];
                AppMain.A2S_SUB_ANM a2SSubAnm2 = anm_tbl[(int)key2];
                rate = AppMain.aoActGetAcceleRate(rate, a2SSubAnm1.texel_accele);
                tex_id = a2SSubAnm1.tex_id;
                rect.left = AppMain.aoActInterpolF32(a2SSubAnm1.texel.left, a2SSubAnm2.texel.left, rate);
                rect.top = AppMain.aoActInterpolF32(a2SSubAnm1.texel.top, a2SSubAnm2.texel.top, rate);
                rect.right = AppMain.aoActInterpolF32(a2SSubAnm1.texel.right, a2SSubAnm2.texel.right, rate);
                rect.bottom = AppMain.aoActInterpolF32(a2SSubAnm1.texel.bottom, a2SSubAnm2.texel.bottom, rate);
                clamp = a2SSubAnm1.clamp;
            }
        }
    }

    public static void aoActMakeMat(
      uint key_num,
      uint frm_num,
      AppMain.A2S_SUB_KEY[] key_tbl,
      AppMain.A2S_SUB_MAT[] mat_tbl,
      int key,
      float frame,
      ref AppMain.AOS_ACT_COL color,
      ref AppMain.AOS_ACT_COL fade,
      out uint blend)
    {
        if (key < 0 || key_num == 0U)
        {
            color.r = byte.MaxValue;
            color.g = byte.MaxValue;
            color.b = byte.MaxValue;
            color.a = byte.MaxValue;
            fade.r = (byte)0;
            fade.g = (byte)0;
            fade.b = (byte)0;
            fade.a = (byte)0;
            blend = 1U;
        }
        else
        {
            uint key1 = (uint)key;
            uint key2 = 0;
            float rate;
            if (!AppMain.aoActGetInterpolInfo(key_tbl, key_num, frm_num, frame, key1, ref key2, out rate))
            {
                AppMain.A2S_SUB_MAT a2SSubMat = mat_tbl[(int)key1];
                color.r = a2SSubMat.base_.r;
                color.g = a2SSubMat.base_.g;
                color.b = a2SSubMat.base_.b;
                color.a = a2SSubMat.base_.a;
                fade.r = a2SSubMat.fade.r;
                fade.g = a2SSubMat.fade.g;
                fade.b = a2SSubMat.fade.b;
                fade.a = a2SSubMat.fade.a;
                blend = a2SSubMat.blend;
            }
            else
            {
                AppMain.A2S_SUB_MAT a2SSubMat1 = mat_tbl[(int)key1];
                AppMain.A2S_SUB_MAT a2SSubMat2 = mat_tbl[(int)key2];
                float acceleRate1 = AppMain.aoActGetAcceleRate(rate, a2SSubMat1.base_accele);
                float acceleRate2 = AppMain.aoActGetAcceleRate(rate, a2SSubMat1.fade_accele);
                color = AppMain.aoActInterpolCol(a2SSubMat1.base_, a2SSubMat2.base_, acceleRate1);
                fade = AppMain.aoActInterpolCol(a2SSubMat1.fade, a2SSubMat2.fade, acceleRate2);
                blend = a2SSubMat1.blend;
            }
        }
    }

    public static void aoActMakeAcm(
      uint key_num,
      uint frm_num,
      AppMain.A2S_SUB_KEY[] key_tbl,
      AppMain.A2S_SUB_ACM[] acm_tbl,
      int key,
      float frame,
      ref float tscale_x,
      ref float tscale_y,
      ref float scale_x,
      ref float scale_y,
      ref float rotate)
    {
        if (key < 0 || key_num == 0U)
        {
            tscale_x = 1f;
            tscale_y = 1f;
            scale_x = 1f;
            scale_y = 1f;
            rotate = 0.0f;
        }
        else
        {
            uint key1 = (uint)key;
            uint key2 = 0;
            float rate;
            if (!AppMain.aoActGetInterpolInfo(key_tbl, key_num, frm_num, frame, key1, ref key2, out rate))
            {
                AppMain.A2S_SUB_ACM a2SSubAcm = acm_tbl[(int)key1];
                tscale_x = a2SSubAcm.trs_scl_x;
                tscale_y = a2SSubAcm.trs_scl_y;
                scale_x = a2SSubAcm.scl_x;
                scale_y = a2SSubAcm.scl_y;
                rotate = a2SSubAcm.rot;
            }
            else
            {
                AppMain.A2S_SUB_ACM a2SSubAcm1 = acm_tbl[(int)key1];
                AppMain.A2S_SUB_ACM a2SSubAcm2 = acm_tbl[(int)key2];
                float acceleRate1 = AppMain.aoActGetAcceleRate(rate, a2SSubAcm1.trs_scl_accele);
                float acceleRate2 = AppMain.aoActGetAcceleRate(rate, a2SSubAcm1.scl_accele);
                float acceleRate3 = AppMain.aoActGetAcceleRate(rate, a2SSubAcm1.rot_accele);
                tscale_x = AppMain.aoActInterpolF32(a2SSubAcm1.trs_scl_x, a2SSubAcm2.trs_scl_x, acceleRate1);
                tscale_y = AppMain.aoActInterpolF32(a2SSubAcm1.trs_scl_y, a2SSubAcm2.trs_scl_y, acceleRate1);
                scale_x = AppMain.aoActInterpolF32(a2SSubAcm1.scl_x, a2SSubAcm2.scl_x, acceleRate2);
                scale_y = AppMain.aoActInterpolF32(a2SSubAcm1.scl_y, a2SSubAcm2.scl_y, acceleRate2);
                rotate = AppMain.aoActInterpolF32(a2SSubAcm1.rot, a2SSubAcm2.rot, acceleRate3);
            }
        }
    }

    public static void aoActMakeHit(
      uint key_num,
      uint frm_num,
      AppMain.A2S_SUB_KEY[] key_tbl,
      AppMain.A2S_SUB_HIT[] hit_tbl,
      int key,
      float frame,
      AppMain.AOS_ACT_HITP hit)
    {
        hit.scale_x = 1f;
        hit.scale_y = 1f;
        if (key < 0 || key_num == 0U)
        {
            hit.type = AppMain.AOE_ACT_HIT.AOD_ACT_HIT_NONE;
        }
        else
        {
            uint key1 = (uint)key;
            uint key2 = 0;
            float rate;
            if (!AppMain.aoActGetInterpolInfo(key_tbl, key_num, frm_num, frame, key1, ref key2, out rate))
            {
                AppMain.A2S_SUB_HIT a2SSubHit = hit_tbl[(int)key1];
                hit.flag = a2SSubHit.flag;
                switch (a2SSubHit.type)
                {
                    case 1:
                        hit.type = AppMain.AOE_ACT_HIT.AOD_ACT_HIT_RECT;
                        hit.rect.Assign(ref a2SSubHit.rect);
                        break;
                    case 2:
                        hit.type = AppMain.AOE_ACT_HIT.AOD_ACT_HIT_CIRCLE;
                        hit.rect.Assign(ref a2SSubHit.rect);
                        break;
                    default:
                        hit.type = AppMain.AOE_ACT_HIT.AOD_ACT_HIT_NONE;
                        break;
                }
            }
            else
            {
                AppMain.A2S_SUB_HIT a2SSubHit1 = hit_tbl[(int)key1];
                AppMain.A2S_SUB_HIT a2SSubHit2 = hit_tbl[(int)key2];
                if ((int)a2SSubHit1.type != (int)a2SSubHit2.type)
                {
                    hit.flag = a2SSubHit1.flag;
                    switch (a2SSubHit1.type)
                    {
                        case 1:
                            hit.type = AppMain.AOE_ACT_HIT.AOD_ACT_HIT_RECT;
                            hit.rect.Assign(ref a2SSubHit1.rect);
                            break;
                        case 2:
                            hit.type = AppMain.AOE_ACT_HIT.AOD_ACT_HIT_CIRCLE;
                            hit.SetCircle(ref a2SSubHit1.circle);
                            break;
                        default:
                            hit.type = AppMain.AOE_ACT_HIT.AOD_ACT_HIT_NONE;
                            break;
                    }
                }
                else
                {
                    rate = AppMain.aoActGetAcceleRate(rate, a2SSubHit1.hit_accele);
                    hit.flag = a2SSubHit1.flag;
                    switch (a2SSubHit1.type)
                    {
                        case 1:
                            hit.type = AppMain.AOE_ACT_HIT.AOD_ACT_HIT_RECT;
                            hit.rect = new AppMain.AOS_ACT_RECT()
                            {
                                left = AppMain.aoActInterpolF32(a2SSubHit1.rect.left, a2SSubHit2.rect.left, rate),
                                top = AppMain.aoActInterpolF32(a2SSubHit1.rect.top, a2SSubHit2.rect.top, rate),
                                right = AppMain.aoActInterpolF32(a2SSubHit1.rect.right, a2SSubHit2.rect.right, rate),
                                bottom = AppMain.aoActInterpolF32(a2SSubHit1.rect.bottom, a2SSubHit2.rect.bottom, rate)
                            };
                            break;
                        case 2:
                            hit.type = AppMain.AOE_ACT_HIT.AOD_ACT_HIT_CIRCLE;
                            var circle = new AppMain.AOS_ACT_CIRCLE()
                            {
                                center_x = AppMain.aoActInterpolF32(a2SSubHit1.circle.center_x, a2SSubHit2.circle.center_x, rate),
                                center_y = AppMain.aoActInterpolF32(a2SSubHit1.circle.center_y, a2SSubHit2.circle.center_y, rate),
                                radius = AppMain.aoActInterpolF32(a2SSubHit1.circle.radius, a2SSubHit2.circle.radius, rate)
                            };
                            hit.SetCircle(ref circle);
                            break;
                        default:
                            hit.type = AppMain.AOE_ACT_HIT.AOD_ACT_HIT_NONE;
                            break;
                    }
                }
            }
        }
    }

    public static void aoActSerachKey(
      AppMain.A2S_SUB_KEY[] key,
      uint key_num,
      ref float frame,
      ref int last)
    {
        if ((double)frame < 1.0)
        {
            frame = 0.0f;
            last = 0;
        }
        else
        {
            uint num1 = (uint)frame;
            uint num2;
            if (last < 0)
            {
                num2 = key_num / 2U;
                for (uint index = (key_num + 1U) / 2U; index > 1U; index = (index + 1U) / 2U)
                {
                    if (key[(int)num2].frm <= num1)
                        num2 += index / 2U;
                    else
                        num2 -= index / 2U;
                }
                if (key[(int)num2].frm > num1)
                    --num2;
            }
            else
            {
                num2 = (uint)last;
                do
                {
                    if (key[(int)num2].frm <= num1)
                    {
                        if (key[(int)(num2 + 1U)].frm <= num1)
                            ++num2;
                        else
                            goto label_20;
                    }
                    else
                        --num2;
                    if (num2 <= 0U)
                    {
                        num2 = 0U;
                        goto label_20;
                    }
                }
                while (num2 < key_num - 1U);
                num2 = key_num - 1U;
            }
        label_20:
            last = (int)num2;
            frame -= (float)key[(int)num2].frm;
        }
    }

    public static float aoActGetLoopFrame(float frame, uint len)
    {
        return (float)((uint)frame % len) + (frame - (float)(uint)frame);
    }

    public static bool aoActGetInterpolInfo(
      AppMain.A2S_SUB_KEY[] key_tbl,
      uint key_num,
      uint frm_num,
      float frame,
      uint key1,
      ref uint key2,
      out float rate)
    {
        AppMain.A2S_SUB_KEY a2SSubKey1 = key_tbl[(int)key1];
        if (a2SSubKey1.interpol != 1U && a2SSubKey1.interpol != 2U || key_num <= 1U)
        {
            rate = 0.0f;
            return false;
        }
        if (key1 + 1U < key_num)
        {
            key2 = key1 + 1U;
            AppMain.A2S_SUB_KEY a2SSubKey2 = key_tbl[(int)key1 + 1];
            rate = frame / (float)(a2SSubKey2.frm - a2SSubKey1.frm);
        }
        else
        {
            key2 = 0U;
            rate = frame / (float)(frm_num - a2SSubKey1.frm);
        }
        return true;
    }

    public static void aoActGetInterpolSpline(
      AppMain.A2S_SUB_TRS t0,
      AppMain.A2S_SUB_TRS t1,
      AppMain.A2S_SUB_TRS t2,
      AppMain.A2S_SUB_TRS t3,
      float rate,
      ref float trans_x,
      ref float trans_y,
      ref float trans_z)
    {
        float num1 = rate;
        float num2 = num1 * num1;
        float num3 = num2 * num1;
        float num4 = (float)(-0.5 * ((double)num3 + (double)num1)) + num2;
        float num5 = (float)(1.5 * (double)num3 - 2.5 * (double)num2 + 1.0);
        float num6 = (float)(-1.5 * (double)num3 + 2.0 * (double)num2 + 0.5 * (double)num1);
        float num7 = (float)(0.5 * ((double)num3 - (double)num2));
        trans_x = (float)((double)t0.trs_x * (double)num4 + (double)t1.trs_x * (double)num5 + (double)t2.trs_x * (double)num6 + (double)t3.trs_x * (double)num7);
        trans_y = (float)((double)t0.trs_y * (double)num4 + (double)t1.trs_y * (double)num5 + (double)t2.trs_y * (double)num6 + (double)t3.trs_y * (double)num7);
        trans_z = (float)((double)t0.trs_z * (double)num4 + (double)t1.trs_z * (double)num5 + (double)t2.trs_z * (double)num6 + (double)t3.trs_z * (double)num7);
    }

    public static float aoActGetAcceleRate(float rate, float accele)
    {
        return (float)((double)rate * (double)accele + (double)rate * (double)rate * (1.0 - (double)accele));
    }

    public static AppMain.AOS_SPRITE aoActAllocSprite()
    {
        if (AppMain.g_ao_act_spr_num >= AppMain.g_ao_act_spr_buf_size)
            return (AppMain.AOS_SPRITE)null;
        AppMain.AOS_SPRITE aosSprite = AppMain.g_ao_act_spr_ref[(int)AppMain.g_ao_act_spr_alloc];
        ++AppMain.g_ao_act_spr_alloc;
        if (AppMain.g_ao_act_spr_alloc >= AppMain.g_ao_act_spr_buf_size)
            AppMain.g_ao_act_spr_alloc = 0U;
        ++AppMain.g_ao_act_spr_num;
        if (AppMain.g_ao_act_spr_num > AppMain.g_ao_act_spr_peak)
            AppMain.g_ao_act_spr_peak = AppMain.g_ao_act_spr_num;
        return aosSprite;
    }

    public static void aoActFreeSprite(AppMain.AOS_SPRITE spr)
    {
        if (AppMain.g_ao_act_spr_num == 0U)
            return;
        AppMain.AoActSortUnregSprite(spr);
        AppMain.g_ao_act_spr_ref[(int)AppMain.g_ao_act_spr_free] = spr;
        ++AppMain.g_ao_act_spr_free;
        if (AppMain.g_ao_act_spr_free >= AppMain.g_ao_act_spr_buf_size)
            AppMain.g_ao_act_spr_free = 0U;
        --AppMain.g_ao_act_spr_num;
        if (AppMain.g_ao_act_spr_num <= AppMain.g_ao_act_spr_peak)
            return;
        AppMain.g_ao_act_spr_peak = AppMain.g_ao_act_spr_num;
    }

    public static AppMain.AOS_ACTION aoActAllocAction()
    {
        if (AppMain.g_ao_act_num >= AppMain.g_ao_act_buf_size)
            return (AppMain.AOS_ACTION)null;
        AppMain.AOS_ACTION aosAction = AppMain.g_ao_act_ref[(int)AppMain.g_ao_act_alloc];
        ++AppMain.g_ao_act_alloc;
        if (AppMain.g_ao_act_alloc >= AppMain.g_ao_act_buf_size)
            AppMain.g_ao_act_alloc = 0U;
        ++AppMain.g_ao_act_num;
        if (AppMain.g_ao_act_num > AppMain.g_ao_act_peak)
            AppMain.g_ao_act_peak = AppMain.g_ao_act_num;
        return aosAction;
    }

    public static void aoActFreeAction(AppMain.AOS_ACTION act)
    {
        if (AppMain.g_ao_act_num == 0U || Array.IndexOf<AppMain.AOS_ACTION>(AppMain.g_ao_act_buf, act) < 0)
            return;
        AppMain.AoActSortUnregAction(act);
        AppMain.g_ao_act_ref[(int)AppMain.g_ao_act_free] = act;
        ++AppMain.g_ao_act_free;
        if (AppMain.g_ao_act_free >= AppMain.g_ao_act_buf_size)
            AppMain.g_ao_act_free = 0U;
        --AppMain.g_ao_act_num;
        if (AppMain.g_ao_act_num <= AppMain.g_ao_act_peak)
            return;
        AppMain.g_ao_act_peak = AppMain.g_ao_act_num;
    }

    public static uint aoActGetAmaActState(AppMain.A2S_AMA_ACT act, float frame)
    {
        uint num = 0;
        if (act != null)
        {
            for (; act.next != null; act = act.next)
            {
                float frmNum = (float)act.frm_num;
                if ((double)frmNum <= (double)frame)
                    frame -= frmNum;
                else
                    break;
            }
            if (AppMain.aoActIsAmaActEnd(act, frame))
                num |= 1U;
            if (AppMain.aoActIsAmaTrsEnd(act.mtn, frame))
                num |= 2U;
            if (AppMain.aoActIsAmaMtnEnd(act.mtn, frame))
                num |= 4U;
            if (AppMain.aoActIsAmaAnmEnd(act.anm, frame))
                num |= 8U;
            if (AppMain.aoActIsAmaMatEnd(act.anm, frame))
                num |= 16U;
            if (act.acm != null)
            {
                if (((int)act.acm.flag & 16) != 0)
                {
                    if (AppMain.aoActIsAmaTrsEnd(act.mtn, frame))
                        num |= 32U;
                }
                else if (AppMain.aoActIsAmaAcmTrsEnd(act.acm, frame))
                    num |= 32U;
                if (((int)act.acm.flag & 8) != 0)
                {
                    if (AppMain.aoActIsAmaMtnEnd(act.mtn, frame))
                        num |= 64U;
                }
                else if (AppMain.aoActIsAmaAcmMtnEnd(act.acm, frame))
                    num |= 64U;
                if (((int)act.acm.flag & 32) != 0)
                {
                    if (AppMain.aoActIsAmaMatEnd(act.anm, frame))
                        num |= 128U;
                }
                else if (AppMain.aoActIsAmaAcmMatEnd(act.acm, frame))
                    num |= 128U;
            }
            if (AppMain.aoActIsAmaUsrEnd(act.usr, frame))
                num |= 256U;
            if (AppMain.aoActIsAmaHitEnd(act.hit, frame))
                num |= 512U;
        }
        return num;
    }

    public static bool aoActIsAmaActEnd(AppMain.A2S_AMA_ACT act, float frame)
    {
        return act == null || (double)act.frm_num <= (double)frame;
    }

    public static bool aoActIsAmaTrsEnd(AppMain.A2S_AMA_MTN mtn, float frame)
    {
        return mtn == null || mtn.trs_key_tbl == null || ((int)mtn.flag & 2) == 0 && (double)mtn.trs_frm_num <= (double)frame;
    }

    public static bool aoActIsAmaMtnEnd(AppMain.A2S_AMA_MTN mtn, float frame)
    {
        return mtn == null || mtn.mtn_key_tbl == null || ((int)mtn.flag & 1) == 0 && (double)mtn.mtn_frm_num <= (double)frame;
    }

    public static bool aoActIsAmaAnmEnd(AppMain.A2S_AMA_ANM anm, float frame)
    {
        return anm == null || anm.anm_key_tbl == null || ((int)anm.flag & 1) == 0 && (double)anm.anm_frm_num <= (double)frame;
    }

    public static bool aoActIsAmaMatEnd(AppMain.A2S_AMA_ANM anm, float frame)
    {
        return anm == null || anm.mat_key_tbl == null || ((int)anm.flag & 2) == 0 && (double)anm.mat_frm_num <= (double)frame;
    }

    public static bool aoActIsAmaAcmTrsEnd(AppMain.A2S_AMA_ACM acm, float frame)
    {
        return acm == null || acm.trs_key_tbl == null || ((int)acm.flag & 2) == 0 && (double)acm.trs_frm_num <= (double)frame;
    }

    public static bool aoActIsAmaAcmMtnEnd(AppMain.A2S_AMA_ACM acm, float frame)
    {
        return acm == null || acm.acm_key_tbl == null || ((int)acm.flag & 1) == 0 && (double)acm.acm_frm_num <= (double)frame;
    }

    public static bool aoActIsAmaAcmMatEnd(AppMain.A2S_AMA_ACM acm, float frame)
    {
        return acm == null || acm.mat_key_tbl == null || ((int)acm.flag & 4) == 0 && (double)acm.mat_frm_num <= (double)frame;
    }

    public static bool aoActIsAmaUsrEnd(AppMain.A2S_AMA_USR usr, float frame)
    {
        return usr == null || usr.usr_key_tbl == null || ((int)usr.flag & 1) == 0 && (double)usr.usr_frm_num <= (double)frame;
    }

    public static bool aoActIsAmaHitEnd(AppMain.A2S_AMA_HIT hit, float frame)
    {
        return hit == null || hit.hit_key_tbl == null || ((int)hit.flag & 1) == 0 && (double)hit.hit_frm_num <= (double)frame;
    }

    public static AppMain.A2S_AMA_ACT aoActGetAmaAct(AppMain.AOS_ACTION act)
    {
        AppMain.A2S_AMA_ACT a2SAmaAct = (AppMain.A2S_AMA_ACT)null;
        if (act == null)
            return (AppMain.A2S_AMA_ACT)null;
        switch (act.type)
        {
            case AppMain.AOE_ACT_TYPE.AOD_ACT_TYPE_ACTION:
                a2SAmaAct = (AppMain.A2S_AMA_ACT)act.data;
                break;
            case AppMain.AOE_ACT_TYPE.AOD_ACT_TYPE_NODE:
                a2SAmaAct = ((AppMain.A2S_AMA_NODE)act.data).act;
                break;
        }
        return a2SAmaAct;
    }

    public static float aoActInterpolF32(float d1, float d2, float rate)
    {
        return (float)((double)d1 * (1.0 - (double)rate) + (double)d2 * (double)rate);
    }

    public static AppMain.AOS_ACT_COL aoActInterpolCol(
      AppMain.A2S_SUB_COL d1,
      AppMain.A2S_SUB_COL d2,
      float rate)
    {
        AppMain.AOS_ACT_COL aosActCol = new AppMain.AOS_ACT_COL();
        int num1 = (int)((double)byte.MaxValue * (double)rate);
        if (num1 < 0)
            num1 = 0;
        else if (num1 > (int)byte.MaxValue)
            num1 = (int)byte.MaxValue;
        int num2 = (int)byte.MaxValue - num1;
        aosActCol.r = (byte)(((int)d1.r * num2 + (int)d2.r * num1) / (int)byte.MaxValue);
        aosActCol.g = (byte)(((int)d1.g * num2 + (int)d2.g * num1) / (int)byte.MaxValue);
        aosActCol.b = (byte)(((int)d1.b * num2 + (int)d2.b * num1) / (int)byte.MaxValue);
        aosActCol.a = (byte)(((int)d1.a * num2 + (int)d2.a * num1) / (int)byte.MaxValue);
        return aosActCol;
    }

    public static void aoActDrawTask(AppMain.AMS_TCB tcb)
    {
        AppMain.AOS_ACT_DRAW work = (AppMain.AOS_ACT_DRAW)AppMain.amTaskGetWork(tcb);
        AppMain.amDrawPushState();
        AppMain.amDrawInitState();
        AppMain.AoActDrawPre();
        AppMain.nnSetPrimitive3DAlphaFuncGL(519U, 0.5f);
        AppMain.nnSetPrimitive3DDepthMaskGL(false);
        AppMain.nnSetPrimitive3DDepthFuncGL(519U);
        for (uint index1 = 0; index1 < work.count; ++index1)
        {
            AppMain.AOS_SPRITE aosSprite1 = work.sprite[(int)index1];
            if (aosSprite1.blend == 2U)
                AppMain.nnSetPrimitiveBlend(0);
            else
                AppMain.nnSetPrimitiveBlend(1);
            bool flag;
            if (aosSprite1.fade.a > (byte)0)
            {
                AppMain.amDrawSetFogColor((float)aosSprite1.fade.r / (float)byte.MaxValue, (float)aosSprite1.fade.g / (float)byte.MaxValue, (float)aosSprite1.fade.b / (float)byte.MaxValue);
                float fnear = (float)(2.0 - (double)aosSprite1.fade.a / (double)byte.MaxValue);
                AppMain.amDrawSetFogRange(fnear, fnear + 1f);
                AppMain.amDrawSetFog(1);
                flag = true;
            }
            else
            {
                AppMain.amDrawSetFog(0);
                flag = false;
            }
            if (aosSprite1.tex_id >= 0 && aosSprite1.texlist != null)
            {
                AppMain.nnSetPrimitiveTexNum(aosSprite1.texlist, aosSprite1.tex_id);
                AppMain.nnSetPrimitiveTexState(0, 0, ((int)aosSprite1.clamp & 2) == 0 ? 0 : 1, ((int)aosSprite1.clamp & 1) == 0 ? 0 : 1);
                AppMain.nnBeginDrawPrimitive3D(4, aosSprite1.blend != 0U ? 1 : 0, 0, 0);
                AppMain.NNS_PRIM3D_PCT[] aoActDrawTaskPct = AppMain.aoActDrawTask_pct;
                AppMain.aoActDrawTask_pct_array.buffer = aoActDrawTaskPct;
                AppMain.aoActDrawTask_pct_array.offset = 0;
                do
                {
                    aoActDrawTaskPct[0].Tex.u = aoActDrawTaskPct[1].Tex.u = aosSprite1.uv.left;
                    aoActDrawTaskPct[2].Tex.u = aoActDrawTaskPct[3].Tex.u = aosSprite1.uv.right;
                    aoActDrawTaskPct[0].Tex.v = aoActDrawTaskPct[2].Tex.v = aosSprite1.uv.top;
                    aoActDrawTaskPct[1].Tex.v = aoActDrawTaskPct[3].Tex.v = aosSprite1.uv.bottom;
                    aoActDrawTaskPct[0].Col = (uint)((int)aosSprite1.color.r << 24 | (int)aosSprite1.color.g << 16 | (int)aosSprite1.color.b << 8) | (uint)aosSprite1.color.a;
                    aoActDrawTaskPct[1].Col = aoActDrawTaskPct[2].Col = aoActDrawTaskPct[3].Col = aoActDrawTaskPct[0].Col;
                    aoActDrawTaskPct[0].Pos.x = aoActDrawTaskPct[1].Pos.x = aosSprite1.offset.left;
                    aoActDrawTaskPct[2].Pos.x = aoActDrawTaskPct[3].Pos.x = aosSprite1.offset.right;
                    aoActDrawTaskPct[0].Pos.y = aoActDrawTaskPct[2].Pos.y = aosSprite1.offset.top;
                    aoActDrawTaskPct[1].Pos.y = aoActDrawTaskPct[3].Pos.y = aosSprite1.offset.bottom;
                    aoActDrawTaskPct[0].Pos.z = aoActDrawTaskPct[1].Pos.z = aoActDrawTaskPct[2].Pos.z = aoActDrawTaskPct[3].Pos.z = -2f;
                    if ((double)aosSprite1.rotate != 0.0)
                    {
                        float pSn;
                        float pCs;
                        AppMain.amSinCos(AppMain.NNM_RADtoA32(aosSprite1.rotate), out pSn, out pCs);
                        for (int index2 = 0; index2 < 4; ++index2)
                        {
                            float num1 = (float)((double)aoActDrawTaskPct[index2].Pos.x * (double)pCs - (double)aoActDrawTaskPct[index2].Pos.y * (double)pSn);
                            float num2 = (float)((double)aoActDrawTaskPct[index2].Pos.x * (double)pSn + (double)aoActDrawTaskPct[index2].Pos.y * (double)pCs);
                            aoActDrawTaskPct[index2].Pos.x = num1;
                            aoActDrawTaskPct[index2].Pos.y = num2;
                        }
                    }
                    aoActDrawTaskPct[0].Pos.x += aosSprite1.center_x;
                    aoActDrawTaskPct[1].Pos.x += aosSprite1.center_x;
                    aoActDrawTaskPct[2].Pos.x += aosSprite1.center_x;
                    aoActDrawTaskPct[3].Pos.x += aosSprite1.center_x;
                    aoActDrawTaskPct[0].Pos.y += aosSprite1.center_y;
                    aoActDrawTaskPct[1].Pos.y += aosSprite1.center_y;
                    aoActDrawTaskPct[2].Pos.y += aosSprite1.center_y;
                    aoActDrawTaskPct[3].Pos.y += aosSprite1.center_y;
                    if (AppMain.SyGetEvtInfo().cur_evt_id >= (short)3)
                    {
                        float num = aoActDrawTaskPct[1].Pos.y - aoActDrawTaskPct[0].Pos.y;
                        aoActDrawTaskPct[0].Pos.y *= 0.9f;
                        aoActDrawTaskPct[0].Pos.y += 32f;
                        aoActDrawTaskPct[2].Pos.y *= 0.9f;
                        aoActDrawTaskPct[2].Pos.y += 32f;
                        aoActDrawTaskPct[1].Pos.y = aoActDrawTaskPct[0].Pos.y + num;
                        aoActDrawTaskPct[3].Pos.y = aoActDrawTaskPct[2].Pos.y + num;
                    }
                    AppMain.aoActDrawCorW(AppMain.aoActDrawTask_pct_array, 0, 4U, aosSprite1.flag);
                    aoActDrawTaskPct[5] = aoActDrawTaskPct[3];
                    aoActDrawTaskPct[4] = aoActDrawTaskPct[1];
                    aoActDrawTaskPct[3] = aoActDrawTaskPct[2];
                    AppMain.nnDrawPrimitive3D(0, (object)AppMain.aoActDrawTask_pct_array, 6);
                    if (index1 + 1U < work.count)
                    {
                        AppMain.AOS_SPRITE aosSprite2 = work.sprite[(int)(index1 + 1U)];
                        if ((int)aosSprite1.blend == (int)aosSprite2.blend && (int)aosSprite1.fade.c == (int)aosSprite2.fade.c && (aosSprite1.texlist == aosSprite2.texlist && aosSprite1.tex_id == aosSprite2.tex_id) && (int)aosSprite1.clamp == (int)aosSprite2.clamp)
                        {
                            aosSprite1 = aosSprite2;
                            ++index1;
                        }
                        else
                            aosSprite1 = (AppMain.AOS_SPRITE)null;
                    }
                    else
                        aosSprite1 = (AppMain.AOS_SPRITE)null;
                }
                while (aosSprite1 != null);
                AppMain.nnEndDrawPrimitive3D();
            }
            else
            {
                AppMain.nnSetPrimitiveTexNum((AppMain.NNS_TEXLIST)null, -1);
                AppMain.nnBeginDrawPrimitive3D(2, aosSprite1.blend != 0U ? 1 : 0, 0, 0);
                AppMain.NNS_PRIM3D_PC[] aoActDrawTaskPc = AppMain.aoActDrawTask_pc;
                do
                {
                    aoActDrawTaskPc[0].Col = (uint)((int)aosSprite1.color.r << 24 | (int)aosSprite1.color.g << 16 | (int)aosSprite1.color.b << 8) | (uint)aosSprite1.color.a;
                    aoActDrawTaskPc[1].Col = aoActDrawTaskPc[2].Col = aoActDrawTaskPc[3].Col = aoActDrawTaskPc[0].Col;
                    aoActDrawTaskPc[0].Pos.x = aoActDrawTaskPc[1].Pos.x = aosSprite1.offset.left;
                    aoActDrawTaskPc[2].Pos.x = aoActDrawTaskPc[3].Pos.x = aosSprite1.offset.right;
                    aoActDrawTaskPc[0].Pos.y = aoActDrawTaskPc[2].Pos.y = aosSprite1.offset.top;
                    aoActDrawTaskPc[1].Pos.y = aoActDrawTaskPc[3].Pos.y = aosSprite1.offset.bottom;
                    aoActDrawTaskPc[0].Pos.z = aoActDrawTaskPc[1].Pos.z = aoActDrawTaskPc[2].Pos.z = aoActDrawTaskPc[3].Pos.z = -2f;
                    if ((double)aosSprite1.rotate != 0.0)
                    {
                        float pSn;
                        float pCs;
                        AppMain.amSinCos(AppMain.NNM_DEGtoA32(aosSprite1.rotate), out pSn, out pCs);
                        for (int index2 = 0; index2 < 4; ++index2)
                        {
                            float num1 = (float)((double)aoActDrawTaskPc[index2].Pos.x * (double)pCs - (double)aoActDrawTaskPc[index2].Pos.y * (double)pSn);
                            float num2 = (float)((double)aoActDrawTaskPc[index2].Pos.x * (double)pSn + (double)aoActDrawTaskPc[index2].Pos.y * (double)pCs);
                            aoActDrawTaskPc[index2].Pos.x = num1;
                            aoActDrawTaskPc[index2].Pos.y = num2;
                        }
                    }
                    aoActDrawTaskPc[0].Pos.x += aosSprite1.center_x;
                    aoActDrawTaskPc[1].Pos.x += aosSprite1.center_x;
                    aoActDrawTaskPc[2].Pos.x += aosSprite1.center_x;
                    aoActDrawTaskPc[3].Pos.x += aosSprite1.center_x;
                    aoActDrawTaskPc[0].Pos.y += aosSprite1.center_y;
                    aoActDrawTaskPc[1].Pos.y += aosSprite1.center_y;
                    aoActDrawTaskPc[2].Pos.y += aosSprite1.center_y;
                    aoActDrawTaskPc[3].Pos.y += aosSprite1.center_y;
                    float num = aoActDrawTaskPc[1].Pos.y - aoActDrawTaskPc[0].Pos.y;
                    aoActDrawTaskPc[0].Pos.y *= 0.9f;
                    aoActDrawTaskPc[0].Pos.y += 32f;
                    aoActDrawTaskPc[2].Pos.y *= 0.9f;
                    aoActDrawTaskPc[2].Pos.y += 32f;
                    aoActDrawTaskPc[1].Pos.y = aoActDrawTaskPc[0].Pos.y + num;
                    aoActDrawTaskPc[3].Pos.y = aoActDrawTaskPc[2].Pos.y + num;
                    AppMain.aoActDrawCorW(aoActDrawTaskPc, 0, 4U, aosSprite1.flag);
                    aoActDrawTaskPc[5] = aoActDrawTaskPc[3];
                    aoActDrawTaskPc[4] = aoActDrawTaskPc[1];
                    aoActDrawTaskPc[3] = aoActDrawTaskPc[2];
                    AppMain.nnDrawPrimitive3D(0, (object)aoActDrawTaskPc, 6);
                    if (index1 + 1U < work.count)
                    {
                        AppMain.AOS_SPRITE aosSprite2 = work.sprite[(int)(index1 + 1U)];
                        if ((int)aosSprite1.blend == (int)aosSprite2.blend && (int)aosSprite1.fade.c == (int)aosSprite2.fade.c && (aosSprite2.texlist == null || aosSprite2.tex_id < 0))
                        {
                            aosSprite1 = aosSprite2;
                            ++index1;
                        }
                        else
                            aosSprite1 = (AppMain.AOS_SPRITE)null;
                    }
                    else
                        aosSprite1 = (AppMain.AOS_SPRITE)null;
                }
                while (aosSprite1 != null);
                AppMain.nnEndDrawPrimitive3D();
            }
            if (flag)
                AppMain.amDrawSetFog(0);
        }
        AppMain.amDrawPopState();
    }

    public static void aoActDrawSprState(AppMain.AOS_SPRITE spr_tbl)
    {
        AppMain.aoActDrawSprState_spr_tbl[0] = spr_tbl;
        AppMain.aoActDrawSprState(AppMain.aoActDrawSprState_spr_tbl, 1U);
    }

    public static void aoActDrawSprState(AppMain.AOS_SPRITE[] spr_tbl, uint num)
    {
        uint num1;
        for (uint index1 = 0; index1 < num; index1 += num1)
        {
            AppMain.AOS_SPRITE aosSprite1 = spr_tbl[(int)index1];
            num1 = 1U;
            uint num2 = index1 + 1U;
            while (num2 < num)
            {
                AppMain.AOS_SPRITE aosSprite2 = spr_tbl[(int)num2];
                if ((int)aosSprite1.blend == (int)aosSprite2.blend && (int)aosSprite1.fade.c == (int)aosSprite2.fade.c && (aosSprite1.texlist == aosSprite2.texlist && aosSprite1.tex_id == aosSprite2.tex_id) && (int)aosSprite1.clamp == (int)aosSprite2.clamp)
                {
                    ++num2;
                    ++num1;
                }
                else
                    break;
            }
            AppMain.AMS_PARAM_DRAW_PRIMITIVE setParam = AppMain.GlobalPool<AppMain.AMS_PARAM_DRAW_PRIMITIVE>.Alloc();
            if (aosSprite1.fade.a > (byte)0)
            {
                AppMain.amDrawSetFogColor(AppMain.g_ao_act_sys_draw_state, (float)aosSprite1.fade.r / (float)byte.MaxValue, (float)aosSprite1.fade.g / (float)byte.MaxValue, (float)aosSprite1.fade.b / (float)byte.MaxValue);
                float fnear = (float)(2.0 - (double)aosSprite1.fade.a / (double)byte.MaxValue);
                AppMain.amDrawSetFogRange(AppMain.g_ao_act_sys_draw_state, fnear, fnear + 1f);
                AppMain.amDrawSetFog(AppMain.g_ao_act_sys_draw_state, 1);
            }
            else
                AppMain.amDrawSetFog(AppMain.g_ao_act_sys_draw_state, 0);
            setParam.mtx = (AppMain.NNS_MATRIX)null;
            setParam.type = 0;
            setParam.count = 6 * (int)num1;
            setParam.ablend = aosSprite1.blend != 0U ? 1 : 0;
            setParam.sortZ = 0.0f;
            setParam.bldSrc = 770;
            setParam.bldDst = 771;
            setParam.bldMode = 32774;
            setParam.aTest = (short)0;
            setParam.zMask = (short)1;
            setParam.zTest = (short)0;
            setParam.noSort = (short)1;
            if (aosSprite1.tex_id >= 0 && aosSprite1.texlist != null)
            {
                setParam.texlist = aosSprite1.texlist;
                setParam.texId = aosSprite1.tex_id;
                setParam.uwrap = ((int)aosSprite1.clamp & 2) == 0 ? 0 : 1;
                setParam.vwrap = ((int)aosSprite1.clamp & 1) == 0 ? 0 : 1;
                setParam.vtxPCT3D = AppMain.amDrawAlloc_NNS_PRIM3D_PCT(6 * (int)num1);
                AppMain.NNS_PRIM3D_PCT[] buffer = setParam.vtxPCT3D.buffer;
                int offset = setParam.vtxPCT3D.offset;
                setParam.format3D = 4;
                for (int index2 = 0; (long)index2 < (long)num1; ++index2)
                {
                    AppMain.AOS_SPRITE aosSprite2 = spr_tbl[(long)index1 + (long)index2];
                    int index3 = offset + 6 * index2;
                    buffer[index3].Tex.u = buffer[index3 + 1].Tex.u = aosSprite2.uv.left;
                    buffer[index3 + 2].Tex.u = buffer[index3 + 3].Tex.u = aosSprite2.uv.right;
                    buffer[index3].Tex.v = buffer[index3 + 2].Tex.v = aosSprite2.uv.top;
                    buffer[index3 + 1].Tex.v = buffer[index3 + 3].Tex.v = aosSprite2.uv.bottom;
                    buffer[index3].Col = (uint)((int)aosSprite2.color.r << 24 | (int)aosSprite2.color.g << 16 | (int)aosSprite2.color.b << 8) | (uint)aosSprite2.color.a;
                    buffer[index3 + 1].Col = buffer[index3 + 2].Col = buffer[index3 + 3].Col = buffer[index3].Col;
                    buffer[index3].Pos.x = buffer[index3 + 1].Pos.x = aosSprite2.offset.left;
                    buffer[index3 + 2].Pos.x = buffer[index3 + 3].Pos.x = aosSprite2.offset.right;
                    buffer[index3].Pos.y = buffer[index3 + 2].Pos.y = aosSprite2.offset.top;
                    buffer[index3 + 1].Pos.y = buffer[index3 + 3].Pos.y = aosSprite2.offset.bottom;
                    buffer[index3].Pos.z = buffer[index3 + 1].Pos.z = buffer[index3 + 2].Pos.z = buffer[index3 + 3].Pos.z = -2f;
                    if ((double)aosSprite2.rotate != 0.0)
                    {
                        float pSn;
                        float pCs;
                        AppMain.amSinCos(AppMain.NNM_DEGtoA32(aosSprite2.rotate), out pSn, out pCs);
                        for (int index4 = 0; index4 < 4; ++index4)
                        {
                            int index5 = index3 + index4;
                            float num3 = (float)((double)buffer[index5].Pos.x * (double)pCs - (double)buffer[index5].Pos.y * (double)pSn);
                            float num4 = (float)((double)buffer[index5].Pos.x * (double)pSn + (double)buffer[index5].Pos.y * (double)pCs);
                            buffer[index5].Pos.x = num3;
                            buffer[index5].Pos.y = num4;
                        }
                    }
                    buffer[index3].Pos.x += aosSprite2.center_x;
                    buffer[index3 + 1].Pos.x += aosSprite2.center_x;
                    buffer[index3 + 2].Pos.x += aosSprite2.center_x;
                    buffer[index3 + 3].Pos.x += aosSprite2.center_x;
                    buffer[index3].Pos.y += aosSprite2.center_y;
                    buffer[index3 + 1].Pos.y += aosSprite2.center_y;
                    buffer[index3 + 2].Pos.y += aosSprite2.center_y;
                    buffer[index3 + 3].Pos.y += aosSprite2.center_y;
                    float num5 = buffer[index3 + 1].Pos.y - buffer[index3].Pos.y;
                    buffer[index3].Pos.y *= 0.9f;
                    buffer[index3].Pos.y += 32f;
                    buffer[index3 + 2].Pos.y *= 0.9f;
                    buffer[index3 + 2].Pos.y += 32f;
                    buffer[index3 + 1].Pos.y = buffer[index3].Pos.y + num5;
                    buffer[index3 + 3].Pos.y = buffer[index3 + 2].Pos.y + num5;
                    AppMain.aoActDrawCorW(setParam.vtxPCT3D, index3 - offset, 4U, aosSprite2.flag);
                    buffer[index3 + 5] = buffer[index3 + 3];
                    buffer[index3 + 4] = buffer[index3 + 1];
                    buffer[index3 + 3] = buffer[index3 + 2];
                }
            }
            else
            {
                setParam.texlist = (AppMain.NNS_TEXLIST)null;
                setParam.texId = -1;
                AppMain.NNS_PRIM3D_PC[] v = AppMain.amDrawAlloc_NNS_PRIM3D_PC(6 * (int)num1);
                setParam.vtxPC3D = v;
                setParam.format3D = 2;
                for (int index2 = 0; (long)index2 < (long)num1; ++index2)
                {
                    AppMain.AOS_SPRITE aosSprite2 = spr_tbl[(long)index1 + (long)index2];
                    int i0 = 6 * index2;
                    v[i0].Col = (uint)((int)aosSprite2.color.r << 24 | (int)aosSprite2.color.g << 16 | (int)aosSprite2.color.b << 8) | (uint)aosSprite2.color.a;
                    v[i0 + 1].Col = v[i0 + 2].Col = v[i0 + 3].Col = v[i0].Col;
                    v[i0].Pos.x = v[i0 + 1].Pos.x = aosSprite2.offset.left;
                    v[i0 + 2].Pos.x = v[i0 + 3].Pos.x = aosSprite2.offset.right;
                    v[i0].Pos.y = v[i0 + 2].Pos.y = aosSprite2.offset.top;
                    v[i0 + 1].Pos.y = v[i0 + 3].Pos.y = aosSprite2.offset.bottom;
                    v[i0].Pos.z = v[i0 + 1].Pos.z = v[i0 + 2].Pos.z = v[i0 + 3].Pos.z = -2f;
                    if ((double)aosSprite2.rotate != 0.0)
                    {
                        float pSn;
                        float pCs;
                        AppMain.amSinCos(AppMain.NNM_DEGtoA32(aosSprite2.rotate), out pSn, out pCs);
                        for (int index3 = 0; index3 < 4; ++index3)
                        {
                            int index4 = i0 + index3;
                            float num3 = (float)((double)v[index4].Pos.x * (double)pCs - (double)v[index4].Pos.y * (double)pSn);
                            float num4 = (float)((double)v[index4].Pos.x * (double)pSn + (double)v[index4].Pos.y * (double)pCs);
                            v[index4].Pos.x = num3;
                            v[index4].Pos.y = num4;
                        }
                    }
                    v[i0].Pos.x += aosSprite2.center_x;
                    v[i0 + 1].Pos.x += aosSprite2.center_x;
                    v[i0 + 2].Pos.x += aosSprite2.center_x;
                    v[i0 + 3].Pos.x += aosSprite2.center_x;
                    v[i0].Pos.y += aosSprite2.center_y;
                    v[i0 + 1].Pos.y += aosSprite2.center_y;
                    v[i0 + 2].Pos.y += aosSprite2.center_y;
                    v[i0 + 3].Pos.y += aosSprite2.center_y;
                    float num5 = v[i0 + 1].Pos.y - v[i0].Pos.y;
                    v[i0].Pos.y *= 0.9f;
                    v[i0].Pos.y += 32f;
                    v[i0 + 2].Pos.y *= 0.9f;
                    v[i0 + 2].Pos.y += 32f;
                    v[i0 + 1].Pos.y = v[i0].Pos.y + num5;
                    v[i0 + 3].Pos.y = v[i0 + 2].Pos.y + num5;
                    AppMain.aoActDrawCorW(v, i0, 4U, aosSprite2.flag);
                    v[i0 + 5] = v[i0 + 3];
                    v[i0 + 4] = v[i0 + 1];
                    v[i0 + 3] = v[i0 + 2];
                }
            }
            AppMain.amDrawPrimitive3D(AppMain.g_ao_act_sys_draw_state, setParam);
            AppMain.GlobalPool<AppMain.AMS_PARAM_DRAW_PRIMITIVE>.Release(setParam);
        }
        AppMain.amDrawSetFog(AppMain.g_ao_act_sys_draw_state, 0);
    }

    public static void aoActDrawSortState()
    {
        if (AppMain.g_ao_act_sort_num <= 0U)
            return;
        if (AppMain.aoActDrawSortState_spr_tbl == null || (long)AppMain.aoActDrawSortState_spr_tbl.Length < (long)AppMain.g_ao_act_sort_num)
            AppMain.aoActDrawSortState_spr_tbl = new AppMain.AOS_SPRITE[(int)AppMain.g_ao_act_sort_num];
        for (int index = 0; (long)index < (long)AppMain.g_ao_act_sort_num; ++index)
            AppMain.aoActDrawSortState_spr_tbl[index] = AppMain.g_ao_act_sort_buf[index].sprite;
        AppMain.aoActDrawSprState(AppMain.aoActDrawSortState_spr_tbl, AppMain.g_ao_act_sort_num);
    }

    public static void aoActDrawCorW(
      AppMain.ArrayPointer<AppMain.NNS_PRIM3D_PN> v,
      uint vnum,
      uint flag)
    {
        switch (flag & 3U)
        {
            case 0:
                AppMain.AoActDrawCorWide(v, vnum, AppMain.AOE_ACT_CORW.AOD_ACT_CORW_NONE);
                break;
            case 1:
                AppMain.AoActDrawCorWide(v, vnum, AppMain.AOE_ACT_CORW.AOD_ACT_CORW_CENTER);
                break;
            case 2:
                if (((int)flag & 16) != 0)
                {
                    AppMain.AoActDrawCorWide(v, vnum, AppMain.AOE_ACT_CORW.AOD_ACT_CORW_LEFT_S);
                    break;
                }
                AppMain.AoActDrawCorWide(v, vnum, AppMain.AOE_ACT_CORW.AOD_ACT_CORW_LEFT);
                break;
            case 3:
                if (((int)flag & 16) != 0)
                {
                    AppMain.AoActDrawCorWide(v, vnum, AppMain.AOE_ACT_CORW.AOD_ACT_CORW_RIGHT_S);
                    break;
                }
                AppMain.AoActDrawCorWide(v, vnum, AppMain.AOE_ACT_CORW.AOD_ACT_CORW_RIGHT);
                break;
        }
    }

    public static void aoActDrawCorW(AppMain.NNS_PRIM3D_PC[] v, int i0, uint vnum, uint flag)
    {
        switch (flag & 3U)
        {
            case 0:
                AppMain.AoActDrawCorWide(v, i0, vnum, AppMain.AOE_ACT_CORW.AOD_ACT_CORW_NONE);
                break;
            case 1:
                AppMain.AoActDrawCorWide(v, i0, vnum, AppMain.AOE_ACT_CORW.AOD_ACT_CORW_CENTER);
                break;
            case 2:
                if (((int)flag & 16) != 0)
                {
                    AppMain.AoActDrawCorWide(v, i0, vnum, AppMain.AOE_ACT_CORW.AOD_ACT_CORW_LEFT_S);
                    break;
                }
                AppMain.AoActDrawCorWide(v, i0, vnum, AppMain.AOE_ACT_CORW.AOD_ACT_CORW_LEFT);
                break;
            case 3:
                if (((int)flag & 16) != 0)
                {
                    AppMain.AoActDrawCorWide(v, i0, vnum, AppMain.AOE_ACT_CORW.AOD_ACT_CORW_RIGHT_S);
                    break;
                }
                AppMain.AoActDrawCorWide(v, i0, vnum, AppMain.AOE_ACT_CORW.AOD_ACT_CORW_RIGHT);
                break;
        }
    }

    public static void aoActDrawCorW(AppMain.NNS_PRIM3D_PCT_ARRAY v, int i0, uint vnum, uint flag)
    {
        switch (flag & 3U)
        {
            case 0:
                AppMain.AoActDrawCorWide(v, i0, vnum, AppMain.AOE_ACT_CORW.AOD_ACT_CORW_NONE);
                break;
            case 1:
                AppMain.AoActDrawCorWide(v, i0, vnum, AppMain.AOE_ACT_CORW.AOD_ACT_CORW_CENTER);
                break;
            case 2:
                if (((int)flag & 16) != 0)
                {
                    AppMain.AoActDrawCorWide(v, i0, vnum, AppMain.AOE_ACT_CORW.AOD_ACT_CORW_LEFT_S);
                    break;
                }
                AppMain.AoActDrawCorWide(v, i0, vnum, AppMain.AOE_ACT_CORW.AOD_ACT_CORW_LEFT);
                break;
            case 3:
                if (((int)flag & 16) != 0)
                {
                    AppMain.AoActDrawCorWide(v, i0, vnum, AppMain.AOE_ACT_CORW.AOD_ACT_CORW_RIGHT_S);
                    break;
                }
                AppMain.AoActDrawCorWide(v, i0, vnum, AppMain.AOE_ACT_CORW.AOD_ACT_CORW_RIGHT);
                break;
        }
    }
}