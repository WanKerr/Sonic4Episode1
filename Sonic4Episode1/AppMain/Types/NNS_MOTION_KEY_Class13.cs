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
    public class NNS_MOTION_KEY_Class13
    {
        public float Frame;
        public AppMain.NNS_ROTATE_A32 Value;

        public NNS_MOTION_KEY_Class13()
        {
        }

        public NNS_MOTION_KEY_Class13(AppMain.NNS_MOTION_KEY_Class13 motionKey)
        {
            this.Frame = motionKey.Frame;
            this.Value = motionKey.Value;
        }

        public AppMain.NNS_MOTION_KEY_Class13 Assign(AppMain.NNS_MOTION_KEY_Class13 motionKey)
        {
            if (this != motionKey)
            {
                this.Frame = motionKey.Frame;
                this.Value = motionKey.Value;
            }
            return this;
        }

        public static AppMain.NNS_MOTION_KEY_Class13 Read(BinaryReader reader)
        {
            AppMain.NNS_MOTION_KEY_Class13 motion = new AppMain.NNS_MOTION_KEY_Class13();
            motion.Frame = reader.ReadSingle();
            motion.Value = new NNS_ROTATE_A32
            {
                x = reader.ReadInt32(),
                y = reader.ReadInt32(),
                z = reader.ReadInt32()
            };

            return motion;
        }

        public static AppMain.NNS_MOTION_KEY_Class13[] Read(BinaryReader reader, int count)
        {
            AppMain.NNS_MOTION_KEY_Class13[] motionArray = new AppMain.NNS_MOTION_KEY_Class13[count];
            for (int index = 0; index < count; ++index)
                motionArray[index] = AppMain.NNS_MOTION_KEY_Class13.Read(reader);
            return motionArray;
        }
    }
}
