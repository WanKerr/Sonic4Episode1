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
    public class NNS_TEXTURE_FILTERMODE
    {
        public ushort MagFilter;
        public ushort MinFilter;
        public float Anisotropy;

        public static AppMain.NNS_TEXTURE_FILTERMODE Read(BinaryReader reader)
        {
            return new AppMain.NNS_TEXTURE_FILTERMODE()
            {
                MagFilter = reader.ReadUInt16(),
                MinFilter = reader.ReadUInt16(),
                Anisotropy = reader.ReadSingle()
            };
        }

        public AppMain.NNS_TEXTURE_FILTERMODE Assign(AppMain.NNS_TEXTURE_FILTERMODE filterMode)
        {
            this.MagFilter = filterMode.MagFilter;
            this.MinFilter = filterMode.MinFilter;
            this.Anisotropy = filterMode.Anisotropy;
            return this;
        }
    }
}
