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
    public class AOS_ACT_HITP
    {
        public uint flag;
        public AppMain.AOE_ACT_HIT type;
        public float scale_x;
        public float scale_y;
        public AppMain.AOS_ACT_RECT rect;

        public void Assign(AppMain.AOS_ACT_HITP other)
        {
            this.flag = other.flag;
            this.type = other.type;
            this.scale_x = other.scale_x;
            this.scale_y = other.scale_y;
            this.rect = other.rect;
        }

        public void Clear()
        {
            this.flag = 0U;
            this.type = AppMain.AOE_ACT_HIT.AOD_ACT_HIT_RECT;
            this.scale_x = this.scale_y = 0.0f;
        }

        public void GetCircle(ref AppMain.AOS_ACT_CIRCLE circle)
        {
            circle.center_x = this.rect.left;
            circle.center_y = this.rect.top;
            circle.radius = this.rect.right;
            circle.pad = (uint)this.rect.bottom;
        }

        public void SetCircle(ref AppMain.A2S_SUB_CIRCLE circle)
        {
            this.rect.left = circle.center_x;
            this.rect.top = circle.center_y;
            this.rect.right = circle.radius;
            this.rect.bottom = (float)circle.pad;
        }

        public void SetCircle(ref AppMain.AOS_ACT_CIRCLE circle)
        {
            this.rect.left = circle.center_x;
            this.rect.top = circle.center_y;
            this.rect.right = circle.radius;
            this.rect.bottom = (float)circle.pad;
        }
    }
}
