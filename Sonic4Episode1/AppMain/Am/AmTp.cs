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
    public static bool amTpIsTouchOn(int index)
    {
        return ((int)AppMain._am_tp_touch[index].flag & 1) != 0;
    }

    public static bool amTpIsTouchPush(int index)
    {
        return ((int)AppMain._am_tp_touch[index].flag & 4) != 0;
    }

    public static bool amTpIsTouchPull(int index)
    {
        return ((int)AppMain._am_tp_touch[index].flag & 8) != 0;
    }

    public static void AMM_TP_BIT_SET16(ref ushort val, int shift, bool b)
    {
        byte num = b ? (byte)1 : (byte)0;
        val = (ushort)((int)val & ~(1 << shift) | (int)num << shift);
    }

    private void amTpInit()
    {
        for (int index = 0; index < 4; ++index)
            AppMain._am_tp_touch[index] = new AppMain.AMS_TP_TOUCH_STATUS();
    }

    private void amTpExecute()
    {
        this._amTpUpdateTouch_req();
        for (int index = 0; index < 4; ++index)
            this.amTpUpdateStatus(AppMain._am_tp_touch[index], AppMain._am_tp_touch[index].core);
    }

    private void amTpUpdateStatus(AppMain.AMS_TP_TOUCH_STATUS status, AppMain.AMS_TP_TOUCH_CORE core)
    {
        bool b1 = ((int)core.sampling_flag & 1) != 0;
        bool b2 = ((int)status.flag & 1) != 0;
        bool flag = b1 ^ b2;
        AppMain.AMM_TP_BIT_SET16(ref status.flag, 7, ((int)core.sampling_flag & 128) != 0);
        AppMain.AMM_TP_BIT_SET16(ref status.flag, 1, b2);
        AppMain.AMM_TP_BIT_SET16(ref status.flag, 0, b1);
        AppMain.AMM_TP_BIT_SET16(ref status.flag, 2, flag & b1);
        AppMain.AMM_TP_BIT_SET16(ref status.flag, 3, flag & b2);
        status.prev[0] = status.on[0];
        status.prev[1] = status.on[1];
        ushort[] samplingBuf = core.sampling_buf;
        status.on[0] = samplingBuf[0];
        status.on[1] = samplingBuf[1];
        if (((int)status.flag & 4) != 0)
        {
            status.push[0] = status.on[0];
            status.push[1] = status.on[1];
        }
        else if (((int)status.flag & 8) != 0)
        {
            status.pull[0] = status.prev[0];
            status.pull[1] = status.prev[1];
        }
        if (status.core == core)
            return;
        status.core = core;
    }

    private void _amTpUpdateTouch_req()
    {
        AppMain.AMS_IPHONE_TP_DATA touchReqDispData = AppMain._amTpUpdateTouch_req_DispData;
        for (int TouchIndex = 0; TouchIndex < 4; ++TouchIndex)
        {
            AppMain.AMS_TP_TOUCH_CORE core = AppMain._am_tp_touch[TouchIndex].core;
            AppMain.amIPhoneRequestTouch(touchReqDispData, TouchIndex);
            if (touchReqDispData.touch == (ushort)1)
            {
                if (touchReqDispData.validity == (ushort)1)
                {
                    core.sampling_buf[0] = touchReqDispData.x;
                    core.sampling_buf[1] = touchReqDispData.y;
                    core.sampling_flag |= (byte)1;
                    core.sampling_flag &= (byte)127;
                }
                else
                    core.sampling_flag |= (byte)128;
            }
            else
                core.sampling_flag &= (byte)126;
        }
    }

}