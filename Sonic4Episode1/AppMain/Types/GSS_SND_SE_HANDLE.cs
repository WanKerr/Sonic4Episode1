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
    public class GSS_SND_SE_HANDLE : AppMain.IClearable
    {
        public readonly AppMain.GSS_SND_CTRL_PARAM snd_ctrl_param = new AppMain.GSS_SND_CTRL_PARAM();
        public uint flag;
        public AppMain.CriAuPlayer au_player;
        public uint cur_pause_level;

        public void Clear()
        {
            this.flag = 0U;
            this.snd_ctrl_param.Clear();
            this.cur_pause_level = 0U;
            this.au_player.Destroy();
            this.au_player = (AppMain.CriAuPlayer)null;
        }
    }
}
