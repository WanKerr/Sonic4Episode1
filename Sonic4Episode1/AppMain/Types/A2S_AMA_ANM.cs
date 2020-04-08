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
    public class A2S_AMA_ANM
    {
        public uint flag;
        public uint anm_key_num;
        public uint anm_frm_num;
        public int anm_key_tbl_offset;
        public AppMain.A2S_SUB_KEY[] anm_key_tbl;
        public int anm_tbl_offset;
        public AppMain.A2S_SUB_ANM[] anm_tbl;
        public uint mat_key_num;
        public uint mat_frm_num;
        public int mat_key_tbl_offset;
        public AppMain.A2S_SUB_KEY[] mat_key_tbl;
        public int mat_tbl_offset;
        public AppMain.A2S_SUB_MAT[] mat_tbl;

        internal void Assign(AppMain.A2S_AMA_ANM old)
        {
            this.flag = old.flag;
            this.anm_key_num = old.anm_key_num;
            this.anm_frm_num = old.anm_frm_num;
            if (old.anm_key_tbl != null)
            {
                this.anm_key_tbl = new AppMain.A2S_SUB_KEY[old.anm_key_tbl.Length];
                Array.Copy((Array)old.anm_key_tbl, (Array)this.anm_key_tbl, old.anm_key_tbl.Length);
            }
            if (old.anm_tbl != null)
            {
                this.anm_tbl = AppMain.New<AppMain.A2S_SUB_ANM>(old.anm_tbl.Length);
                for (int index = 0; index < this.anm_tbl.Length; ++index)
                    this.anm_tbl[index].Assign(old.anm_tbl[index]);
            }
            this.mat_key_num = old.mat_key_num;
            this.mat_frm_num = old.mat_frm_num;
            if (old.mat_key_tbl != null)
            {
                this.mat_key_tbl = new AppMain.A2S_SUB_KEY[old.mat_key_tbl.Length];
                Array.Copy((Array)old.mat_key_tbl, (Array)this.mat_key_tbl, old.mat_key_tbl.Length);
            }
            if (old.mat_tbl == null)
                return;
            this.mat_tbl = AppMain.New<AppMain.A2S_SUB_MAT>(old.mat_tbl.Length);
            for (int index = 0; (long)index < (long)old.mat_key_num; ++index)
                this.mat_tbl[index].Assign(old.mat_tbl[index]);
        }
    }
}
