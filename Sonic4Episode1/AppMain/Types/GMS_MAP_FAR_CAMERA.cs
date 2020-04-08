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
    public class GMS_MAP_FAR_CAMERA
    {
        public int camera_id;
        public int camera_type;
        public float camera_speed_x;
        public float camera_speed_y;

        internal void Clear()
        {
            this.camera_id = 0;
            this.camera_type = 0;
            this.camera_speed_x = 0.0f;
            this.camera_speed_y = 0.0f;
        }
    }
}
