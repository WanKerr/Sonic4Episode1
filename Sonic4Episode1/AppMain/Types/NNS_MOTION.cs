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
    public class NNS_MOTION : AppMain.IClearable
    {
        public uint fType;
        public float StartFrame;
        public float EndFrame;
        public int nSubmotion;
        public AppMain.NNS_SUBMOTION[] pSubmotion;
        public float FrameRate;
        public uint Reserved0;
        public uint Reserved1;

        public NNS_MOTION()
        {
        }

        public NNS_MOTION(AppMain.NNS_MOTION motion)
        {
            this.fType = motion.fType;
            this.StartFrame = motion.StartFrame;
            this.EndFrame = motion.EndFrame;
            this.nSubmotion = motion.nSubmotion;
            this.pSubmotion = motion.pSubmotion;
            this.FrameRate = motion.FrameRate;
            this.Reserved0 = motion.Reserved0;
            this.Reserved1 = motion.Reserved1;
        }

        public AppMain.NNS_MOTION Assign(AppMain.NNS_MOTION motion)
        {
            if (this != motion)
            {
                this.fType = motion.fType;
                this.StartFrame = motion.StartFrame;
                this.EndFrame = motion.EndFrame;
                this.nSubmotion = motion.nSubmotion;
                this.pSubmotion = motion.pSubmotion;
                this.FrameRate = motion.FrameRate;
                this.Reserved0 = motion.Reserved0;
                this.Reserved1 = motion.Reserved1;
            }
            return this;
        }

        public void Clear()
        {
            this.fType = 0U;
            this.StartFrame = 0.0f;
            this.EndFrame = 0.0f;
            this.nSubmotion = 0;
            this.pSubmotion = (AppMain.NNS_SUBMOTION[])null;
            this.FrameRate = 0.0f;
            this.Reserved0 = 0U;
            this.Reserved1 = 0U;
        }

        public static AppMain.NNS_MOTION Read(BinaryReader reader, long data0Pos)
        {
            AppMain.NNS_MOTION nnsMotion = new AppMain.NNS_MOTION();
            nnsMotion.fType = reader.ReadUInt32();
            nnsMotion.StartFrame = reader.ReadSingle();
            nnsMotion.EndFrame = reader.ReadSingle();
            nnsMotion.nSubmotion = reader.ReadInt32();
            uint num = reader.ReadUInt32();
            if (num != 0U)
            {
                long position = reader.BaseStream.Position;
                reader.BaseStream.Seek(data0Pos + (long)num, SeekOrigin.Begin);
                nnsMotion.pSubmotion = new AppMain.NNS_SUBMOTION[nnsMotion.nSubmotion];
                for (int index = 0; index < nnsMotion.nSubmotion; ++index)
                    nnsMotion.pSubmotion[index] = AppMain.NNS_SUBMOTION.Read(reader, nnsMotion.fType & 31U, data0Pos);
                reader.BaseStream.Seek(position, SeekOrigin.Begin);
            }
            nnsMotion.FrameRate = reader.ReadSingle();
            nnsMotion.Reserved0 = reader.ReadUInt32();
            nnsMotion.Reserved1 = reader.ReadUInt32();
            return nnsMotion;
        }
    }
}
