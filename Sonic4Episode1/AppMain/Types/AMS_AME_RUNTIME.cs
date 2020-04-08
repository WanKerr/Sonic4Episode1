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
    public class AMS_AME_RUNTIME : AppMain.AMS_AME_LIST
    {
        public readonly AppMain.AMS_AME_LIST child_head = new AppMain.AMS_AME_LIST();
        public readonly AppMain.AMS_AME_LIST child_tail = new AppMain.AMS_AME_LIST();
        public readonly AppMain.AMS_AME_LIST work_head = (AppMain.AMS_AME_LIST)new AppMain.AMS_AME_RUNTIME_WORK();
        public readonly AppMain.AMS_AME_LIST work_tail = (AppMain.AMS_AME_LIST)new AppMain.AMS_AME_RUNTIME_WORK();
        public readonly AppMain.AMS_AME_LIST active_head = (AppMain.AMS_AME_LIST)new AppMain.AMS_AME_RUNTIME_WORK();
        public readonly AppMain.AMS_AME_LIST active_tail = (AppMain.AMS_AME_LIST)new AppMain.AMS_AME_RUNTIME_WORK();
        public int state;
        public float amount;
        public uint count;
        public AppMain.AMS_AME_ECB ecb;
        public AppMain.AMS_AME_NODE node;
        public AppMain.AMS_AME_RUNTIME parent_runtime;
        public AppMain.AMS_AME_RUNTIME spawn_runtime;
        public AppMain.AMS_AME_RUNTIME_WORK work;
        public int child_num;
        public short work_num;
        public short active_num;
        public AppMain.NNS_TEXLIST texlist;

        public new void Clear()
        {
            this.next = (AppMain.AMS_AME_LIST)null;
            this.prev = (AppMain.AMS_AME_LIST)null;
            this.state = 0;
            this.amount = 0.0f;
            this.count = 0U;
            this.ecb = (AppMain.AMS_AME_ECB)null;
            this.node = (AppMain.AMS_AME_NODE)null;
            this.parent_runtime = (AppMain.AMS_AME_RUNTIME)null;
            this.spawn_runtime = (AppMain.AMS_AME_RUNTIME)null;
            this.work = (AppMain.AMS_AME_RUNTIME_WORK)null;
            this.child_head.Clear();
            this.child_tail.Clear();
            this.child_num = 0;
            this.work_head.Clear();
            this.work_tail.Clear();
            this.active_head.Clear();
            this.active_tail.Clear();
            this.work_num = (short)0;
            this.active_num = (short)0;
            this.texlist = (AppMain.NNS_TEXLIST)null;
        }
    }
}
