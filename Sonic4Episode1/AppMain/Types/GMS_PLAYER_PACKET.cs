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
    public class GMS_PLAYER_PACKET
    {
        public AppMain.VecFx32 pos = new AppMain.VecFx32();
        public ushort disp_flag;
        public short anime_speed;
        public byte act_state;
        public byte dir_x;
        public byte dir_y;
        public byte dir_z;
        public uint move_flag;
        public uint player_flag;
        public uint gmk_flag;
        public short move_x;
        public short move_y;
        public int camera_pos_x;
        public int camera_pos_y;
        public uint time;
    }
}
