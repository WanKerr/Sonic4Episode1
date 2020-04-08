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
    public class GMS_EVE_DATA_EV_HEADER
    {
        public ushort width;
        public ushort height;
        public uint[] ofst;
        public AppMain.GMS_EVE_DATA_EV_LIST[] ev_list;

        public GMS_EVE_DATA_EV_HEADER()
        {
        }

        public GMS_EVE_DATA_EV_HEADER(AppMain.AmbChunk data)
        {
            using (MemoryStream memoryStream = new MemoryStream(data.array, data.offset, data.array.Length - data.offset))
            {
                using (BinaryReader binaryReader = new BinaryReader((Stream)memoryStream))
                {
                    this.width = binaryReader.ReadUInt16();
                    this.height = binaryReader.ReadUInt16();
                    int n = (int)this.width * (int)this.height;
                    this.ofst = new uint[(int)(uint)n];
                    for (int index = 0; index < n; ++index)
                        this.ofst[index] = binaryReader.ReadUInt32();
                    this.ev_list = AppMain.New<AppMain.GMS_EVE_DATA_EV_LIST>(n);
                    for (int index1 = 0; index1 < n; ++index1)
                    {
                        binaryReader.BaseStream.Seek((long)this.ofst[index1], SeekOrigin.Begin);
                        this.ev_list[index1].eve_num = binaryReader.ReadUInt16();
                        if (this.ev_list[index1].eve_num > (ushort)0)
                        {
                            this.ev_list[index1].eve_rec = AppMain.New<AppMain.GMS_EVE_RECORD_EVENT>((int)this.ev_list[index1].eve_num);
                            for (int index2 = 0; index2 < (int)this.ev_list[index1].eve_num; ++index2)
                            {
                                this.ev_list[index1].eve_rec[index2].pos_x = binaryReader.ReadByte();
                                this.ev_list[index1].eve_rec[index2].pos_y = binaryReader.ReadByte();
                                this.ev_list[index1].eve_rec[index2].id = binaryReader.ReadUInt16();
                                this.ev_list[index1].eve_rec[index2].flag = binaryReader.ReadUInt16();
                                this.ev_list[index1].eve_rec[index2].left = binaryReader.ReadSByte();
                                this.ev_list[index1].eve_rec[index2].top = binaryReader.ReadSByte();
                                this.ev_list[index1].eve_rec[index2].width = binaryReader.ReadByte();
                                this.ev_list[index1].eve_rec[index2].height = binaryReader.ReadByte();
                                this.ev_list[index1].eve_rec[index2].word_param = binaryReader.ReadUInt16();
                            }
                        }
                    }
                }
            }
        }

        public void loadData(byte[] data)
        {
            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                using (BinaryReader binaryReader = new BinaryReader((Stream)memoryStream))
                {
                    this.width = binaryReader.ReadUInt16();
                    this.height = binaryReader.ReadUInt16();
                    int num1 = (int)this.width * (int)this.height;
                    for (int index1 = 0; index1 < num1; ++index1)
                    {
                        this.ev_list[index1].eve_num = binaryReader.ReadUInt16();
                        if (this.ev_list[index1].eve_num > (ushort)0)
                        {
                            for (int index2 = 0; index2 < (int)this.ev_list[index1].eve_num; ++index2)
                            {
                                byte num2 = binaryReader.ReadByte();
                                byte num3 = binaryReader.ReadByte();
                                ushort num4 = binaryReader.ReadUInt16();
                                ushort num5 = binaryReader.ReadUInt16();
                                ushort num6 = binaryReader.ReadUInt16();
                                if (num4 < (ushort)60 || (ushort)300 <= num4 && num4 < (ushort)300 || (ushort)308 <= num4 && num4 < (ushort)335)
                                {
                                    this.ev_list[index1].eve_rec[index2].pos_x = num2;
                                    this.ev_list[index1].eve_rec[index2].pos_y = num3;
                                    this.ev_list[index1].eve_rec[index2].id = num4;
                                    this.ev_list[index1].eve_rec[index2].flag = num5;
                                    this.ev_list[index1].eve_rec[index2].word_param = num6;
                                }
                            }
                        }
                    }
                }
            }
        }

        public byte[] saveData()
        {
            byte[] numArray = (byte[])null;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter binaryWriter = new BinaryWriter((Stream)memoryStream))
                {
                    binaryWriter.Write(this.width);
                    binaryWriter.Write(this.height);
                    int num = (int)this.width * (int)this.height;
                    for (int index1 = 0; index1 < num; ++index1)
                    {
                        binaryWriter.Write(this.ev_list[index1].eve_num);
                        if (this.ev_list[index1].eve_num > (ushort)0)
                        {
                            for (int index2 = 0; index2 < (int)this.ev_list[index1].eve_num; ++index2)
                            {
                                binaryWriter.Write(this.ev_list[index1].eve_rec[index2].pos_x);
                                binaryWriter.Write(this.ev_list[index1].eve_rec[index2].pos_y);
                                binaryWriter.Write(this.ev_list[index1].eve_rec[index2].id);
                                binaryWriter.Write(this.ev_list[index1].eve_rec[index2].flag);
                                binaryWriter.Write(this.ev_list[index1].eve_rec[index2].word_param);
                            }
                        }
                    }
                }
                byte[] array = memoryStream.ToArray();
                int length = array.Length;
                numArray = new byte[length];
                Array.Copy((Array)array, (Array)numArray, length);
            }
            return numArray;
        }

        public void Clear()
        {
            this.width = (ushort)0;
            this.height = (ushort)0;
            this.ofst = (uint[])null;
            this.ev_list = (AppMain.GMS_EVE_DATA_EV_LIST[])null;
        }
    }
}
