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
    public class NNS_SUBMOTION
    {
        public uint fType;
        public uint fIPType;
        public int Id;
        public float StartFrame;
        public float EndFrame;
        public float StartKeyFrame;
        public float EndKeyFrame;
        public int nKeyFrame;
        public int KeySize;
        public object pKeyList;

        public short Id0
        {
            get
            {
                return (short)(this.Id & (int)ushort.MaxValue);
            }
            set
            {
                this.Id = this.Id & -65536 | (int)value & (int)ushort.MaxValue;
            }
        }

        public short Id1
        {
            get
            {
                return (short)(this.Id >> 16 & (int)ushort.MaxValue);
            }
            set
            {
                this.Id = this.Id & (int)ushort.MaxValue | (int)value << 16;
            }
        }

        public NNS_SUBMOTION()
        {
        }

        public NNS_SUBMOTION(AppMain.NNS_SUBMOTION subMotion)
        {
            this.fType = subMotion.fType;
            this.fIPType = subMotion.fIPType;
            this.Id = subMotion.Id;
            this.StartFrame = subMotion.StartFrame;
            this.EndFrame = subMotion.EndFrame;
            this.StartKeyFrame = subMotion.StartKeyFrame;
            this.EndKeyFrame = subMotion.EndKeyFrame;
            this.nKeyFrame = subMotion.nKeyFrame;
            this.KeySize = subMotion.KeySize;
            this.pKeyList = subMotion.pKeyList;
        }

        public AppMain.NNS_SUBMOTION Assign(AppMain.NNS_SUBMOTION subMotion)
        {
            if (this != subMotion)
            {
                this.fType = subMotion.fType;
                this.fIPType = subMotion.fIPType;
                this.Id = subMotion.Id;
                this.StartFrame = subMotion.StartFrame;
                this.EndFrame = subMotion.EndFrame;
                this.StartKeyFrame = subMotion.StartKeyFrame;
                this.EndKeyFrame = subMotion.EndKeyFrame;
                this.nKeyFrame = subMotion.nKeyFrame;
                this.KeySize = subMotion.KeySize;
                this.pKeyList = subMotion.pKeyList;
            }
            return this;
        }

        public static AppMain.NNS_SUBMOTION Read(
          BinaryReader reader,
          uint motionType,
          long data0Pos)
        {
            AppMain.NNS_SUBMOTION submotion = new AppMain.NNS_SUBMOTION();
            submotion.fType = reader.ReadUInt32();
            submotion.fIPType = reader.ReadUInt32();
            submotion.Id = reader.ReadInt32();
            submotion.StartFrame = reader.ReadSingle();
            submotion.EndFrame = reader.ReadSingle();
            submotion.StartKeyFrame = reader.ReadSingle();
            submotion.EndKeyFrame = reader.ReadSingle();
            submotion.nKeyFrame = reader.ReadInt32();
            submotion.KeySize = reader.ReadInt32();
            uint num = reader.ReadUInt32();
            if (num != 0U)
            {
                long position = reader.BaseStream.Position;
                reader.BaseStream.Seek(data0Pos + (long)num, SeekOrigin.Begin);
                switch (motionType)
                {
                    case 1:
                        AppMain.NNS_SUBMOTION.ReadNodeMotionKeyFrames(reader, submotion);
                        break;
                    case 2:
                        AppMain.NNS_SUBMOTION.ReadCameraMotionKeyFrames(reader, submotion);
                        break;
                    case 4:
                        AppMain.NNS_SUBMOTION.ReadLightMotionKeyFrames(reader, submotion);
                        break;
                    case 8:
                        AppMain.NNS_SUBMOTION.ReadMorthMotionKeyFrames(reader, submotion);
                        break;
                    case 16:
                        AppMain.NNS_SUBMOTION.ReadMaterialMotionKeyFrames(reader, submotion);
                        break;
                }
                reader.BaseStream.Seek(position, SeekOrigin.Begin);
            }
            return submotion;
        }

        private static void ReadNodeMotionKeyFrames(
          BinaryReader reader,
          AppMain.NNS_SUBMOTION submotion)
        {
            uint num1 = submotion.fType & 3U;
            int fType = (int)submotion.fType;
            uint num2 = submotion.fType & 4294967040U;
            uint num3 = submotion.fIPType & 3703U;
            if (((int)num2 & 1792) != 0)
            {
                if (1U == num1)
                {
                    switch (num3)
                    {
                        case 1:
                        case 16:
                            throw new NotImplementedException("Investigation needed");
                        default:
                            submotion.pKeyList = (object)AppMain.NNS_MOTION_KEY_Class1.Read(reader, submotion.nKeyFrame);
                            break;
                    }
                }
                else
                {
                    if (2U == num1)
                        throw new NotImplementedException("Investigation needed");
                    throw new NotImplementedException("Investigation needed");
                }
            }
            else if (((int)num2 & 30720) != 0)
            {
                if (1U == num1)
                {
                    submotion.pKeyList = (object)AppMain.NNS_MOTION_KEY_Class13.Read(reader, submotion.nKeyFrame);
                }
                else if (2U != num1)
                    throw new NotImplementedException("Investigation needed");
                else if (num3 == 1U)
                    throw new NotImplementedException("Investigation needed");
                else if (num3 == 512)
                {
                    submotion.pKeyList = (object)AppMain.NNS_MOTION_KEY_Class16.Read(reader, submotion.nKeyFrame);
                }
                else
                    submotion.pKeyList = (object)AppMain.NNS_MOTION_KEY_Class14.Read(reader, submotion.nKeyFrame);

                Console.WriteLine("{0}, {1}, {2}", num1, submotion.KeySize, num3);
            }
            else if (((int)num2 & 229376) != 0)
            {
                if (1U == num1)
                {
                    switch (num3)
                    {
                        case 1:
                        case 16:
                            throw new NotImplementedException("Investigation needed");
                        default:
                            submotion.pKeyList = (object)AppMain.NNS_MOTION_KEY_Class1.Read(reader, submotion.nKeyFrame);
                            break;
                    }
                }
                else
                {
                    if (2U == num1)
                        throw new NotImplementedException("Investigation needed");
                    throw new NotImplementedException("Investigation needed");
                }
            }
            else
            {
                if (((int)num2 & 786432) != 0)
                {
                    if (1U == num1)
                        throw new NotImplementedException("Investigation needed");
                    if (2U == num1)
                        throw new NotImplementedException("Investigation needed");
                    throw new NotImplementedException("Investigation needed");
                }
                if (((int)num2 & 1048576) == 0)
                    throw new NotImplementedException("Investigation needed");
                if (1U == num1)
                {
                    submotion.pKeyList = (object)AppMain.NNS_MOTION_KEY_Class11.Read(reader, submotion.nKeyFrame);
                }
                else
                {
                    if (2U != num1)
                        throw new NotImplementedException("Investigation needed");
                    if (submotion.KeySize != 8)
                        throw new NotImplementedException("Investigation needed");
                    submotion.pKeyList = (object)AppMain.NNS_MOTION_KEY_Class16.Read(reader, submotion.nKeyFrame);
                }
            }
        }

        private static void ReadCameraMotionKeyFrames(
          BinaryReader reader,
          AppMain.NNS_SUBMOTION submotion)
        {
            int fType1 = (int)submotion.fType;
            int fType2 = (int)submotion.fType;
            int fType3 = (int)submotion.fType;
            int fIpType = (int)submotion.fIPType;
            throw new NotImplementedException();
        }

        private static void ReadLightMotionKeyFrames(
          BinaryReader reader,
          AppMain.NNS_SUBMOTION submotion)
        {
            int fType1 = (int)submotion.fType;
            int fType2 = (int)submotion.fType;
            int fType3 = (int)submotion.fType;
            int fIpType = (int)submotion.fIPType;
            throw new NotImplementedException();
        }

        private static void ReadMorthMotionKeyFrames(
          BinaryReader reader,
          AppMain.NNS_SUBMOTION submotion)
        {
            int fType1 = (int)submotion.fType;
            int fType2 = (int)submotion.fType;
            int fType3 = (int)submotion.fType;
            int fIpType = (int)submotion.fIPType;
            throw new NotImplementedException();
        }

        private static void ReadMaterialMotionKeyFrames(
          BinaryReader reader,
          AppMain.NNS_SUBMOTION submotion)
        {
            int fType1 = (int)submotion.fType;
            int fType2 = (int)submotion.fType;
            uint num1 = submotion.fType & 4294967040U;
            uint num2 = submotion.fIPType & 3703U;
            if (((int)num1 & 256) != 0)
                throw new NotImplementedException("Investigation needed");
            if (((int)num1 & 3584) != 0)
            {
                switch (num2)
                {
                    case 1:
                    case 16:
                    label_8:
                        throw new NotImplementedException("Investigation needed");
                    default:
                        switch (submotion.KeySize)
                        {
                            case 8:
                                submotion.pKeyList = (object)AppMain.NNS_MOTION_KEY_Class1.Read(reader, submotion.nKeyFrame);
                                return;
                            case 16:
                                submotion.pKeyList = (object)AppMain.NNS_MOTION_KEY_Class5.Read(reader, submotion.nKeyFrame);
                                return;
                            default:
                                goto label_8;
                        }
                }
            }
            else
            {
                if (((int)num1 & 4096) != 0)
                    throw new NotImplementedException("Investigation needed");
                if (((int)num1 & 57344) != 0)
                {
                    switch (num2)
                    {
                        case 1:
                        case 16:
                            throw new NotImplementedException("Investigation needed");
                        default:
                            submotion.pKeyList = (object)AppMain.NNS_MOTION_KEY_Class5.Read(reader, submotion.nKeyFrame);
                            break;
                    }
                }
                else
                {
                    if (((int)num1 & 65536) != 0)
                        throw new NotImplementedException("Investigation needed");
                    if (((int)num1 & 131072) != 0)
                        throw new NotImplementedException("Investigation needed");
                    if (((int)num1 & 1835008) != 0)
                    {
                        switch (num2)
                        {
                            case 1:
                            case 16:
                                throw new NotImplementedException("Investigation needed");
                            default:
                                submotion.pKeyList = (object)AppMain.NNS_MOTION_KEY_Class5.Read(reader, submotion.nKeyFrame);
                                break;
                        }
                    }
                    else
                    {
                        if (((int)num1 & 2097152) != 0)
                            throw new NotImplementedException("Investigation needed");
                        if (((int)num1 & 4194304) != 0)
                        {
                            switch (num2)
                            {
                                case 1:
                                    break;
                                case 16:
                                    break;
                                default:
                                    if (submotion.KeySize != 8)
                                        break;
                                    submotion.pKeyList = (object)AppMain.NNS_MOTION_KEY_Class1.Read(reader, submotion.nKeyFrame);
                                    break;
                            }
                        }
                        else if (((int)num1 & 25165824) != 0)
                        {
                            switch (num2)
                            {
                                case 1:
                                    throw new NotImplementedException("Investigation needed");
                                case 16:
                                    submotion.pKeyList = (object)AppMain.NNS_MOTION_KEY_Class2.Read(reader, submotion.nKeyFrame);
                                    break;
                                default:
                                    submotion.pKeyList = (object)AppMain.NNS_MOTION_KEY_Class1.Read(reader, submotion.nKeyFrame);
                                    break;
                            }
                        }
                        else
                        {
                            if (((int)num1 & 33554432) != 0)
                                throw new NotImplementedException("Investigation needed");
                            throw new NotImplementedException("Investigation needed");
                        }
                    }
                }
            }
        }
    }
}
