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
    public class AMS_DRAWSTATE
    {
        public readonly AppMain.AMS_DRAWSTATE_DIFFUSE diffuse = new AppMain.AMS_DRAWSTATE_DIFFUSE();
        public readonly AppMain.AMS_DRAWSTATE_AMBIENT ambient = new AppMain.AMS_DRAWSTATE_AMBIENT();
        public readonly AppMain.AMS_DRAWSTATE_SPECULAR specular = new AppMain.AMS_DRAWSTATE_SPECULAR();
        public readonly AppMain.AMS_DRAWSTATE_ENVMAP envmap = new AppMain.AMS_DRAWSTATE_ENVMAP();
        public readonly AppMain.AMS_DRAWSTATE_ALPHA alpha = new AppMain.AMS_DRAWSTATE_ALPHA();
        public readonly AppMain.AMS_DRAWSTATE_BLEND blend = new AppMain.AMS_DRAWSTATE_BLEND();
        public readonly AppMain.AMS_DRAWSTATE_TEXOFFSET[] texoffset = AppMain.New<AppMain.AMS_DRAWSTATE_TEXOFFSET>(4);
        public readonly AppMain.AMS_DRAWSTATE_FOG fog = new AppMain.AMS_DRAWSTATE_FOG();
        public readonly AppMain.AMS_DRAWSTATE_FOG_COLOR fog_color = new AppMain.AMS_DRAWSTATE_FOG_COLOR();
        public readonly AppMain.AMS_DRAWSTATE_FOG_RANGE fog_range = new AppMain.AMS_DRAWSTATE_FOG_RANGE();
        public readonly AppMain.AMS_DRAWSTATE_Z_MODE zmode = new AppMain.AMS_DRAWSTATE_Z_MODE();
        public uint drawflag;

        public AMS_DRAWSTATE()
        {
        }

        public AMS_DRAWSTATE(AppMain.AMS_DRAWSTATE drawState)
        {
            this.drawflag = drawState.drawflag;
            this.diffuse.Assign(drawState.diffuse);
            this.ambient.Assign(drawState.ambient);
            this.specular.Assign(drawState.specular);
            this.envmap.Assign(drawState.envmap);
            this.alpha.Assign(drawState.alpha);
            this.blend.Assign(drawState.blend);
            for (int index = 0; index < 4; ++index)
                this.texoffset[index].Assign(drawState.texoffset[index]);
            this.fog.flag = drawState.fog.flag;
            this.fog_color.Assign(drawState.fog_color);
            this.fog_range.Assign(drawState.fog_range);
            this.zmode.Assign(drawState.zmode);
        }

        public AppMain.AMS_DRAWSTATE Assign(AppMain.AMS_DRAWSTATE drawState)
        {
            if (this != drawState)
            {
                this.drawflag = drawState.drawflag;
                this.diffuse.Assign(drawState.diffuse);
                this.ambient.Assign(drawState.ambient);
                this.specular.Assign(drawState.specular);
                this.envmap.Assign(drawState.envmap);
                this.alpha.Assign(drawState.alpha);
                this.blend.Assign(drawState.blend);
                for (int index = 0; index < 4; ++index)
                    this.texoffset[index].Assign(drawState.texoffset[index]);
                this.fog.flag = drawState.fog.flag;
                this.fog_color.Assign(drawState.fog_color);
                this.fog_range.Assign(drawState.fog_range);
                this.zmode.Assign(drawState.zmode);
            }
            return this;
        }

        public void Clear()
        {
            this.drawflag = 0U;
            this.diffuse.Clear();
            this.ambient.Clear();
            this.specular.Clear();
            this.envmap.Clear();
            this.alpha.Clear();
            this.blend.Clear();
            for (int index = 0; index < 4; ++index)
                this.texoffset[index].Clear();
            this.fog.Clear();
            this.fog_color.Clear();
            this.fog_range.Clear();
            this.zmode.Clear();
        }
    }
}
