using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using mpp;

public partial class AppMain
{
    private static bool AoYsdFileIsYsdFile(object file)
    {
        return file is AppMain.YSDS_HEADER;
    }

    private static uint AoYsdFileGetPageNum(AppMain.YSDS_HEADER file)
    {
        return !AppMain.AoYsdFileIsYsdFile((object)file) ? 0U : file.page_num;
    }

    private static uint AoYsdFileGetPageTime(AppMain.YSDS_HEADER file, uint page_no)
    {
        return page_no >= AppMain.AoYsdFileGetPageNum(file) ? 0U : file.pages[(int)page_no].time;
    }

    private static bool AoYsdFileIsPageShowImage(AppMain.YSDS_HEADER file, uint page_no)
    {
        return page_no < AppMain.AoYsdFileGetPageNum(file) && file.pages[(int)page_no].show >= 0;
    }

    private static uint AoYsdFileGetPageShowImageNo(AppMain.YSDS_HEADER file, uint page_no)
    {
        return !AppMain.AoYsdFileIsPageShowImage(file, page_no) ? 0U : (uint)file.pages[(int)page_no].show;
    }

    private static bool AoYsdFileIsPageHideImage(AppMain.YSDS_HEADER file, uint page_no)
    {
        return page_no < AppMain.AoYsdFileGetPageNum(file) && file.pages[(int)page_no].hide >= 0;
    }

    private static uint AoYsdFileGetPageOption(AppMain.YSDS_HEADER file, uint page_no)
    {
        return page_no >= AppMain.AoYsdFileGetPageNum(file) ? 0U : file.pages[(int)page_no].option;
    }

    private static uint AoYsdFileGetLineNum(AppMain.YSDS_HEADER file, uint page_no)
    {
        return file.pages[(int)page_no].line_num;
    }

    private static uint AoYsdFileGetLineId(AppMain.YSDS_HEADER file, uint page_no, uint line_no)
    {
        return file.pages[(int)page_no].lines[(int)line_no].id;
    }

    private static string AoYsdFileGetLineString(
      AppMain.YSDS_HEADER file,
      uint page_no,
      uint line_no)
    {
        return file.pages[(int)page_no].lines[(int)line_no].s;
    }

}