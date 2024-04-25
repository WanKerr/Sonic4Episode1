using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using gs.backup;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

#if WASM
using MediaPlayer = Microsoft.Xna.Framework.Media.WasmMediaPlayer;
#endif

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

    public class GSS_SND_CTRL_PARAM : IClearable
    {
        public uint fade_state;
        public int fade_frame_max;
        public int fade_frame_cnt;
        public float fade_vol;
        public float fade_sub_vol;
        public float volume;
        public float pitch;
        public float pan;

        public void Clear()
        {
            this.fade_state = 0U;
            this.fade_frame_max = this.fade_frame_cnt = 0;
            this.pitch = this.fade_vol = this.fade_sub_vol = this.volume = this.pan = 0.0f;
        }
    }

    public class GSS_SND_SCB
    {
        public readonly GSS_SND_CTRL_PARAM snd_ctrl_param = new GSS_SND_CTRL_PARAM();
        public uint flag;
        public int snd_data_type;
        public int auply_no;
        public uint cur_pause_level;
        public error_state noplay_error_state;

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
        private AISAC_LIST[] aisac_list;
        private int effectscount;

        public float pan;

        public static Song m_songBGMIntro;
        public static Song m_songBGMLoop;

        public CriAuPlayer()
        {
            this.status = STATUS_STOP;
            this.cue = -1;
        }

        public static uint GetCueId(string name)
        {
            //return (uint)sound_fx_list[name].cue;

            if (sound_fx_list.TryGetValue(name, out var cue)) return (uint)cue.cue;

            return unchecked((uint)-1);
        }

        public static string GetCueName(uint id)
        {
            foreach (KeyValuePair<string, SOUND_TABLE> soundFx in sound_fx_list)
            {
                if (soundFx.Value.cue == id)
                    return soundFx.Key;
            }

            return null;
        }

        public void SetPitch(float val)
        {
            this.pitch = val;
        }

        public void SetAisac(string s, float val)
        {
            if (this.type != 0)
                return;
            this.Pause(val < 0.1);
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

                this.status = STATUS_STOP;
                this.status_paused = false;
            }
            else
            {
                this.m_stGMState = MediaState.Stopped;
                if (this.se_name != null && this.se_name == m_ActiveSong)
                    MediaPlayer.Stop();
                this.status = STATUS_STOP;
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
                    if (this.status == STATUS_PLAYING && !this.status_paused)
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
                            if (this.status != STATUS_PLAYING || this.sound[index].State != SoundState.Stopped)
                            {
                                flag = false;
                                break;
                            }
                        }
                    }
                }

                if (flag)
                    this.status = STATUS_PLAYEND;
            }

            if (this.type != 1)
                return;
            if (m_ActiveSong == this.se_name &&
                m_fBGVolume != (double)this.volume[0])
            {
                MediaPlayer.Volume = this.volume[0];
                this.m_fBGVolume = this.volume[0];
            }

            if (this.status != STATUS_PLAYING || this.m_stGMState != MediaState.Stopped)
                flag = false;
            if (!flag)
                return;
            this.status = STATUS_PLAYEND;
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
                if (this.se_name != m_ActiveSong)
                    return;
                MediaPlayer.Pause();
            }
            else if (m_ActiveSong != this.se_name)
            {
                string seName = this.se_name;
                this.se_name = null;
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
                        this.sound[index].Volume = this.volume[index] * volume;
                    }
                }
            }
            else
            {
                if (m_ActiveSong != this.se_name || m_fBGVolume == (double)volume)
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
            byteArray = null;
            if (!bgMusic && cacheFxSounds.ContainsKey(fileName))
                return 0;
            int num1 = 0;

            System.Diagnostics.Debug.Assert(!bgMusic);

            var effect = Sonic4Ep1.pInstance.Content.Load<SoundEffect>("SOUND\\" + fileName);
            cacheFxSounds.Add(fileName, effect);

            //using (Stream stream = TitleContainer.OpenStream("Content\\SOUND\\" + fileName + ".xnb"))
            //{
            //    using (BinaryReader binaryReader = new BinaryReader(stream))
            //    {
            //        if (!bgMusic)
            //        {
            //            SoundEffect soundEffect = SoundEffect.FromStream(stream);
            //            AppMain.cacheFxSounds.Add(fileName, soundEffect);
            //        }
            //        else
            //        {
            //            binaryReader.ReadInt32();
            //            binaryReader.ReadInt32();
            //            binaryReader.ReadInt32();
            //            binaryReader.ReadInt32();
            //            int num2 = binaryReader.ReadInt32();
            //            int num3 = (int) binaryReader.ReadInt16();
            //            channels = (int) binaryReader.ReadInt16();
            //            sampleRate = binaryReader.ReadInt32();
            //            binaryReader.ReadInt32();
            //            int num4 = (int) binaryReader.ReadInt16();
            //            int num5 = (int) binaryReader.ReadInt16();
            //            if (num2 == 18)
            //            {
            //                int count = (int) binaryReader.ReadInt16();
            //                binaryReader.ReadBytes(count);
            //            }

            //            binaryReader.ReadInt32();
            //            int count1 = binaryReader.ReadInt32();
            //            byteArray = binaryReader.ReadBytes(count1);
            //            num1 = count1;
            //            if (loop && loopEnd == 0)
            //                loopEnd = byteArray.Length;
            //            loopStart += loopStart % num4;
            //            loopEnd -= loopEnd % num4;
            //        }
            //    }
            //}

            return num1;
        }

        internal void SetCue(string se_name)
        {
            try
            {
                for (; this.se_name != null; this.se_name = null)
                {
                    if (this.se_name == se_name)
                    {
                        this.Stop();
                        this.status = STATUS_PREP;
                        // return;
                    }
                }

                SOUND_TABLE soundTable = null;
                if (sound_fx_list.TryGetValue(se_name, out soundTable))
                {
                    this.cue = -1;
                    this.type = 0;
                }
                else if (sound_bgm_list.TryGetValue(se_name, out soundTable))
                {
                    this.type = 1;
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
                        // play sound effect

                        this.activefx = 0;
                        this.status_paused = false;
                        for (int index = 0; index < this.effectscount; ++index)
                        {
                            if (cacheFxSounds.TryGetValue(soundTable.filename[index], out var cacheFxSound))
                            {
                                this.m_sndEffect = cacheFxSound;
                                if (soundTable.loop[index])
                                {
                                    var instance = cacheFxSound.CreateInstance();
                                    this.sound[index] = instance;
                                    this.volume[index] = soundTable.volume[index];
                                    instance.IsLooped = soundTable.loop[index];
                                    instance.Volume = soundTable.volume[index];
                                    instance.Pitch = this.pitch;
                                    instance.Pan = this.pan;
                                }
                                else
                                {
                                    this.sound[index] = cacheFxSound.CreateInstance();
                                    this.sound[index].Volume = soundTable.volume[index];
                                    this.sound[index].Pitch = this.pitch;
                                    this.sound[index].Pan = this.pan;
                                    this.volume[index] = soundTable.volume[index];
                                }
                            }
                            else
                                mppAssertNotImpl();
                        }
                    }
                    else
                    {
                        m_songBGMIntro = gsSoundGetPreloadedBGM(soundTable.filename[0] + "Intro");
                        if (m_songBGMIntro == null)
                        {
                            m_songBGMIntro = Sonic4Ep1.pInstance.Content.Load<Song>("SOUND\\" + soundTable.filename[0] + "\\Intro");
                            bgmPreloadedList.Add(soundTable.filename[0] + "Intro", m_songBGMIntro);

#if WASM
                            MediaPlayer.LoadSong(m_songBGMIntro);
#endif
                        }

                        //Console.WriteLine(m_songBGMIntro);

                        if (soundTable.loop[0])
                        {
                            m_songBGMLoop = gsSoundGetPreloadedBGM(soundTable.filename[0] + "Loop");
                            if (m_songBGMLoop == null)
                            {
                                m_songBGMLoop = Sonic4Ep1.pInstance.Content.Load<Song>("SOUND\\" + soundTable.filename[0] + "\\Loop");
                                bgmPreloadedList.Add(soundTable.filename[0] + "Loop", m_songBGMLoop);
                            }

                            //Console.WriteLine(m_songBGMLoop);
                        }
                        else
                        {
                            m_songBGMLoop = null;
                        }

                        m_ActiveSong = se_name;
                        this.m_fBGVolume = -1f;
                        this.m_bLoop = soundTable.loop[0];
                    }

                    this.status = STATUS_PREP;
                }
                else
                    this.status = STATUS_ERROR;
            }
            catch (Exception ex)
            {
                ex.ToString();
                this.status = STATUS_ERROR;
            }
        }

        internal void Play()
        {
            if (this.type == 1)
            {
                if (this.se_name != m_ActiveSong)
                {
                    string seName = this.se_name;
                    this.se_name = null;
                    this.SetCue(seName);
                }

                if (this.se_name != null)
                {
                    if (this.se_name == m_ActiveSong)
                    {
                        try
                        {
#if WASM
                            MediaPlayer.Stop();
                            MediaPlayer.IsRepeating = false;
                            MediaPlayer.Play(m_songBGMIntro, m_songBGMLoop);
#else
                            MediaPlayer.Stop();
                            MediaPlayer.IsRepeating = false;
                            MediaPlayer.Play(m_songBGMIntro);

#if FNA
                            if (m_songBGMLoop != null)
                                MediaPlayer.LoadSong(m_songBGMLoop);
#endif
#endif
                            this.m_stGMState = MediaState.Playing;
                        }
                        catch (UnauthorizedAccessException ext)
                        {
                            g_ao_sys_global.is_playing_device_bgm_music = true;
                        }
                    }
                }

                this.status = STATUS_PLAYING;
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
                            //this.sound[index].Volume = this.volume[index];
                            System.Diagnostics.Debug.WriteLine(this.sound[index].Volume);

                            if (this.sound[index].State == SoundState.Paused)
                                this.sound[index].Resume();
                            else
                                this.sound[index].Play();
                        }
                        else if (this.m_sndEffect != null)
                            this.m_sndEffect.Play();
                    }
                }

                this.status = STATUS_PLAYING;
            }
        }

        internal void ReleaseCue()
        {
            this.se_name = null;
            this.aisac = null;
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
                        this.sound[index] = null;
                    }
                }

                this.cue = -1;
                this.status = STATUS_STOP;
            }
            else
            {
                this.m_stGMState = MediaState.Stopped;
                if (this.se_name != null && this.se_name == m_ActiveSong)
                    MediaPlayer.Stop();
                m_ActiveSong = null;
                this.status = STATUS_STOP;
            }

            this.oldAisacParam = -1000f;
        }

        internal void ResetParameters()
        {
            for (int index = 0; index < this.effectscount; ++index)
            {
                this.volume[index] = 1f;
                this.aisac = null;
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

        internal static CriAuPlayer Create(AMS_CRIAUDIO_INTERFACE cri_if)
        {
            return new CriAuPlayer();
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
        public AISAC_LIST[] asiac;

        public SOUND_TABLE(int count)
        {
            this.count = count;
            this.volume = new float[count];
            this.filename = new string[count];
            this.loop = new bool[count];
            this.loopStart = new int[count];
            this.loopEnd = new int[count];
            this.pitch = new float[count];
            this.asiac = new AISAC_LIST[count];
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
        public readonly CriAuPlayer[] auply;

        public AMS_CRIAUDIO_INTERFACE()
        {
            this.auply = New<CriAuPlayer>(8);
            for (int index = 0; index < 8; ++index)
                this.auply[index].type = 1;
        }
    }

    private static bool gsSoundFillBGMCache(int iListIndex)
    {
        foreach (string index in bgmLists[iListIndex])
        {
            SOUND_TABLE soundBgm = sound_bgm_list[index];
            if (soundBgm.loop[0])
            {
                var intro = Sonic4Ep1.pInstance.Content.Load<Song>($"SOUND\\{soundBgm.filename[0]}\\Intro");
                var loop = Sonic4Ep1.pInstance.Content.Load<Song>($"SOUND\\{soundBgm.filename[0]}\\Loop");
                bgmPreloadedList.Add(soundBgm.filename[0] + "Intro", intro);
                bgmPreloadedList.Add(soundBgm.filename[0] + "Loop", loop);
            }
            else
            {
                Song song = Sonic4Ep1.pInstance.Content.Load<Song>("SOUND\\" + soundBgm.filename[0] + "\\Intro");
                bgmPreloadedList.Add(soundBgm.filename[0] + "Intro", song);
            }
        }

        return true;
    }

    private static Song gsSoundGetPreloadedBGM(string sName)
    {
        Song song;
        return !bgmPreloadedList.TryGetValue(sName, out song) ? null : song;
    }

    public static bool GsSoundPrepareBGMForLevel(int iLevel)
    {
        if (m_iBGMPreparedLevel == iLevel)
            return true;
        bgmPreloadedList.Clear();
        gsSoundFillBGMCache(0);
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
            gsSoundFillBGMCache(iListIndex);
        m_iBGMPreparedLevel = iLevel;
        return true;
    }

    private static bool GsSoundIsBgmStop(GSS_SND_SCB scb)
    {
        return ((int)scb.flag & 1) == 0 || ((int)scb.flag & 2) != 0;
    }

    private static bool GsSoundIsBgmPause(GSS_SND_SCB scb)
    {
        return ((int)scb.flag & 1) != 0 && scb.cur_pause_level == int.MaxValue && ((int)scb.flag & 4) != 0;
    }

    private static void gsSoundFillSoundTable(string filename, Dictionary<string, SOUND_TABLE> list)
    {
        using (Stream stream = TitleContainer.OpenStream("Content\\SOUND\\" + filename))
        {
            using (StreamReader streamReader = new StreamReader(stream))
            {
                while (streamReader.Peek() >= 0)
                {
                    string[] strArray1 = streamReader.ReadLine().Split('|');
                    SOUND_TABLE soundTable = new SOUND_TABLE(strArray1.Length);
                    for (int index1 = 0; index1 < strArray1.Length; ++index1)
                    {
                        int startIndex = strArray1[index1].IndexOf("Aisac");
                        if (startIndex != -1)
                        {
                            string str = strArray1[index1].Substring(startIndex);
                            strArray1[index1] = strArray1[index1].Substring(0, startIndex - 1);
                            string[] strArray2 = str.Split('#');
                            soundTable.asiac[index1] = new AISAC_LIST(strArray2.Length - 1);
                            for (int index2 = 1; index2 < strArray2.Length; ++index2)
                            {
                                string[] strArray3 = null;
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
                                        CultureInfo.InvariantCulture);
                                    soundTable.asiac[index1].values[index2 - 1][index3][1] =
                                        soundTable.asiac[index1].types[index2 - 1] == 1
                                            ? float.Parse(strArray4[1],
                                                CultureInfo.InvariantCulture) / 1000f
                                            : float.Parse(strArray4[1], CultureInfo.InvariantCulture);
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
                            CultureInfo.InvariantCulture);
#if !FNA
                        if (soundTable.volume[index1] > 1.0)
                            soundTable.volume[index1] = 1f;
#endif
                        soundTable.pitch[index1] = int.Parse(strArray5[index1 == 0 ? 4 : 1]) / 1000f;
                        soundTable.filename[index1] = strArray5[index1 == 0 ? 5 : 2];
                        soundTable.loop[index1] = int.Parse(strArray5[index1 == 0 ? 6 : 3]) == 1;
                        soundTable.loopStart[index1] = int.Parse(strArray5[index1 == 0 ? 7 : 4]);
                        soundTable.loopEnd[index1] = int.Parse(strArray5[index1 == 0 ? 8 : 5]);
                    }


                    //Console.WriteLine(soundTable.name);
                    list[soundTable.name] = soundTable;
                }
            }
        }
    }

    public static void MediaPlayer_ActiveSongChanged(object sender, EventArgs e)
    {
#if FNA
        MediaPlayer.IsRepeating = MediaPlayer.Queue.ActiveSongIndex == 1;
#endif
    }

    public static void MediaPlayer_MediaStateChanged(object sender, EventArgs e)
    {
        if (g_ao_sys_global.is_playing_device_bgm_music)
            return;

        MediaState state = MediaPlayer.State;
        AMS_CRIAUDIO_INTERFACE global = amCriAudioGetGlobal();
        string str = MediaPlayer.Queue.ActiveSong?.Name;
        for (int index = 0; index < global.auply.Length; ++index)
        {
            var auply = global.auply[index];

            //Console.WriteLine(CriAuPlayer.m_ActiveSong);
            if (auply.type == CriAuPlayer.TYPE_BGM && auply.se_name == CriAuPlayer.m_ActiveSong) // TYPE_BGM
            {
                if (MediaPlayer.State != MediaState.Playing && auply.status == CriAuPlayer.STATUS_PLAYING) // STATUS_PLAYING 
                {
                    auply.m_stGMState = state;
                    auply.status = CriAuPlayer.STATUS_PLAYEND; // STATUS_PLAYEND
                }
            }

#if !FNA
            if (state != MediaState.Playing && 
                auply.m_stGMState != MediaState.Stopped && 
                auply.m_bLoop && CriAuPlayer.m_songBGMLoop != null && 
                str != CriAuPlayer.m_songBGMLoop?.Name)
            {
                MediaPlayer.Play(CriAuPlayer.m_songBGMLoop);
                MediaPlayer.IsRepeating = true;
            }
#endif

            //if (str != null && auply.se_name != null && (str == CriAuPlayer.m_songBGMIntro.Name || str == CriAuPlayer.m_songBGMLoop?.Name))
            //{
            //    double songDiuration = AppMain.CriAuPlayer.m_songBGMIntro.Duration.TotalMilliseconds;
            //    double playPosition = MediaPlayer.PlayPosition.TotalMilliseconds;
            //    if (state == MediaState.Paused && auply.m_stGMState != MediaState.Paused)
            //    {
            //        if (playPosition + 1.0 >= songDiuration || playPosition == 0.0)
            //        {
            //            if (!auply.m_bLoop && auply.se_name.IndexOf("_speedup") != -1)
            //            {
            //                string se_name = auply.se_name.Replace("_speedup", "");
            //                auply.SetCue(se_name);
            //                auply.Play();
            //                break;
            //            }

            //            auply.m_stGMState = MediaState.Stopped;
            //            auply.status = 3;
            //            break;
            //        }

            //        if (auply.m_stGMState != MediaState.Playing)
            //            break;

            //        MediaPlayer.Resume();
            //        break;
            //    }

            //    if (playPosition == 0.0 && !auply.m_bLoop && auply.se_name.IndexOf("_speedup") != -1)
            //    {
            //        string se_name = auply.se_name.Replace("_speedup", "");
            //        auply.SetCue(se_name);
            //        auply.Play();
            //        break;
            //    }

            //    auply.m_stGMState = state;
            //    auply.status = 3;
            //    break;
            //}
        }
    }

    private static void GsSoundInit()
    {
        MediaPlayer.MediaStateChanged += new EventHandler<EventArgs>(MediaPlayer_MediaStateChanged);
        MediaPlayer.ActiveSongChanged += MediaPlayer_ActiveSongChanged;
        if (sound_fx_list == null)
        {
            var modernSfx = SSave.CreateInstance().GetRemaster().ModernSoundEffects;
            sound_fx_list = new Dictionary<string, SOUND_TABLE>(130);
            gsSoundFillSoundTable(modernSfx ? "SND_FX_NEW.inf" : "SND_FX.inf", sound_fx_list);
        }

        if (sound_bgm_list == null)
        {
            var modernMusic = SSave.CreateInstance().GetRemaster().BetterMusic;
            sound_bgm_list = new Dictionary<string, SOUND_TABLE>(50);
            gsSoundFillSoundTable(modernMusic ? "SND_BGM_NEW.inf" : "SND_BGM.inf", sound_bgm_list);
        }

        LoadPrioritySoundsIntoCache();
        GSS_MAIN_SYS_INFO mainSysInfo = GsGetMainSysInfo();
        gsSoundInitSystemMainInfo();
        gsSoundInitSndScbHeap();
        gsSoundInitSeHandleHeap();
        int vol_bgm;
        int vol_se;
        GetSavedSoundVolumes(out vol_bgm, out vol_se);
        mainSysInfo.bgm_volume = vol_bgm / 10f;
        mainSysInfo.se_volume = vol_se / 10f;
        for (int snd_type = 0; snd_type < 2; ++snd_type)
            GsSoundSetVolume(snd_type, 1f);
    }

    private static void GetSavedSoundVolumes(out int vol_bgm, out int vol_se)
    {
        vol_bgm = 10;
        vol_se = 10;
        try
        {
            var sys = SSave.CreateInstance();
            vol_bgm = (int)((sys.GetOption()?.GetVolumeBgm() / 10f) ?? (uint)vol_bgm);
            vol_se = (int)((sys.GetOption()?.GetVolumeBgm() / 10f) ?? (uint)vol_se);
        }
        catch (Exception ex)
        {
        }
    }

    private static void GsSoundHalt()
    {
        amCriAudioGetGlobal();
        for (int index = 0; index < 8; ++index)
        {
            if (((int)gs_sound_scb_heap[index].flag & 1) != 0)
                GsSoundStopBgm(gs_sound_scb_heap[index]);
        }
    }

    private static void LoadSFX(SOUND_TABLE tbl)
    {
        try
        {
            for (int index = 0; index < tbl.count; ++index)
            {
                byte[] byteArray = null;
                int channels = 0;
                int sampleRate = 0;
                CriAuPlayer.LoadSound(tbl.filename[index], tbl.loop[index], ref tbl.loopStart[index],
                    ref tbl.loopEnd[index], out byteArray, ref sampleRate, ref channels, false);
            }
        }
        catch (FileNotFoundException ex)
        {
            string str1 = ex.ToString();
            string str2 = str1 + str1;
            mppSoundNotImplAssert();
        }
    }

    private static void LoadPrioritySoundsIntoCache()
    {
        if (b_bPrioritySoundsLoaded)
            return;
        try
        {
            LoadSFX(sound_fx_list["Sega_Logo"]);
            LoadSFX(sound_fx_list["Ok"]);
            LoadSFX(sound_fx_list["Window"]);
            LoadSFX(sound_fx_list["Cancel"]);
            LoadSFX(sound_fx_list["Cursol"]);
        }
        catch
        {
        }


        b_bPrioritySoundsLoaded = true;
    }

    private static bool SoundPartialCache(int iPercent)
    {
        if (g_bSoundsPrecached)
            return true;
        int count = sound_fx_list.Count;
        int num = Math.Min(Math.Max(count * iPercent / 100, 1), count - g_iCurrentCachedIndex);
        for (int currentCachedIndex = g_iCurrentCachedIndex;
            currentCachedIndex < g_iCurrentCachedIndex + num;
            ++currentCachedIndex)
            LoadSFX(sound_fx_list
                .ElementAt(currentCachedIndex).Value);
        g_iCurrentCachedIndex += num;
        g_bSoundsPrecached = g_iCurrentCachedIndex == count;
        return g_bSoundsPrecached;
    }

    private static void GsSoundReset()
    {
        for (int snd_type = 0; snd_type < 2; ++snd_type)
            GsSoundSetVolume(snd_type, 1f);
        gsSoundResetSeHandleHeap();
        gsSoundResetSndScbHeap();
        gsSoundResetSystemMainInfo();
    }

    private static void GsSoundExit()
    {
        for (int snd_type = 0; snd_type < 2; ++snd_type)
            GsSoundSetVolume(snd_type, 0.0f);
        GsSoundHalt();
        GsSoundEnd();
        gsSoundClearSeHandleHeap();
        gsSoundResetSndScbHeap();
        gsSoundClearSystemMainInfo();
    }

    private static void GsSoundBegin(ushort task_pause_level, uint task_prio, int task_group)
    {
        GsSoundSetVolumeFromMainSysInfo();
        gs_sound_tcb = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(gsSoundProcMain),
            null, 0U, task_pause_level, task_prio, task_group,
            null, "GS_SND_MAIN");
    }

    private static void GsSoundEnd()
    {
        if (gs_sound_tcb == null)
            return;
        mtTaskClearTcb(gs_sound_tcb);
        gs_sound_tcb = null;
    }

    private static bool GsSoundIsRunning()
    {
        return gs_sound_tcb != null;
    }

    private static GSS_SND_SYS_MAIN_INFO GsSoundGetSysMainInfo()
    {
        return gs_sound_sys_main_info;
    }

    private static void GsSoundPlaySe(string se_name)
    {
        GsSoundPlaySe(se_name, null, 0);
    }

    private static void GsSoundPlaySe(string se_name, GSS_SND_SE_HANDLE se_handle)
    {
        GsSoundPlaySe(se_name, se_handle, 0);
    }

    private static void GsSoundPlaySe(
        string se_name,
        GSS_SND_SE_HANDLE se_handle,
        int fade_frame)
    {
        gsSoundPlaySe(se_name, 0U, se_handle, fade_frame);
    }

    private static void GsSoundPlaySeForce(string se_name)
    {
        GsSoundPlaySeForce(se_name, null, 0);
    }

    private static void GsSoundPlaySeForce(string se_name, GSS_SND_SE_HANDLE se_handle)
    {
        GsSoundPlaySeForce(se_name, se_handle, 0);
    }

    private static void GsSoundPlaySeForce(
        string se_name,
        GSS_SND_SE_HANDLE se_handle,
        int fade_frame)
    {
        gsSoundPlaySe(se_name, 0U, se_handle, fade_frame);
    }

    private static void GsSoundPlaySeForce(
        string se_name,
        GSS_SND_SE_HANDLE se_handle,
        int fade_frame,
        bool bDontPlay)
    {
        gsSoundPlaySe(se_name, 0U, se_handle, fade_frame, bDontPlay);
    }

    private static void GsSoundPlaySeByIdForce(
        uint se_id,
        GSS_SND_SE_HANDLE se_handle,
        int fade_frame)
    {
        gsSoundPlaySe(null, se_id, se_handle, fade_frame);
    }

    private static void GsSoundStopSe()
    {
        GsSoundStopSe(0, false);
    }

    private static void GsSoundStopSe(int fade_frame, bool is_immediate)
    {
        for (int index = 0; index < 16; ++index)
        {
            GSS_SND_SE_HANDLE se_handle = gs_sound_se_handle_heap[index];
            if (((int)se_handle.flag & 1) != 0)
                gsSoundCriSeStop(se_handle, fade_frame, is_immediate);
        }
    }

    private static void GsSoundPauseSe(uint pause_level)
    {
        GsSoundPauseSe(pause_level, 0);
    }

    private static void GsSoundPauseSe(uint pause_level, int fade_frame)
    {
        for (int index = 0; index < 16; ++index)
        {
            GSS_SND_SE_HANDLE se_handle = gs_sound_se_handle_heap[index];
            if (((int)se_handle.flag & 1) != 0)
            {
                if (se_handle.cur_pause_level < pause_level)
                    se_handle.cur_pause_level = pause_level;
                gsSoundCriSePause(se_handle, fade_frame);
            }
        }
    }

    private static void GsSoundResumeSe(uint pause_level)
    {
        GsSoundResumeSe(pause_level, 0);
    }

    private static void GsSoundResumeSe(uint pause_level, int fade_frame)
    {
        for (int index = 0; index < 16; ++index)
        {
            GSS_SND_SE_HANDLE se_handle = gs_sound_se_handle_heap[index];
            if (((int)se_handle.flag & 1) != 0 && se_handle.cur_pause_level <= pause_level)
            {
                se_handle.cur_pause_level = 0U;
                gsSoundCriSeResume(se_handle, fade_frame);
            }
        }
    }

    private static void GmSoundStopSE(GSS_SND_SE_HANDLE se_handle)
    {
        GsSoundStopSeHandle(se_handle, 0);
    }

    private static void GsSoundStopSeHandle(GSS_SND_SE_HANDLE se_handle)
    {
        GsSoundStopSeHandle(se_handle, 0);
    }

    private static void GsSoundStopSeHandle(GSS_SND_SE_HANDLE se_handle, int fade_frame)
    {
        gsSoundCriSeStop(se_handle, fade_frame, false, false);
    }

    private static void GsSoundPlayBgm(GSS_SND_SCB scb, string bgm_name, int fade_frame)
    {
#if WASM
        fade_frame = 0;
#endif

        scb.cur_pause_level = 0U;
        if (scb.snd_data_type == 1 || g_ao_sys_global.is_playing_device_bgm_music)
            return;
        gsSoundCriStrmStop(scb, 0);
        AMS_CRIAUDIO_INTERFACE global = amCriAudioGetGlobal();
        CriAuPlayer criAuPlayer = global.auply[scb.auply_no];
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

        amCriAudioStrmPlay((uint)scb.auply_no, bgm_name);
        gsSoundCriStrmSetFadeIn(scb, fade_frame);
        gsSoundUpdateVolume();
        criAuPlayer.Update();
    }

    private static void GsSoundStopBgm(GSS_SND_SCB scb)
    {
        GsSoundStopBgm(scb, 0);
    }

    private static void GsSoundStopBgm(GSS_SND_SCB scb, int fade_frame)
    {
        if (scb.snd_data_type == 1 || g_ao_sys_global.is_playing_device_bgm_music)
            return;
        gsSoundCriStrmStop(scb, fade_frame);
    }

    private static void GsSoundPauseBgm(GSS_SND_SCB scb, int fade_frame)
    {
        if (((int)scb.flag & 1) == 0)
            return;
        if (scb.cur_pause_level < int.MaxValue)
            scb.cur_pause_level = int.MaxValue;
        if (scb.snd_data_type == 1 || g_ao_sys_global.is_playing_device_bgm_music)
            return;
        gsSoundCriStrmPause(scb, fade_frame);
    }

    private static void GsSoundResumeBgm(GSS_SND_SCB scb, int fade_frame)
    {
        if (((int)scb.flag & 1) == 0 || scb.cur_pause_level > int.MaxValue)
            return;
        scb.cur_pause_level = 0U;
        if (scb.snd_data_type == 1 || g_ao_sys_global.is_playing_device_bgm_music)
            return;
        gsSoundCriStrmResume(scb, fade_frame);
    }

    private static void GsSoundSetVolumeFromMainSysInfo()
    {
        GSS_MAIN_SYS_INFO mainSysInfo = GsGetMainSysInfo();
        GsSoundSetVolume(0, mainSysInfo.bgm_volume);
        GsSoundSetVolume(1, mainSysInfo.se_volume);
    }

    private static float GsSoundGetVolume(int snd_type)
    {
        return gs_sound_volume[snd_type];
    }

    private static void GsSoundSetVolume(int snd_type, float vol)
    {
        gs_sound_volume[snd_type] = vol;
        if (snd_type != 1)
            return;
        SoundEffect.MasterVolume = vol;
    }

    private static void GsSoundScbSetVolume(GSS_SND_SCB scb, float vol)
    {
        if (scb.snd_data_type == 1)
            return;
        scb.snd_ctrl_param.volume = vol;
    }

    private static void GsSoundScbSetSeqMute(GSS_SND_SCB scb, bool mute_on)
    {
        int sndDataType = scb.snd_data_type;
    }

    private static GSS_SND_SCB GsSoundAssignScb(int snd_data_type)
    {
        for (int scb_no = 0; scb_no < 8; ++scb_no)
        {
            if ((gs_sound_scb_heap_usage_flag[scb_no >> 3] & 1 << (scb_no & 7)) == 0)
            {
                gs_sound_scb_heap_usage_flag[scb_no >> 3] |= (byte)(1 << (scb_no & 7));
                gsSoundInitSndScb(gs_sound_scb_heap[scb_no], scb_no, snd_data_type);
                return gs_sound_scb_heap[scb_no];
            }
        }

        return null;
    }

    private static void GsSoundResignScb(GSS_SND_SCB scb)
    {
    }

    private static GSS_SND_SE_HANDLE GsSoundAllocSeHandle()
    {
        for (int index = 0; index < 16; ++index)
        {
            if ((gs_sound_se_handle_heap_usage_flag[index >> 3] & 1 << (index & 7)) == 0)
            {
                gs_sound_se_handle_heap_usage_flag[index >> 3] |= (byte)(1 << (index & 7));
                gsSoundInitSeHandle(gs_sound_se_handle_heap[index]);
                gs_sound_se_handle_heap[index].snd_ctrl_param.pitch = 0.0f;
                return gs_sound_se_handle_heap[index];
            }
        }

        gsSoundInitSeHandle(gs_sound_se_handle_error);
        gs_sound_se_handle_error.snd_ctrl_param.pitch = 0.0f;
        return gs_sound_se_handle_error;
    }

    private static void GsSoundFreeSeHandle(GSS_SND_SE_HANDLE se_handle)
    {
        if (gs_sound_se_handle_error == se_handle)
        {
            gsSoundClearSeHandle(gs_sound_se_handle_error);
        }
        else
        {
            for (int index = 0; index < 16; ++index)
            {
                if (gs_sound_se_handle_heap[index] == se_handle)
                {
                    gsSoundClearSeHandle(se_handle);
                    gs_sound_se_handle_heap_usage_flag[index >> 3] &= (byte)~(1 << (index & 7));
                    break;
                }
            }
        }
    }

    private static void GsSoundRequestFreeSeHandle(GSS_SND_SE_HANDLE se_handle)
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

    private static void gsSoundProcMain(MTS_TASK_TCB tcb)
    {
        amCriAudioGetGlobal();
        gsSoundUpdateSystemSuspendWait();
        gsSoundUpdateSystemControlVolume();
        for (int index = 0; index < 8; ++index)
        {
            if (((int)gs_sound_scb_heap[index].flag & 1) != 0)
                gsSoundUpdateSndScb(gs_sound_scb_heap[index]);
        }

        for (int index = 0; index < 16; ++index)
        {
            GSS_SND_SE_HANDLE se_handle = gs_sound_se_handle_heap[index];
            if (((int)se_handle.flag & 1) != 0)
                gsSoundUpdateSndSeHandle(se_handle);
        }

        if (((int)gs_sound_se_handle_error.flag & 1) != 0)
            gsSoundUpdateSndSeHandle(gs_sound_se_handle_error);
        gsSoundUpdateVolume();
        for (int index = 0; index < 16; ++index)
        {
            GSS_SND_SE_HANDLE se_handle = gs_sound_se_handle_heap[index];
            if (((int)se_handle.flag & 1) != 0)
            {
                switch (se_handle.au_player.GetStatus())
                {
                    case 0:
                    case 3:
                        if (((int)se_handle.flag & 16) != 0)
                        {
                            GsSoundFreeSeHandle(se_handle);
                            continue;
                        }

                        if (((int)se_handle.flag & 2) != 0 && ((int)se_handle.flag & int.MinValue) == 0)
                        {
                            gsSoundClearSeHandle(se_handle);
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
            else if (((int)se_handle.flag & 16) != 0)
                GsSoundFreeSeHandle(se_handle);
        }

        if (((int)gs_sound_se_handle_error.flag & 1) != 0)
        {
            switch (gs_sound_se_handle_error.au_player.GetStatus())
            {
                case 0:
                case 3:
                    if (((int)gs_sound_se_handle_error.flag & 16) != 0)
                    {
                        GsSoundFreeSeHandle(gs_sound_se_handle_error);
                        break;
                    }

                    if (((int)gs_sound_se_handle_error.flag & 2) != 0)
                    {
                        gsSoundClearSeHandle(gs_sound_se_handle_error);
                        break;
                    }

                    break;
                case 4:
                    gs_sound_se_handle_error.au_player.Stop();
                    goto case 0;
                default:
                    gs_sound_se_handle_error.au_player.Update();
                    break;
            }
        }
        else if (((int)gs_sound_se_handle_error.flag & 16) != 0)
            GsSoundFreeSeHandle(gs_sound_se_handle_error);

        for (int index = 0; index < 8; ++index)
        {
            if (((int)gs_sound_scb_heap[index].flag & 1) != 0)
                gsSoundUpdateSndScbStatus(gs_sound_scb_heap[index]);
        }

        for (int index = 0; index < 16; ++index)
        {
            if (((int)gs_sound_se_handle_heap[index].flag & 1) != 0)
                gsSoundUpdateSeHandleStatus(gs_sound_se_handle_heap[index]);
        }

        if (((int)gs_sound_se_handle_error.flag & 1) == 0)
            return;
        gsSoundUpdateSeHandleStatus(gs_sound_se_handle_error);
    }

    private static void gsSoundInitSystemMainInfo()
    {
        gsSoundClearSystemMainInfo();
    }

    private static void gsSoundResetSystemMainInfo()
    {
        gs_sound_sys_main_info.flag &= 4294967293U;
    }

    private static void gsSoundClearSystemMainInfo()
    {
        gs_sound_sys_main_info.Clear();
    }

    private static void gsSoundSetEnableSystemControlVolume(bool enable)
    {
        if (enable)
            gs_sound_sys_main_info.flag |= 1U;
        else
            gs_sound_sys_main_info.flag &= 4294967294U;
    }

    private static bool gsSoundIsSystemControlVolumeEnabled()
    {
        return ((int)gs_sound_sys_main_info.flag & 1) != 0;
    }

    private static void gsSoundUpdateSystemControlVolume()
    {
        gsSoundIsSystemControlVolumeEnabled();
    }

    private static float gsSoundGetGlobalVolume()
    {
        float num = 1f;
        if (gsSoundIsSystemControlVolumeEnabled())
            num *= gs_sound_sys_main_info.system_cnt_vol;
        if (gsSoundIsSystemSuspendWait())
            num = 0.0f;
        return num;
    }

    private static float gsSoundGetSndScbMuteVolume(GSS_SND_SCB scb)
    {
        return GsSystemBgmIsPlay() && ((int)scb.flag & int.MinValue) != 0 ? 0.0f : 1f;
    }

    private static void gsSoundInitSndScbHeap()
    {
        gsSoundResetSndScbHeap();
    }

    private static void gsSoundResetSndScbHeap()
    {
        for (int scb_no = 0; scb_no < 8; ++scb_no)
            gsSoundClearSndScb(gs_sound_scb_heap[scb_no], gsSoundGetAuplyNo(scb_no));
        Array.Clear(gs_sound_scb_heap_usage_flag, 0, gs_sound_scb_heap_usage_flag.Length);
    }

    private static uint gsSndGetFreeScbNum()
    {
        uint num1 = 8;
        int num2 = 1;
        for (int index = 0; index < num2; ++index)
            num1 -= AkMathCountBitPopulation(gs_sound_scb_heap_usage_flag[index]);
        return num1;
    }

    private static void gsSoundInitSndScb(GSS_SND_SCB scb, int scb_no, int snd_data_type)
    {
        gsSoundClearSndScb(scb, gsSoundGetAuplyNo(scb_no));
        scb.snd_data_type = snd_data_type;
        if (snd_data_type != 1)
        {
            scb.auply_no = gsSoundGetAuplyNo(scb_no);
            scb.snd_ctrl_param.fade_vol = 1f;
            scb.snd_ctrl_param.fade_sub_vol = 1f;
            scb.snd_ctrl_param.volume = 1f;
        }

        scb.noplay_error_state.sample = uint.MaxValue;
        scb.noplay_error_state.counter = 0U;
        scb.flag |= 1U;
    }

    private static void gsSoundClearSndScb(GSS_SND_SCB scb, int auply_no)
    {
        CriAuPlayer criAuPlayer = amCriAudioGetGlobal().auply[auply_no];
        if (criAuPlayer != null)
        {
            criAuPlayer.ReleaseCue();
            criAuPlayer.ResetParameters();
        }

        scb.Clear();
    }

    private static void gsSoundUpdateSndScb(GSS_SND_SCB scb)
    {
        if (scb.snd_data_type == 1)
            return;
        CriAuPlayer au_player = amCriAudioGetGlobal().auply[scb.auply_no];
        gsSoundUpdateSndCtrl(scb.snd_ctrl_param, au_player);
        if (gsSoundCheckSndScbStop(scb) || (6 & (int)scb.flag) != 0)
        {
            scb.noplay_error_state.sample = uint.MaxValue;
            scb.noplay_error_state.counter = 0U;
        }
        else
        {
            uint numPlayedSamples = (uint)au_player.GetNumPlayedSamples();
            if ((int)scb.noplay_error_state.sample != (int)numPlayedSamples)
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

    private static void gsSoundUpdateSndScbStatus(GSS_SND_SCB scb)
    {
        if (gsSoundCheckSndScbStop(scb))
            scb.flag |= 2U;
        else
            scb.flag &= 4294967293U;
        if (gsSoundCheckSndScbPause(scb))
            scb.flag |= 4U;
        else
            scb.flag &= 4294967291U;
    }

    private static bool gsSoundCheckSndScbPause(GSS_SND_SCB scb)
    {
        if (scb.snd_data_type == 1)
            return false;
        AMS_CRIAUDIO_INTERFACE global = amCriAudioGetGlobal();
        return scb.snd_ctrl_param.fade_state == 3U || global.auply[scb.auply_no].IsPaused();
    }

    private static bool gsSoundCheckSndScbStop(GSS_SND_SCB scb)
    {
        if (scb.snd_data_type == 1)
            return true;
        switch (amCriAudioGetGlobal().auply[scb.auply_no].GetStatus())
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

    private static void gsSoundCriStrmSetFadeIn(GSS_SND_SCB scb, int fade_frame)
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

    private static void gsSoundCriStrmStop(GSS_SND_SCB scb, int fade_frame)
    {
        gsSoundCriStrmStop(scb, fade_frame, false);
    }

    private static void gsSoundCriStrmStop(GSS_SND_SCB scb, int fade_frame, bool is_takeover)
    {
        AMS_CRIAUDIO_INTERFACE global = amCriAudioGetGlobal();
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

    private static void gsSoundCriStrmPause(GSS_SND_SCB scb, int fade_frame)
    {
        AMS_CRIAUDIO_INTERFACE global = amCriAudioGetGlobal();
        if (scb.snd_data_type != 0)
            return;
        if (scb.snd_ctrl_param.fade_state == 2U)
            gsSoundCriStrmStop(scb, fade_frame, true);
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

    private static void gsSoundCriStrmResume(GSS_SND_SCB scb, int fade_frame)
    {
        AMS_CRIAUDIO_INTERFACE global = amCriAudioGetGlobal();
        if (scb.snd_data_type != 0 || !gsSoundCheckSndScbPause(scb))
            return;
        global.auply[scb.auply_no].Pause(false);
        if (fade_frame == 0)
        {
            scb.snd_ctrl_param.fade_state = 0U;
            scb.snd_ctrl_param.fade_frame_max = scb.snd_ctrl_param.fade_frame_cnt = 0;
            scb.snd_ctrl_param.fade_vol = 1f;
        }
        else
            gsSoundCriStrmSetFadeIn(scb, fade_frame);
    }

    private static void gsSoundUpdateSndCtrl(
        GSS_SND_CTRL_PARAM snd_ctrl_param,
        CriAuPlayer au_player)
    {
        amCriAudioGetGlobal();
        au_player.pan = snd_ctrl_param.pan;

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
                            ? (float)(1.0 * (snd_ctrl_param.fade_frame_cnt /
                                              (double)snd_ctrl_param.fade_frame_max))
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

    private static void gsSoundSeHandleUpdateVolume(GSS_SND_SE_HANDLE se_handle)
    {
        if (se_handle.au_player == null)
            return;
        se_handle.au_player.SetVolume(se_handle.snd_ctrl_param.volume * se_handle.snd_ctrl_param.fade_vol *
                                      se_handle.snd_ctrl_param.fade_sub_vol * gsSoundGetGlobalVolume());
        se_handle.au_player.SetPitch(se_handle.snd_ctrl_param.pitch);
    }

    private static void gsSoundUpdateVolume()
    {
        AMS_CRIAUDIO_INTERFACE global = amCriAudioGetGlobal();
        for (int index = 0; index < 8; ++index)
        {
            if (((int)gs_sound_scb_heap[index].flag & 1) != 0 &&
                gs_sound_scb_heap[index].snd_data_type == 0)
            {
                int status = global.auply[gs_sound_scb_heap[index].auply_no].GetStatus();
                if (status != 0 && 3 != status)
                    global.auply[gs_sound_scb_heap[index].auply_no].SetVolume(
                        gs_sound_volume[0] * gs_sound_scb_heap[index].snd_ctrl_param.volume *
                        gs_sound_scb_heap[index].snd_ctrl_param.fade_vol *
                        gs_sound_scb_heap[index].snd_ctrl_param.fade_sub_vol *
                        gsSoundGetGlobalVolume() *
                        gsSoundGetSndScbMuteVolume(gs_sound_scb_heap[index]));
            }
        }

        for (int index = 0; index < 16; ++index)
        {
            if (((int)gs_sound_se_handle_heap[index].flag & 1) != 0 &&
                gsSoundIsSeHandleCueSet(gs_sound_se_handle_heap[index]))
                gsSoundSeHandleUpdateVolume(gs_sound_se_handle_heap[index]);
        }

        if (((int)gs_sound_se_handle_error.flag & 1) == 0 ||
            !gsSoundIsSeHandleCueSet(gs_sound_se_handle_error))
            return;
        gsSoundSeHandleUpdateVolume(gs_sound_se_handle_error);
    }

    private static void gsSoundInitSeHandleHeap()
    {
        gsSoundResetSeHandleHeap();
    }

    private static void gsSoundResetSeHandleHeap()
    {
        AMS_CRIAUDIO_INTERFACE global = amCriAudioGetGlobal();
        gsSoundClearSeHandleHeap();
        for (int index = 0; index < 16; ++index)
            gs_sound_se_handle_heap[index].au_player = CriAuPlayer.Create(global);
        gs_sound_se_handle_error.au_player = CriAuPlayer.Create(global);
        gs_sound_se_handle_default = New<GSS_SND_SE_HANDLE>(3);
        for (int index = 0; index < gs_sound_se_handle_default.Length; ++index)
            gs_sound_se_handle_default[index] = GsSoundAllocSeHandle();
    }

    private static void gsSoundClearSeHandleHeap()
    {
        amCriAudioGetGlobal();
        if (gs_sound_se_handle_default != null)
        {
            for (int index = 0; index < gs_sound_se_handle_default.Length; ++index)
                GsSoundFreeSeHandle(gs_sound_se_handle_default[index]);
            gs_sound_se_handle_default = null;
        }

        for (int index = 0; index < 16; ++index)
        {
            gsSoundClearSeHandle(gs_sound_se_handle_heap[index]);
            if (gs_sound_se_handle_heap[index].au_player != null)
            {
                gs_sound_se_handle_heap[index].au_player.Destroy();
                gs_sound_se_handle_heap[index].au_player = null;
            }
        }

        gsSoundClearSeHandle(gs_sound_se_handle_error);
        if (gs_sound_se_handle_error.au_player != null)
        {
            gs_sound_se_handle_error.au_player.Destroy();
            gs_sound_se_handle_error.au_player = null;
        }

        Array.Clear(gs_sound_se_handle_heap_usage_flag, 0,
            gs_sound_se_handle_heap_usage_flag.Length);
    }

    private static uint gsSoundGetFreeSeHandleNum()
    {
        uint num1 = 16;
        int num2 = 2;
        for (int index = 0; index < num2; ++index)
            num1 -= AkMathCountBitPopulation(gs_sound_se_handle_heap_usage_flag[index]);
        return num1;
    }

    private static void gsSoundInitSeHandle(GSS_SND_SE_HANDLE se_handle)
    {
        gsSoundInitSeHandle(se_handle, false);
    }

    private static void gsSoundInitSeHandle(GSS_SND_SE_HANDLE se_handle, bool b_reset)
    {
        amCriAudioGetGlobal();
        if (se_handle.au_player != null && 1 == se_handle.au_player.GetStatus())
            se_handle.au_player.Update();
        gsSoundClearSeHandle(se_handle, b_reset);
        se_handle.flag |= 1U;
        se_handle.snd_ctrl_param.fade_vol = 1f;
        se_handle.snd_ctrl_param.fade_sub_vol = 1f;
        se_handle.snd_ctrl_param.volume = 1f;
    }

    private static void gsSoundClearSeHandle(GSS_SND_SE_HANDLE se_handle)
    {
        gsSoundClearSeHandle(se_handle, false);
    }

    private static void gsSoundClearSeHandle(GSS_SND_SE_HANDLE se_handle, bool b_takeover_cue)
    {
        amCriAudioGetGlobal();
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

    private static void gsSoundUpdateSndSeHandle(GSS_SND_SE_HANDLE se_handle)
    {
        gsSoundUpdateSndCtrl(se_handle.snd_ctrl_param, se_handle.au_player);
    }

    private static void gsSoundUpdateSeHandleStatus(GSS_SND_SE_HANDLE se_handle)
    {
        if (gsSoundCheckSeHandleStop(se_handle))
            se_handle.flag |= 4U;
        else
            se_handle.flag &= 4294967291U;
        if (gsSoundCheckSeHandlePause(se_handle))
            se_handle.flag |= 8U;
        else
            se_handle.flag &= 4294967287U;
    }

    private static bool gsSoundCheckSeHandlePause(GSS_SND_SE_HANDLE se_handle)
    {
        amCriAudioGetGlobal();
        return se_handle.au_player != null &&
               (se_handle.snd_ctrl_param.fade_state == 3U || se_handle.au_player.IsPaused());
    }

    private static bool gsSoundCheckSeHandleStop(GSS_SND_SE_HANDLE se_handle)
    {
        amCriAudioGetGlobal();
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

    private static bool gsSoundIsSeHandleCueSet(GSS_SND_SE_HANDLE se_handle)
    {
        amCriAudioGetGlobal();
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

    private static GSS_SND_SE_HANDLE gsSoundGetDefaultSeHandle()
    {
        for (int index = 0; index < gs_sound_se_handle_default.Length; ++index)
        {
            if (gs_sound_se_handle_default[index].au_player.sound == null ||
                gs_sound_se_handle_default[index].au_player.sound[0] == null ||
                gs_sound_se_handle_default[index].au_player.sound[0].State == SoundState.Stopped)
                return gs_sound_se_handle_default[index];
        }

        return gs_sound_se_handle_default[0];
    }

    private static void gsSoundCriSeSetFadeIn(GSS_SND_SE_HANDLE se_handle, int fade_frame)
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
        GSS_SND_SE_HANDLE se_handle,
        int fade_frame,
        bool is_immediate)
    {
        gsSoundCriSeStop(se_handle, fade_frame, is_immediate, false);
    }

    private static void gsSoundCriSeStop(
        GSS_SND_SE_HANDLE se_handle,
        int fade_frame,
        bool is_immediate,
        bool is_takeover)
    {
        amCriAudioGetGlobal();
        if (((int)se_handle.flag & 1) == 0 || se_handle.au_player == null)
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

    private static void gsSoundCriSePause(GSS_SND_SE_HANDLE se_handle, int fade_frame)
    {
        amCriAudioGetGlobal();
        if (se_handle.snd_ctrl_param.fade_state == 2U)
            gsSoundCriSeStop(se_handle, fade_frame, true, true);
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

    private static void gsSoundCriSeResume(GSS_SND_SE_HANDLE se_handle, int fade_frame)
    {
        amCriAudioGetGlobal();
        if (!gsSoundCheckSeHandlePause(se_handle))
            return;
        se_handle.au_player.Pause(false);
        if (fade_frame == 0)
        {
            se_handle.snd_ctrl_param.fade_state = 0U;
            se_handle.snd_ctrl_param.fade_frame_max = se_handle.snd_ctrl_param.fade_frame_cnt = 0;
            se_handle.snd_ctrl_param.fade_vol = 1f;
        }
        else
            gsSoundCriSeSetFadeIn(se_handle, fade_frame);
    }

    private static void gsSoundStopSe(GSS_SND_SE_HANDLE se_handle)
    {
        se_handle.au_player.Pause(true);
        se_handle.au_player.Stop(0);
    }

    private static void gsSoundPlaySe(
        string se_name,
        uint se_id,
        GSS_SND_SE_HANDLE se_handle,
        int fade_frame)
    {
        gsSoundPlaySe(se_name, se_id, se_handle, fade_frame, false);
    }

    private static void gsSoundPlaySe(
        string se_name,
        uint se_id,
        GSS_SND_SE_HANDLE se_handle,
        int fade_frame,
        bool bDontPlay)
    {
        amCriAudioGetGlobal();
        if (se_handle == null)
            se_handle = gsSoundGetDefaultSeHandle();
        if (se_handle.au_player.IsPaused())
            se_handle.au_player.Stop(1);
        if (((int)se_handle.flag & int.MinValue) != 0)
        {
            gsSoundInitSeHandle(se_handle, true);
            se_handle.flag |= 2147483648U;
        }
        else
            gsSoundInitSeHandle(se_handle, true);

        se_handle.flag |= 2U;
        if (se_name == null)
            se_name = CriAuPlayer.GetCueName(se_id);
        se_handle.au_player.SetCue(se_name);
        gsSoundCriSeSetFadeIn(se_handle, fade_frame);
        gsSoundSeHandleUpdateVolume(se_handle);
        if (bDontPlay)
            return;
        se_handle.au_player.Play();
    }

    private static bool gsSoundIsSystemSuspendWait()
    {
        bool flag = false;
        if (GsSoundGetSysMainInfo().suspend_wait_count > 0)
            flag = true;
        return flag;
    }

    private static void gsSoundUpdateSystemSuspendWait()
    {
        GSS_SND_SYS_MAIN_INFO sysMainInfo = GsSoundGetSysMainInfo();
        if (sysMainInfo.suspend_wait_count > 0)
            --sysMainInfo.suspend_wait_count;
        if (!GsMainSysGetSuspendedFlag())
            return;
        sysMainInfo.suspend_wait_count = 8;
    }

    private static void GsSoundPlaySeById(uint se_id)
    {
        gsSoundPlaySe(null, se_id, null, 0);
    }
}