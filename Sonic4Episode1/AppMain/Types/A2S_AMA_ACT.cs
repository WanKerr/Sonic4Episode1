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
    public class A2S_AMA_ACT
    {
        public int _off;
        public uint flag;
        public uint id;
        public uint frm_num;
        public uint pad1;
        public AppMain.A2S_SUB_RECT ofst;
        public int mtn_offset;
        public AppMain.A2S_AMA_MTN mtn;
        public int anm_offset;
        public AppMain.A2S_AMA_ANM anm;
        public int acm_offset;
        public AppMain.A2S_AMA_ACM acm;
        public int usr_offset;
        public AppMain.A2S_AMA_USR usr;
        public int hit_offset;
        public AppMain.A2S_AMA_HIT hit;
        public int next_offset;
        public AppMain.A2S_AMA_ACT next;

        public void Assign(AppMain.A2S_AMA_ACT old)
        {
            this.flag = old.flag;
            this.id = old.flag;
            this.frm_num = old.frm_num;
            this.pad1 = old.pad1;
            this.ofst = old.ofst;
            if (old.mtn != null)
            {
                this.mtn = new AppMain.A2S_AMA_MTN();
                this.mtn.Assign(old.mtn);
            }
            if (old.anm != null)
            {
                this.anm = new AppMain.A2S_AMA_ANM();
                this.anm.Assign(old.anm);
            }
            if (old.acm != null)
            {
                this.acm = new AppMain.A2S_AMA_ACM();
                this.acm.Assign(old.acm);
            }
            if (old.usr != null)
            {
                this.usr = new AppMain.A2S_AMA_USR();
                this.usr.Assign(old.usr);
            }
            if (old.hit == null)
                return;
            this.hit = new AppMain.A2S_AMA_HIT();
            this.hit.Assign(old.hit);
        }
    }
}
