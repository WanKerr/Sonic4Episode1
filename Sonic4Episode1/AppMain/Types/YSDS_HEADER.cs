using System.IO;

public partial class AppMain
{
    public class YSDS_HEADER
    {
        public byte[] masic;
        public uint page_num;
        public YSDS_PAGE[] pages;

        public YSDS_HEADER(byte[] data)
        {
            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(memoryStream))
                {
                    this.masic = br.ReadBytes(4);
                    this.page_num = br.ReadUInt32();
                    this.pages = New<YSDS_PAGE>((int)this.page_num);
                    for (int index = 0; index < page_num; ++index)
                    {
                        this.pages[index].time = br.ReadUInt32();
                        this.pages[index].show = br.ReadInt32();
                        this.pages[index].hide = br.ReadInt32();
                        this.pages[index].option = br.ReadUInt32();
                        this.pages[index].line_num = br.ReadUInt32();
                        this.pages[index].line_tbl_ofst = br.ReadUInt32();
                        this.pages[index].lines = New<YSDS_LINE>((int)this.pages[index].line_num);
                    }
                    for (int index1 = 0; index1 < page_num; ++index1)
                    {
                        br.BaseStream.Seek(pages[index1].line_tbl_ofst, SeekOrigin.Begin);
                        for (int index2 = 0; index2 < pages[index1].line_num; ++index2)
                        {
                            this.pages[index1].lines[index2].id = br.ReadUInt32();
                            this.pages[index1].lines[index2].str_ofst = br.ReadUInt32();
                        }
                    }
                    for (int index1 = 0; index1 < page_num; ++index1)
                    {
                        for (int index2 = 0; index2 < pages[index1].line_num; ++index2)
                        {
                            br.BaseStream.Seek(pages[index1].lines[index2].str_ofst, SeekOrigin.Begin);
                            this.pages[index1].lines[index2].s = readChars(br);
                        }
                    }
                }
            }
        }
    }
}
