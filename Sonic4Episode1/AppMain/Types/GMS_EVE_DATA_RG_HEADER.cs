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
    public class GMS_EVE_DATA_RG_HEADER
    {
        public ushort width;
        public ushort height;
        public uint[] ofst;
        public AppMain.GMS_EVE_DATA_RG_LIST[] ring;

        public GMS_EVE_DATA_RG_HEADER()
        {
        }

        public GMS_EVE_DATA_RG_HEADER(AppMain.AmbChunk data)
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
                    this.ring = AppMain.New<AppMain.GMS_EVE_DATA_RG_LIST>(n);
                    for (int index1 = 0; index1 < n; ++index1)
                    {
                        binaryReader.BaseStream.Seek((long)this.ofst[index1], SeekOrigin.Begin);
                        this.ring[index1].ring_num = binaryReader.ReadUInt16();
                        if (this.ring[index1].ring_num > (ushort)0)
                        {
                            this.ring[index1].ring_data = AppMain.New<AppMain.GMS_EVE_RECORD_RING>((int)this.ring[index1].ring_num);
                            for (int index2 = 0; index2 < (int)this.ring[index1].ring_num; ++index2)
                            {
                                this.ring[index1].ring_data[index2].pos_x = binaryReader.ReadByte();
                                this.ring[index1].ring_data[index2].pos_y = binaryReader.ReadByte();
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
                    int num = (int)this.width * (int)this.height;
                    for (int index1 = 0; index1 < num; ++index1)
                    {
                        this.ring[index1].ring_num = binaryReader.ReadUInt16();
                        if (this.ring[index1].ring_num > (ushort)0)
                        {
                            for (int index2 = 0; index2 < (int)this.ring[index1].ring_num; ++index2)
                            {
                                this.ring[index1].ring_data[index2].pos_x = binaryReader.ReadByte();
                                this.ring[index1].ring_data[index2].pos_y = binaryReader.ReadByte();
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
                        binaryWriter.Write(this.ring[index1].ring_num);
                        if (this.ring[index1].ring_num > (ushort)0)
                        {
                            for (int index2 = 0; index2 < (int)this.ring[index1].ring_num; ++index2)
                            {
                                binaryWriter.Write(this.ring[index1].ring_data[index2].pos_x);
                                binaryWriter.Write(this.ring[index1].ring_data[index2].pos_y);
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
        }
    }
}
