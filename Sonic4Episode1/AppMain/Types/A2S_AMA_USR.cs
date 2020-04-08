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
    public class A2S_AMA_USR
    {
        public uint flag;
        public uint usr_key_num;
        public uint usr_frm_num;
        public int usr_key_tbl_offset;
        public AppMain.A2S_SUB_KEY[] usr_key_tbl;
        public int usr_tbl_offset;
        public AppMain.A2S_SUB_USR[] usr_tbl;

        internal void Assign(AppMain.A2S_AMA_USR old)
        {
            this.flag = old.flag;
            this.usr_key_num = old.usr_key_num;
            this.usr_frm_num = old.usr_frm_num;
            if (old.usr_key_tbl != null)
            {
                this.usr_key_tbl = new AppMain.A2S_SUB_KEY[old.usr_key_tbl.Length];
                Array.Copy((Array)old.usr_key_tbl, (Array)this.usr_key_tbl, old.usr_key_tbl.Length);
            }
            if (old.usr_tbl == null)
                return;
            this.usr_tbl = new AppMain.A2S_SUB_USR[old.usr_tbl.Length];
            Array.Copy((Array)old.usr_tbl, (Array)this.usr_tbl, old.usr_tbl.Length);
        }
    }
}
