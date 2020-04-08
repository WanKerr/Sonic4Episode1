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
    public struct NNS_MOTION_KEY_Class14
    {
        public short Frame;
        public short Value;

        public static AppMain.NNS_MOTION_KEY_Class14 Read(BinaryReader reader)
        {
            return new AppMain.NNS_MOTION_KEY_Class14()
            {
                Frame = reader.ReadInt16(),
                Value = reader.ReadInt16()
            };
        }

        public static AppMain.NNS_MOTION_KEY_Class14[] Read(BinaryReader reader, int count)
        {
            AppMain.NNS_MOTION_KEY_Class14[] motionKeyClass14Array = new AppMain.NNS_MOTION_KEY_Class14[count];
            for (int index = 0; index < count; ++index)
                motionKeyClass14Array[index] = AppMain.NNS_MOTION_KEY_Class14.Read(reader);
            return motionKeyClass14Array;
        }
    }
}
