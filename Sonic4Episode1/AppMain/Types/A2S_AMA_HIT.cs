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
    public class A2S_AMA_HIT
    {
        public uint flag;
        public uint hit_key_num;
        public uint hit_frm_num;
        public int hit_key_tbl_offset;
        public AppMain.A2S_SUB_KEY[] hit_key_tbl;
        public int hit_tbl_offset;
        public AppMain.A2S_SUB_HIT[] hit_tbl;

        internal void Assign(AppMain.A2S_AMA_HIT old)
        {
            this.flag = old.flag;
            this.hit_key_num = old.hit_key_num;
            this.hit_frm_num = old.hit_frm_num;
            if (old.hit_key_tbl != null)
            {
                this.hit_key_tbl = new AppMain.A2S_SUB_KEY[old.hit_key_tbl.Length];
                Array.Copy((Array)old.hit_key_tbl, (Array)this.hit_key_tbl, old.hit_key_tbl.Length);
            }
            if (old.hit_tbl == null)
                return;
            this.hit_tbl = AppMain.New<AppMain.A2S_SUB_HIT>(old.hit_tbl.Length);
            for (int index = 0; index < this.hit_tbl.Length; ++index)
                this.hit_tbl[index].Assign(old.hit_tbl[index]);
        }
    }
}
