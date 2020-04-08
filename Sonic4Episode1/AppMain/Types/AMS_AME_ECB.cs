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
    public class AMS_AME_ECB : AppMain.AMS_AME_LIST
    {
        public readonly AppMain.NNS_VECTOR4D translate = new AppMain.NNS_VECTOR4D();
        public readonly AppMain.AMS_AME_BOUNDING bounding = new AppMain.AMS_AME_BOUNDING();
        public int attribute;
        public int priority;
        public AppMain.NNS_QUATERNION rotate;
        public int transparency;
        public float size_rate;
        public AppMain.NNS_OBJECT pObj;
        public AppMain.AMS_AME_ENTRY entry_head;
        public AppMain.AMS_AME_ENTRY entry_tail;
        public int entry_num;
        public uint drawState;
        public uint drawObjState;
        public int skip_update;

        public override void Clear()
        {
            this.next = (AppMain.AMS_AME_LIST)null;
            this.prev = (AppMain.AMS_AME_LIST)null;
            this.attribute = 0;
            this.priority = 0;
            this.translate.Clear();
            this.rotate.Clear();
            this.bounding.Clear();
            this.transparency = 0;
            this.size_rate = 0.0f;
            this.pObj = (AppMain.NNS_OBJECT)null;
            this.entry_head = (AppMain.AMS_AME_ENTRY)null;
            this.entry_tail = (AppMain.AMS_AME_ENTRY)null;
            this.entry_num = 0;
            this.drawState = 0U;
            this.drawObjState = 0U;
            this.skip_update = 0;
        }
    }
}
