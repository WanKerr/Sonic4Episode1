using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using accel;
using dbg;
using er;
using er.web;
using gs;
using gs.backup;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using mpp;
using setting;

public partial class AppMain
{
    public enum GMD_TASK_GROUP_NO
    {
        GMD_TASK_GROUP_NO_PLAYER = 1,
        GMD_TASK_GROUP_NO_START = 1,
        GMD_TASK_GROUP_NO_ENEMY = 2,
        GMD_TASK_GROUP_NO_DECO = 3,
        GMD_TASK_GROUP_NO_OBJSYS = 4,
        GMD_TASK_GROUP_NO_GAMESYS = 5,
        GMD_TASK_GROUP_NO_END = 6,
    }
}
