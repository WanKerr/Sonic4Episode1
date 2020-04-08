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
    public class A2S_AMA_MTN
    {
        public uint flag;
        public uint mtn_key_num;
        public uint mtn_frm_num;
        public int mtn_key_tbl_offset;
        public AppMain.A2S_SUB_KEY[] mtn_key_tbl;
        public int mtn_tbl_offset;
        public AppMain.A2S_SUB_MTN[] mtn_tbl;
        public uint trs_key_num;
        public uint trs_frm_num;
        public int trs_key_tbl_offset;
        public AppMain.A2S_SUB_KEY[] trs_key_tbl;
        public int trs_tbl_offset;
        public AppMain.A2S_SUB_TRS[] trs_tbl;

        internal void Assign(AppMain.A2S_AMA_MTN old)
        {
            this.flag = old.flag;
            this.mtn_key_num = old.mtn_key_num;
            this.mtn_frm_num = old.mtn_frm_num;
            if (old.mtn_key_tbl != null)
            {
                this.mtn_key_tbl = new AppMain.A2S_SUB_KEY[old.mtn_key_tbl.Length];
                Array.Copy((Array)old.mtn_key_tbl, (Array)old.mtn_key_tbl, old.mtn_key_tbl.Length);
            }
            if (old.mtn_tbl != null)
            {
                this.mtn_tbl = AppMain.New<AppMain.A2S_SUB_MTN>(old.mtn_tbl.Length);
                for (int index = 0; index < this.mtn_tbl.Length; ++index)
                    this.mtn_tbl[index].Assign(old.mtn_tbl[index]);
            }
            this.trs_key_num = old.trs_key_num;
            this.trs_frm_num = old.trs_frm_num;
            if (old.trs_key_tbl != null)
            {
                this.trs_key_tbl = new AppMain.A2S_SUB_KEY[old.trs_key_tbl.Length];
                Array.Copy((Array)old.trs_key_tbl, (Array)this.trs_key_tbl, old.trs_key_tbl.Length);
            }
            if (old.trs_tbl == null)
                return;
            this.trs_tbl = AppMain.New<AppMain.A2S_SUB_TRS>(old.trs_tbl.Length);
            for (int index = 0; index < this.trs_tbl.Length; ++index)
                this.trs_tbl[index].Assign(old.trs_tbl[index]);
        }
    }
}
