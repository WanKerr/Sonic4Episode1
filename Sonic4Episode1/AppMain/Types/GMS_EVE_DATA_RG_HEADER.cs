using System;
using System.IO;

public partial class AppMain
{
    public class GMS_EVE_DATA_RG_HEADER
    {
        public ushort width;
        public ushort height;
        public uint[] ofst;
        public GMS_EVE_DATA_RG_LIST[] ring;

        public GMS_EVE_DATA_RG_HEADER()
        {
        }

        public GMS_EVE_DATA_RG_HEADER(AmbChunk data)
        {
            using (MemoryStream memoryStream = new MemoryStream(data.array, data.offset, data.array.Length - data.offset))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream))
                {
                    this.width = binaryReader.ReadUInt16();
                    this.height = binaryReader.ReadUInt16();
                    int n = width * height;
                    this.ofst = new uint[(int)(uint)n];
                    for (int index = 0; index < n; ++index)
                        this.ofst[index] = binaryReader.ReadUInt32();
                    this.ring = New<GMS_EVE_DATA_RG_LIST>(n);
                    for (int index1 = 0; index1 < n; ++index1)
                    {
                        binaryReader.BaseStream.Seek(this.ofst[index1], SeekOrigin.Begin);
                        this.ring[index1].ring_num = binaryReader.ReadUInt16();
                        if (this.ring[index1].ring_num > 0)
                        {
                            this.ring[index1].ring_data = New<GMS_EVE_RECORD_RING>(ring[index1].ring_num);
                            for (int index2 = 0; index2 < ring[index1].ring_num; ++index2)
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
                using (BinaryReader binaryReader = new BinaryReader(memoryStream))
                {
                    this.width = binaryReader.ReadUInt16();
                    this.height = binaryReader.ReadUInt16();
                    int num = width * height;
                    for (int index1 = 0; index1 < num; ++index1)
                    {
                        this.ring[index1].ring_num = binaryReader.ReadUInt16();
                        if (this.ring[index1].ring_num > 0)
                        {
                            for (int index2 = 0; index2 < ring[index1].ring_num; ++index2)
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
            byte[] numArray = null;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
                {
                    binaryWriter.Write(this.width);
                    binaryWriter.Write(this.height);
                    int num = width * height;
                    for (int index1 = 0; index1 < num; ++index1)
                    {
                        binaryWriter.Write(this.ring[index1].ring_num);
                        if (this.ring[index1].ring_num > 0)
                        {
                            for (int index2 = 0; index2 < ring[index1].ring_num; ++index2)
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
                Array.Copy(array, numArray, length);
            }
            return numArray;
        }

        public void Clear()
        {
            this.width = 0;
            this.height = 0;
            this.ofst = null;
        }
    }
}
