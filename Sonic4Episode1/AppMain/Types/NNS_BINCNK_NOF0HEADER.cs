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
    public class NNS_BINCNK_NOF0HEADER
    {
        public uint Id;
        public int OfsNextId;
        public int nData;
        public int Pad;

        public static AppMain.NNS_BINCNK_NOF0HEADER Read(BinaryReader reader)
        {
            return new AppMain.NNS_BINCNK_NOF0HEADER()
            {
                Id = reader.ReadUInt32(),
                OfsNextId = reader.ReadInt32(),
                nData = reader.ReadInt32(),
                Pad = reader.ReadInt32()
            };
        }
    }
}
