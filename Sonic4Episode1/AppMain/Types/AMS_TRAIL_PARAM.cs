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
    public class AMS_TRAIL_PARAM
    {
        public AppMain.NNS_RGBA startColor;
        public AppMain.NNS_RGBA endColor;
        public AppMain.NNS_RGBA ptclColor;
        public float startSize;
        public float endSize;
        public AppMain.OBS_OBJECT_WORK trail_obj_work;
        public AppMain.NNS_TEXLIST texlist;
        public int texId;
        public float life;
        public float vanish_time;
        public float zBias;
        public float ptclSize;
        public short partsNum;
        public short ptclFlag;
        public short ptclTexId;
        public short blendType;
        public short zTest;
        public short zMask;
        public float time;
        public float vanish_rate;
        public short trailId;
        public short trailPartsId;
        public short trailPartsNum;
        public short state;
        public short list_no;

        public AppMain.VecFx32 trail_pos
        {
            get
            {
                return this.trail_obj_work.pos;
            }
        }

        public void Clear()
        {
            this.startColor.Clear();
            this.endColor.Clear();
            this.ptclColor.Clear();
            this.startSize = this.endSize = 0.0f;
            this.trail_obj_work = (AppMain.OBS_OBJECT_WORK)null;
            this.texlist = (AppMain.NNS_TEXLIST)null;
            this.texId = 0;
            this.life = this.vanish_time = this.zBias = 0.0f;
            this.ptclSize = 0.0f;
            this.partsNum = this.ptclFlag = this.ptclTexId = this.blendType = this.zTest = this.zMask = (short)0;
            this.time = this.vanish_rate = 0.0f;
            this.trailId = this.trailPartsId = this.trailPartsNum = this.state = (short)0;
            this.list_no = (short)0;
        }

        public AppMain.AMS_TRAIL_PARAM Assign(AppMain.AMS_TRAIL_PARAM source)
        {
            if (this == source)
                return this;
            this.startColor = source.startColor;
            this.endColor = source.endColor;
            this.ptclColor = source.ptclColor;
            this.startSize = source.startSize;
            this.endSize = source.endSize;
            this.trail_obj_work = source.trail_obj_work;
            this.texlist = source.texlist;
            this.texId = source.texId;
            this.life = source.life;
            this.vanish_time = source.vanish_time;
            this.zBias = source.zBias;
            this.ptclSize = source.ptclSize;
            this.partsNum = source.partsNum;
            this.ptclFlag = source.ptclFlag;
            this.ptclTexId = source.ptclTexId;
            this.blendType = source.blendType;
            this.zTest = source.zTest;
            this.zMask = source.zMask;
            this.time = source.time;
            this.vanish_rate = source.vanish_rate;
            this.trailId = source.trailId;
            this.trailPartsId = source.trailPartsId;
            this.trailPartsNum = source.trailPartsNum;
            this.state = source.state;
            this.list_no = source.list_no;
            return this;
        }
    }
}
