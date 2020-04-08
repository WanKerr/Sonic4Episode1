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
    public class AMS_DISPLAYLIST_MANAGER
    {
        public readonly AppMain.AMS_DRAW_SORT[] sortlist = AppMain.New<AppMain.AMS_DRAW_SORT>(512);
        public readonly AppMain.AMS_DISPLAYLIST[] displaylist = AppMain.New<AppMain.AMS_DISPLAYLIST>(4);
        public readonly AppMain.AMS_REGISTLIST[] registlist = AppMain.New<AppMain.AMS_REGISTLIST>(256);
        public int write_index;
        public int last_index;
        public int read_index;
        public AppMain.ArrayPointer<object> command_buf_ptr;
        public AppMain.AMS_COMMAND_BUFFER_HEADER write_header;
        public int reg_write_num;
        public AppMain.AMS_COMMAND_BUFFER_HEADER read_header;
        public int sort_num;
        public int regist_num;
        public int reg_write_index;
        public int reg_read_index;
        public int reg_end_index;
    }
}
