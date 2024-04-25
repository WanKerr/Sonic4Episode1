public partial class AppMain
{
    public static AMS_CRIAUDIO_INTERFACE amCriAudioGetGlobal()
    {
        return pAu;
    }

    private void amCriAudioInit()
    {
    }

    private void amCriAudioExit()
    {
        for (uint index = 0; index < 8U; ++index)
        {
            if (pAu.auply[(int)index] != null)
            {
                pAu.auply[(int)index].Destroy();
                pAu.auply[(int)index] = null;
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
            if (pAu.auply[(int)index] != null)
            {
                pAu.auply[(int)index].Destroy();
                pAu.auply[(int)index] = null;
            }
        }
    }

    private void amCriAudioExcuteMain()
    {
        for (uint index = 0; index < 8U; ++index)
        {
            if (pAu.auply[(int)index] != null && pAu.auply[(int)index].GetStatus() != 0)
                pAu.auply[(int)index].Update();
        }
    }

    private static void amCriAudioStrmPlay(uint Id, string CueName)
    {
        pAu.auply[(int)Id].Stop();
        pAu.auply[(int)Id].SetCue(CueName);
        pAu.auply[(int)Id].Play();
    }

}