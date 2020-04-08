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
    private class CMain_PauseMenu : AppMain.ITaskLinkAsv
    {
        private static AppMain.CMain_PauseMenu instance_ = new AppMain.CMain_PauseMenu();
        private static readonly int[] c_return_table = new int[3]
        {
      0,
      2,
      4
        };
        private static readonly int[] c_local_create_table = new int[2]
        {
      2,
      3
        };
        private static AppMain.CMain_PauseMenu.SLocalCreateActionTable[] local_create_action_table = new AppMain.CMain_PauseMenu.SLocalCreateActionTable[17]
        {
      new AppMain.CMain_PauseMenu.SLocalCreateActionTable()
      {
        file = 0,
        tex = 0,
        idx = 0
      },
      new AppMain.CMain_PauseMenu.SLocalCreateActionTable()
      {
        file = 0,
        tex = 0,
        idx = 1
      },
      new AppMain.CMain_PauseMenu.SLocalCreateActionTable()
      {
        file = 0,
        tex = 0,
        idx = 2
      },
      new AppMain.CMain_PauseMenu.SLocalCreateActionTable()
      {
        file = 0,
        tex = 0,
        idx = 3
      },
      new AppMain.CMain_PauseMenu.SLocalCreateActionTable()
      {
        file = 0,
        tex = 0,
        idx = 4
      },
      new AppMain.CMain_PauseMenu.SLocalCreateActionTable()
      {
        file = 0,
        tex = 0,
        idx = 5
      },
      new AppMain.CMain_PauseMenu.SLocalCreateActionTable()
      {
        file = 0,
        tex = 0,
        idx = 6
      },
      new AppMain.CMain_PauseMenu.SLocalCreateActionTable()
      {
        file = 0,
        tex = 0,
        idx = 7
      },
      new AppMain.CMain_PauseMenu.SLocalCreateActionTable()
      {
        file = 0,
        tex = 0,
        idx = 8
      },
      new AppMain.CMain_PauseMenu.SLocalCreateActionTable()
      {
        file = 0,
        tex = 0,
        idx = 9
      },
      new AppMain.CMain_PauseMenu.SLocalCreateActionTable()
      {
        file = 1,
        tex = 1,
        idx = 0
      },
      new AppMain.CMain_PauseMenu.SLocalCreateActionTable()
      {
        file = 1,
        tex = 1,
        idx = 1
      },
      new AppMain.CMain_PauseMenu.SLocalCreateActionTable()
      {
        file = 1,
        tex = 1,
        idx = 3
      },
      new AppMain.CMain_PauseMenu.SLocalCreateActionTable()
      {
        file = 1,
        tex = 1,
        idx = 4
      },
      new AppMain.CMain_PauseMenu.SLocalCreateActionTable()
      {
        file = 1,
        tex = 1,
        idx = 5
      },
      new AppMain.CMain_PauseMenu.SLocalCreateActionTable()
      {
        file = 1,
        tex = 1,
        idx = 9
      },
      new AppMain.CMain_PauseMenu.SLocalCreateActionTable()
      {
        file = 1,
        tex = 1,
        idx = 8
      }
        };
        private static int[] c_local_create_trg_table = new int[3]
        {
      2,
      5,
      8
        };
        private static readonly int[] c_trg_table_select = new int[3]
        {
      0,
      1,
      2
        };
        private static readonly int[][] c_btn_action_table_select = new int[3][]
        {
      new int[3]{ 1, 2, 3 },
      new int[3]{ 4, 5, 6 },
      new int[3]{ 7, 8, 9 }
        };
        private static readonly int[] c_trg_table = new int[2]
        {
      0,
      1
        };
        private static readonly int[][] c_btn_action_table = new int[2][]
        {
      new int[3]{ 1, 2, 3 },
      new int[3]{ 4, 5, 6 }
        };
        private static readonly string[] c_se_name_tbl = new string[4]
        {
      "Ok",
      "Cancel",
      "Window",
      "Pause"
        };
        private readonly bool[] m_flag = new bool[9];
        private readonly AppMain.AMS_FS[] m_fs = new AppMain.AMS_FS[4];
        private readonly object[] m_file = new object[4];
        private readonly AppMain.AOS_TEXTURE[] m_tex = AppMain.New<AppMain.AOS_TEXTURE>(2);
        private AppMain.CMain_PauseMenu.SAction[] m_act = AppMain.New<AppMain.CMain_PauseMenu.SAction>(17);
        private readonly CTrgAoAction[] m_trg = AppMain.New<CTrgAoAction>(3);
        private const uint c_pause_btn_se_frame = 15;
        private const uint c_fade_in_frame = 8;
        private const uint c_fade_out_frame = 8;
        private const uint c_fade_enter_efct_frame = 10;
        public AppMain.AMS_FS pause_amb;
        public AppMain.AMS_FS lang_amb;
        private int m_return;
        private int m_really;
        private AppMain.GSS_SND_SE_HANDLE m_se_handle;
        private AppMain.CProcCount m_procCount;

        private CMain_PauseMenu()
        {
            this.m_procCount = new AppMain.CProcCount((object)this);
        }

        public static AppMain.CMain_PauseMenu CreateInstance()
        {
            return AppMain.CMain_PauseMenu.instance_;
        }

        public override void operator_brackets()
        {
            if (this.m_flag[0])
                this.preUpdate();
            this.m_procCount.operator_brackets();
            if (!this.m_flag[0])
                return;
            this.update();
            this.draw();
        }

        public bool Create()
        {
            return true;
        }

        public void Release()
        {
            if (!this.m_flag[0])
                return;
            if (this.m_flag[3])
            {
                for (int index = 0; index < this.m_file.Length; ++index)
                    this.m_file[0] = (object)null;
                this.m_flag[3] = false;
                this.m_flag[4] = false;
            }
            this.DetachTask();
            this.m_flag.Initialize();
        }

        public void LoadFile()
        {
            this.fileLoadingStart();
        }

        public void CreateTexture()
        {
            this.creatingStart();
        }

        public void ReleaseTexture()
        {
            this.releasingStart();
        }

        public void Start(uint prio)
        {
            this.fadeInStart((int)prio);
        }

        public void Cancel()
        {
            this.m_flag[8] = true;
        }

        public bool IsValid()
        {
            return this.m_flag[6];
        }

        public bool IsEmpty()
        {
            return !this.m_flag[0];
        }

        public bool IsLoadFile()
        {
            return this.m_flag[4];
        }

        public bool IsCreatedTexture()
        {
            return this.m_flag[6];
        }

        public bool IsReleasedTexture()
        {
            return !this.m_flag[5];
        }

        public bool IsPlay()
        {
            return !this.m_flag[7];
        }

        public int GetResult()
        {
            return this.m_return;
        }

        private void preUpdate()
        {
            if (!this.m_flag[7] || this.m_flag[1])
                return;
            for (int index = 0; index < this.m_trg.Length; ++index)
                this.m_trg[index].Update();
        }

        private void update()
        {
            if (!this.m_flag[7])
                return;
            AppMain.AoActAcmPush();
            AppMain.AoActAcmApplyTrans(0.0f, 0.0f, -1000f);
            for (int index = 0; index < this.m_act.Length; ++index)
            {
                this.m_act[index].Update();
                if (index == 16 || index == 15)
                    this.m_act[index].act.sprite.center_y += 5f;
            }
            AppMain.AoActAcmPop();
        }

        private void draw()
        {
            if (!AppMain._am_sample_draw_enable || !this.m_flag[7] || this.m_flag[1])
                return;
            for (int index = 0; index < this.m_act.Length; ++index)
                this.m_act[index].Draw();
        }

        private void fileLoadingStart()
        {
            this.m_file[0] = (object)AppMain.readAMAFile("G_COM/MENU/G_PAUSE.AMA");
            this.pause_amb = AppMain.amFsReadBackground("G_COM/MENU/G_PAUSE.AMB");
            this.m_file[1] = (object)AppMain.readAMAFile("G_COM/MENU/G_PAUSE_L.AMA");
            int language = AppMain.GsEnvGetLanguage();
            this.lang_amb = AppMain.amFsReadBackground(file.c_lang_amb[language]);
            this.m_flag[0] = true;
            this.m_flag[3] = true;
            this.AttachTask("gmPauseMenu.Load", 28928U, 0U, 0U);
            this.m_procCount.SetProc(new AppMain.ITaskAsv.FProc(this.fileLoading));
        }

        private void fileLoading()
        {
            if (!AppMain.amFsIsComplete(this.pause_amb))
                return;
            this.m_file[2] = (object)AppMain.readAMBFile(this.pause_amb);
            AppMain.GsEnvGetLanguage();
            if (!AppMain.amFsIsComplete(this.lang_amb))
                return;
            this.m_file[3] = (object)AppMain.readAMBFile(this.lang_amb);
            this.m_flag[4] = true;
            this.DetachTask();
        }

        private void creatingStart()
        {
            for (int index1 = 0; index1 < AppMain.CMain_PauseMenu.c_local_create_table.Length; ++index1)
            {
                int index2 = AppMain.CMain_PauseMenu.c_local_create_table[index1];
                AppMain.AoTexBuild(this.m_tex[index1], (AppMain.AMS_AMB_HEADER)this.m_file[index2]);
                AppMain.AoTexLoad(this.m_tex[index1]);
            }
            this.m_flag[5] = true;
            this.AttachTask("gmPauseMenu.Build", 28928U, 0U, 0U);
            this.m_procCount.SetProc(new AppMain.ITaskAsv.FProc(this.creating));
        }

        private void creating()
        {
            bool flag = true;
            for (int index = 0; index < this.m_tex.Length; ++index)
            {
                if (!AppMain.AoTexIsLoaded(this.m_tex[index]))
                {
                    flag = false;
                    break;
                }
            }
            if (!flag)
                return;
            this.m_flag[6] = true;
            this.DetachTask();
        }

        private void fadeInStart()
        {
            this.fadeInStart(28928);
        }

        private void fadeInStart(int prio)
        {
            if (!AppMain.CMain_PauseMenu.canGoStageSelect())
            {
                AppMain.CMain_PauseMenu.local_create_action_table[11].idx = 2;
                AppMain.CMain_PauseMenu.local_create_action_table[14].idx = 7;
            }
            else
            {
                AppMain.CMain_PauseMenu.local_create_action_table[11].idx = 1;
                AppMain.CMain_PauseMenu.local_create_action_table[14].idx = 5;
            }
            AppMain.CMain_PauseMenu.local_create_action_table[13].idx = !AppMain.CMain_PauseMenu.isSpecialStage() ? 4 : 6;
            for (int index = 0; index < 17; ++index)
            {
                AppMain.CMain_PauseMenu.SLocalCreateActionTable createActionTable = AppMain.CMain_PauseMenu.local_create_action_table[index];
                AppMain.A2S_AMA_HEADER ama = (AppMain.A2S_AMA_HEADER)this.m_file[createActionTable.file];
                AppMain.CMain_PauseMenu.SAction saction = this.m_act[index];
                saction.act = AppMain.AoActCreate(ama, (uint)createActionTable.idx);
                saction.tex = this.m_tex[createActionTable.tex];
                saction.flag[0] = true;
                saction.flag[1] = true;
                saction.AcmInit();
            }
            for (int index = 0; index < 3; ++index)
            {
                AppMain.CMain_PauseMenu.SAction saction = this.m_act[AppMain.CMain_PauseMenu.c_local_create_trg_table[index]];
                this.m_trg[index].Create(saction.act);
            }
            this.m_flag[7] = true;
            this.m_act[0].flag[1] = false;
            this.m_act[0].scale = new Vector2(0.0f, 0.0f);
            this.m_se_handle = AppMain.GsSoundAllocSeHandle();
            this.AttachTask("gmPauseMenu.Execute", (uint)prio, 0U, 0U);
            this.playSe(0);
            this.m_procCount.SetProc(new AppMain.ITaskAsv.FProc(this.fadeIn));
        }

        private void fadeIn()
        {
            if (15U >= this.m_procCount.GetCount())
                return;
            this.playSe(3);
            this.m_procCount.SetProc(new AppMain.ITaskAsv.FProc(this.fadeIn2));
        }

        private void fadeIn2()
        {
            float num = (float)(this.m_procCount.GetCount() / 8U);
            this.m_act[0].scale = new Vector2(num, num);
            if (8U >= this.m_procCount.GetCount())
                return;
            this.waitStart();
        }

        private void waitStart()
        {
            for (int index = 1; index < 13; ++index)
                this.m_act[index].flag[1] = false;
            this.m_procCount.SetProc(new AppMain.ITaskAsv.FProc(this.wait));
        }

        private void wait()
        {
            bool flag = false;
            for (int index = 0; index < AppMain._am_tp_touch.Length; ++index)
            {
                if (AppMain.amTpIsTouchOn(index))
                {
                    flag = true;
                    break;
                }
            }
            if (flag && 60U >= this.m_procCount.GetCount())
                return;
            this.selectStart();
        }

        private void selectStart()
        {
            for (int index = 0; index < this.m_act.Length; ++index)
            {
                this.m_act[index].flag[0] = true;
                AppMain.AoActSetFrame(this.m_act[index].act, 0.0f);
                this.m_act[index].pos = new Vector3(0.0f, 0.0f, 0.0f);
            }
            for (int index = 1; index < 13; ++index)
                this.m_act[index].flag[1] = false;
            for (int index = 13; index < 17; ++index)
                this.m_act[index].flag[1] = true;
            this.m_return = 6;
            this.m_procCount.SetProc(new AppMain.ITaskAsv.FProc(this.select));
        }

        private void select()
        {
            int trg_idx = -1;
            for (int index1 = 0; index1 < AppMain.CMain_PauseMenu.c_trg_table_select.Length; ++index1)
            {
                CTrgAoAction ctrgAoAction = this.m_trg[AppMain.CMain_PauseMenu.c_trg_table_select[index1]];
                float frame;
                if (ctrgAoAction.GetState(0U)[10] && ctrgAoAction.GetState(0U)[1])
                {
                    frame = 2f;
                    trg_idx = index1;
                }
                else
                    frame = !ctrgAoAction.GetState(0U)[0] ? 0.0f : 3f;
                for (int index2 = 0; index2 < AppMain.CMain_PauseMenu.c_btn_action_table_select[index1].Length; ++index2)
                    AppMain.AoActSetFrame(this.m_act[AppMain.CMain_PauseMenu.c_btn_action_table_select[index1][index2]].act, frame);
            }
            if (-1 != trg_idx)
            {
                int[] numArray = AppMain.CMain_PauseMenu.c_btn_action_table_select[trg_idx];
                for (int index = numArray[0]; index < numArray[2] + 1; ++index)
                    this.m_act[index].flag[0] = false;
                this.m_return = AppMain.CMain_PauseMenu.TrgIdxToReturnIdx(trg_idx);
            }
            else if (this.m_flag[8])
                this.m_return = 4;
            if (0 <= AppMain.GmMainKeyCheckPauseKeyPush())
            {
                this.pauseBtnCancelStart();
            }
            else
            {
                if (AppMain.isBackKeyPressed())
                {
                    this.m_return = 4;
                    AppMain.setBackKeyRequest(false);
                }
                switch (this.m_return)
                {
                    case 4:
                        this.playSe(1);
                        this.enterEfctStart();
                        break;
                    case 6:
                        break;
                    default:
                        this.playSe(0);
                        this.reallyStart();
                        break;
                }
            }
        }

        private void reallyStart()
        {
            for (int index = 0; index < this.m_act.Length; ++index)
            {
                AppMain.CMain_PauseMenu.SAction saction = this.m_act[index];
                saction.flag[0] = true;
                AppMain.AoActSetFrame(saction.act, 0.0f);
                saction.pos = new Vector3(0.0f, 0.0f, 0.0f);
            }
            for (int index = 7; index < 10; ++index)
                this.m_act[index].flag[1] = true;
            for (int index = 13; index < 15; ++index)
            {
                AppMain.CMain_PauseMenu.SAction saction = this.m_act[index];
                float num = 27f / 16f;
                saction.pos = new Vector3(480f, 269f, 0.0f);
                saction.scale = new Vector2(num, num);
            }
            switch (this.m_return)
            {
                case 0:
                    this.m_act[13].flag[1] = false;
                    this.m_act[14].flag[1] = true;
                    this.m_act[10].flag[1] = true;
                    this.m_act[11].flag[1] = true;
                    this.m_act[12].flag[1] = true;
                    this.m_act[15].flag[1] = false;
                    this.m_act[16].flag[1] = false;
                    for (int index = 1; index < 4; ++index)
                        this.m_act[index].pos = new Vector3(0.0f, 194f, 0.0f);
                    for (int index = 4; index < 7; ++index)
                        this.m_act[index].pos = new Vector3(0.0f, 194f, 0.0f);
                    break;
                case 2:
                case 3:
                    this.m_act[13].flag[1] = true;
                    this.m_act[14].flag[1] = false;
                    this.m_act[10].flag[1] = true;
                    this.m_act[11].flag[1] = true;
                    this.m_act[12].flag[1] = true;
                    this.m_act[15].flag[1] = false;
                    this.m_act[16].flag[1] = false;
                    for (int index = 1; index < 4; ++index)
                        this.m_act[index].pos = new Vector3(380f, 194f, 0.0f);
                    for (int index = 4; index < 7; ++index)
                        this.m_act[index].pos = new Vector3(-380f, 194f, 0.0f);
                    break;
            }
            this.m_really = 6;
            this.m_procCount.SetProc(new AppMain.ITaskAsv.FProc(this.really));
        }

        private void really()
        {
            int trg_idx = -1;
            for (int index1 = 0; index1 < AppMain.CMain_PauseMenu.c_trg_table.Length; ++index1)
            {
                CTrgAoAction ctrgAoAction = this.m_trg[AppMain.CMain_PauseMenu.c_trg_table[index1]];
                float frame;
                if (ctrgAoAction.GetState(0U)[10] && ctrgAoAction.GetState(0U)[1])
                {
                    frame = 2f;
                    trg_idx = index1;
                }
                else
                    frame = !ctrgAoAction.GetState(0U)[0] ? 0.0f : 3f;
                for (int index2 = 0; index2 < AppMain.CMain_PauseMenu.c_btn_action_table[index1].Length; ++index2)
                    AppMain.AoActSetFrame(this.m_act[AppMain.CMain_PauseMenu.c_btn_action_table[index1][index2]].act, frame);
            }
            if (-1 != trg_idx)
            {
                int[] numArray = AppMain.CMain_PauseMenu.c_btn_action_table[trg_idx];
                for (int index = numArray[0]; index < numArray[2] + 1; ++index)
                    this.m_act[index].flag[0] = false;
                this.m_really = AppMain.CMain_PauseMenu.TrgIdxToReturnIdx(trg_idx);
            }
            else if (this.m_flag[8])
                this.m_really = 4;
            if (0 <= AppMain.GmMainKeyCheckPauseKeyPush())
            {
                this.m_return = 4;
                this.pauseBtnCancelStart();
            }
            else if (AppMain.isBackKeyPressed())
            {
                this.m_return = 4;
                AppMain.setBackKeyRequest(false);
                this.playSe(1);
                this.selectStart();
            }
            else
            {
                if (6 == this.m_really)
                    return;
                if (this.m_return == this.m_really)
                {
                    this.playSe(0);
                    this.enterEfctStart();
                }
                else
                {
                    this.playSe(1);
                    this.selectStart();
                }
            }
        }

        private void enterEfctStart()
        {
            this.m_procCount.SetProc(new AppMain.ITaskAsv.FProc(this.enterEfct));
        }

        private void enterEfct()
        {
            if (10U >= this.m_procCount.GetCount())
                return;
            this.fadeOutStart();
        }

        private void pauseBtnCancelStart()
        {
            this.playSe(0);
            this.m_procCount.SetProc(new AppMain.ITaskAsv.FProc(this.pauseBtnCancel));
        }

        private void pauseBtnCancel()
        {
            if (15U >= this.m_procCount.GetCount())
                return;
            this.fadeOutStart();
        }

        private void fadeOutStart()
        {
            foreach (AppMain.CMain_PauseMenu.SAction saction in this.m_act)
            {
                saction.flag[0] = true;
                saction.flag[1] = true;
            }
            this.m_act[0].flag[0] = false;
            this.m_act[0].flag[1] = false;
            this.playSe(3);
            this.m_procCount.SetProc(new AppMain.ITaskAsv.FProc(this.fadeOut));
        }

        private void fadeOut()
        {
            float num = 1f - (float)(this.m_procCount.GetCount() / 8U);
            this.m_act[0].scale = new Vector2(num, num);
            if (8U >= this.m_procCount.GetCount())
                return;
            foreach (AppMain.CMain_PauseMenu.SAction saction in this.m_act)
                AppMain.AoActDelete(saction.act);
            AppMain.GsSoundFreeSeHandle(this.m_se_handle);
            this.m_flag[7] = false;
            this.DetachTask();
        }

        private void releasingStart()
        {
            foreach (AppMain.AOS_TEXTURE tex in this.m_tex)
                AppMain.AoTexRelease(tex);
            this.AttachTask("gmPauseMenu.Flush", 28928U, 0U, 0U);
            this.m_procCount.SetProc(new AppMain.ITaskAsv.FProc(this.releasing));
        }

        private void releasing()
        {
            bool flag = true;
            foreach (AppMain.AOS_TEXTURE tex in this.m_tex)
            {
                if (!AppMain.AoTexIsReleased(tex))
                {
                    flag = false;
                    break;
                }
            }
            if (!flag)
                return;
            this.m_flag[5] = false;
            this.m_flag[6] = false;
            this.DetachTask();
        }

        private void playSe(int se)
        {
            AppMain.GmSoundPlaySE(AppMain.CMain_PauseMenu.c_se_name_tbl[se]);
        }

        private static bool canGoStageSelect()
        {
            return AppMain.GsMainSysIsStageClear(0) && !AppMain.GsTrialIsTrial();
        }

        private static bool isSpecialStage()
        {
            return AppMain.GSM_MAIN_STAGE_IS_SPSTAGE();
        }

        private static int TrgIdxToReturnIdx(int trg_idx)
        {
            int num = AppMain.CMain_PauseMenu.c_return_table[trg_idx];
            if (!AppMain.CMain_PauseMenu.canGoStageSelect() && 2 == num)
                num = 3;
            return num;
        }

        private class EReturn
        {
            public const int Retry = 0;
            public const int Option = 1;
            public const int Back = 2;
            public const int MainMenu = 3;
            public const int Cancel = 4;
            public const int Max = 5;
            public const int None = 6;
        }

        private class BFlag
        {
            public const int Create = 0;
            public const int NoUpdate = 1;
            public const int NoDraw = 2;
            public const int LoadFile = 3;
            public const int LoadedFile = 4;
            public const int CreateTexture = 5;
            public const int CreatedTexture = 6;
            public const int Start = 7;
            public const int ReqCancel = 8;
            public const int Max = 9;
            public const int None = 10;
        }

        private class EMemFile
        {
            public const int Ama = 0;
            public const int AmaLang = 1;
            public const int Amb = 2;
            public const int AmbLang = 3;
            public const int Max = 4;
            public const int None = 5;
        }

        private class ETex
        {
            public const int Amb = 0;
            public const int AmbLang = 1;
            public const int Max = 2;
            public const int None = 3;
        }

        private class EAct
        {
            public const int Bgi = 0;
            public const int Btn1Left = 1;
            public const int Btn1Center = 2;
            public const int Btn1Right = 3;
            public const int Btn3Left = 4;
            public const int Btn3Center = 5;
            public const int Btn3Right = 6;
            public const int Btn4Left = 7;
            public const int Btn4Center = 8;
            public const int Btn4Right = 9;
            public const int Retry = 10;
            public const int Back = 11;
            public const int Cancel = 12;
            public const int MsgRetry = 13;
            public const int MsgReturn = 14;
            public const int No = 15;
            public const int Yes = 16;
            public const int Max = 17;
            public const int None = 18;
        }

        private class ETrg
        {
            public const int Btn1 = 0;
            public const int Btn3 = 1;
            public const int Btn4 = 2;
            public const int Max = 3;
            public const int None = 4;
        }

        private class SAction
        {
            public readonly bool[] flag = new bool[2];
            public AppMain.AOS_ACTION act;
            public AppMain.AOS_TEXTURE tex;
            public Vector2 scale;
            public Vector3 pos;
            public AppMain.AOS_ACT_COL color;

            public void AcmInit()
            {
                this.pos = new Vector3(0.0f, 0.0f, 0.0f);
                this.scale = new Vector2(1f, 1f);
                this.color.c = uint.MaxValue;
            }

            public void Update()
            {
                AppMain.AoActAcmPush();
                float frame = this.flag[0] ? 0.0f : 1f;
                AppMain.AoActSetTexture(AppMain.AoTexGetTexList(this.tex));
                if (1.0 != (double)this.scale.X || 1.0 != (double)this.scale.Y)
                    AppMain.AoActAcmApplyScale(this.scale.X, this.scale.Y);
                if (0.0 != (double)this.pos.X || 0.0 != (double)this.pos.Y || 0.0 != (double)this.pos.Z)
                    AppMain.AoActAcmApplyTrans(this.pos.X, this.pos.Y, this.pos.Z);
                if (uint.MaxValue != this.color.c)
                    AppMain.AoActAcmApplyColor(this.color);
                AppMain.AoActUpdate(this.act, frame);
                AppMain.AoActAcmPop();
            }

            public void Draw()
            {
                if (this.flag[1])
                    return;
                AppMain.AoActSortRegAction(this.act);
            }

            public class BFlag
            {
                public const int NoUpdate = 0;
                public const int NoDraw = 1;
                public const int Max = 2;
                public const int None = 3;
            }
        }

        private class ESe
        {
            public const int Enter = 0;
            public const int Cancel = 1;
            public const int Window = 2;
            public const int Pause = 3;
            public const int Max = 4;
            public const int None = 5;
        }

        private class SLocalCreateActionTable
        {
            public int file;
            public int tex;
            public int idx;
        }
    }
}
