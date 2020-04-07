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
    public static AppMain.AMS_CRIAUDIO_INTERFACE amCriAudioGetGlobal()
    {
        return AppMain.pAu;
    }

    private void amCriAudioInit()
    {
    }

    private void amCriAudioExit()
    {
        for (uint index = 0; index < 8U; ++index)
        {
            if (AppMain.pAu.auply[(int)index] != null)
            {
                AppMain.pAu.auply[(int)index].Destroy();
                AppMain.pAu.auply[(int)index] = (AppMain.CriAuPlayer)null;
            }
        }
    }

    private void amCriAudioCreateCueSheet(string filePath, int csbType, int prio)
    {
    }

    private void amCriAudioDestroyCueSheet(int csbType)
    {
        for (uint index = 0; index < 8U; ++index)
        {
            if (AppMain.pAu.auply[(int)index] != null)
            {
                AppMain.pAu.auply[(int)index].Destroy();
                AppMain.pAu.auply[(int)index] = (AppMain.CriAuPlayer)null;
            }
        }
    }

    private void amCriAudioExcuteMain()
    {
        for (uint index = 0; index < 8U; ++index)
        {
            if (AppMain.pAu.auply[(int)index] != null && AppMain.pAu.auply[(int)index].GetStatus() != 0)
                AppMain.pAu.auply[(int)index].Update();
        }
    }

    private static void amCriAudioStrmPlay(uint Id, string CueName)
    {
        AppMain.pAu.auply[(int)Id].Stop();
        AppMain.pAu.auply[(int)Id].SetCue(CueName);
        AppMain.pAu.auply[(int)Id].Play();
    }

}