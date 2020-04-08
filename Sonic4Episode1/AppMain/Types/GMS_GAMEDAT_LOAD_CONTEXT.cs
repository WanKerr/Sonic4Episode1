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
    public class GMS_GAMEDAT_LOAD_CONTEXT
    {
        public AppMain.GME_GAMEDAT_LOAD_STATE state;
        public string file_path;
        public int bb_no;
        public AppMain.AMS_FS fs_req;
        public AppMain.GMS_GAMEDAT_LOAD_DATA load_data;
        public ushort char_id;
        public ushort ply_no;
        public ushort stage_id;
        public ushort data_no;

        internal void Clear()
        {
            this.state = AppMain.GME_GAMEDAT_LOAD_STATE.GMD_GAMEDAT_LOAD_STATE_LOADING;
            this.file_path = "";
            this.bb_no = 0;
            this.fs_req = (AppMain.AMS_FS)null;
            this.load_data = (AppMain.GMS_GAMEDAT_LOAD_DATA)null;
            this.char_id = (ushort)0;
            this.ply_no = (ushort)0;
            this.stage_id = (ushort)0;
            this.data_no = (ushort)0;
        }
    }
}
