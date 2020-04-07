using System;

[Flags]
public enum ControllerConsts : int
{
    UP = 1,
    DOWN = 2,
    LEFT = 4,
    RIGHT = 8,

    ALT_UP = 16,
    ALT_DOWN = 32,
    ALT_LEFT = 64,
    ALT_RIGHT = 128,

    JUMP_BUTTON = 256,
    SUPER_SONIC = 512,
    START = 1024,
    CONFIRM = 2048,
    CANCEL = 4096,

    A = 8192,
    B = 16384,
    X = 32768,
    Y = 65536,
    L = 131072,
    R = 262144,
}

public struct ControllerReading
{
    public bool connected;
    public bool allow_vibrate;

    public ControllerConsts direction;

    public ushort direct;
    public ushort stand;
    public ushort release;
    public ushort repeat;

    public short timer_lv;
    public short timer_btn;

    public int[] keep_time;
    public int[] keep_atime;
    public int[] last_time;
    public int[] last_atime;

    public ushort adirect;
    public ushort astand;
    public ushort arelease;
    public ushort arepeat;

    public short alx;
    public short aly;
    public short arx;
    public short ary;
}

public abstract class Controller
{
    public ControllerReading reading;

    public Controller()
    {
        reading = new ControllerReading();
        reading.keep_time = new int[0];
        reading.keep_atime = new int[0];
        reading.last_atime = new int[0];
        reading.last_time = new int[0];
    }

    public abstract void SetVibrationEnabled(bool enabled);
    public abstract void SetVibration(ushort left, ushort right);

    public ControllerReading UpdateControllerReading()
    {
        ushort num = 0;
        ushort num2 = (ushort)(reading.direct ^ num);
        reading.stand = (ushort)(num2 & num);
        reading.release = (ushort)(num2 & ~num);
        reading.direct = num;
        reading.direction = 0;

        UpdateControllerReading(ref reading);

        if (reading.alx < -16384)
        {
            reading.direction |= ControllerConsts.LEFT;
        }
        else if (reading.alx > 16384)
        {
            reading.direction |= ControllerConsts.RIGHT;
        }

        if (reading.aly < -16384)
        {
            reading.direction |= ControllerConsts.DOWN;
        }
        else if (reading.aly > 16384)
        {
            reading.direction |= ControllerConsts.UP;
        }

        if (reading.arx < -16384)
        {
            reading.direction |= ControllerConsts.ALT_LEFT;
        }
        else if (reading.arx > 16384)
        {
            reading.direction |= ControllerConsts.ALT_RIGHT;
        }
        if (reading.ary < -16384)
        {
            reading.direction |= ControllerConsts.ALT_DOWN;
        }
        else if (reading.ary > 16384)
        {
            reading.direction |= ControllerConsts.ALT_UP;
        }

        ushort num4 = (ushort)(reading.adirect ^ (ushort)reading.direction);
        reading.astand = (ushort)(num4 & (ushort)reading.direction);
        reading.arelease = (ushort)(num4 & ~(ushort)reading.direction);
        reading.adirect = (ushort)reading.direction;
        reading.repeat = 0;
        reading.arepeat = 0;
        if (((reading.stand | reading.astand) & 15) != 0)
        {
            reading.repeat = (ushort)(num & 15);
            reading.arepeat = (ushort)((ushort)reading.direction & 15);
            reading.timer_lv = 30;
        }
        else
        {
            if ((reading.timer_lv -= 1) == 0)
            {
                reading.repeat = (ushort)(num & 15);
                reading.arepeat = (ushort)((ushort)reading.direction & 15);
                reading.timer_lv = 5;
            }
        }
        if (((reading.stand | reading.astand) & 65520) != 0)
        {
            reading.repeat |= (ushort)(num & 65520);
            reading.arepeat |= (ushort)((ushort)reading.direction & 65520);
            reading.timer_btn = 30;
        }
        else
        {
            if ((reading.timer_btn -= 1) == 0)
            {
                reading.repeat |= (ushort)(num & 65520);
                reading.arepeat |= (ushort)((ushort)reading.direction & 65520);
                reading.timer_btn = 5;
            }
        }

        int[] keep_time = reading.keep_time;
        int[] keep_atime = reading.keep_atime;
        int j = 0;
        while (j < 0)
        {
            if ((num2 & 1) != 0)
            {
                reading.last_time[j] = keep_time[j];
                keep_time[j] = 1;
            }
            else
            {
                keep_time[j]++;
            }
            if ((num4 & 1) != 0)
            {
                reading.last_atime[j] = keep_atime[j];
                keep_atime[j] = 1;
            }
            else
            {
                keep_atime[j]++;
            }
            j++;
            num2 = (ushort)(num2 >> 1);
            num4 = (ushort)(num4 >> 1);
        }

        return reading;
    }

    public abstract void UpdateControllerReading(ref ControllerReading reading);
}