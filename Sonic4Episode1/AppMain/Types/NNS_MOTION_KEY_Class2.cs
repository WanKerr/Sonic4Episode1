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
    public class NNS_MOTION_KEY_Class2
    {
        public readonly AppMain.NNS_MOTION_BEZIER_HANDLE Bhandle = new AppMain.NNS_MOTION_BEZIER_HANDLE();
        public float Frame;
        public float Value;

        public NNS_MOTION_KEY_Class2()
        {
        }

        public NNS_MOTION_KEY_Class2(AppMain.NNS_MOTION_KEY_Class2 motionKey)
        {
            this.Frame = motionKey.Frame;
            this.Value = motionKey.Value;
            this.Bhandle.Assign(motionKey.Bhandle);
        }

        public AppMain.NNS_MOTION_KEY_Class2 Assign(AppMain.NNS_MOTION_KEY_Class2 motionKey)
        {
            if (this != motionKey)
            {
                this.Frame = motionKey.Frame;
                this.Value = motionKey.Value;
                this.Bhandle.Assign(motionKey.Bhandle);
            }
            return this;
        }

        public static AppMain.NNS_MOTION_KEY_Class2 Read(BinaryReader reader)
        {
            AppMain.NNS_MOTION_KEY_Class2 nnsMotionKeyClass2 = new AppMain.NNS_MOTION_KEY_Class2();
            nnsMotionKeyClass2.Frame = reader.ReadSingle();
            nnsMotionKeyClass2.Value = reader.ReadSingle();
            nnsMotionKeyClass2.Bhandle.Assign(AppMain.NNS_MOTION_BEZIER_HANDLE.Read(reader));
            return nnsMotionKeyClass2;
        }

        public static AppMain.NNS_MOTION_KEY_Class2[] Read(BinaryReader reader, int count)
        {
            AppMain.NNS_MOTION_KEY_Class2[] nnsMotionKeyClass2Array = new AppMain.NNS_MOTION_KEY_Class2[count];
            for (int index = 0; index < count; ++index)
                nnsMotionKeyClass2Array[index] = AppMain.NNS_MOTION_KEY_Class2.Read(reader);
            return nnsMotionKeyClass2Array;
        }
    }
}
