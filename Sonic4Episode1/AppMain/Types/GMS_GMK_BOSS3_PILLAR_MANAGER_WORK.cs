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
    public class GMS_GMK_BOSS3_PILLAR_MANAGER_WORK : AppMain.IOBS_OBJECT_WORK
    {
        public readonly AppMain.OBS_OBJECT_WORK[] obj_work_pillar = new AppMain.OBS_OBJECT_WORK[26];
        public readonly AppMain.OBS_OBJECT_WORK[] obj_work_wall = new AppMain.OBS_OBJECT_WORK[2];
        public readonly AppMain.GMS_ENEMY_3D_WORK gimmick_work;
        public int pattern_no;

        public GMS_GMK_BOSS3_PILLAR_MANAGER_WORK()
        {
            this.gimmick_work = new AppMain.GMS_ENEMY_3D_WORK((object)this);
        }

        public AppMain.OBS_OBJECT_WORK Cast()
        {
            return this.gimmick_work.ene_com.obj_work;
        }

        public static explicit operator AppMain.OBS_OBJECT_WORK(
          AppMain.GMS_GMK_BOSS3_PILLAR_MANAGER_WORK work)
        {
            return work.gimmick_work.ene_com.obj_work;
        }
    }
}
