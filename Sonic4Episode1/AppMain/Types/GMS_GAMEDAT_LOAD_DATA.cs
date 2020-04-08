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
    public class GMS_GAMEDAT_LOAD_DATA
    {
        public string path;
        public AppMain.GMS_GAMEDAT_LOAD_DATA._alloc_ alloc;
        public AppMain.GMS_GAMEDAT_LOAD_DATA._proc_pre_ proc_pre;
        public AppMain.GMS_GAMEDAT_LOAD_DATA._proc_post_ proc_post;
        public int user_data;

        public GMS_GAMEDAT_LOAD_DATA(
          string _path,
          AppMain.GMS_GAMEDAT_LOAD_DATA._alloc_ _alloc,
          AppMain.GMS_GAMEDAT_LOAD_DATA._proc_pre_ _proc_pre,
          AppMain.GMS_GAMEDAT_LOAD_DATA._proc_post_ _proc_post,
          int udata)
        {
            this.path = _path;
            this.alloc = _alloc;
            this.proc_pre = _proc_pre;
            this.proc_post = _proc_post;
            this.user_data = udata;
        }

        public delegate object _alloc_(string s);

        public delegate void _proc_pre_(AppMain.GMS_GAMEDAT_LOAD_CONTEXT contex);

        public delegate void _proc_post_(AppMain.GMS_GAMEDAT_LOAD_CONTEXT contex);
    }
}
