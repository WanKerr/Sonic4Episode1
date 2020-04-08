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
    public class DMS_LOGO_COM_LOAD_FILE_INFO
    {
        public string file_path;
        public AppMain.post_func_Delegate post_func;

        public DMS_LOGO_COM_LOAD_FILE_INFO(string _file_path, AppMain.post_func_Delegate _post_func)
        {
            this.file_path = _file_path;
            this.post_func = _post_func;
        }
    }
}
