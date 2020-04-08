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
    public class NNS_MOTION_KEY_Class11
    {
        public float Frame;
        public int Value;

        public NNS_MOTION_KEY_Class11()
        {
        }

        public NNS_MOTION_KEY_Class11(AppMain.NNS_MOTION_KEY_Class11 motionKey)
        {
            this.Frame = motionKey.Frame;
            this.Value = motionKey.Value;
        }

        public AppMain.NNS_MOTION_KEY_Class11 Assign(AppMain.NNS_MOTION_KEY_Class11 motionKey)
        {
            if (this != motionKey)
            {
                this.Frame = motionKey.Frame;
                this.Value = motionKey.Value;
            }
            return this;
        }

        public static AppMain.NNS_MOTION_KEY_Class11 Read(BinaryReader reader)
        {
            return new AppMain.NNS_MOTION_KEY_Class11()
            {
                Frame = reader.ReadSingle(),
                Value = reader.ReadInt32()
            };
        }

        public static AppMain.NNS_MOTION_KEY_Class11[] Read(BinaryReader reader, int count)
        {
            AppMain.NNS_MOTION_KEY_Class11[] motionKeyClass11Array = new AppMain.NNS_MOTION_KEY_Class11[count];
            for (int index = 0; index < count; ++index)
                motionKeyClass11Array[index] = AppMain.NNS_MOTION_KEY_Class11.Read(reader);
            return motionKeyClass11Array;
        }
    }
}
