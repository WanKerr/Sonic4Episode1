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
    public struct NNS_MOTION_KEY_Class1
    {
        public float Frame;
        public float Value;

        public NNS_MOTION_KEY_Class1(AppMain.NNS_MOTION_KEY_Class1 motionKey)
        {
            this.Frame = motionKey.Frame;
            this.Value = motionKey.Value;
        }

        public AppMain.NNS_MOTION_KEY_Class1 Assign(AppMain.NNS_MOTION_KEY_Class1 motionKey)
        {
            this.Frame = motionKey.Frame;
            this.Value = motionKey.Value;
            return this;
        }

        public static AppMain.NNS_MOTION_KEY_Class1 Read(BinaryReader reader)
        {
            return new AppMain.NNS_MOTION_KEY_Class1()
            {
                Frame = reader.ReadSingle(),
                Value = reader.ReadSingle()
            };
        }

        public static AppMain.NNS_MOTION_KEY_Class1[] Read(BinaryReader reader, int count)
        {
            AppMain.NNS_MOTION_KEY_Class1[] nnsMotionKeyClass1Array = new AppMain.NNS_MOTION_KEY_Class1[count];
            for (int index = 0; index < count; ++index)
                nnsMotionKeyClass1Array[index] = AppMain.NNS_MOTION_KEY_Class1.Read(reader);
            return nnsMotionKeyClass1Array;
        }
    }
}
