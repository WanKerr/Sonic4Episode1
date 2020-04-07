using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using accel;
using er;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using mpp;

public partial class AppMain
{
    public class CProc<TCrtpType> where TCrtpType : class, AppMain.IFunctor
    {
        private AppMain.CProc<TCrtpType>.FProc m_proc;
        private TCrtpType m_it;

        public virtual void operator_brackets()
        {
            if ((object)this.m_it == null || this.IsNoneProc())
                return;
            this.m_proc();
        }

        public bool IsNoneProc()
        {
            return this.m_proc == null;
        }

        public bool IsProc(AppMain.CProc<TCrtpType>.FProc proc)
        {
            return this.m_proc == proc;
        }

        public bool IsProc()
        {
            return this.IsNoneProc();
        }

        public AppMain.CProc<TCrtpType>.FProc GetProc()
        {
            return this.m_proc;
        }

        public void SetTarget(TCrtpType it)
        {
            this.m_it = it;
            this.SetProc();
        }

        public void SetTarget()
        {
            this.m_it = default(TCrtpType);
            this.SetProc();
        }

        public void SetProc(AppMain.CProc<TCrtpType>.FProc proc)
        {
            this.m_proc = proc;
        }

        public void SetProc()
        {
            this.m_proc = (AppMain.CProc<TCrtpType>.FProc)null;
        }

        public CProc()
        {
            this.m_it = default(TCrtpType);
            this.m_proc = (AppMain.CProc<TCrtpType>.FProc)null;
        }

        public CProc(TCrtpType it)
        {
            this.m_it = it;
            this.m_proc = (AppMain.CProc<TCrtpType>.FProc)null;
        }

        ~CProc()
        {
        }

        public delegate void FProc();
    }

    public class CProcCount<TCrtpType> : AppMain.CProc<TCrtpType> where TCrtpType : class, AppMain.IFunctor
    {
        private ulong m_counter;

        public override void operator_brackets()
        {
            ++this.m_counter;
            base.operator_brackets();
        }

        public ulong GetCount()
        {
            return this.m_counter;
        }

        public new void SetProc(AppMain.CProc<TCrtpType>.FProc proc)
        {
            this.ResetCounter();
            base.SetProc(proc);
        }

        public new void SetProc()
        {
            this.ResetCounter();
            base.SetProc();
        }

        public CProcCount()
        {
        }

        public CProcCount(TCrtpType it)
          : base(it)
        {
        }

        ~CProcCount()
        {
        }

        protected void ResetCounter()
        {
            this.m_counter = ulong.MaxValue;
        }
    }

    public class CMainTask<TCrtpType> : AppMain.CProcCount<AppMain.CMain>, AppMain.IFunctor
    {
        public AppMain.ITaskLink m_pTaskLink;

        public CMainTask()
        {
            this.SetTarget((AppMain.CMain)(object)this);
            this.m_pTaskLink = new AppMain.ITaskLink((AppMain.IFunctor)this);
        }

        ~CMainTask()
        {
        }

        public override void operator_brackets()
        {
            base.operator_brackets();
        }
    }
    public class CMain : AppMain.CMainTask<AppMain.CMain>
    {
        public static readonly int[] c_return_table = new int[2]
        {
      0,
      1
        };
        private static AppMain.CMain.SLocalUnfoldTable[] c_local_unfold_table = new AppMain.CMain.SLocalUnfoldTable[6]
        {
      new AppMain.CMain.SLocalUnfoldTable(AppMain.CMain.EMemFile.Type.None, 0U),
      new AppMain.CMain.SLocalUnfoldTable(AppMain.CMain.EMemFile.Type.None, 0U),
      new AppMain.CMain.SLocalUnfoldTable(AppMain.CMain.EMemFile.Type.Global, 0U),
      new AppMain.CMain.SLocalUnfoldTable(AppMain.CMain.EMemFile.Type.Global, 1U),
      new AppMain.CMain.SLocalUnfoldTable(AppMain.CMain.EMemFile.Type.Lang, 0U),
      new AppMain.CMain.SLocalUnfoldTable(AppMain.CMain.EMemFile.Type.Lang, 1U)
        };
        public static readonly AppMain.CMain.SLocalCreateActionTable[] c_local_create_action_table = new AppMain.CMain.SLocalCreateActionTable[9]
        {
      new AppMain.CMain.SLocalCreateActionTable(AppMain.CMain.EFile.EFileEnum.LangAma, AppMain.CMain.ETex.Type.Lang, 0),
      new AppMain.CMain.SLocalCreateActionTable(AppMain.CMain.EFile.EFileEnum.GlobalAma, AppMain.CMain.ETex.Type.Global, 0),
      new AppMain.CMain.SLocalCreateActionTable(AppMain.CMain.EFile.EFileEnum.GlobalAma, AppMain.CMain.ETex.Type.Global, 1),
      new AppMain.CMain.SLocalCreateActionTable(AppMain.CMain.EFile.EFileEnum.GlobalAma, AppMain.CMain.ETex.Type.Global, 2),
      new AppMain.CMain.SLocalCreateActionTable(AppMain.CMain.EFile.EFileEnum.GlobalAma, AppMain.CMain.ETex.Type.Global, 3),
      new AppMain.CMain.SLocalCreateActionTable(AppMain.CMain.EFile.EFileEnum.GlobalAma, AppMain.CMain.ETex.Type.Global, 4),
      new AppMain.CMain.SLocalCreateActionTable(AppMain.CMain.EFile.EFileEnum.GlobalAma, AppMain.CMain.ETex.Type.Global, 5),
      new AppMain.CMain.SLocalCreateActionTable(AppMain.CMain.EFile.EFileEnum.LangAma, AppMain.CMain.ETex.Type.Lang, 1),
      new AppMain.CMain.SLocalCreateActionTable(AppMain.CMain.EFile.EFileEnum.LangAma, AppMain.CMain.ETex.Type.Lang, 2)
        };
        public static readonly AppMain.CMain.EAct.Type[] c_local_create_trg_table = new AppMain.CMain.EAct.Type[2]
        {
      AppMain.CMain.EAct.Type.BuyCenter,
      AppMain.CMain.EAct.Type.CancelCenter
        };
        public static readonly AppMain.CMain.EAct.Type[][] c_btn_action_table = new AppMain.CMain.EAct.Type[2][]
        {
      new AppMain.CMain.EAct.Type[3]
      {
        AppMain.CMain.EAct.Type.BuyLeft,
        AppMain.CMain.EAct.Type.BuyCenter,
        AppMain.CMain.EAct.Type.BuyRight
      },
      new AppMain.CMain.EAct.Type[3]
      {
        AppMain.CMain.EAct.Type.CancelLeft,
        AppMain.CMain.EAct.Type.CancelCenter,
        AppMain.CMain.EAct.Type.CancelRight
      }
        };
        private BitArray m_flag = new BitArray(8);
        private readonly AppMain.AMS_FS[] m_fs = new AppMain.AMS_FS[2];
        private readonly object[] m_file = new object[6];
        private AppMain.AOS_TEXTURE[] m_tex = AppMain.New<AppMain.AOS_TEXTURE>(2);
        private AppMain.CMain.SAction[] m_act = AppMain.New<AppMain.CMain.SAction>(9);
        private CTrgAoAction[] m_trg = AppMain.New<CTrgAoAction>(2);
        private int[] m_result;

        public override void operator_brackets()
        {
            if (this.m_flag[0])
                this.preUpdate();
            base.operator_brackets();
            if (!this.m_flag[0])
                return;
            this.update();
            this.draw();
        }

        public bool Create()
        {
            this.m_flag[0] = true;
            return true;
        }

        public void Release()
        {
            if (!this.m_flag[0])
                return;
            if (this.m_flag[3])
            {
                for (int index = 0; index < 2; ++index)
                    this.m_file[index] = (object)null;
                this.m_flag[3] = false;
                this.m_flag[4] = false;
            }
            this.m_pTaskLink.DetachTask();
            this.m_flag.SetAll(false);
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

        public void Start(int[] result)
        {
            this.m_result = result;
            this.fadeInStart();
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

        ~CMain()
        {
        }

        private void preUpdate()
        {
            if (!this.m_flag[7] || this.m_flag[1])
                return;
            for (int index = 0; index < 2; ++index)
                this.m_trg[index].Update();
        }

        private void update()
        {
            if (!this.m_flag[7] || this.m_flag[1])
                return;
            for (int index = 0; index < 2; ++index)
                this.m_trg[index].Update();
        }

        private void draw()
        {
            if (!AppMain._am_sample_draw_enable || !this.m_flag[7] || this.m_flag[1])
                return;
            for (int index = 0; index < 9; ++index)
                this.m_act[index].Draw();
        }

        private void fileLoadingStart()
        {
            this.m_fs[0] = AppMain.amFsReadBackground(AppMain.c_global);
            int language = AppMain.GsEnvGetLanguage();
            this.m_fs[1] = AppMain.amFsReadBackground(AppMain.c_lang[language]);
            this.m_flag[3] = true;
            this.m_pTaskLink.AttachTask("dmBuyScreen::Load", AppMain.c_priority, AppMain.c_user, AppMain.c_attribute);
            this.SetProc(new AppMain.CProc<AppMain.CMain>.FProc(this.fileLoading));
        }

        public static void fileLoadingS(object pArg)
        {
            ((AppMain.CMain)pArg).fileLoading();
        }

        private void fileLoading()
        {
            bool flag = true;
            for (int index = 0; index < 2; ++index)
            {
                if (!AppMain.amFsIsComplete(this.m_fs[index]))
                {
                    flag = false;
                    break;
                }
            }
            if (!flag)
                return;
            for (int index = 0; index < 2; ++index)
                this.m_file[index] = (object)this.m_fs[index];
            for (uint index = 0; index < 6U; ++index)
            {
                AppMain.CMain.SLocalUnfoldTable slocalUnfoldTable = AppMain.CMain.c_local_unfold_table[(int)index];
                if (slocalUnfoldTable.file < AppMain.CMain.EMemFile.Type.Max)
                {
                    AppMain.AmbChunk buf = AppMain.amBindGet(this.m_fs[(int)slocalUnfoldTable.file], (int)slocalUnfoldTable.index, out string _);
                    object obj = !AppMain.AoActIsAma(buf.array, buf.offset) ? (object)AppMain.readAMBFile(buf) : (object)AppMain.readAMAFile((object)buf);
                    this.m_file[(int)index] = obj;
                }
            }
            this.m_flag[4] = true;
            this.m_pTaskLink.DetachTask();
        }

        private void creatingStart()
        {
            AppMain.CMain.EFile.EFileEnum[] efileEnumArray = new AppMain.CMain.EFile.EFileEnum[2]
            {
        AppMain.CMain.EFile.EFileEnum.GlobalAmb,
        AppMain.CMain.EFile.EFileEnum.LangAmb
            };
            for (int index = 0; index < 2; ++index)
            {
                AppMain.AoTexBuild(this.m_tex[index], (AppMain.AMS_AMB_HEADER)this.m_file[(int)efileEnumArray[index]]);
                AppMain.AoTexLoad(this.m_tex[index]);
            }
            this.m_flag[5] = true;
            this.m_pTaskLink.AttachTask("dmBuyScreen::Build", AppMain.c_priority, AppMain.c_user, AppMain.c_attribute);
            this.SetProc(new AppMain.CProc<AppMain.CMain>.FProc(this.creating));
        }

        public static void creatingS(object pArg)
        {
            ((AppMain.CMain)pArg).creating();
        }

        public void creating()
        {
            bool flag = true;
            for (int index = 0; index < 2; ++index)
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
            this.m_pTaskLink.DetachTask();
        }

        private void fadeInStart()
        {
            for (uint index = 0; (long)index < (long)AppMain.CMain.c_local_create_action_table.Length; ++index)
            {
                AppMain.CMain.SLocalCreateActionTable createActionTable = AppMain.CMain.c_local_create_action_table[(int)index];
                AppMain.A2S_AMA_HEADER ama = AppMain.readAMAFile(this.m_file[(int)createActionTable.file]);
                AppMain.CMain.SAction saction = this.m_act[(int)index];
                saction.act = AppMain.AoActCreate(ama, (uint)createActionTable.idx);
                saction.tex = this.m_tex[(int)(uint)createActionTable.tex];
                saction.flag[0] = true;
                saction.AcmInit();
            }
            for (uint index = 0; (long)index < (long)AppMain.CMain.c_local_create_trg_table.Length; ++index)
            {
                AppMain.CMain.SAction saction = this.m_act[(int)AppMain.CMain.c_local_create_trg_table[(int)index]];
                this.m_trg[(int)index].Create(saction.act);
            }
            AppMain.IzFadeInitEasy(0U, 0U, 30f);
            this.m_flag[7] = true;
            this.m_pTaskLink.AttachTask("dmBuyScreen::Execute", AppMain.c_priority, AppMain.c_user, AppMain.c_attribute);
            this.SetProc(new AppMain.CProc<AppMain.CMain>.FProc(this.fadeIn));
        }

        private void fadeIn()
        {
            if (!AppMain.IzFadeIsEnd())
                return;
            AppMain.IzFadeExit();
            this.waitStart();
        }

        private void waitStart()
        {
            this.SetProc(new AppMain.CProc<AppMain.CMain>.FProc(this.wait));
        }

        private void wait()
        {
            bool flag = false;
            int index = 0;
            for (int length = AppMain._am_tp_touch.Length; index < length; ++index)
            {
                if (AppMain.amTpIsTouchOn(index))
                {
                    flag = true;
                    break;
                }
            }
            if (flag && 60UL >= this.GetCount())
                return;
            this.selectStart();
        }

        private void selectStart()
        {
            this.SetProc(new AppMain.CProc<AppMain.CMain>.FProc(this.select));
        }

        private void select()
        {
            int trg_idx = -1;
            for (int index1 = 0; index1 < 2; ++index1)
            {
                CTrgAoAction ctrgAoAction = this.m_trg[index1];
                float frame;
                if (ctrgAoAction.GetState(0U)[10] && ctrgAoAction.GetState(0U)[1])
                {
                    frame = 1f;
                    trg_idx = index1;
                }
                else
                    frame = !ctrgAoAction.GetState(0U)[0] ? 0.0f : 2f;
                for (uint index2 = 0; (long)index2 < (long)AppMain.arrayof((Array)AppMain.CMain.c_btn_action_table[index1]); ++index2)
                    AppMain.AoActSetFrame(this.m_act[(int)AppMain.CMain.c_btn_action_table[index1][(int)index2]].act, frame);
            }
            if (-1 == trg_idx)
                return;
            AppMain.CMain.EAct.Type[] typeArray = AppMain.CMain.c_btn_action_table[trg_idx];
            for (int index = (int)typeArray[0]; (AppMain.CMain.EAct.Type)index < typeArray[2] + 1; ++index)
                this.m_act[index].flag[0] = false;
            AppMain.DmSoundPlaySE("Ok");
            this.m_result[0] = AppMain.CMain.TrgIdxToReturnIdx(trg_idx);
            this.enterEfctStart();
        }

        private void enterEfctStart()
        {
            this.SetProc(new AppMain.CProc<AppMain.CMain>.FProc(this.enterEfct));
        }

        private void enterEfct()
        {
            if (30UL >= this.GetCount())
                return;
            if (this.m_result[0] == 0)
            {
                XBOXLive.showGuide();
                XBOXLive.isTrial(true);
                this.fadeOutStart();
            }
            else
                this.fadeOutStart();
        }

        private void fadeOutStart()
        {
            AppMain.IzFadeInitEasy(0U, 1U, 30f);
            this.SetProc(new AppMain.CProc<AppMain.CMain>.FProc(this.fadeOut));
        }

        private void fadeOut()
        {
            if (!AppMain.IzFadeIsEnd())
                return;
            for (int index = 0; index < 9; ++index)
                AppMain.AoActDelete(this.m_act[index].act);
            this.m_flag[7] = false;
            this.m_pTaskLink.DetachTask();
        }

        private void releasingStart()
        {
            for (int index = 0; index < 2; ++index)
                AppMain.AoTexRelease(this.m_tex[index]);
            this.m_pTaskLink.AttachTask("dmBuyScreen::Flush", AppMain.c_priority, AppMain.c_user, AppMain.c_attribute);
            this.SetProc(new AppMain.CProc<AppMain.CMain>.FProc(this.releasing));
        }

        public static void releasingS(object pArg)
        {
            ((AppMain.CMain)pArg).releasing();
        }

        private void releasing()
        {
            bool flag = true;
            for (int index = 0; index < 2; ++index)
            {
                if (!AppMain.AoTexIsReleased(this.m_tex[index]))
                {
                    flag = false;
                    break;
                }
            }
            if (!flag)
                return;
            this.m_flag[5] = false;
            this.m_flag[6] = false;
            this.m_pTaskLink.DetachTask();
        }

        private static int TrgIdxToReturnIdx(int trg_idx)
        {
            return AppMain.CMain.c_return_table[trg_idx];
        }

        private class BFlag
        {
            public enum EFlag
            {
                Create,
                NoUpdate,
                NoDraw,
                LoadFile,
                LoadedFile,
                CreateTexture,
                CreatedTexture,
                Start,
                Max,
                None,
            }
        }

        public class EMemFile
        {
            public enum Type
            {
                Global,
                Lang,
                Max,
                None,
            }
        }

        public class EFile : AppMain.CMain.EMemFile
        {
            public enum EFileEnum
            {
                GlobalAma = 2,
                GlobalAmb = 3,
                LangAma = 4,
                LangAmb = 5,
                Max = 6,
                None = 7,
            }
        }

        public class ETex
        {
            public enum Type
            {
                Global,
                Lang,
                Max,
                None,
            }
        }

        public class EAct
        {
            public enum Type
            {
                Bgi,
                BuyLeft,
                BuyCenter,
                BuyRight,
                CancelLeft,
                CancelCenter,
                CancelRight,
                Buy,
                Cancel,
                Max,
                None,
            }
        }

        private class ETrg
        {
            public enum Type
            {
                Buy,
                Cancel,
                Max,
                None,
            }
        }

        public class SAction
        {
            public bool[] flag = new bool[3];
            public AppMain.AOS_ACTION act;
            public AppMain.AOS_TEXTURE tex;
            public CArray2<float> scale;
            public CArray3<float> pos;
            public AppMain.AOS_ACT_COL color;

            public void AcmInit()
            {
                this.pos = accel.CArray3<float>.initializer(0.0f, 0.0f, 0.0f);
                this.scale = CArray2<float>.initializer(1f, 1f);
                this.color.c = uint.MaxValue;
                this.act.sprite.texlist = this.tex.texlist;
            }

            public void Update()
            {
                AppMain.AoActAcmPush();
                float frame = this.flag[0] ? 0.0f : 1f;
                AppMain.AoActSetTexture(AppMain.AoTexGetTexList(this.tex));
                if (!CArray2<float>.initializer(1f, 1f).equals(this.scale))
                    AppMain.AoActAcmApplyScale(this.scale.x, this.scale.y);
                if (!CArray3<float>.initializer(0.0f, 0.0f, 0.0f).equals(this.pos))
                    AppMain.AoActAcmApplyTrans(this.pos.x, this.pos.y, this.pos.z);
                if (uint.MaxValue != this.color.c)
                    AppMain.AoActAcmApplyColor(this.color);
                AppMain.AoActUpdate(this.act, frame);
                AppMain.AoActAcmPop();
            }

            public void Draw()
            {
                if (this.flag[1])
                    return;
                if (this.flag[2])
                    AppMain.AoActSortRegAction(this.act);
                else
                    AppMain.AoActDraw(this.act);
            }

            public class BFlag
            {
                public enum ESAction
                {
                    NoUpdate,
                    NoDraw,
                    SortDraw,
                    Max,
                    None,
                }
            }
        }

        private class SLocalUnfoldTable
        {
            public AppMain.CMain.EMemFile.Type file;
            public uint index;

            public SLocalUnfoldTable(AppMain.CMain.EMemFile.Type type, uint _index)
            {
                this.file = type;
                this.index = _index;
            }
        }

        public struct SLocalCreateActionTable
        {
            public AppMain.CMain.EFile.EFileEnum file;
            public AppMain.CMain.ETex.Type tex;
            public int idx;

            public SLocalCreateActionTable(
              AppMain.CMain.EFile.EFileEnum file,
              AppMain.CMain.ETex.Type tex,
              int idx)
            {
                this.file = file;
                this.tex = tex;
                this.idx = idx;
            }
        }
    }
}