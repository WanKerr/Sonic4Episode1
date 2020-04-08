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
    public class GMS_GMK_ROCK_CHASE_WORK : AppMain.IOBS_OBJECT_WORK
    {
        public readonly AppMain.GMS_ENEMY_3D_WORK enemy_work;
        public AppMain.GMS_EFFECT_3DES_WORK effect_work;
        public int target_bound;
        public int current_bound;
        public int length;
        public int speed;
        public ushort angle_z;
        public ushort reserve;
        public uint dir_type;
        public AppMain.GSS_SND_SE_HANDLE se_handle;
        public bool flag_vib;
        public AppMain.GMS_ENEMY_3D_WORK hook_work;

        public GMS_GMK_ROCK_CHASE_WORK()
        {
            this.enemy_work = new AppMain.GMS_ENEMY_3D_WORK((object)this);
        }

        public AppMain.OBS_OBJECT_WORK Cast()
        {
            return this.enemy_work.ene_com.obj_work;
        }
    }
}
