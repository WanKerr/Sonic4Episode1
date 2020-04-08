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
    public class AOS_ACTION
    {
        public object data;
        public uint flag;
        public uint state;
        public AppMain.AOE_ACT_TYPE type;
        public float frame;
        public AppMain._last_key last_key;
        public AppMain.AOS_ACTION child;
        public AppMain.AOS_ACTION sibling;
        public AppMain.AOS_SPRITE sprite;

        public void Clear()
        {
            this.data = (object)0;
            this.flag = 0U;
            this.state = 0U;
            this.type = AppMain.AOE_ACT_TYPE.AOD_ACT_TYPE_ACTION;
            this.frame = 0.0f;
            this.last_key.Clear();
            this.child = (AppMain.AOS_ACTION)null;
            this.sibling = (AppMain.AOS_ACTION)null;
            this.sprite = (AppMain.AOS_SPRITE)null;
        }
    }
}
