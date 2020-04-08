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
    public class GMS_BOSS5_BODY_RECT_SETTING_INFO
    {
        public AppMain.GMS_BOSS5_RECTPOINT_SETTING_INFO[] point_setting_info = AppMain.New<AppMain.GMS_BOSS5_RECTPOINT_SETTING_INFO>(3);
        public int is_invincible;
        public int is_leakage;

        public GMS_BOSS5_BODY_RECT_SETTING_INFO(
          int invis,
          int leakage,
          AppMain.GMS_BOSS5_RECTPOINT_SETTING_INFO[] info)
        {
            this.is_invincible = invis;
            this.is_leakage = leakage;
            this.point_setting_info = info;
        }
    }
}
