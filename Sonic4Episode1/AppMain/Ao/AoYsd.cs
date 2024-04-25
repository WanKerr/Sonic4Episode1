public partial class AppMain
{
    private static bool AoYsdFileIsYsdFile(object file)
    {
        return file is YSDS_HEADER;
    }

    private static uint AoYsdFileGetPageNum(YSDS_HEADER file)
    {
        return !AoYsdFileIsYsdFile(file) ? 0U : file.page_num;
    }

    private static uint AoYsdFileGetPageTime(YSDS_HEADER file, uint page_no)
    {
        return page_no >= AoYsdFileGetPageNum(file) ? 0U : file.pages[(int)page_no].time;
    }

    private static bool AoYsdFileIsPageShowImage(YSDS_HEADER file, uint page_no)
    {
        return page_no < AoYsdFileGetPageNum(file) && file.pages[(int)page_no].show >= 0;
    }

    private static uint AoYsdFileGetPageShowImageNo(YSDS_HEADER file, uint page_no)
    {
        return !AoYsdFileIsPageShowImage(file, page_no) ? 0U : (uint)file.pages[(int)page_no].show;
    }

    private static bool AoYsdFileIsPageHideImage(YSDS_HEADER file, uint page_no)
    {
        return page_no < AoYsdFileGetPageNum(file) && file.pages[(int)page_no].hide >= 0;
    }

    private static uint AoYsdFileGetPageOption(YSDS_HEADER file, uint page_no)
    {
        return page_no >= AoYsdFileGetPageNum(file) ? 0U : file.pages[(int)page_no].option;
    }

    private static uint AoYsdFileGetLineNum(YSDS_HEADER file, uint page_no)
    {
        return file.pages[(int)page_no].line_num;
    }

    private static uint AoYsdFileGetLineId(YSDS_HEADER file, uint page_no, uint line_no)
    {
        return file.pages[(int)page_no].lines[(int)line_no].id;
    }

    private static string AoYsdFileGetLineString(
      YSDS_HEADER file,
      uint page_no,
      uint line_no)
    {
        return file.pages[(int)page_no].lines[(int)line_no].s;
    }

}