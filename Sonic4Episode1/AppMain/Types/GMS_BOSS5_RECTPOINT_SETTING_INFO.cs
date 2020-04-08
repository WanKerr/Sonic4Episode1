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
    public class GMS_BOSS5_RECTPOINT_SETTING_INFO
    {
        public short[][] rect_size = AppMain.New<short>(3, 4);
        public uint enable_bit_flag;

        public GMS_BOSS5_RECTPOINT_SETTING_INFO()
        {
        }

        public GMS_BOSS5_RECTPOINT_SETTING_INFO(uint bitFlag, short[][] rectsize)
        {
            this.enable_bit_flag = bitFlag;
            this.rect_size = rectsize;
        }
    }
}
