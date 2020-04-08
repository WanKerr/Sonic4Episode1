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
    public class OBS_ACTION2D_AMA_WORK : AppMain.IClearable
    {
        public readonly AppMain.AOS_TEXTURE ao_tex = new AppMain.AOS_TEXTURE();
        public uint flag;
        public AppMain.AOS_ACTION act;
        public AppMain.NNS_TEXLIST texlist;
        public AppMain.A2S_AMA_HEADER ama;
        public AppMain.OBS_DATA_WORK ama_data_work;
        public uint act_id;
        public float frame;
        public float speed;
        public int type_node;
        public AppMain.AOS_ACT_COL color;
        public AppMain.AOS_ACT_COL fade;

        public void Clear()
        {
            this.flag = 0U;
            this.act = (AppMain.AOS_ACTION)null;
            this.ao_tex.Clear();
            this.texlist = (AppMain.NNS_TEXLIST)null;
            this.ama = (AppMain.A2S_AMA_HEADER)null;
            this.ama_data_work = (AppMain.OBS_DATA_WORK)null;
            this.act_id = 0U;
            this.frame = 0.0f;
            this.speed = 0.0f;
            this.type_node = 0;
            this.color.Clear();
            this.fade.Clear();
        }
    }
}
