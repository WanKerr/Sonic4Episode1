public partial class AppMain
{

    private static int objMapGetDiff(int lCol, sbyte sPix, sbyte sDelta)
    {
        return lCol <= 0 ? (sDelta <= 0 ? lCol + sPix : -(sPix + 1)) : (sDelta <= 0 ? 8 - sPix : lCol - (sPix + 1));
    }

    private static int objMapGetForward(sbyte sPix, sbyte sDelta)
    {
        return sDelta <= 0 ? 1 + sPix : 8 - sPix;
    }

    private static int objMapGetBack(sbyte sPix, sbyte sDelta)
    {
        return sDelta <= 0 ? sPix - 8 : -(sPix + 1);
    }

    private static int objMapGetForwardRev(sbyte sPix, sbyte sDelta)
    {
        return sDelta <= 0 ? sPix : 8 - (sPix + 1);
    }


}