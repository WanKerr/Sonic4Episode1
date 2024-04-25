public partial class AppMain
{
    private static void amPadInit()
    {
        amPadInit(0, 0U);
    }

    private static void amPadInit(int pad_num)
    {
        amPadInit(pad_num, 0U);
    }

    private static void amPadInit(int pad_num, uint format)
    {
        for (int index = 0; index < 4; ++index)
        {
            _am_pad[index].Clear();
            _am_pad[index].vib_flag = 1;
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
                _am_pad[port].state &= 4294967293U;
            else
                _am_pad[port].state |= 2U;
        }
        else if (flag != 0)
        {
            for (port = 0; port < 4; ++port)
                _am_pad[port].state &= 4294967293U;
        }
        else
        {
            for (port = 0; port < 4; ++port)
                _am_pad[port].state |= 2U;
        }
    }

    private static void amPadGetData()
    {
        for (int index1 = 0; index1 < 4; ++index1)
        {
            AMS_PAD_DATA amsPadData = _am_pad[index1];
            ushort num1 = 0;
            ushort num2 = (ushort)(amsPadData.direct ^ (uint)num1);
            amsPadData.stand = (ushort)(num2 & (uint)num1);
            amsPadData.release = (ushort)(num2 & (uint)~num1);
            amsPadData.direct = num1;
            ushort num3 = 0;
            if (amsPadData.alx < -16384)
                num3 |= 4;
            else if (amsPadData.alx > 16384)
                num3 |= 8;
            if (amsPadData.aly < -16384)
                num3 |= 2;
            else if (amsPadData.aly > 16384)
                num3 |= 1;
            if (amsPadData.arx < -16384)
                num3 |= 64;
            else if (amsPadData.arx > 16384)
                num3 |= 128;
            if (amsPadData.ary < -16384)
                num3 |= 32;
            else if (amsPadData.ary > 16384)
                num3 |= 16;
            ushort num4 = (ushort)(amsPadData.adirect ^ (uint)num3);
            amsPadData.astand = (ushort)(num4 & (uint)num3);
            amsPadData.arelease = (ushort)(num4 & (uint)~num3);
            amsPadData.adirect = num3;
            amsPadData.repeat = 0;
            amsPadData.arepeat = 0;
            if (((amsPadData.stand | amsPadData.astand) & 15) != 0)
            {
                amsPadData.repeat = (ushort)(num1 & 15U);
                amsPadData.arepeat = (ushort)(num3 & 15U);
                amsPadData.timer_lv = 30;
            }
            else if (--amsPadData.timer_lv == 0)
            {
                amsPadData.repeat = (ushort)(num1 & 15U);
                amsPadData.arepeat = (ushort)(num3 & 15U);
                amsPadData.timer_lv = 5;
            }
            if (((amsPadData.stand | amsPadData.astand) & 65520) != 0)
            {
                amsPadData.repeat |= (ushort)(num1 & 65520U);
                amsPadData.arepeat |= (ushort)(num3 & 65520U);
                amsPadData.timer_btn = 30;
            }
            else if (--amsPadData.timer_btn == 0)
            {
                amsPadData.repeat |= (ushort)(num1 & 65520U);
                amsPadData.arepeat |= (ushort)(num3 & 65520U);
                amsPadData.timer_btn = 5;
            }
            int[] keepTime = amsPadData.keep_time;
            int[] keepAtime = amsPadData.keep_atime;
            int index2 = 0;
            while (index2 < 0)
            {
                if ((num2 & 1) != 0)
                {
                    amsPadData.last_time[index2] = keepTime[index2];
                    keepTime[index2] = 1;
                }
                else
                    ++keepTime[index2];
                if ((num4 & 1) != 0)
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
            amPadSetVibration(port, 0, 0);
        if (port == -1)
        {
            for (port = 0; port < 4; ++port)
                _am_pad[port].vib_flag = flag;
        }
        else
            _am_pad[port].vib_flag = flag;
    }

    private static void amPadSetVibration(int port, ushort left, ushort right)
    {
        AoPad.controllerSource[port].SetVibration(left, right);
    }

}