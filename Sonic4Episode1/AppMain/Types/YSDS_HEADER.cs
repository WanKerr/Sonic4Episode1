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
    public class YSDS_HEADER
    {
        public byte[] masic;
        public uint page_num;
        public AppMain.YSDS_PAGE[] pages;

        public YSDS_HEADER(byte[] data)
        {
            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader((Stream)memoryStream))
                {
                    this.masic = br.ReadBytes(4);
                    this.page_num = br.ReadUInt32();
                    this.pages = AppMain.New<AppMain.YSDS_PAGE>((int)this.page_num);
                    for (int index = 0; (long)index < (long)this.page_num; ++index)
                    {
                        this.pages[index].time = br.ReadUInt32();
                        this.pages[index].show = br.ReadInt32();
                        this.pages[index].hide = br.ReadInt32();
                        this.pages[index].option = br.ReadUInt32();
                        this.pages[index].line_num = br.ReadUInt32();
                        this.pages[index].line_tbl_ofst = br.ReadUInt32();
                        this.pages[index].lines = AppMain.New<AppMain.YSDS_LINE>((int)this.pages[index].line_num);
                    }
                    for (int index1 = 0; (long)index1 < (long)this.page_num; ++index1)
                    {
                        br.BaseStream.Seek((long)this.pages[index1].line_tbl_ofst, SeekOrigin.Begin);
                        for (int index2 = 0; (long)index2 < (long)this.pages[index1].line_num; ++index2)
                        {
                            this.pages[index1].lines[index2].id = br.ReadUInt32();
                            this.pages[index1].lines[index2].str_ofst = br.ReadUInt32();
                        }
                    }
                    for (int index1 = 0; (long)index1 < (long)this.page_num; ++index1)
                    {
                        for (int index2 = 0; (long)index2 < (long)this.pages[index1].line_num; ++index2)
                        {
                            br.BaseStream.Seek((long)this.pages[index1].lines[index2].str_ofst, SeekOrigin.Begin);
                            this.pages[index1].lines[index2].s = AppMain.readChars(br);
                        }
                    }
                }
            }
        }
    }
}
