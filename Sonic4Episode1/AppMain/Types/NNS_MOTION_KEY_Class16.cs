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
    public class NNS_MOTION_KEY_Class16
    {
        public short Frame;
        public AppMain.NNS_ROTATE_A16 Value;

        public NNS_MOTION_KEY_Class16()
        {
        }

        public NNS_MOTION_KEY_Class16(AppMain.NNS_MOTION_KEY_Class16 motionKey)
        {
            this.Frame = motionKey.Frame;
            this.Value = motionKey.Value;
        }

        public AppMain.NNS_MOTION_KEY_Class16 Assign(AppMain.NNS_MOTION_KEY_Class16 motionKey)
        {
            if (this != motionKey)
            {
                this.Frame = motionKey.Frame;
                this.Value = motionKey.Value;
            }
            return this;
        }

        public static AppMain.NNS_MOTION_KEY_Class16 Read(BinaryReader reader)
        {
            return new AppMain.NNS_MOTION_KEY_Class16()
            {
                Frame = reader.ReadInt16(),
                Value = {
                  x = reader.ReadInt16(),
                  y = reader.ReadInt16(),
                  z = reader.ReadInt16()
                }
            };
        }

        public static AppMain.NNS_MOTION_KEY_Class16[] Read(BinaryReader reader, int count)
        {
            AppMain.NNS_MOTION_KEY_Class16[] motionKeyClass16Array = new AppMain.NNS_MOTION_KEY_Class16[count];
            for (int index = 0; index < count; ++index)
                motionKeyClass16Array[index] = AppMain.NNS_MOTION_KEY_Class16.Read(reader);
            return motionKeyClass16Array;
        }
    }
}
