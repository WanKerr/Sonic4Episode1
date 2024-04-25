public partial class AppMain
{
    public static bool amTpIsTouchOn(int index)
    {
        return (_am_tp_touch[index].flag & 1) != 0;
    }

    public static bool amTpIsTouchPush(int index)
    {
        return (_am_tp_touch[index].flag & 4) != 0;
    }

    public static bool amTpIsTouchPull(int index)
    {
        return (_am_tp_touch[index].flag & 8) != 0;
    }

    public static void AMM_TP_BIT_SET16(ref ushort val, int shift, bool b)
    {
        byte num = b ? (byte)1 : (byte)0;
        val = (ushort)(val & ~(1 << shift) | num << shift);
    }

    private void amTpInit()
    {
        for (int index = 0; index < 4; ++index)
            _am_tp_touch[index] = new AMS_TP_TOUCH_STATUS();
    }

    private void amTpExecute()
    {
        this._amTpUpdateTouch_req();
        for (int index = 0; index < 4; ++index)
            this.amTpUpdateStatus(_am_tp_touch[index], _am_tp_touch[index].core);
    }

    private void amTpUpdateStatus(AMS_TP_TOUCH_STATUS status, AMS_TP_TOUCH_CORE core)
    {
        bool b1 = (core.sampling_flag & 1) != 0;
        bool b2 = (status.flag & 1) != 0;
        bool flag = b1 ^ b2;
        AMM_TP_BIT_SET16(ref status.flag, 7, (core.sampling_flag & 128) != 0);
        AMM_TP_BIT_SET16(ref status.flag, 1, b2);
        AMM_TP_BIT_SET16(ref status.flag, 0, b1);
        AMM_TP_BIT_SET16(ref status.flag, 2, flag & b1);
        AMM_TP_BIT_SET16(ref status.flag, 3, flag & b2);
        status.prev[0] = status.on[0];
        status.prev[1] = status.on[1];
        ushort[] samplingBuf = core.sampling_buf;
        status.on[0] = samplingBuf[0];
        status.on[1] = samplingBuf[1];
        if ((status.flag & 4) != 0)
        {
            status.push[0] = status.on[0];
            status.push[1] = status.on[1];
        }
        else if ((status.flag & 8) != 0)
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
        AMS_IPHONE_TP_DATA touchReqDispData = _amTpUpdateTouch_req_DispData;
        for (int TouchIndex = 0; TouchIndex < 4; ++TouchIndex)
        {
            AMS_TP_TOUCH_CORE core = _am_tp_touch[TouchIndex].core;
            amIPhoneRequestTouch(touchReqDispData, TouchIndex);
            if (touchReqDispData.touch == 1)
            {
                if (touchReqDispData.validity == 1)
                {
                    core.sampling_buf[0] = touchReqDispData.x;
                    core.sampling_buf[1] = touchReqDispData.y;
                    core.sampling_flag |= 1;
                    core.sampling_flag &= 127;
                }
                else
                    core.sampling_flag |= 128;
            }
            else
                core.sampling_flag &= 126;
        }
    }

}