using System;

public class AoPad
{
    internal static ControllerSource controllerSource
        = new ControllerSource();

    public static void AoPadUpdate()
    {
        controllerSource.Update();
    }

    public static ControllerConsts AoPadDirect()
    {
        if (AppMain.AoAccountGetCurrentId() < (controllerSource?.Count ?? 0))
        {
            return PAD_DIRECT(AppMain.AoAccountGetCurrentId());
        }
        return 0;
    }

    public static ControllerConsts AoPadStand()
    {
        if (AppMain.AoAccountGetCurrentId() < (controllerSource?.Count ?? 0))
        {
            return PAD_STAND(AppMain.AoAccountGetCurrentId());
        }
        return 0;
    }

    public static ControllerConsts AoPadRepeat()
    {
        if (AppMain.AoAccountGetCurrentId() < (controllerSource?.Count ?? 0))
        {
            return PAD_REPEAT(AppMain.AoAccountGetCurrentId());
        }
        return 0;
    }

    public static ControllerConsts AoPadRelease()
    {
        if (AppMain.AoAccountGetCurrentId() < (controllerSource?.Count ?? 0))
        {
            return PAD_RELEASE(AppMain.AoAccountGetCurrentId());
        }
        return 0;
    }

    public static ControllerConsts AoPadADirect()
    {
        if (AppMain.AoAccountGetCurrentId() < (controllerSource?.Count ?? 0))
        {
            return PAD_ADIRECT(AppMain.AoAccountGetCurrentId());
        }
        return 0;
    }

    public static bool AoPadADirect(ControllerConsts key)
    {
        if (AppMain.AoAccountGetCurrentId() < (controllerSource?.Count ?? 0))
        {
            return (PAD_ADIRECT(AppMain.AoAccountGetCurrentId()) & key) != 0;
        }
        return false;
    }

    public static ControllerConsts AoPadAStand()
    {
        if (AppMain.AoAccountGetCurrentId() < (controllerSource?.Count ?? 0))
        {
            return PAD_ASTAND(AppMain.AoAccountGetCurrentId());
        }
        return 0;
    }

    public static ControllerConsts AoPadARepeat()
    {
        if (AppMain.AoAccountGetCurrentId() < (controllerSource?.Count ?? 0))
        {
            return PAD_AREPEAT(AppMain.AoAccountGetCurrentId());
        }
        return 0;
    }

    public static ControllerConsts AoPadARelease()
    {
        if (AppMain.AoAccountGetCurrentId() < (controllerSource?.Count ?? 0))
        {
            return PAD_ARELEASE(AppMain.AoAccountGetCurrentId());
        }
        return 0;
    }

    public static ControllerConsts AoPadMDirect()
    {
        if (AppMain.AoAccountGetCurrentId() < (controllerSource?.Count ?? 0))
        {
            return (ControllerConsts)PAD_MDIRECT(AppMain.AoAccountGetCurrentId());
        }
        return 0;
    }

    public static ControllerConsts AoPadMStand()
    {
        if (AppMain.AoAccountGetCurrentId() < (controllerSource?.Count ?? 0))
        {
            return (ControllerConsts)PAD_MSTAND(AppMain.AoAccountGetCurrentId());
        }
        return 0;
    }

    public static ControllerConsts AoPadMRepeat()
    {
        if (AppMain.AoAccountGetCurrentId() < (controllerSource?.Count ?? 0))
        {
            return (ControllerConsts)PAD_MREPEAT(AppMain.AoAccountGetCurrentId());
        }
        return 0;
    }

    public static ControllerConsts AoPadMRelease()
    {
        if (AppMain.AoAccountGetCurrentId() < (controllerSource?.Count ?? 0))
        {
            return (ControllerConsts)PAD_MRELEASE(AppMain.AoAccountGetCurrentId());
        }
        return 0;
    }

    public static short AoPadAnalogLX()
    {
        if (AppMain.AoAccountGetCurrentId() < (controllerSource?.Count ?? 0))
        {
            return PAD_A_LX(AppMain.AoAccountGetCurrentId());
        }
        return 0;
    }

    public static short AoPadAnalogLY()
    {
        if (AppMain.AoAccountGetCurrentId() < (controllerSource?.Count ?? 0))
        {
            return PAD_A_LY(AppMain.AoAccountGetCurrentId());
        }
        return 0;
    }

    public static short AoPadAnalogRX()
    {
        if (AppMain.AoAccountGetCurrentId() < (controllerSource?.Count ?? 0))
        {
            return PAD_A_RX(AppMain.AoAccountGetCurrentId());
        }
        return 0;
    }

    public static short AoPadAnalogRY()
    {
        if (AppMain.AoAccountGetCurrentId() < (controllerSource?.Count ?? 0))
        {
            return PAD_A_RY(AppMain.AoAccountGetCurrentId());
        }
        return 0;
    }
    
    public static int AoPadSomeoneDirect(ControllerConsts key)
    {
        for (int i = 0; i < (controllerSource?.Count ?? 0); i++)
        {
            if ((PAD_DIRECT(i) & key) != 0)
            {
                return i;
            }
        }
        return -1;
    }

    public static int AoPadSomeoneStand(ControllerConsts key)
    {
        for (int i = 0; i < (controllerSource?.Count ?? 0); i++)
        {
            if ((PAD_STAND(i) & key) != 0)
            {
                return i;
            }
        }
        return -1;
    }

    public static int AoPadSomeoneRepeat(ControllerConsts key)
    {
        for (int i = 0; i < (controllerSource?.Count ?? 0); i++)
        {
            if ((PAD_REPEAT(i) & key) != 0)
            {
                return i;
            }
        }
        return -1;
    }

    public static int AoPadSomeoneRelease(ControllerConsts key)
    {
        for (int i = 0; i < (controllerSource?.Count ?? 0); i++)
        {
            if ((PAD_RELEASE(i) & key) != 0)
            {
                return i;
            }
        }
        return -1;
    }

    public static int AoPadSomeoneADirect(ControllerConsts key)
    {
        for (int i = 0; i < (controllerSource?.Count ?? 0); i++)
        {
            if ((PAD_ADIRECT(i) & key) != 0)
            {
                return i;
            }
        }
        return -1;
    }

    public static int AoPadSomeoneAStand(ControllerConsts key)
    {
        for (int i = 0; i < (controllerSource?.Count ?? 0); i++)
        {
            if ((PAD_ASTAND(i) & key) != 0)
            {
                return i;
            }
        }
        return -1;
    }

    public static int AoPadSomeoneARepeat(ControllerConsts key)
    {
        for (int i = 0; i < (controllerSource?.Count ?? 0); i++)
        {
            if ((PAD_AREPEAT(i) & key) != 0)
            {
                return i;
            }
        }
        return -1;
    }

    public static int AoPadSomeoneARelease(ControllerConsts key)
    {
        for (int i = 0; i < (controllerSource?.Count ?? 0); i++)
        {
            if ((PAD_ARELEASE(i) & key) != 0)
            {
                return i;
            }
        }
        return -1;
    }

    public static int AoPadSomeoneMDirect(ushort key)
    {
        for (int i = 0; i < (controllerSource?.Count ?? 0); i++)
        {
            if ((PAD_MDIRECT(i) & key) != 0)
            {
                return i;
            }
        }
        return -1;
    }

    public static int AoPadSomeoneMStand(ControllerConsts key)
    {
        for (int i = 0; i < (controllerSource?.Count ?? 0); i++)
        {
            if ((PAD_MSTAND(i) & (int)key) != 0)
            {
                return i;
            }
        }
        return -1;
    }

    public static int AoPadSomeoneMRepeat(ushort key)
    {
        for (int i = 0; i < (controllerSource?.Count ?? 0); i++)
        {
            if ((PAD_MREPEAT(i) & key) != 0)
            {
                return i;
            }
        }
        return -1;
    }

    public static int AoPadSomeoneMRelease(ushort key)
    {
        for (int i = 0; i < (controllerSource?.Count ?? 0); i++)
        {
            if ((PAD_MRELEASE(i) & key) != 0)
            {
                return i;
            }
        }
        return -1;
    }

    public static ControllerConsts AoPadPortDirect(uint port)
    {
        return PAD_DIRECT((int)port);
    }

    public static ControllerConsts AoPadPortStand(uint port)
    {
        return PAD_STAND((int)port);
    }

    public static ControllerConsts AoPadPortRepeat(uint port)
    {
        return PAD_REPEAT((int)port);
    }

    public static ControllerConsts AoPadPortRelease(uint port)
    {
        return PAD_RELEASE((int)port);
    }

    public static ControllerConsts AoPadPortADirect(uint port)
    {
        return PAD_ADIRECT((int)port);
    }

    public static ControllerConsts AoPadPortAStand(uint port)
    {
        return PAD_ASTAND((int)port);
    }

    public static ControllerConsts AoPadPortARepeat(uint port)
    {
        return PAD_AREPEAT((int)port);
    }

    public static ControllerConsts AoPadPortARelease(uint port)
    {
        return PAD_ARELEASE((int)port);
    }

    public static ControllerConsts AoPadPortMDirect(uint port)
    {
        return (ControllerConsts)PAD_MDIRECT((int)port);
    }

    public static ControllerConsts AoPadPortMStand(uint port)
    {
        return (ControllerConsts)PAD_MSTAND((int)port);
    }

    public static ControllerConsts AoPadPortMRepeat(uint port)
    {
        return (ControllerConsts)PAD_MREPEAT((int)port);
    }

    public static ControllerConsts AoPadPortMRelease(uint port)
    {
        return (ControllerConsts)PAD_MRELEASE((int)port);
    }

    public static short AoPadPortAnalogLX(uint port)
    {
        return PAD_A_LX((int)port);
    }

    public static short AoPadPortAnalogLY(uint port)
    {
        return PAD_A_LY((int)port);
    }

    public static short AoPadPortAnalogRX(uint port)
    {
        return PAD_A_RX((int)port);
    }

    public static short AoPadPortAnalogRY(uint port)
    {
        return PAD_A_RY((int)port);
    }

    public static bool AoPadIsConnected(uint port)
    {
        return PAD_CONNECT((int)port) != 0;
    }

    public static bool AoPadIsConnected()
    {
        int num = AppMain.AoAccountGetCurrentId();
        return num >= 0 && AoPadIsConnected((uint)num);
    }

    public static void AoPadEnableVibration(bool flag)
    {

    }

    public static void AoPadSetVibration(ushort left, ushort right)
    {    
        if (AppMain.AoAccountGetCurrentId() < (controllerSource?.Count ?? 0))
        {
            controllerSource[AppMain.AoAccountGetCurrentId()].SetVibration(left, right);
        }
    }

    public static int PAD_CONNECT(int _port)
    {
        return controllerSource[_port] != null ? 1 : 0;
    }

    public static ControllerConsts PAD_DIRECT(int _port)
    {
        return (ControllerConsts)controllerSource[_port].reading.direct;
    }

    public static ControllerConsts PAD_STAND(int _port)
    {
        return (ControllerConsts)controllerSource[_port].reading.stand;
    }

    public static ControllerConsts PAD_REPEAT(int _port)
    {
        return (ControllerConsts)controllerSource[_port].reading.repeat;
    }

    public static ControllerConsts PAD_RELEASE(int _port)
    {
        return (ControllerConsts)controllerSource[_port].reading.release;
    }

    public static ControllerConsts PAD_ADIRECT(int _port)
    {
        if ((controllerSource?.Count ?? 0) == 0)
            return 0;

        return (ControllerConsts)controllerSource[_port].reading.adirect;
    }

    public static ControllerConsts PAD_ASTAND(int _port)
    {
        return (ControllerConsts)controllerSource[_port].reading.astand;
    }

    public static ControllerConsts PAD_AREPEAT(int _port)
    {
        return (ControllerConsts)controllerSource[_port].reading.arepeat;
    }

    public static ControllerConsts PAD_ARELEASE(int _port)
    {
        return (ControllerConsts)controllerSource[_port].reading.arelease;
    }

    public static int PAD_MDIRECT(int _port)
    {
        return controllerSource[_port].reading.direct | controllerSource[_port].reading.adirect;
    }

    public static int PAD_MSTAND(int _port)
    {
        return controllerSource[_port].reading.stand | controllerSource[_port].reading.astand;
    }

    public static int PAD_MREPEAT(int _port)
    {
        return controllerSource[_port].reading.repeat | controllerSource[_port].reading.arepeat;
    }

    public static int PAD_MRELEASE(int _port)
    {
        return controllerSource[_port].reading.release | controllerSource[_port].reading.arelease;
    }

    public static short PAD_A_LX(int _port)
    {
        return controllerSource[_port].reading.alx;
    }

    public static short PAD_A_LY(int _port)
    {
        return controllerSource[_port].reading.aly;
    }

    public static short PAD_A_RX(int _port)
    {
        return controllerSource[_port].reading.arx;
    }

    public static short PAD_A_RY(int _port)
    {
        return controllerSource[_port].reading.ary;
    }

    public static int PAD_KEEP_TIME(int _port, int _key_id)
    {
        return controllerSource[_port].reading.keep_time[_key_id];
    }

    public static int PAD_LAST_TIME(int _port, int _key_id)
    {
        return controllerSource[_port].reading.last_time[_key_id];
    }

    public static int PAD_KEEP_ATIME(int _port, int _key_id)
    {
        return controllerSource[_port].reading.keep_atime[_key_id];
    }

    public static int PAD_LAST_ATIME(int _port, int _key_id)
    {
        return controllerSource[_port].reading.last_atime[_key_id];
    }

}
