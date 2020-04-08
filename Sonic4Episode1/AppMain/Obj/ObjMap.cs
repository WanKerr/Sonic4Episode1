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

    private static int objMapGetDiff(int lCol, sbyte sPix, sbyte sDelta)
    {
        return lCol <= 0 ? (sDelta <= (sbyte)0 ? lCol + (int)sPix : -((int)sPix + 1)) : (sDelta <= (sbyte)0 ? 8 - (int)sPix : lCol - ((int)sPix + 1));
    }

    private static int objMapGetForward(sbyte sPix, sbyte sDelta)
    {
        return sDelta <= (sbyte)0 ? 1 + (int)sPix : 8 - (int)sPix;
    }

    private static int objMapGetBack(sbyte sPix, sbyte sDelta)
    {
        return sDelta <= (sbyte)0 ? (int)sPix - 8 : -((int)sPix + 1);
    }

    private static int objMapGetForwardRev(sbyte sPix, sbyte sDelta)
    {
        return sDelta <= (sbyte)0 ? (int)sPix : 8 - ((int)sPix + 1);
    }


}