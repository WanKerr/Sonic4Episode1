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
    private static void amPadInit()
    {
        AppMain.amPadInit(0, 0U);
    }

    private static void amPadInit(int pad_num)
    {
        AppMain.amPadInit(pad_num, 0U);
    }

    private static void amPadInit(int pad_num, uint format)
    {
        for (int index = 0; index < 4; ++index)
        {
            AppMain._am_pad[index].Clear();
            AppMain._am_pad[index].vib_flag = 1;
        }
    }

    private static void amPadExit()
    {
    }

    private static void amPadSetDeviceFormat(int port, uint format)
    {
    }

    private static void amPadSetMapping(int port, ushort mapping)
    {
    }

    private static void amPadSetMappingGC(int port, ushort mapping)
    {
    }

    private static void amPadEnableInput(int port, int flag)
    {
        if (port != -1)
        {
            if (flag != 0)
                AppMain._am_pad[port].state &= 4294967293U;
            else
                AppMain._am_pad[port].state |= 2U;
        }
        else if (flag != 0)
        {
            for (port = 0; port < 4; ++port)
                AppMain._am_pad[port].state &= 4294967293U;
        }
        else
        {
            for (port = 0; port < 4; ++port)
                AppMain._am_pad[port].state |= 2U;
        }
    }

    private static void amPadGetData()
    {
        for (int index1 = 0; index1 < 4; ++index1)
        {
            AppMain.AMS_PAD_DATA amsPadData = AppMain._am_pad[index1];
            ushort num1 = 0;
            ushort num2 = (ushort)((uint)amsPadData.direct ^ (uint)num1);
            amsPadData.stand = (ushort)((uint)num2 & (uint)num1);
            amsPadData.release = (ushort)((uint)num2 & (uint)~num1);
            amsPadData.direct = num1;
            ushort num3 = 0;
            if (amsPadData.alx < (short)-16384)
                num3 |= (ushort)4;
            else if (amsPadData.alx > (short)16384)
                num3 |= (ushort)8;
            if (amsPadData.aly < (short)-16384)
                num3 |= (ushort)2;
            else if (amsPadData.aly > (short)16384)
                num3 |= (ushort)1;
            if (amsPadData.arx < (short)-16384)
                num3 |= (ushort)64;
            else if (amsPadData.arx > (short)16384)
                num3 |= (ushort)128;
            if (amsPadData.ary < (short)-16384)
                num3 |= (ushort)32;
            else if (amsPadData.ary > (short)16384)
                num3 |= (ushort)16;
            ushort num4 = (ushort)((uint)amsPadData.adirect ^ (uint)num3);
            amsPadData.astand = (ushort)((uint)num4 & (uint)num3);
            amsPadData.arelease = (ushort)((uint)num4 & (uint)~num3);
            amsPadData.adirect = num3;
            amsPadData.repeat = (ushort)0;
            amsPadData.arepeat = (ushort)0;
            if ((((int)amsPadData.stand | (int)amsPadData.astand) & 15) != 0)
            {
                amsPadData.repeat = (ushort)((uint)num1 & 15U);
                amsPadData.arepeat = (ushort)((uint)num3 & 15U);
                amsPadData.timer_lv = (short)30;
            }
            else if (--amsPadData.timer_lv == (short)0)
            {
                amsPadData.repeat = (ushort)((uint)num1 & 15U);
                amsPadData.arepeat = (ushort)((uint)num3 & 15U);
                amsPadData.timer_lv = (short)5;
            }
            if ((((int)amsPadData.stand | (int)amsPadData.astand) & 65520) != 0)
            {
                amsPadData.repeat |= (ushort)((uint)num1 & 65520U);
                amsPadData.arepeat |= (ushort)((uint)num3 & 65520U);
                amsPadData.timer_btn = (short)30;
            }
            else if (--amsPadData.timer_btn == (short)0)
            {
                amsPadData.repeat |= (ushort)((uint)num1 & 65520U);
                amsPadData.arepeat |= (ushort)((uint)num3 & 65520U);
                amsPadData.timer_btn = (short)5;
            }
            int[] keepTime = amsPadData.keep_time;
            int[] keepAtime = amsPadData.keep_atime;
            int index2 = 0;
            while (index2 < 0)
            {
                if (((int)num2 & 1) != 0)
                {
                    amsPadData.last_time[index2] = keepTime[index2];
                    keepTime[index2] = 1;
                }
                else
                    ++keepTime[index2];
                if (((int)num4 & 1) != 0)
                {
                    amsPadData.last_atime[index2] = keepAtime[index2];
                    keepAtime[index2] = 1;
                }
                else
                    ++keepAtime[index2];
                ++index2;
                num2 >>= 1;
                num4 >>= 1;
            }
        }
    }

    private static void amPadEnableVibration(int port, int flag)
    {
        if (flag != 0)
            AppMain.amPadSetVibration(port, (ushort)0, (ushort)0);
        if (port == -1)
        {
            for (port = 0; port < 4; ++port)
                AppMain._am_pad[port].vib_flag = flag;
        }
        else
            AppMain._am_pad[port].vib_flag = flag;
    }

    private static void amPadSetVibration(int port, ushort left, ushort right)
    {
    }

}