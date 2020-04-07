using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using gs.backup;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

public partial class AppMain
{
    public class GSS_SND_SYS_MAIN_INFO
    {
        public uint flag;
        public float system_cnt_vol;
        public int suspend_wait_count;

        internal void Clear()
        {
            this.flag = 0U;
            this.system_cnt_vol = 0.0f;
            this.suspend_wait_count = 0;
        }
    }

    public class GSS_SND_CTRL_PARAM : AppMain.IClearable
    {
        public uint fade_state;
        public int fade_frame_max;
        public int fade_frame_cnt;
        public float fade_vol;
        public float fade_sub_vol;
        public float volume;
        public float pitch;

        public void Clear()
        {
            this.fade_state = 0U;
            this.fade_frame_max = this.fade_frame_cnt = 0;
            this.pitch = this.fade_vol = this.fade_sub_vol = this.volume = 0.0f;
        }
    }

    public class GSS_SND_SCB
    {
        public readonly AppMain.GSS_SND_CTRL_PARAM snd_ctrl_param = new AppMain.GSS_SND_CTRL_PARAM();
        public uint flag;
        public int snd_data_type;
        public int auply_no;
        public uint cur_pause_level;
        public AppMain.GSS_SND_SCB.error_state noplay_error_state;

        internal void Clear()
        {
            this.flag = 0U;
            this.snd_data_type = 0;
            this.snd_ctrl_param.Clear();
            this.auply_no = 0;
            this.cur_pause_level = 0U;
        }

        public struct error_state
        {
            public uint sample;
            public uint counter;
        }
    }

    public class CriAuPlayer
    {
        public int cue = -1;
        private int activefx = -1;
        private float oldAisacParam = -1000f;
        public const int TYPE_FX = 0;
        public const int TYPE_BGM = 1;
        public const int STOP_MODE_RELEASE = 0;
        public const int STOP_MODE_IMMEDIATE = 1;
        public const int STATUS_STOP = 0;
        public const int STATUS_PREP = 1;
        public const int STATUS_PLAYING = 2;
        public const int STATUS_PLAYEND = 3;
        public const int STATUS_ERROR = 4;
        private const float NON_AISAC = -1000f;
        public MediaState m_stGMState;
        public float m_fBGVolume;
        public static string m_ActiveSong;
        public int type;
        public bool status_paused;
        public string se_name;
        private string aisac;
        private float[] volume;
        public bool m_bLoop;
        private float pitch;
        public int status;
        public SoundEffectInstance[] sound;
        private SoundEffect m_sndEffect;
        private AppMain.AISAC_LIST[] aisac_list;
        private int effectscount;
        public static Song m_songBGM;

        public CriAuPlayer()
        {
            this.status = 0;
            this.cue = -1;
        }

        public static uint GetCueId(string name)
        {
            return (uint) AppMain.sound_fx_list[name].cue;
        }

        public static string GetCueName(uint id)
        {
            foreach (KeyValuePair<string, AppMain.SOUND_TABLE> soundFx in AppMain.sound_fx_list)
            {
                if ((long) soundFx.Value.cue == (long) id)
                    return soundFx.Key;
            }

            return (string) null;
        }

        public void SetPitch(float val)
        {
            this.pitch = val;
        }

        public void SetAisac(string s, float val)
        {
            if (this.type != 0)
                return;
            this.Pause((double) val < 0.1);
        }

        internal int GetStatus()
        {
            return this.status;
        }

        internal void Stop()
        {
            if (this.type == 0)
            {
                this.activefx = 0;
                this.status_paused = false;
                for (int index = 0; index < this.effectscount; ++index)
                {
                    if (this.sound[index] != null)
                        this.sound[index].Stop();
                }

                this.status = 0;
                this.status_paused = false;
            }
            else
            {
                this.m_stGMState = MediaState.Stopped;
                if (this.se_name != null && this.se_name == AppMain.CriAuPlayer.m_ActiveSong)
                    MediaPlayer.Stop();
                this.status = 0;
            }

            this.oldAisacParam = -1000f;
        }

        internal void Update()
        {
            bool flag = true;
            if (this.type == 0)
            {
                if (this.cue == 128)
                {
                    if (this.status == 2 && !this.status_paused)
                    {
                        if (this.sound[0].State == SoundState.Stopped && this.sound[1].State == SoundState.Paused)
                        {
                            this.activefx = 1;
                            this.sound[1].Play();
                            flag = false;
                        }
                        else
                            flag = this.sound[0].State == SoundState.Stopped &&
                                   this.sound[1].State == SoundState.Stopped;
                    }
                    else
                        flag = false;

                    this.sound[this.activefx].Volume = this.volume[0];
                }
                else
                {
                    for (int index = 0; index < this.effectscount; ++index)
                    {
                        if (this.sound[index] != null)
                        {
                            this.sound[index].Volume = this.volume[index];
                            if (this.status != 2 || this.sound[index].State != SoundState.Stopped)
                            {
                                flag = false;
                                break;
                            }
                        }
                    }
                }

                if (flag)
                    this.status = 3;
            }

            if (this.type != 1)
                return;
            if (AppMain.CriAuPlayer.m_ActiveSong == this.se_name &&
                (double) this.m_fBGVolume != (double) this.volume[0])
            {
                MediaPlayer.Volume = this.volume[0];
                this.m_fBGVolume = this.volume[0];
            }

            if (this.status != 2 || this.m_stGMState != MediaState.Stopped)
                flag = false;
            if (!flag)
                return;
            this.status = 3;
        }

        internal void Stop(int mode)
        {
            this.Stop();
            if (mode != 0)
                return;
            this.ReleaseCue();
        }

        internal void Pause(bool pause)
        {
            if (this.type == 0)
            {
                if (this.cue == 128)
                {
                    if (this.sound[this.activefx] != null)
                    {
                        if (pause)
                            this.sound[this.activefx].Pause();
                        else
                            this.sound[this.activefx].Resume();
                    }
                }
                else
                {
                    for (int index = 0; index < this.effectscount; ++index)
                    {
                        if (this.sound[index] != null)
                        {
                            if (pause)
                                this.sound[index].Pause();
                            else
                                this.sound[index].Resume();
                        }
                    }
                }

                this.status_paused = pause;
            }
            else if (pause)
            {
                this.m_stGMState = MediaState.Paused;
                if (!(this.se_name == AppMain.CriAuPlayer.m_ActiveSong))
                    return;
                MediaPlayer.Pause();
            }
            else if (AppMain.CriAuPlayer.m_ActiveSong != this.se_name)
            {
                string seName = this.se_name;
                this.se_name = (string) null;
                this.SetCue(seName);
                this.Play();
            }
            else
            {
                this.m_stGMState = MediaState.Playing;
                MediaPlayer.Resume();
            }
        }

        internal void SetVolume(float volume)
        {
            if (this.aisac != null)
                return;
            if (this.type == 0)
            {
                for (int index = 0; index < this.effectscount; ++index)
                {
                    if (this.sound[index] != null)
                    {
                        this.volume[index] = volume;
                        this.sound[index].Volume = volume;
                    }
                }
            }
            else
            {
                if (!(AppMain.CriAuPlayer.m_ActiveSong == this.se_name) || (double) this.m_fBGVolume == (double) volume)
                    return;
                this.m_fBGVolume = volume;
                this.volume[0] = volume;
                MediaPlayer.Volume = volume;
            }
        }

        public static int LoadSound(
            string fileName,
            bool loop,
            ref int loopStart,
            ref int loopEnd,
            out byte[] byteArray,
            ref int sampleRate,
            ref int channels,
            bool bgMusic)
        {
            byteArray = (byte[]) null;
            if (!bgMusic && AppMain.cacheFxSounds.ContainsKey(fileName))
                return 0;
            int num1 = 0;
            using (Stream stream = TitleContainer.OpenStream("Content\\SOUND\\" + fileName + ".wav"))
            {
                using (BinaryReader binaryReader = new BinaryReader(stream))
                {
                    if (!bgMusic)
                    {
                        SoundEffect soundEffect = SoundEffect.FromStream(stream);
                        AppMain.cacheFxSounds.Add(fileName, soundEffect);
                    }
                    else
                    {
                        binaryReader.ReadInt32();
                        binaryReader.ReadInt32();
                        binaryReader.ReadInt32();
                        binaryReader.ReadInt32();
                        int num2 = binaryReader.ReadInt32();
                        int num3 = (int) binaryReader.ReadInt16();
                        channels = (int) binaryReader.ReadInt16();
                        sampleRate = binaryReader.ReadInt32();
                        binaryReader.ReadInt32();
                        int num4 = (int) binaryReader.ReadInt16();
                        int num5 = (int) binaryReader.ReadInt16();
                        if (num2 == 18)
                        {
                            int count = (int) binaryReader.ReadInt16();
                            binaryReader.ReadBytes(count);
                        }

                        binaryReader.ReadInt32();
                        int count1 = binaryReader.ReadInt32();
                        byteArray = binaryReader.ReadBytes(count1);
                        num1 = count1;
                        if (loop && loopEnd == 0)
                            loopEnd = byteArray.Length;
                        loopStart += loopStart % num4;
                        loopEnd -= loopEnd % num4;
                    }
                }
            }

            return num1;
        }

        internal void SetCue(string se_name)
        {
            try
            {
                for (; this.se_name != null; this.se_name = (string) null)
                {
                    if (this.se_name == se_name)
                    {
                        this.Stop();
                        this.status = 1;
                        // return;
                    }
                }

                AppMain.SOUND_TABLE soundTable = (AppMain.SOUND_TABLE) null;
                if (AppMain.sound_fx_list.ContainsKey(se_name))
                {
                    this.cue = -1;
                    this.type = 0;
                    soundTable = AppMain.sound_fx_list[se_name];
                }
                else if (AppMain.sound_bgm_list.ContainsKey(se_name))
                {
                    this.type = 1;
                    soundTable = AppMain.sound_bgm_list[se_name];
                }

                if (soundTable != null)
                {
                    this.effectscount = soundTable.count;
                    this.se_name = se_name;
                    this.aisac_list = soundTable.asiac;
                    this.cue = soundTable.cue;
                    this.sound = new SoundEffectInstance[this.effectscount];
                    this.volume = new float[this.effectscount];
                    if (this.type == 0)
                    {
                        this.activefx = 0;
                        this.status_paused = false;
                        for (int index = 0; index < this.effectscount; ++index)
                        {
                            if (AppMain.cacheFxSounds.ContainsKey(soundTable.filename[index]))
                            {
                                SoundEffect cacheFxSound = AppMain.cacheFxSounds[soundTable.filename[index]];
                                this.m_sndEffect = cacheFxSound;
                                if (soundTable.loop[index])
                                {
                                    SoundEffectInstance instance;
                                    try
                                    {
                                        instance = cacheFxSound.CreateInstance();
                                    }
                                    catch (Exception ex1)
                                    {
                                        GC.Collect();
                                        try
                                        {
                                            instance = cacheFxSound.CreateInstance();
                                        }
                                        catch (Exception ex2)
                                        {
                                            this.status = 4;
                                            return;
                                        }
                                    }

                                    this.sound[index] = instance;
                                    this.sound[index].IsLooped = soundTable.loop[index];
                                    instance.Volume = 0.0f;
                                    instance.Pitch = this.pitch;
                                    instance.Play();
                                    instance.Pause();
                                    instance.Volume = soundTable.volume[index];
                                }
                                else
                                {
                                    this.sound[index] = cacheFxSound.CreateInstance();
                                    this.sound[index].Volume = soundTable.volume[index];
                                    this.sound[index].Play();
                                }
                            }
                            else
                                AppMain.mppAssertNotImpl();
                        }
                    }
                    else
                    {
                        AppMain.CriAuPlayer.m_songBGM = AppMain.gsSoundGetPreloadedBGM(soundTable.filename[0]);
                        if (AppMain.CriAuPlayer.m_songBGM == (Song) null)
                        {
                            AppMain.CriAuPlayer.m_songBGM =
                                Sonic4Ep1.pInstance.Content.Load<Song>("Sound/" + soundTable.filename[0]);
                            AppMain.bgmPreloadedList.Add(soundTable.filename[0], AppMain.CriAuPlayer.m_songBGM);
                        }

                        AppMain.CriAuPlayer.m_ActiveSong = se_name;
                        this.m_fBGVolume = -1f;
                        this.m_bLoop = soundTable.loop[0];
                    }

                    this.status = 1;
                }
                else
                    this.status = 4;
            }
            catch (Exception ex)
            {
                ex.ToString();
                this.status = 4;
            }
        }

        internal void Play()
        {
            if (this.type == 1)
            {
                if (this.se_name != AppMain.CriAuPlayer.m_ActiveSong)
                {
                    string seName = this.se_name;
                    this.se_name = (string) null;
                    this.SetCue(seName);
                }

                if (this.se_name != null)
                {
                    if (this.se_name == AppMain.CriAuPlayer.m_ActiveSong)
                    {
                        try
                        {
                            MediaPlayer.IsRepeating = this.m_bLoop;
                            MediaPlayer.Stop();
                            this.m_stGMState = MediaState.Playing;
                            MediaPlayer.Play(AppMain.CriAuPlayer.m_songBGM);
                        }
                        catch (UnauthorizedAccessException ex)
                        {
                            AppMain.g_ao_sys_global.is_playing_device_bgm_music = true;
                        }
                    }
                }

                this.status = 2;
            }
            else
            {
                if (this.cue == 128)
                {
                    this.activefx = 0;
                    if (this.sound[0] != null)
                        this.sound[this.activefx].Resume();
                }
                else
                {
                    for (int index = 0; index < this.effectscount; ++index)
                    {
                        if (this.sound[index] != null)
                        {
                            this.sound[index].Pitch = this.pitch;
                            this.sound[index].Resume();
                        }
                        else if (this.m_sndEffect != null)
                            this.m_sndEffect.Play();
                    }
                }

                this.status = 2;
            }
        }

        internal void ReleaseCue()
        {
            this.se_name = (string) null;
            this.aisac = (string) null;
            this.activefx = -1;
            this.status_paused = false;
            if (this.type == 0)
            {
                for (int index = 0; index < this.effectscount; ++index)
                {
                    if (this.sound[index] != null)
                    {
                        this.sound[index].Stop();
                        this.sound[index].Dispose();
                        this.sound[index] = (SoundEffectInstance) null;
                    }
                }

                this.cue = -1;
                this.status = 0;
            }
            else
            {
                this.m_stGMState = MediaState.Stopped;
                if (this.se_name != null && this.se_name == AppMain.CriAuPlayer.m_ActiveSong)
                    MediaPlayer.Stop();
                AppMain.CriAuPlayer.m_ActiveSong = (string) null;
                this.status = 0;
            }

            this.oldAisacParam = -1000f;
        }

        internal void ResetParameters()
        {
            for (int index = 0; index < this.effectscount; ++index)
            {
                this.volume[index] = 1f;
                this.aisac = (string) null;
            }
        }

        internal bool IsPaused()
        {
            if (this.type != 0)
                return this.m_stGMState == MediaState.Paused;
            if (this.cue == 128)
                return this.status_paused;
            int index = 0;
            return index < this.effectscount && this.sound[index] != null &&
                   this.sound[index].State == SoundState.Paused;
        }

        internal int GetNumPlayedSamples()
        {
            return 1;
        }

        internal void Destroy()
        {
            this.ReleaseCue();
        }

        internal static AppMain.CriAuPlayer Create(AppMain.AMS_CRIAUDIO_INTERFACE cri_if)
        {
            return new AppMain.CriAuPlayer();
        }
    }

    public class AISAC_LIST
    {
        public int count;
        public int[] types;
        public float[][][] values;

        public AISAC_LIST(int count)
        {
            this.count = count;
            this.types = new int[count];
            this.values = new float[count][][];
        }
    }

    public class SOUND_TABLE
    {
        public int cue;
        public int type;
        public string name;
        public string uid;
        public float[] volume;
        public string[] filename;
        public bool[] loop;
        public int[] loopStart;
        public int[] loopEnd;
        public float[] pitch;
        public int count;
        public AppMain.AISAC_LIST[] asiac;

        public SOUND_TABLE(int count)
        {
            this.count = count;
            this.volume = new float[count];
            this.filename = new string[count];
            this.loop = new bool[count];
            this.loopStart = new int[count];
            this.loopEnd = new int[count];
            this.pitch = new float[count];
            this.asiac = new AppMain.AISAC_LIST[count];
        }
    }

    public struct AOS_SYS_GLOBAL
    {
        public bool is_show_ui;
        public bool is_signin_changed;
        public bool is_playing_device_bgm_music;
    }

    public class AMS_CRIAUDIO_INTERFACE
    {
        public readonly AppMain.CriAuPlayer[] auply;

        public AMS_CRIAUDIO_INTERFACE()
        {
            this.auply = AppMain.New<AppMain.CriAuPlayer>(8);
            for (int index = 0; index < 8; ++index)
                this.auply[index].type = 1;
        }
    }

    private static bool gsSoundFillBGMCache(int iListIndex)
    {
        foreach (string index in AppMain.bgmLists[iListIndex])
        {
            AppMain.SOUND_TABLE soundBgm = AppMain.sound_bgm_list[index];
            Song song = Sonic4Ep1.pInstance.Content.Load<Song>("Sound/" + soundBgm.filename[0]);
            AppMain.bgmPreloadedList.Add(soundBgm.filename[0], song);
        }

        return true;
    }

    private static Song gsSoundGetPreloadedBGM(string sName)
    {
        Song song;
        return !AppMain.bgmPreloadedList.TryGetValue(sName, out song) ? (Song) null : song;
    }

    public static bool GsSoundPrepareBGMForLevel(int iLevel)
    {
        if (AppMain.m_iBGMPreparedLevel == iLevel)
            return true;
        AppMain.bgmPreloadedList.Clear();
        AppMain.gsSoundFillBGMCache(0);
        int iListIndex = 0;
        if (iLevel >= 0 && iLevel <= 3)
            iListIndex = 1;
        else if (iLevel >= 4 && iLevel <= 7)
            iListIndex = 2;
        else if (iLevel >= 8 && iLevel <= 11)
            iListIndex = 3;
        else if (iLevel >= 12 && iLevel <= 15)
            iListIndex = 4;
        if (iListIndex != 0)
            AppMain.gsSoundFillBGMCache(iListIndex);
        AppMain.m_iBGMPreparedLevel = iLevel;
        return true;
    }

    private static bool GsSoundIsBgmStop(AppMain.GSS_SND_SCB scb)
    {
        return ((int) scb.flag & 1) == 0 || ((int) scb.flag & 2) != 0;
    }

    private static bool GsSoundIsBgmPause(AppMain.GSS_SND_SCB scb)
    {
        return ((int) scb.flag & 1) != 0 && scb.cur_pause_level == (uint) int.MaxValue && ((int) scb.flag & 4) != 0;
    }

    private static void gsSoundFillSoundTable(string filename, Dictionary<string, AppMain.SOUND_TABLE> list)
    {
        using (Stream stream = TitleContainer.OpenStream("Content\\SOUND\\" + filename))
        {
            using (StreamReader streamReader = new StreamReader(stream))
            {
                while (streamReader.Peek() >= 0)
                {
                    string[] strArray1 = streamReader.ReadLine().Split('|');
                    AppMain.SOUND_TABLE soundTable = new AppMain.SOUND_TABLE(strArray1.Length);
                    for (int index1 = 0; index1 < strArray1.Length; ++index1)
                    {
                        int startIndex = strArray1[index1].IndexOf("Aisac");
                        if (startIndex != -1)
                        {
                            string str = strArray1[index1].Substring(startIndex);
                            strArray1[index1] = strArray1[index1].Substring(0, startIndex - 1);
                            string[] strArray2 = str.Split('#');
                            soundTable.asiac[index1] = new AppMain.AISAC_LIST(strArray2.Length - 1);
                            for (int index2 = 1; index2 < strArray2.Length; ++index2)
                            {
                                string[] strArray3 = (string[]) null;
                                if (strArray2[index2].StartsWith("Volume"))
                                {
                                    soundTable.asiac[index1].types[index2 - 1] = 0;
                                    strArray3 = strArray2[index2].Substring(7).Split(' ');
                                }
                                else if (strArray2[index2].StartsWith("Pitch"))
                                {
                                    soundTable.asiac[index1].types[index2 - 1] = 1;
                                    strArray3 = strArray2[index2].Substring(6).Split(' ');
                                }

                                soundTable.asiac[index1].values[index2 - 1] = new float[strArray3.Length][];
                                for (int index3 = 0; index3 < strArray3.Length; ++index3)
                                {
                                    string[] strArray4 = strArray3[index3].Split(',');
                                    soundTable.asiac[index1].values[index2 - 1][index3] = new float[2];
                                    soundTable.asiac[index1].values[index2 - 1][index3][0] = float.Parse(strArray4[0],
                                        (IFormatProvider) CultureInfo.InvariantCulture);
                                    soundTable.asiac[index1].values[index2 - 1][index3][1] =
                                        soundTable.asiac[index1].types[index2 - 1] == 1
                                            ? float.Parse(strArray4[1],
                                                (IFormatProvider) CultureInfo.InvariantCulture) / 1000f
                                            : float.Parse(strArray4[1], (IFormatProvider) CultureInfo.InvariantCulture);
                                }
                            }
                        }

                        string[] strArray5 = strArray1[index1].Split(',');
                        if (index1 == 0)
                        {
                            soundTable.name = strArray5[0];
                            soundTable.cue = int.Parse(strArray5[1]);
                            soundTable.uid = strArray5[2];
                        }

                        soundTable.volume[index1] = float.Parse(strArray5[index1 == 0 ? 3 : 0],
                            (IFormatProvider) CultureInfo.InvariantCulture);
                        if ((double) soundTable.volume[index1] > 1.0)
                            soundTable.volume[index1] = 1f;
                        else if ((double) soundTable.volume[index1] < -1.0)
                            soundTable.volume[index1] = -1f;
                        soundTable.pitch[index1] = (float) int.Parse(strArray5[index1 == 0 ? 4 : 1]) / 1000f;
                        soundTable.filename[index1] = strArray5[index1 == 0 ? 5 : 2];
                        soundTable.loop[index1] = int.Parse(strArray5[index1 == 0 ? 6 : 3]) == 1;
                        soundTable.loopStart[index1] = int.Parse(strArray5[index1 == 0 ? 7 : 4]);
                        soundTable.loopEnd[index1] = int.Parse(strArray5[index1 == 0 ? 8 : 5]);
                    }

                    list[soundTable.name] = soundTable;
                }
            }
        }
    }

    public static void MediaPlayer_MediaStateChanged(object sender, EventArgs e)
    {
        if (AppMain.g_ao_sys_global.is_playing_device_bgm_music)
            return;

        MediaState state = MediaPlayer.State;
        AppMain.AMS_CRIAUDIO_INTERFACE global = AppMain.amCriAudioGetGlobal();
        string str = MediaPlayer.Queue.ActiveSong != (Song) null ? MediaPlayer.Queue.ActiveSong.Name : (string) null;
        for (int index = 0; index < global.auply.Length; ++index)
        {
            if (str != null && global.auply[index].se_name != null &&
                (global.auply[index].se_name != null &&
                 str == "Sound\\" + AppMain.sound_bgm_list[global.auply[index].se_name].filename[0]))
            {
                double totalMilliseconds1 = AppMain.CriAuPlayer.m_songBGM.Duration.TotalMilliseconds;
                double totalMilliseconds2 = MediaPlayer.PlayPosition.TotalMilliseconds;
                if (state == MediaState.Paused && global.auply[index].m_stGMState != MediaState.Paused)
                {
                    if (totalMilliseconds2 + 1.0 >= totalMilliseconds1 || totalMilliseconds2 == 0.0)
                    {
                        if (!global.auply[index].m_bLoop && global.auply[index].se_name.IndexOf("_speedup") != -1)
                        {
                            string se_name = global.auply[index].se_name.Replace("_speedup", "");
                            global.auply[index].SetCue(se_name);
                            global.auply[index].Play();
                            break;
                        }

                        global.auply[index].m_stGMState = MediaState.Stopped;
                        global.auply[index].status = 3;
                        break;
                    }

                    if (global.auply[index].m_stGMState != MediaState.Playing)
                        break;
                    MediaPlayer.Resume();
                    break;
                }

                if (state != MediaState.Stopped || global.auply[index].m_stGMState == MediaState.Stopped)
                    break;
                if (totalMilliseconds2 == 0.0 && !global.auply[index].m_bLoop &&
                    global.auply[index].se_name.IndexOf("_speedup") != -1)
                {
                    string se_name = global.auply[index].se_name.Replace("_speedup", "");
                    global.auply[index].SetCue(se_name);
                    global.auply[index].Play();
                    break;
                }

                global.auply[index].m_stGMState = state;
                global.auply[index].status = 3;
                break;
            }
        }
    }

    private static void GsSoundInit()
    {
        MediaPlayer.MediaStateChanged += new EventHandler<EventArgs>(AppMain.MediaPlayer_MediaStateChanged);
        if (AppMain.sound_fx_list == null)
        {
            var modernSfx = SSave.CreateInstance().GetRemaster().ModernSoundEffects;
            AppMain.sound_fx_list = new Dictionary<string, AppMain.SOUND_TABLE>(130);
            AppMain.gsSoundFillSoundTable(modernSfx ? "SND_FX_GENS.inf" : "SND_FX.inf", AppMain.sound_fx_list);
        }

        if (AppMain.sound_bgm_list == null)
        {
            AppMain.sound_bgm_list = new Dictionary<string, AppMain.SOUND_TABLE>(50);
            AppMain.gsSoundFillSoundTable("SND_BGM.inf", AppMain.sound_bgm_list);
        }

        AppMain.LoadPrioritySoundsIntoCache();
        GC.Collect();
        AppMain.GSS_MAIN_SYS_INFO mainSysInfo = AppMain.GsGetMainSysInfo();
        AppMain.gsSoundInitSystemMainInfo();
        AppMain.gsSoundInitSndScbHeap();
        AppMain.gsSoundInitSeHandleHeap();
        int vol_bgm;
        int vol_se;
        AppMain.GetSavedSoundVolumes(out vol_bgm, out vol_se);
        mainSysInfo.bgm_volume = (float) vol_bgm / 10f;
        mainSysInfo.se_volume = (float) vol_se / 10f;
        for (int snd_type = 0; snd_type < 2; ++snd_type)
            AppMain.GsSoundSetVolume(snd_type, 1f);
    }

    private static void GetSavedSoundVolumes(out int vol_bgm, out int vol_se)
    {
        vol_bgm = 10;
        vol_se = 10;
        try
        {
            var sys = gs.backup.SSave.CreateInstance();
            vol_bgm = (int) ((sys.GetOption()?.GetVolumeBgm() / 10f) ?? (uint) vol_bgm);
            vol_se = (int) ((sys.GetOption()?.GetVolumeBgm() / 10f) ?? (uint) vol_se);
        }
        catch (Exception ex)
        {
        }
    }

    private static void GsSoundHalt()
    {
        AppMain.amCriAudioGetGlobal();
        for (int index = 0; index < 8; ++index)
        {
            if (((int) AppMain.gs_sound_scb_heap[index].flag & 1) != 0)
                AppMain.GsSoundStopBgm(AppMain.gs_sound_scb_heap[index]);
        }
    }

    private static void LoadSFX(AppMain.SOUND_TABLE tbl)
    {
        try
        {
            for (int index = 0; index < tbl.count; ++index)
            {
                byte[] byteArray = (byte[]) null;
                int channels = 0;
                int sampleRate = 0;
                AppMain.CriAuPlayer.LoadSound(tbl.filename[index], tbl.loop[index], ref tbl.loopStart[index],
                    ref tbl.loopEnd[index], out byteArray, ref sampleRate, ref channels, false);
            }
        }
        catch (FileNotFoundException ex)
        {
            string str1 = ex.ToString();
            string str2 = str1 + str1;
            AppMain.mppSoundNotImplAssert();
        }
    }

    private static void LoadPrioritySoundsIntoCache()
    {
        if (AppMain.b_bPrioritySoundsLoaded)
            return;
        AppMain.LoadSFX(AppMain.sound_fx_list["Sega_Logo"]);
        AppMain.LoadSFX(AppMain.sound_fx_list["Ok"]);
        AppMain.LoadSFX(AppMain.sound_fx_list["Window"]);
        AppMain.LoadSFX(AppMain.sound_fx_list["Cancel"]);
        AppMain.LoadSFX(AppMain.sound_fx_list["Cursol"]);
        AppMain.b_bPrioritySoundsLoaded = true;
    }

    private static bool SoundPartialCache(int iPercent)
    {
        if (AppMain.g_bSoundsPrecached)
            return true;
        int count = AppMain.sound_fx_list.Count;
        int num = Math.Min(Math.Max(count * iPercent / 100, 1), count - AppMain.g_iCurrentCachedIndex);
        for (int currentCachedIndex = AppMain.g_iCurrentCachedIndex;
            currentCachedIndex < AppMain.g_iCurrentCachedIndex + num;
            ++currentCachedIndex)
            AppMain.LoadSFX(AppMain.sound_fx_list
                .ElementAt<KeyValuePair<string, AppMain.SOUND_TABLE>>(currentCachedIndex).Value);
        AppMain.g_iCurrentCachedIndex += num;
        AppMain.g_bSoundsPrecached = AppMain.g_iCurrentCachedIndex == count;
        return AppMain.g_bSoundsPrecached;
    }

    private static void GsSoundReset()
    {
        for (int snd_type = 0; snd_type < 2; ++snd_type)
            AppMain.GsSoundSetVolume(snd_type, 1f);
        AppMain.gsSoundResetSeHandleHeap();
        AppMain.gsSoundResetSndScbHeap();
        AppMain.gsSoundResetSystemMainInfo();
    }

    private static void GsSoundExit()
    {
        for (int snd_type = 0; snd_type < 2; ++snd_type)
            AppMain.GsSoundSetVolume(snd_type, 0.0f);
        AppMain.GsSoundHalt();
        AppMain.GsSoundEnd();
        AppMain.gsSoundClearSeHandleHeap();
        AppMain.gsSoundResetSndScbHeap();
        AppMain.gsSoundClearSystemMainInfo();
    }

    private static void GsSoundBegin(ushort task_pause_level, uint task_prio, int task_group)
    {
        AppMain.GsSoundSetVolumeFromMainSysInfo();
        AppMain.gs_sound_tcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.gsSoundProcMain),
            (AppMain.GSF_TASK_PROCEDURE) null, 0U, task_pause_level, task_prio, task_group,
            (AppMain.TaskWorkFactoryDelegate) null, "GS_SND_MAIN");
    }

    private static void GsSoundEnd()
    {
        if (AppMain.gs_sound_tcb == null)
            return;
        AppMain.mtTaskClearTcb(AppMain.gs_sound_tcb);
        AppMain.gs_sound_tcb = (AppMain.MTS_TASK_TCB) null;
    }

    private static bool GsSoundIsRunning()
    {
        return AppMain.gs_sound_tcb != null;
    }

    private static AppMain.GSS_SND_SYS_MAIN_INFO GsSoundGetSysMainInfo()
    {
        return AppMain.gs_sound_sys_main_info;
    }

    private static void GsSoundPlaySe(string se_name)
    {
        AppMain.GsSoundPlaySe(se_name, (AppMain.GSS_SND_SE_HANDLE) null, 0);
    }

    private static void GsSoundPlaySe(string se_name, AppMain.GSS_SND_SE_HANDLE se_handle)
    {
        AppMain.GsSoundPlaySe(se_name, se_handle, 0);
    }

    private static void GsSoundPlaySe(
        string se_name,
        AppMain.GSS_SND_SE_HANDLE se_handle,
        int fade_frame)
    {
        AppMain.gsSoundPlaySe(se_name, 0U, se_handle, fade_frame);
    }

    private static void GsSoundPlaySeForce(string se_name)
    {
        AppMain.GsSoundPlaySeForce(se_name, (AppMain.GSS_SND_SE_HANDLE) null, 0);
    }

    private static void GsSoundPlaySeForce(string se_name, AppMain.GSS_SND_SE_HANDLE se_handle)
    {
        AppMain.GsSoundPlaySeForce(se_name, se_handle, 0);
    }

    private static void GsSoundPlaySeForce(
        string se_name,
        AppMain.GSS_SND_SE_HANDLE se_handle,
        int fade_frame)
    {
        AppMain.gsSoundPlaySe(se_name, 0U, se_handle, fade_frame);
    }

    private static void GsSoundPlaySeForce(
        string se_name,
        AppMain.GSS_SND_SE_HANDLE se_handle,
        int fade_frame,
        bool bDontPlay)
    {
        AppMain.gsSoundPlaySe(se_name, 0U, se_handle, fade_frame, bDontPlay);
    }

    private static void GsSoundPlaySeByIdForce(
        uint se_id,
        AppMain.GSS_SND_SE_HANDLE se_handle,
        int fade_frame)
    {
        AppMain.gsSoundPlaySe((string) null, se_id, se_handle, fade_frame);
    }

    private static void GsSoundStopSe()
    {
        AppMain.GsSoundStopSe(0, false);
    }

    private static void GsSoundStopSe(int fade_frame, bool is_immediate)
    {
        for (int index = 0; index < 16; ++index)
        {
            AppMain.GSS_SND_SE_HANDLE se_handle = AppMain.gs_sound_se_handle_heap[index];
            if (((int) se_handle.flag & 1) != 0)
                AppMain.gsSoundCriSeStop(se_handle, fade_frame, is_immediate);
        }
    }

    private static void GsSoundPauseSe(uint pause_level)
    {
        AppMain.GsSoundPauseSe(pause_level, 0);
    }

    private static void GsSoundPauseSe(uint pause_level, int fade_frame)
    {
        for (int index = 0; index < 16; ++index)
        {
            AppMain.GSS_SND_SE_HANDLE se_handle = AppMain.gs_sound_se_handle_heap[index];
            if (((int) se_handle.flag & 1) != 0)
            {
                if (se_handle.cur_pause_level < pause_level)
                    se_handle.cur_pause_level = pause_level;
                AppMain.gsSoundCriSePause(se_handle, fade_frame);
            }
        }
    }

    private static void GsSoundResumeSe(uint pause_level)
    {
        AppMain.GsSoundResumeSe(pause_level, 0);
    }

    private static void GsSoundResumeSe(uint pause_level, int fade_frame)
    {
        for (int index = 0; index < 16; ++index)
        {
            AppMain.GSS_SND_SE_HANDLE se_handle = AppMain.gs_sound_se_handle_heap[index];
            if (((int) se_handle.flag & 1) != 0 && se_handle.cur_pause_level <= pause_level)
            {
                se_handle.cur_pause_level = 0U;
                AppMain.gsSoundCriSeResume(se_handle, fade_frame);
            }
        }
    }

    private static void GmSoundStopSE(AppMain.GSS_SND_SE_HANDLE se_handle)
    {
        AppMain.GsSoundStopSeHandle(se_handle, 0);
    }

    private static void GsSoundStopSeHandle(AppMain.GSS_SND_SE_HANDLE se_handle)
    {
        AppMain.GsSoundStopSeHandle(se_handle, 0);
    }

    private static void GsSoundStopSeHandle(AppMain.GSS_SND_SE_HANDLE se_handle, int fade_frame)
    {
        AppMain.gsSoundCriSeStop(se_handle, fade_frame, false, false);
    }

    private static void GsSoundPlayBgm(AppMain.GSS_SND_SCB scb, string bgm_name, int fade_frame)
    {
        scb.cur_pause_level = 0U;
        if (scb.snd_data_type == 1 || AppMain.g_ao_sys_global.is_playing_device_bgm_music)
            return;
        AppMain.gsSoundCriStrmStop(scb, 0);
        AppMain.AMS_CRIAUDIO_INTERFACE global = AppMain.amCriAudioGetGlobal();
        AppMain.CriAuPlayer criAuPlayer = global.auply[scb.auply_no];
        if (criAuPlayer != null)
        {
            criAuPlayer.ReleaseCue();
            criAuPlayer.ResetParameters();
        }

        for (int index = 0; index < global.auply.Length; ++index)
        {
            if (global.auply[index].se_name == bgm_name)
            {
                global.auply[index].ReleaseCue();
                global.auply[index].ResetParameters();
                break;
            }
        }

        AppMain.amCriAudioStrmPlay((uint) scb.auply_no, bgm_name);
        AppMain.gsSoundCriStrmSetFadeIn(scb, fade_frame);
        AppMain.gsSoundUpdateVolume();
        criAuPlayer.Update();
    }

    private static void GsSoundStopBgm(AppMain.GSS_SND_SCB scb)
    {
        AppMain.GsSoundStopBgm(scb, 0);
    }

    private static void GsSoundStopBgm(AppMain.GSS_SND_SCB scb, int fade_frame)
    {
        if (scb.snd_data_type == 1 || AppMain.g_ao_sys_global.is_playing_device_bgm_music)
            return;
        AppMain.gsSoundCriStrmStop(scb, fade_frame);
    }

    private static void GsSoundPauseBgm(AppMain.GSS_SND_SCB scb, int fade_frame)
    {
        if (((int) scb.flag & 1) == 0)
            return;
        if (scb.cur_pause_level < (uint) int.MaxValue)
            scb.cur_pause_level = (uint) int.MaxValue;
        if (scb.snd_data_type == 1 || AppMain.g_ao_sys_global.is_playing_device_bgm_music)
            return;
        AppMain.gsSoundCriStrmPause(scb, fade_frame);
    }

    private static void GsSoundResumeBgm(AppMain.GSS_SND_SCB scb, int fade_frame)
    {
        if (((int) scb.flag & 1) == 0 || scb.cur_pause_level > (uint) int.MaxValue)
            return;
        scb.cur_pause_level = 0U;
        if (scb.snd_data_type == 1 || AppMain.g_ao_sys_global.is_playing_device_bgm_music)
            return;
        AppMain.gsSoundCriStrmResume(scb, fade_frame);
    }

    private static void GsSoundSetVolumeFromMainSysInfo()
    {
        AppMain.GSS_MAIN_SYS_INFO mainSysInfo = AppMain.GsGetMainSysInfo();
        AppMain.GsSoundSetVolume(0, mainSysInfo.bgm_volume);
        AppMain.GsSoundSetVolume(1, mainSysInfo.se_volume);
    }

    private static float GsSoundGetVolume(int snd_type)
    {
        return AppMain.gs_sound_volume[snd_type];
    }

    private static void GsSoundSetVolume(int snd_type, float vol)
    {
        AppMain.gs_sound_volume[snd_type] = vol;
        if (snd_type != 1)
            return;
        SoundEffect.MasterVolume = vol;
    }

    private static void GsSoundScbSetVolume(AppMain.GSS_SND_SCB scb, float vol)
    {
        if (scb.snd_data_type == 1)
            return;
        scb.snd_ctrl_param.volume = vol;
    }

    private static void GsSoundScbSetSeqMute(AppMain.GSS_SND_SCB scb, bool mute_on)
    {
        int sndDataType = scb.snd_data_type;
    }

    private static AppMain.GSS_SND_SCB GsSoundAssignScb(int snd_data_type)
    {
        for (int scb_no = 0; scb_no < 8; ++scb_no)
        {
            if (((int) AppMain.gs_sound_scb_heap_usage_flag[scb_no >> 3] & 1 << (scb_no & 7)) == 0)
            {
                AppMain.gs_sound_scb_heap_usage_flag[scb_no >> 3] |= (byte) (1 << (scb_no & 7));
                AppMain.gsSoundInitSndScb(AppMain.gs_sound_scb_heap[scb_no], scb_no, snd_data_type);
                return AppMain.gs_sound_scb_heap[scb_no];
            }
        }

        return (AppMain.GSS_SND_SCB) null;
    }

    private static void GsSoundResignScb(AppMain.GSS_SND_SCB scb)
    {
    }

    private static AppMain.GSS_SND_SE_HANDLE GsSoundAllocSeHandle()
    {
        for (int index = 0; index < 16; ++index)
        {
            if (((int) AppMain.gs_sound_se_handle_heap_usage_flag[index >> 3] & 1 << (index & 7)) == 0)
            {
                AppMain.gs_sound_se_handle_heap_usage_flag[index >> 3] |= (byte) (1 << (index & 7));
                AppMain.gsSoundInitSeHandle(AppMain.gs_sound_se_handle_heap[index]);
                AppMain.gs_sound_se_handle_heap[index].snd_ctrl_param.pitch = 0.0f;
                return AppMain.gs_sound_se_handle_heap[index];
            }
        }

        AppMain.gsSoundInitSeHandle(AppMain.gs_sound_se_handle_error);
        AppMain.gs_sound_se_handle_error.snd_ctrl_param.pitch = 0.0f;
        return AppMain.gs_sound_se_handle_error;
    }

    private static void GsSoundFreeSeHandle(AppMain.GSS_SND_SE_HANDLE se_handle)
    {
        if (AppMain.gs_sound_se_handle_error == se_handle)
        {
            AppMain.gsSoundClearSeHandle(AppMain.gs_sound_se_handle_error);
        }
        else
        {
            for (int index = 0; index < 16; ++index)
            {
                if (AppMain.gs_sound_se_handle_heap[index] == se_handle)
                {
                    AppMain.gsSoundClearSeHandle(se_handle);
                    AppMain.gs_sound_se_handle_heap_usage_flag[index >> 3] &= (byte) ~(1 << (index & 7));
                    break;
                }
            }
        }
    }

    private static void GsSoundRequestFreeSeHandle(AppMain.GSS_SND_SE_HANDLE se_handle)
    {
        se_handle.flag |= 16U;
    }

    private static void GsSoundEnterHBM(object arg)
    {
    }

    private static void GsSoundLeaveHBM(object arg)
    {
    }

    private static int GsSoundFadeHBM(object arg)
    {
        return -1;
    }

    private static void gsSoundProcMain(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.amCriAudioGetGlobal();
        AppMain.gsSoundUpdateSystemSuspendWait();
        AppMain.gsSoundUpdateSystemControlVolume();
        for (int index = 0; index < 8; ++index)
        {
            if (((int) AppMain.gs_sound_scb_heap[index].flag & 1) != 0)
                AppMain.gsSoundUpdateSndScb(AppMain.gs_sound_scb_heap[index]);
        }

        for (int index = 0; index < 16; ++index)
        {
            AppMain.GSS_SND_SE_HANDLE se_handle = AppMain.gs_sound_se_handle_heap[index];
            if (((int) se_handle.flag & 1) != 0)
                AppMain.gsSoundUpdateSndSeHandle(se_handle);
        }

        if (((int) AppMain.gs_sound_se_handle_error.flag & 1) != 0)
            AppMain.gsSoundUpdateSndSeHandle(AppMain.gs_sound_se_handle_error);
        AppMain.gsSoundUpdateVolume();
        for (int index = 0; index < 16; ++index)
        {
            AppMain.GSS_SND_SE_HANDLE se_handle = AppMain.gs_sound_se_handle_heap[index];
            if (((int) se_handle.flag & 1) != 0)
            {
                switch (se_handle.au_player.GetStatus())
                {
                    case 0:
                    case 3:
                        if (((int) se_handle.flag & 16) != 0)
                        {
                            AppMain.GsSoundFreeSeHandle(se_handle);
                            continue;
                        }

                        if (((int) se_handle.flag & 2) != 0 && ((int) se_handle.flag & int.MinValue) == 0)
                        {
                            AppMain.gsSoundClearSeHandle(se_handle);
                            continue;
                        }

                        continue;
                    case 4:
                        se_handle.au_player.Stop();
                        goto case 0;
                    default:
                        se_handle.au_player.Update();
                        continue;
                }
            }
            else if (((int) se_handle.flag & 16) != 0)
                AppMain.GsSoundFreeSeHandle(se_handle);
        }

        if (((int) AppMain.gs_sound_se_handle_error.flag & 1) != 0)
        {
            switch (AppMain.gs_sound_se_handle_error.au_player.GetStatus())
            {
                case 0:
                case 3:
                    if (((int) AppMain.gs_sound_se_handle_error.flag & 16) != 0)
                    {
                        AppMain.GsSoundFreeSeHandle(AppMain.gs_sound_se_handle_error);
                        break;
                    }

                    if (((int) AppMain.gs_sound_se_handle_error.flag & 2) != 0)
                    {
                        AppMain.gsSoundClearSeHandle(AppMain.gs_sound_se_handle_error);
                        break;
                    }

                    break;
                case 4:
                    AppMain.gs_sound_se_handle_error.au_player.Stop();
                    goto case 0;
                default:
                    AppMain.gs_sound_se_handle_error.au_player.Update();
                    break;
            }
        }
        else if (((int) AppMain.gs_sound_se_handle_error.flag & 16) != 0)
            AppMain.GsSoundFreeSeHandle(AppMain.gs_sound_se_handle_error);

        for (int index = 0; index < 8; ++index)
        {
            if (((int) AppMain.gs_sound_scb_heap[index].flag & 1) != 0)
                AppMain.gsSoundUpdateSndScbStatus(AppMain.gs_sound_scb_heap[index]);
        }

        for (int index = 0; index < 16; ++index)
        {
            if (((int) AppMain.gs_sound_se_handle_heap[index].flag & 1) != 0)
                AppMain.gsSoundUpdateSeHandleStatus(AppMain.gs_sound_se_handle_heap[index]);
        }

        if (((int) AppMain.gs_sound_se_handle_error.flag & 1) == 0)
            return;
        AppMain.gsSoundUpdateSeHandleStatus(AppMain.gs_sound_se_handle_error);
    }

    private static void gsSoundInitSystemMainInfo()
    {
        AppMain.gsSoundClearSystemMainInfo();
    }

    private static void gsSoundResetSystemMainInfo()
    {
        AppMain.gs_sound_sys_main_info.flag &= 4294967293U;
    }

    private static void gsSoundClearSystemMainInfo()
    {
        AppMain.gs_sound_sys_main_info.Clear();
    }

    private static void gsSoundSetEnableSystemControlVolume(bool enable)
    {
        if (enable)
            AppMain.gs_sound_sys_main_info.flag |= 1U;
        else
            AppMain.gs_sound_sys_main_info.flag &= 4294967294U;
    }

    private static bool gsSoundIsSystemControlVolumeEnabled()
    {
        return ((int) AppMain.gs_sound_sys_main_info.flag & 1) != 0;
    }

    private static void gsSoundUpdateSystemControlVolume()
    {
        AppMain.gsSoundIsSystemControlVolumeEnabled();
    }

    private static float gsSoundGetGlobalVolume()
    {
        float num = 1f;
        if (AppMain.gsSoundIsSystemControlVolumeEnabled())
            num *= AppMain.gs_sound_sys_main_info.system_cnt_vol;
        if (AppMain.gsSoundIsSystemSuspendWait())
            num = 0.0f;
        return num;
    }

    private static float gsSoundGetSndScbMuteVolume(AppMain.GSS_SND_SCB scb)
    {
        return AppMain.GsSystemBgmIsPlay() && ((int) scb.flag & int.MinValue) != 0 ? 0.0f : 1f;
    }

    private static void gsSoundInitSndScbHeap()
    {
        AppMain.gsSoundResetSndScbHeap();
    }

    private static void gsSoundResetSndScbHeap()
    {
        for (int scb_no = 0; scb_no < 8; ++scb_no)
            AppMain.gsSoundClearSndScb(AppMain.gs_sound_scb_heap[scb_no], AppMain.gsSoundGetAuplyNo(scb_no));
        Array.Clear((Array) AppMain.gs_sound_scb_heap_usage_flag, 0, AppMain.gs_sound_scb_heap_usage_flag.Length);
    }

    private static uint gsSndGetFreeScbNum()
    {
        uint num1 = 8;
        int num2 = 1;
        for (int index = 0; index < num2; ++index)
            num1 -= (uint) AppMain.AkMathCountBitPopulation((uint) AppMain.gs_sound_scb_heap_usage_flag[index]);
        return num1;
    }

    private static void gsSoundInitSndScb(AppMain.GSS_SND_SCB scb, int scb_no, int snd_data_type)
    {
        AppMain.gsSoundClearSndScb(scb, AppMain.gsSoundGetAuplyNo(scb_no));
        scb.snd_data_type = snd_data_type;
        if (snd_data_type != 1)
        {
            scb.auply_no = AppMain.gsSoundGetAuplyNo(scb_no);
            scb.snd_ctrl_param.fade_vol = 1f;
            scb.snd_ctrl_param.fade_sub_vol = 1f;
            scb.snd_ctrl_param.volume = 1f;
        }

        scb.noplay_error_state.sample = uint.MaxValue;
        scb.noplay_error_state.counter = 0U;
        scb.flag |= 1U;
    }

    private static void gsSoundClearSndScb(AppMain.GSS_SND_SCB scb, int auply_no)
    {
        AppMain.CriAuPlayer criAuPlayer = AppMain.amCriAudioGetGlobal().auply[auply_no];
        if (criAuPlayer != null)
        {
            criAuPlayer.ReleaseCue();
            criAuPlayer.ResetParameters();
        }

        scb.Clear();
    }

    private static void gsSoundUpdateSndScb(AppMain.GSS_SND_SCB scb)
    {
        if (scb.snd_data_type == 1)
            return;
        AppMain.CriAuPlayer au_player = AppMain.amCriAudioGetGlobal().auply[scb.auply_no];
        AppMain.gsSoundUpdateSndCtrl(scb.snd_ctrl_param, au_player);
        if (AppMain.gsSoundCheckSndScbStop(scb) || (6 & (int) scb.flag) != 0)
        {
            scb.noplay_error_state.sample = uint.MaxValue;
            scb.noplay_error_state.counter = 0U;
        }
        else
        {
            uint numPlayedSamples = (uint) au_player.GetNumPlayedSamples();
            if ((int) scb.noplay_error_state.sample != (int) numPlayedSamples)
            {
                scb.noplay_error_state.sample = numPlayedSamples;
                scb.noplay_error_state.counter = 0U;
            }
            else
            {
                if (90U >= scb.noplay_error_state.counter++)
                    return;
                scb.noplay_error_state.sample = uint.MaxValue;
                scb.noplay_error_state.counter = 0U;
            }
        }
    }

    private static void gsSoundUpdateSndScbStatus(AppMain.GSS_SND_SCB scb)
    {
        if (AppMain.gsSoundCheckSndScbStop(scb))
            scb.flag |= 2U;
        else
            scb.flag &= 4294967293U;
        if (AppMain.gsSoundCheckSndScbPause(scb))
            scb.flag |= 4U;
        else
            scb.flag &= 4294967291U;
    }

    private static bool gsSoundCheckSndScbPause(AppMain.GSS_SND_SCB scb)
    {
        if (scb.snd_data_type == 1)
            return false;
        AppMain.AMS_CRIAUDIO_INTERFACE global = AppMain.amCriAudioGetGlobal();
        return scb.snd_ctrl_param.fade_state == 3U || global.auply[scb.auply_no].IsPaused();
    }

    private static bool gsSoundCheckSndScbStop(AppMain.GSS_SND_SCB scb)
    {
        if (scb.snd_data_type == 1)
            return true;
        switch (AppMain.amCriAudioGetGlobal().auply[scb.auply_no].GetStatus())
        {
            case 0:
            case 3:
            case 4:
                return true;
            default:
                return false;
        }
    }

    private static int gsSoundGetAuplyNo(int scb_no)
    {
        return scb_no;
    }

    private static void gsSoundCriStrmSetFadeIn(AppMain.GSS_SND_SCB scb, int fade_frame)
    {
        if (scb.snd_data_type != 0)
            return;
        if (fade_frame == 0)
        {
            scb.snd_ctrl_param.fade_state = 0U;
            scb.snd_ctrl_param.fade_frame_max = 0;
            scb.snd_ctrl_param.fade_frame_cnt = 0;
            scb.snd_ctrl_param.fade_vol = 1f;
        }
        else
        {
            scb.snd_ctrl_param.fade_state = 1U;
            scb.snd_ctrl_param.fade_frame_max = fade_frame;
            scb.snd_ctrl_param.fade_frame_cnt = 0;
            scb.snd_ctrl_param.fade_vol = 0.0f;
        }

        scb.snd_ctrl_param.fade_sub_vol = 1f;
    }

    private static void gsSoundCriStrmStop(AppMain.GSS_SND_SCB scb, int fade_frame)
    {
        AppMain.gsSoundCriStrmStop(scb, fade_frame, false);
    }

    private static void gsSoundCriStrmStop(AppMain.GSS_SND_SCB scb, int fade_frame, bool is_takeover)
    {
        AppMain.AMS_CRIAUDIO_INTERFACE global = AppMain.amCriAudioGetGlobal();
        if (scb.snd_data_type != 0)
            return;
        if (is_takeover)
            scb.snd_ctrl_param.fade_sub_vol *= scb.snd_ctrl_param.fade_vol;
        else
            scb.snd_ctrl_param.fade_sub_vol = 1f;
        if (fade_frame == 0)
        {
            scb.snd_ctrl_param.fade_state = 0U;
            scb.snd_ctrl_param.fade_frame_max = scb.snd_ctrl_param.fade_frame_cnt = 0;
            scb.snd_ctrl_param.fade_vol = 0.0f;
            global.auply[scb.auply_no].Stop();
        }
        else
        {
            scb.snd_ctrl_param.fade_state = 2U;
            scb.snd_ctrl_param.fade_frame_max = fade_frame;
            scb.snd_ctrl_param.fade_frame_cnt = 0;
            scb.snd_ctrl_param.fade_vol = 1f;
        }
    }

    private static void gsSoundCriStrmPause(AppMain.GSS_SND_SCB scb, int fade_frame)
    {
        AppMain.AMS_CRIAUDIO_INTERFACE global = AppMain.amCriAudioGetGlobal();
        if (scb.snd_data_type != 0)
            return;
        if (scb.snd_ctrl_param.fade_state == 2U)
            AppMain.gsSoundCriStrmStop(scb, fade_frame, true);
        else if (fade_frame == 0)
        {
            scb.snd_ctrl_param.fade_state = 0U;
            scb.snd_ctrl_param.fade_frame_max = scb.snd_ctrl_param.fade_frame_cnt = 0;
            scb.snd_ctrl_param.fade_vol = 0.0f;
            global.auply[scb.auply_no].Pause(true);
        }
        else
        {
            scb.snd_ctrl_param.fade_state = 3U;
            scb.snd_ctrl_param.fade_frame_max = fade_frame;
            scb.snd_ctrl_param.fade_frame_cnt = 0;
            scb.snd_ctrl_param.fade_vol = 1f;
        }
    }

    private static void gsSoundCriStrmResume(AppMain.GSS_SND_SCB scb, int fade_frame)
    {
        AppMain.AMS_CRIAUDIO_INTERFACE global = AppMain.amCriAudioGetGlobal();
        if (scb.snd_data_type != 0 || !AppMain.gsSoundCheckSndScbPause(scb))
            return;
        global.auply[scb.auply_no].Pause(false);
        if (fade_frame == 0)
        {
            scb.snd_ctrl_param.fade_state = 0U;
            scb.snd_ctrl_param.fade_frame_max = scb.snd_ctrl_param.fade_frame_cnt = 0;
            scb.snd_ctrl_param.fade_vol = 1f;
        }
        else
            AppMain.gsSoundCriStrmSetFadeIn(scb, fade_frame);
    }

    private static void gsSoundUpdateSndCtrl(
        AppMain.GSS_SND_CTRL_PARAM snd_ctrl_param,
        AppMain.CriAuPlayer au_player)
    {
        AppMain.amCriAudioGetGlobal();
        if (snd_ctrl_param.fade_state == 1U || snd_ctrl_param.fade_state == 2U || snd_ctrl_param.fade_state == 3U)
        {
            if (snd_ctrl_param.fade_frame_max <= snd_ctrl_param.fade_frame_cnt)
            {
                if (snd_ctrl_param.fade_state == 1U)
                {
                    snd_ctrl_param.fade_vol = 1f;
                }
                else
                {
                    if (snd_ctrl_param.fade_state == 3U)
                        au_player.Pause(true);
                    else
                        au_player.Stop(1);
                    snd_ctrl_param.fade_vol = 0.0f;
                }

                snd_ctrl_param.fade_state = 0U;
                snd_ctrl_param.fade_frame_max = snd_ctrl_param.fade_frame_cnt = 0;
            }
            else
            {
                switch (au_player.GetStatus())
                {
                    case 2:
                    case 3:
                        if (au_player.IsPaused())
                            break;
                        float num = snd_ctrl_param.fade_frame_max != 0
                            ? (float) (1.0 * ((double) snd_ctrl_param.fade_frame_cnt /
                                              (double) snd_ctrl_param.fade_frame_max))
                            : 1f;
                        snd_ctrl_param.fade_vol = snd_ctrl_param.fade_state != 1U ? 1f - num : num;
                        ++snd_ctrl_param.fade_frame_cnt;
                        break;
                    default:
                        snd_ctrl_param.fade_vol = 0.0f;
                        break;
                }
            }
        }
        else if (au_player.IsPaused())
            snd_ctrl_param.fade_vol = 0.0f;
        else
            snd_ctrl_param.fade_vol = 1f;
    }

    private static void gsSoundSeHandleUpdateVolume(AppMain.GSS_SND_SE_HANDLE se_handle)
    {
        if (se_handle.au_player == null)
            return;
        se_handle.au_player.SetVolume(se_handle.snd_ctrl_param.volume * se_handle.snd_ctrl_param.fade_vol *
                                      se_handle.snd_ctrl_param.fade_sub_vol * AppMain.gsSoundGetGlobalVolume());
        se_handle.au_player.SetPitch(se_handle.snd_ctrl_param.pitch);
    }

    private static void gsSoundUpdateVolume()
    {
        AppMain.AMS_CRIAUDIO_INTERFACE global = AppMain.amCriAudioGetGlobal();
        for (int index = 0; index < 8; ++index)
        {
            if (((int) AppMain.gs_sound_scb_heap[index].flag & 1) != 0 &&
                AppMain.gs_sound_scb_heap[index].snd_data_type == 0)
            {
                int status = global.auply[AppMain.gs_sound_scb_heap[index].auply_no].GetStatus();
                if (status != 0 && 3 != status)
                    global.auply[AppMain.gs_sound_scb_heap[index].auply_no].SetVolume(
                        AppMain.gs_sound_volume[0] * AppMain.gs_sound_scb_heap[index].snd_ctrl_param.volume *
                        AppMain.gs_sound_scb_heap[index].snd_ctrl_param.fade_vol *
                        AppMain.gs_sound_scb_heap[index].snd_ctrl_param.fade_sub_vol *
                        AppMain.gsSoundGetGlobalVolume() *
                        AppMain.gsSoundGetSndScbMuteVolume(AppMain.gs_sound_scb_heap[index]));
            }
        }

        for (int index = 0; index < 16; ++index)
        {
            if (((int) AppMain.gs_sound_se_handle_heap[index].flag & 1) != 0 &&
                AppMain.gsSoundIsSeHandleCueSet(AppMain.gs_sound_se_handle_heap[index]))
                AppMain.gsSoundSeHandleUpdateVolume(AppMain.gs_sound_se_handle_heap[index]);
        }

        if (((int) AppMain.gs_sound_se_handle_error.flag & 1) == 0 ||
            !AppMain.gsSoundIsSeHandleCueSet(AppMain.gs_sound_se_handle_error))
            return;
        AppMain.gsSoundSeHandleUpdateVolume(AppMain.gs_sound_se_handle_error);
    }

    private static void gsSoundInitSeHandleHeap()
    {
        AppMain.gsSoundResetSeHandleHeap();
    }

    private static void gsSoundResetSeHandleHeap()
    {
        AppMain.AMS_CRIAUDIO_INTERFACE global = AppMain.amCriAudioGetGlobal();
        AppMain.gsSoundClearSeHandleHeap();
        for (int index = 0; index < 16; ++index)
            AppMain.gs_sound_se_handle_heap[index].au_player = AppMain.CriAuPlayer.Create(global);
        AppMain.gs_sound_se_handle_error.au_player = AppMain.CriAuPlayer.Create(global);
        AppMain.gs_sound_se_handle_default = AppMain.New<AppMain.GSS_SND_SE_HANDLE>(3);
        for (int index = 0; index < AppMain.gs_sound_se_handle_default.Length; ++index)
            AppMain.gs_sound_se_handle_default[index] = AppMain.GsSoundAllocSeHandle();
    }

    private static void gsSoundClearSeHandleHeap()
    {
        AppMain.amCriAudioGetGlobal();
        if (AppMain.gs_sound_se_handle_default != null)
        {
            for (int index = 0; index < AppMain.gs_sound_se_handle_default.Length; ++index)
                AppMain.GsSoundFreeSeHandle(AppMain.gs_sound_se_handle_default[index]);
            AppMain.gs_sound_se_handle_default = (AppMain.GSS_SND_SE_HANDLE[]) null;
        }

        for (int index = 0; index < 16; ++index)
        {
            AppMain.gsSoundClearSeHandle(AppMain.gs_sound_se_handle_heap[index]);
            if (AppMain.gs_sound_se_handle_heap[index].au_player != null)
            {
                AppMain.gs_sound_se_handle_heap[index].au_player.Destroy();
                AppMain.gs_sound_se_handle_heap[index].au_player = (AppMain.CriAuPlayer) null;
            }
        }

        AppMain.gsSoundClearSeHandle(AppMain.gs_sound_se_handle_error);
        if (AppMain.gs_sound_se_handle_error.au_player != null)
        {
            AppMain.gs_sound_se_handle_error.au_player.Destroy();
            AppMain.gs_sound_se_handle_error.au_player = (AppMain.CriAuPlayer) null;
        }

        Array.Clear((Array) AppMain.gs_sound_se_handle_heap_usage_flag, 0,
            AppMain.gs_sound_se_handle_heap_usage_flag.Length);
    }

    private static uint gsSoundGetFreeSeHandleNum()
    {
        uint num1 = 16;
        int num2 = 2;
        for (int index = 0; index < num2; ++index)
            num1 -= (uint) AppMain.AkMathCountBitPopulation((uint) AppMain.gs_sound_se_handle_heap_usage_flag[index]);
        return num1;
    }

    private static void gsSoundInitSeHandle(AppMain.GSS_SND_SE_HANDLE se_handle)
    {
        AppMain.gsSoundInitSeHandle(se_handle, false);
    }

    private static void gsSoundInitSeHandle(AppMain.GSS_SND_SE_HANDLE se_handle, bool b_reset)
    {
        AppMain.amCriAudioGetGlobal();
        if (se_handle.au_player != null && 1 == se_handle.au_player.GetStatus())
            se_handle.au_player.Update();
        AppMain.gsSoundClearSeHandle(se_handle, b_reset);
        se_handle.flag |= 1U;
        se_handle.snd_ctrl_param.fade_vol = 1f;
        se_handle.snd_ctrl_param.fade_sub_vol = 1f;
        se_handle.snd_ctrl_param.volume = 1f;
    }

    private static void gsSoundClearSeHandle(AppMain.GSS_SND_SE_HANDLE se_handle)
    {
        AppMain.gsSoundClearSeHandle(se_handle, false);
    }

    private static void gsSoundClearSeHandle(AppMain.GSS_SND_SE_HANDLE se_handle, bool b_takeover_cue)
    {
        AppMain.amCriAudioGetGlobal();
        se_handle.flag = 0U;
        se_handle.snd_ctrl_param.fade_vol = 0.0f;
        se_handle.snd_ctrl_param.fade_sub_vol = 0.0f;
        se_handle.snd_ctrl_param.volume = 0.0f;
        se_handle.cur_pause_level = 0U;
        if (se_handle.au_player == null)
            return;
        if (!b_takeover_cue)
            se_handle.au_player.ReleaseCue();
        se_handle.au_player.ResetParameters();
    }

    private static void gsSoundUpdateSndSeHandle(AppMain.GSS_SND_SE_HANDLE se_handle)
    {
        AppMain.gsSoundUpdateSndCtrl(se_handle.snd_ctrl_param, se_handle.au_player);
    }

    private static void gsSoundUpdateSeHandleStatus(AppMain.GSS_SND_SE_HANDLE se_handle)
    {
        if (AppMain.gsSoundCheckSeHandleStop(se_handle))
            se_handle.flag |= 4U;
        else
            se_handle.flag &= 4294967291U;
        if (AppMain.gsSoundCheckSeHandlePause(se_handle))
            se_handle.flag |= 8U;
        else
            se_handle.flag &= 4294967287U;
    }

    private static bool gsSoundCheckSeHandlePause(AppMain.GSS_SND_SE_HANDLE se_handle)
    {
        AppMain.amCriAudioGetGlobal();
        return se_handle.au_player != null &&
               (se_handle.snd_ctrl_param.fade_state == 3U || se_handle.au_player.IsPaused());
    }

    private static bool gsSoundCheckSeHandleStop(AppMain.GSS_SND_SE_HANDLE se_handle)
    {
        AppMain.amCriAudioGetGlobal();
        if (se_handle.au_player == null)
            return true;
        switch (se_handle.au_player.GetStatus())
        {
            case 0:
            case 3:
            case 4:
                return true;
            default:
                return false;
        }
    }

    private static bool gsSoundIsSeHandleCueSet(AppMain.GSS_SND_SE_HANDLE se_handle)
    {
        AppMain.amCriAudioGetGlobal();
        if (se_handle.au_player == null)
            return false;
        switch (se_handle.au_player.GetStatus())
        {
            case 0:
            case 3:
                return false;
            default:
                return true;
        }
    }

    private static AppMain.GSS_SND_SE_HANDLE gsSoundGetDefaultSeHandle()
    {
        for (int index = 0; index < AppMain.gs_sound_se_handle_default.Length; ++index)
        {
            if (AppMain.gs_sound_se_handle_default[index].au_player.sound == null ||
                AppMain.gs_sound_se_handle_default[index].au_player.sound[0] == null ||
                AppMain.gs_sound_se_handle_default[index].au_player.sound[0].State == SoundState.Stopped)
                return AppMain.gs_sound_se_handle_default[index];
        }

        return AppMain.gs_sound_se_handle_default[0];
    }

    private static void gsSoundCriSeSetFadeIn(AppMain.GSS_SND_SE_HANDLE se_handle, int fade_frame)
    {
        if (fade_frame == 0)
        {
            se_handle.snd_ctrl_param.fade_state = 0U;
            se_handle.snd_ctrl_param.fade_frame_max = 0;
            se_handle.snd_ctrl_param.fade_frame_cnt = 0;
            se_handle.snd_ctrl_param.fade_vol = 1f;
        }
        else
        {
            se_handle.snd_ctrl_param.fade_state = 1U;
            se_handle.snd_ctrl_param.fade_frame_max = fade_frame;
            se_handle.snd_ctrl_param.fade_frame_cnt = 0;
            se_handle.snd_ctrl_param.fade_vol = 0.0f;
        }

        se_handle.snd_ctrl_param.fade_sub_vol = 1f;
    }

    private static void gsSoundCriSeStop(
        AppMain.GSS_SND_SE_HANDLE se_handle,
        int fade_frame,
        bool is_immediate)
    {
        AppMain.gsSoundCriSeStop(se_handle, fade_frame, is_immediate, false);
    }

    private static void gsSoundCriSeStop(
        AppMain.GSS_SND_SE_HANDLE se_handle,
        int fade_frame,
        bool is_immediate,
        bool is_takeover)
    {
        AppMain.amCriAudioGetGlobal();
        if (((int) se_handle.flag & 1) == 0 || se_handle.au_player == null)
            return;
        if (is_takeover)
            se_handle.snd_ctrl_param.fade_sub_vol *= se_handle.snd_ctrl_param.fade_vol;
        else
            se_handle.snd_ctrl_param.fade_sub_vol = 1f;
        if (fade_frame == 0)
        {
            se_handle.snd_ctrl_param.fade_state = 0U;
            se_handle.snd_ctrl_param.fade_frame_max = se_handle.snd_ctrl_param.fade_frame_cnt = 0;
            se_handle.snd_ctrl_param.fade_vol = 0.0f;
            if (is_immediate)
                se_handle.au_player.Stop(1);
            else
                se_handle.au_player.Stop();
        }
        else
        {
            se_handle.snd_ctrl_param.fade_state = 2U;
            se_handle.snd_ctrl_param.fade_frame_max = fade_frame;
            se_handle.snd_ctrl_param.fade_frame_cnt = 0;
            se_handle.snd_ctrl_param.fade_vol = 1f;
        }
    }

    private static void gsSoundCriSePause(AppMain.GSS_SND_SE_HANDLE se_handle, int fade_frame)
    {
        AppMain.amCriAudioGetGlobal();
        if (se_handle.snd_ctrl_param.fade_state == 2U)
            AppMain.gsSoundCriSeStop(se_handle, fade_frame, true, true);
        else if (fade_frame == 0)
        {
            se_handle.snd_ctrl_param.fade_state = 0U;
            se_handle.snd_ctrl_param.fade_frame_max = se_handle.snd_ctrl_param.fade_frame_cnt = 0;
            se_handle.snd_ctrl_param.fade_vol = 0.0f;
            se_handle.au_player.Pause(true);
        }
        else
        {
            se_handle.snd_ctrl_param.fade_state = 3U;
            se_handle.snd_ctrl_param.fade_frame_max = fade_frame;
            se_handle.snd_ctrl_param.fade_frame_cnt = 0;
            se_handle.snd_ctrl_param.fade_vol = 1f;
        }
    }

    private static void gsSoundCriSeResume(AppMain.GSS_SND_SE_HANDLE se_handle, int fade_frame)
    {
        AppMain.amCriAudioGetGlobal();
        if (!AppMain.gsSoundCheckSeHandlePause(se_handle))
            return;
        se_handle.au_player.Pause(false);
        if (fade_frame == 0)
        {
            se_handle.snd_ctrl_param.fade_state = 0U;
            se_handle.snd_ctrl_param.fade_frame_max = se_handle.snd_ctrl_param.fade_frame_cnt = 0;
            se_handle.snd_ctrl_param.fade_vol = 1f;
        }
        else
            AppMain.gsSoundCriSeSetFadeIn(se_handle, fade_frame);
    }

    private static void gsSoundStopSe(AppMain.GSS_SND_SE_HANDLE se_handle)
    {
        se_handle.au_player.Pause(true);
        se_handle.au_player.Stop(0);
    }

    private static void gsSoundPlaySe(
        string se_name,
        uint se_id,
        AppMain.GSS_SND_SE_HANDLE se_handle,
        int fade_frame)
    {
        AppMain.gsSoundPlaySe(se_name, se_id, se_handle, fade_frame, false);
    }

    private static void gsSoundPlaySe(
        string se_name,
        uint se_id,
        AppMain.GSS_SND_SE_HANDLE se_handle,
        int fade_frame,
        bool bDontPlay)
    {
        AppMain.amCriAudioGetGlobal();
        if (se_handle == null)
            se_handle = AppMain.gsSoundGetDefaultSeHandle();
        if (se_handle.au_player.IsPaused())
            se_handle.au_player.Stop(1);
        if (((int) se_handle.flag & int.MinValue) != 0)
        {
            AppMain.gsSoundInitSeHandle(se_handle, true);
            se_handle.flag |= 2147483648U;
        }
        else
            AppMain.gsSoundInitSeHandle(se_handle, true);

        se_handle.flag |= 2U;
        if (se_name == null)
            se_name = AppMain.CriAuPlayer.GetCueName(se_id);
        se_handle.au_player.SetCue(se_name);
        AppMain.gsSoundCriSeSetFadeIn(se_handle, fade_frame);
        AppMain.gsSoundSeHandleUpdateVolume(se_handle);
        if (bDontPlay)
            return;
        se_handle.au_player.Play();
    }

    private static bool gsSoundIsSystemSuspendWait()
    {
        bool flag = false;
        if (AppMain.GsSoundGetSysMainInfo().suspend_wait_count > 0)
            flag = true;
        return flag;
    }

    private static void gsSoundUpdateSystemSuspendWait()
    {
        AppMain.GSS_SND_SYS_MAIN_INFO sysMainInfo = AppMain.GsSoundGetSysMainInfo();
        if (sysMainInfo.suspend_wait_count > 0)
            --sysMainInfo.suspend_wait_count;
        if (!AppMain.GsMainSysGetSuspendedFlag())
            return;
        sysMainInfo.suspend_wait_count = 8;
    }

    private static void GsSoundPlaySeById(uint se_id)
    {
        AppMain.gsSoundPlaySe((string) null, se_id, (AppMain.GSS_SND_SE_HANDLE) null, 0);
    }
}