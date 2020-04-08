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
    public class NNS_TEXINFO
    {
        public uint TexName;
        public uint GlobalIndex;
        public uint Bank;
        public uint Flag;

        public NNS_TEXINFO()
        {
        }

        public NNS_TEXINFO(AppMain.NNS_TEXINFO pFrom)
        {
            this.TexName = pFrom.TexName;
            this.GlobalIndex = pFrom.GlobalIndex;
            this.Bank = pFrom.Bank;
            this.Flag = pFrom.Flag;
        }

        public static AppMain.NNS_TEXINFO Read(BinaryReader reader)
        {
            return new AppMain.NNS_TEXINFO()
            {
                TexName = reader.ReadUInt32(),
                GlobalIndex = reader.ReadUInt32(),
                Bank = reader.ReadUInt32(),
                Flag = reader.ReadUInt32()
            };
        }
    }
}
