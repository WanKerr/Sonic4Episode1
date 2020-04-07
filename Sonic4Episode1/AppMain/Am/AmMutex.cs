using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using mpp;

public partial class AppMain
{
    private static void amMutexCreate(object mutex)
    {
    }

    private static int amMutexDelete(object mutex)
    {
        return 1;
    }

    private static void amMutexLock(object mutex)
    {
        AppMain.amAssert(mutex);
        Monitor.Enter(mutex);
    }

    private int amMutexTrylock(object mutex)
    {
        AppMain.amAssert(mutex);
        return !Monitor.TryEnter(mutex) ? 0 : 1;
    }

    private static void amMutexUnlock(object mutex)
    {
        AppMain.amAssert(mutex);
        Monitor.Exit(mutex);
    }
}