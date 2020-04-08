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
    public class NNS_MOTION_KEY_Class5
    {
        public readonly AppMain.NNS_VECTOR Value = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        public float Frame;

        public NNS_MOTION_KEY_Class5()
        {
        }

        public NNS_MOTION_KEY_Class5(AppMain.NNS_MOTION_KEY_Class5 motionKey)
        {
            this.Frame = motionKey.Frame;
            this.Value.Assign(motionKey.Value);
        }

        public AppMain.NNS_MOTION_KEY_Class5 Assign(AppMain.NNS_MOTION_KEY_Class5 motionKey)
        {
            if (this != motionKey)
            {
                this.Frame = motionKey.Frame;
                this.Value.Assign(motionKey.Value);
            }
            return this;
        }

        public static AppMain.NNS_MOTION_KEY_Class5 Read(BinaryReader reader)
        {
            AppMain.NNS_MOTION_KEY_Class5 nnsMotionKeyClass5 = new AppMain.NNS_MOTION_KEY_Class5();
            nnsMotionKeyClass5.Frame = reader.ReadSingle();
            nnsMotionKeyClass5.Value.Assign(AppMain.NNS_VECTOR.Read(reader));
            return nnsMotionKeyClass5;
        }

        public static AppMain.NNS_MOTION_KEY_Class5[] Read(BinaryReader reader, int count)
        {
            AppMain.NNS_MOTION_KEY_Class5[] nnsMotionKeyClass5Array = new AppMain.NNS_MOTION_KEY_Class5[count];
            for (int index = 0; index < count; ++index)
                nnsMotionKeyClass5Array[index] = AppMain.NNS_MOTION_KEY_Class5.Read(reader);
            return nnsMotionKeyClass5Array;
        }
    }
}
