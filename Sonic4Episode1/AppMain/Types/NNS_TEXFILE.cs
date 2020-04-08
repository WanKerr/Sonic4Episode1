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
    public class NNS_TEXFILE
    {
        public uint fType;
        public string Filename;
        public ushort MinFilter;
        public ushort MagFilter;
        public uint GlobalIndex;
        public uint Bank;

        public static AppMain.NNS_TEXFILE Read(BinaryReader reader, long data0Pos)
        {
            AppMain.NNS_TEXFILE nnsTexfile = new AppMain.NNS_TEXFILE();
            nnsTexfile.fType = reader.ReadUInt32();
            uint num1 = reader.ReadUInt32();
            nnsTexfile.MinFilter = reader.ReadUInt16();
            nnsTexfile.MagFilter = reader.ReadUInt16();
            nnsTexfile.GlobalIndex = reader.ReadUInt32();
            nnsTexfile.Bank = reader.ReadUInt32();
            if (num1 != 0U)
            {
                long position = reader.BaseStream.Position;
                reader.BaseStream.Seek(data0Pos + (long)num1, SeekOrigin.Begin);
                StringBuilder stringBuilder = new StringBuilder();
                byte num2;
                while ((num2 = reader.ReadByte()) != (byte)0)
                    stringBuilder.Append((char)num2);
                nnsTexfile.Filename = stringBuilder.ToString().ToUpper();
                reader.BaseStream.Seek(position, SeekOrigin.Begin);
            }
            return nnsTexfile;
        }
    }
}
