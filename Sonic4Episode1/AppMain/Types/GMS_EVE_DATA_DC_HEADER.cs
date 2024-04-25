using System;
using System.IO;

public partial class AppMain
{
    public class GMS_EVE_DATA_DC_HEADER
    {
        public ushort width;
        public ushort height;
        public uint[] ofst;
        public GMS_EVE_DATA_DC_LIST[] dc_list;

        public GMS_EVE_DATA_DC_HEADER()
        {
        }

        public GMS_EVE_DATA_DC_HEADER(AmbChunk data)
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
                    this.dc_list = New<GMS_EVE_DATA_DC_LIST>(n);
                    for (int index1 = 0; index1 < n; ++index1)
                    {
                        binaryReader.BaseStream.Seek(this.ofst[index1], SeekOrigin.Begin);
                        this.dc_list[index1].dec_num = binaryReader.ReadUInt16();
                        if (this.dc_list[index1].dec_num > 0)
                        {
                            this.dc_list[index1].dec_data = New<GMS_EVE_RECORD_DECORATE>(dc_list[index1].dec_num);
                            for (int index2 = 0; index2 < dc_list[index1].dec_num; ++index2)
                            {
                                this.dc_list[index1].dec_data[index2].pos_x = binaryReader.ReadByte();
                                this.dc_list[index1].dec_data[index2].pos_y = binaryReader.ReadByte();
                                this.dc_list[index1].dec_data[index2].id = binaryReader.ReadUInt16();
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
                        this.dc_list[index1].dec_num = binaryReader.ReadUInt16();
                        if (this.dc_list[index1].dec_num > 0)
                        {
                            for (int index2 = 0; index2 < dc_list[index1].dec_num; ++index2)
                            {
                                this.dc_list[index1].dec_data[index2].pos_x = binaryReader.ReadByte();
                                this.dc_list[index1].dec_data[index2].pos_y = binaryReader.ReadByte();
                                this.dc_list[index1].dec_data[index2].id = binaryReader.ReadUInt16();
                            }
                        }
                    }
                }
            }
        }

        public byte[] saveData()
        {
            byte[] numArray;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
                {
                    binaryWriter.Write(this.width);
                    binaryWriter.Write(this.height);
                    int num = width * height;
                    for (int index1 = 0; index1 < num; ++index1)
                    {
                        binaryWriter.Write(this.dc_list[index1].dec_num);
                        if (this.dc_list[index1].dec_num > 0)
                        {
                            for (int index2 = 0; index2 < dc_list[index1].dec_num; ++index2)
                            {
                                binaryWriter.Write(this.dc_list[index1].dec_data[index2].pos_x);
                                binaryWriter.Write(this.dc_list[index1].dec_data[index2].pos_y);
                                binaryWriter.Write(this.dc_list[index1].dec_data[index2].id);
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
