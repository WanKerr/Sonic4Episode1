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
    private enum DME_FILESLCT_SAVE_FILE
    {
        DME_FILESLCT_SAVE_FILE_1,
        DME_FILESLCT_SAVE_FILE_2,
        DME_FILESLCT_SAVE_FILE_3,
        DME_FILESLCT_SAVE_FILE_4,
        DME_FILESLCT_SAVE_FILE_5,
        DME_FILESLCT_SAVE_FILE_6,
        DME_FILESLCT_SAVE_FILE_NUM,
        DME_FILESLCT_SAVE_FILE_NONE,
    }
}
