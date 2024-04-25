using System;
using System.IO;

public partial class AppMain
{
    public class GMS_EVE_DATA_EV_HEADER
    {
        public ushort width;
        public ushort height;
        public uint[] ofst;
        public GMS_EVE_DATA_EV_LIST[] ev_list;

        public GMS_EVE_DATA_EV_HEADER()
        {
        }

        public GMS_EVE_DATA_EV_HEADER(AmbChunk data)
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
                    this.ev_list = New<GMS_EVE_DATA_EV_LIST>(n);
                    for (int index1 = 0; index1 < n; ++index1)
                    {
                        binaryReader.BaseStream.Seek(this.ofst[index1], SeekOrigin.Begin);
                        this.ev_list[index1].eve_num = binaryReader.ReadUInt16();
                        if (this.ev_list[index1].eve_num > 0)
                        {
                            this.ev_list[index1].eve_rec = New<GMS_EVE_RECORD_EVENT>(ev_list[index1].eve_num);
                            for (int index2 = 0; index2 < ev_list[index1].eve_num; ++index2)
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
                using (BinaryReader binaryReader = new BinaryReader(memoryStream))
                {
                    this.width = binaryReader.ReadUInt16();
                    this.height = binaryReader.ReadUInt16();
                    int num1 = width * height;
                    for (int index1 = 0; index1 < num1; ++index1)
                    {
                        this.ev_list[index1].eve_num = binaryReader.ReadUInt16();
                        if (this.ev_list[index1].eve_num > 0)
                        {
                            for (int index2 = 0; index2 < ev_list[index1].eve_num; ++index2)
                            {
                                byte num2 = binaryReader.ReadByte();
                                byte num3 = binaryReader.ReadByte();
                                ushort num4 = binaryReader.ReadUInt16();
                                ushort num5 = binaryReader.ReadUInt16();
                                ushort num6 = binaryReader.ReadUInt16();
                                if (num4 < 60 || 300 <= num4 && num4 < 300 || 308 <= num4 && num4 < 335)
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
                        binaryWriter.Write(this.ev_list[index1].eve_num);
                        if (this.ev_list[index1].eve_num > 0)
                        {
                            for (int index2 = 0; index2 < ev_list[index1].eve_num; ++index2)
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
                Array.Copy(array, numArray, length);
            }
            return numArray;
        }

        public void Clear()
        {
            this.width = 0;
            this.height = 0;
            this.ofst = null;
            this.ev_list = null;
        }
    }
}
