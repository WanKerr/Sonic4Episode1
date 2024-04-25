using System.Collections;
using accel;
using er;

public partial class AppMain
{
    public class CProc<TCrtpType> where TCrtpType : class, IFunctor
    {
        private FProc m_proc;
        private TCrtpType m_it;

        public virtual void operator_brackets()
        {
            if (m_it == null || this.IsNoneProc())
                return;
            this.m_proc();
        }

        public bool IsNoneProc()
        {
            return this.m_proc == null;
        }

        public bool IsProc(FProc proc)
        {
            return this.m_proc == proc;
        }

        public bool IsProc()
        {
            return this.IsNoneProc();
        }

        public FProc GetProc()
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

        public void SetProc(FProc proc)
        {
            this.m_proc = proc;
        }

        public void SetProc()
        {
            this.m_proc = null;
        }

        public CProc()
        {
            this.m_it = default(TCrtpType);
            this.m_proc = null;
        }

        public CProc(TCrtpType it)
        {
            this.m_it = it;
            this.m_proc = null;
        }

        ~CProc()
        {
        }

        public delegate void FProc();
    }

    public class CProcCount<TCrtpType> : CProc<TCrtpType> where TCrtpType : class, IFunctor
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

        public new void SetProc(FProc proc)
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

    public class CMainTask<TCrtpType> : CProcCount<CMain>, IFunctor
    {
        public ITaskLink m_pTaskLink;

        public CMainTask()
        {
            this.SetTarget((CMain)(object)this);
            this.m_pTaskLink = new ITaskLink(this);
        }

        ~CMainTask()
        {
        }

        public override void operator_brackets()
        {
            base.operator_brackets();
        }
    }
    public class CMain : CMainTask<CMain>
    {
        public static readonly int[] c_return_table = new int[2]
        {
      0,
      1
        };
        private static SLocalUnfoldTable[] c_local_unfold_table = new SLocalUnfoldTable[6]
        {
      new SLocalUnfoldTable(EMemFile.Type.None, 0U),
      new SLocalUnfoldTable(EMemFile.Type.None, 0U),
      new SLocalUnfoldTable(EMemFile.Type.Global, 0U),
      new SLocalUnfoldTable(EMemFile.Type.Global, 1U),
      new SLocalUnfoldTable(EMemFile.Type.Lang, 0U),
      new SLocalUnfoldTable(EMemFile.Type.Lang, 1U)
        };
        public static readonly SLocalCreateActionTable[] c_local_create_action_table = new SLocalCreateActionTable[9]
        {
      new SLocalCreateActionTable(EFile.EFileEnum.LangAma, ETex.Type.Lang, 0),
      new SLocalCreateActionTable(EFile.EFileEnum.GlobalAma, ETex.Type.Global, 0),
      new SLocalCreateActionTable(EFile.EFileEnum.GlobalAma, ETex.Type.Global, 1),
      new SLocalCreateActionTable(EFile.EFileEnum.GlobalAma, ETex.Type.Global, 2),
      new SLocalCreateActionTable(EFile.EFileEnum.GlobalAma, ETex.Type.Global, 3),
      new SLocalCreateActionTable(EFile.EFileEnum.GlobalAma, ETex.Type.Global, 4),
      new SLocalCreateActionTable(EFile.EFileEnum.GlobalAma, ETex.Type.Global, 5),
      new SLocalCreateActionTable(EFile.EFileEnum.LangAma, ETex.Type.Lang, 1),
      new SLocalCreateActionTable(EFile.EFileEnum.LangAma, ETex.Type.Lang, 2)
        };
        public static readonly EAct.Type[] c_local_create_trg_table = new EAct.Type[2]
        {
      EAct.Type.BuyCenter,
      EAct.Type.CancelCenter
        };
        public static readonly EAct.Type[][] c_btn_action_table = new EAct.Type[2][]
        {
      new EAct.Type[3]
      {
        EAct.Type.BuyLeft,
        EAct.Type.BuyCenter,
        EAct.Type.BuyRight
      },
      new EAct.Type[3]
      {
        EAct.Type.CancelLeft,
        EAct.Type.CancelCenter,
        EAct.Type.CancelRight
      }
        };
        private BitArray m_flag = new BitArray(8);
        private readonly AMS_FS[] m_fs = new AMS_FS[2];
        private readonly object[] m_file = new object[6];
        private AOS_TEXTURE[] m_tex = New<AOS_TEXTURE>(2);
        private SAction[] m_act = New<SAction>(9);
        private CTrgAoAction[] m_trg = New<CTrgAoAction>(2);
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
                    this.m_file[index] = null;
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
            if (!_am_sample_draw_enable || !this.m_flag[7] || this.m_flag[1])
                return;
            for (int index = 0; index < 9; ++index)
                this.m_act[index].Draw();
        }

        private void fileLoadingStart()
        {
            this.m_fs[0] = amFsReadBackground(c_global);
            int language = GsEnvGetLanguage();
            this.m_fs[1] = amFsReadBackground(c_lang[language]);
            this.m_flag[3] = true;
            this.m_pTaskLink.AttachTask("dmBuyScreen::Load", c_priority, c_user, c_attribute);
            this.SetProc(new FProc(this.fileLoading));
        }

        public static void fileLoadingS(object pArg)
        {
            ((CMain)pArg).fileLoading();
        }

        private void fileLoading()
        {
            bool flag = true;
            for (int index = 0; index < 2; ++index)
            {
                if (!amFsIsComplete(this.m_fs[index]))
                {
                    flag = false;
                    break;
                }
            }
            if (!flag)
                return;
            for (int index = 0; index < 2; ++index)
                this.m_file[index] = this.m_fs[index];
            for (uint index = 0; index < 6U; ++index)
            {
                var slocalUnfoldTable = c_local_unfold_table[(int)index];
                if (slocalUnfoldTable.file < EMemFile.Type.Max)
                {
                    var buf = amBindGet(this.m_fs[(int)slocalUnfoldTable.file], (int)slocalUnfoldTable.index, out string _);
                    object obj = !AoActIsAma(buf.array, buf.offset) ? readAMBFile(buf) : (object)readAMAFile(buf);
                    this.m_file[(int)index] = obj;
                }
            }
            this.m_flag[4] = true;
            this.m_pTaskLink.DetachTask();
        }

        private void creatingStart()
        {
            EFile.EFileEnum[] efileEnumArray = new EFile.EFileEnum[2]
            {
        EFile.EFileEnum.GlobalAmb,
        EFile.EFileEnum.LangAmb
            };
            for (int index = 0; index < 2; ++index)
            {
                AoTexBuild(this.m_tex[index], (AMS_AMB_HEADER)this.m_file[(int)efileEnumArray[index]]);
                AoTexLoad(this.m_tex[index]);
            }
            this.m_flag[5] = true;
            this.m_pTaskLink.AttachTask("dmBuyScreen::Build", c_priority, c_user, c_attribute);
            this.SetProc(new FProc(this.creating));
        }

        public static void creatingS(object pArg)
        {
            ((CMain)pArg).creating();
        }

        public void creating()
        {
            bool flag = true;
            for (int index = 0; index < 2; ++index)
            {
                if (!AoTexIsLoaded(this.m_tex[index]))
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
            for (uint index = 0; index < c_local_create_action_table.Length; ++index)
            {
                var createActionTable = c_local_create_action_table[(int)index];
                var ama = readAMAFile(this.m_file[(int)createActionTable.file]);
                var saction = this.m_act[(int)index];
                saction.act = AoActCreate(ama, (uint)createActionTable.idx);
                saction.tex = this.m_tex[(int)(uint)createActionTable.tex];
                saction.flag[0] = true;
                saction.AcmInit();
            }
            for (uint index = 0; index < c_local_create_trg_table.Length; ++index)
            {
                var saction = this.m_act[(int)c_local_create_trg_table[(int)index]];
                this.m_trg[(int)index].Create(saction.act);
            }
            IzFadeInitEasy(0U, 0U, 30f);
            this.m_flag[7] = true;
            this.m_pTaskLink.AttachTask("dmBuyScreen::Execute", c_priority, c_user, c_attribute);
            this.SetProc(new FProc(this.fadeIn));
        }

        private void fadeIn()
        {
            if (!IzFadeIsEnd())
                return;
            IzFadeExit();
            this.waitStart();
        }

        private void waitStart()
        {
            this.SetProc(new FProc(this.wait));
        }

        private void wait()
        {
            bool flag = false;
            int index = 0;
            for (int length = _am_tp_touch.Length; index < length; ++index)
            {
                if (amTpIsTouchOn(index))
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
            this.SetProc(new FProc(this.select));
        }

        private void select()
        {
            int trg_idx = -1;
            for (int index1 = 0; index1 < 2; ++index1)
            {
                var ctrgAoAction = this.m_trg[index1];
                float frame;
                if (ctrgAoAction.GetState(0U)[10] && ctrgAoAction.GetState(0U)[1])
                {
                    frame = 1f;
                    trg_idx = index1;
                }
                else
                    frame = !ctrgAoAction.GetState(0U)[0] ? 0.0f : 2f;
                for (uint index2 = 0; index2 < arrayof(c_btn_action_table[index1]); ++index2)
                    AoActSetFrame(this.m_act[(int)c_btn_action_table[index1][(int)index2]].act, frame);
            }
            if (-1 == trg_idx)
                return;
            EAct.Type[] typeArray = c_btn_action_table[trg_idx];
            for (int index = (int)typeArray[0]; (EAct.Type)index < typeArray[2] + 1; ++index)
                this.m_act[index].flag[0] = false;
            DmSoundPlaySE("Ok");
            this.m_result[0] = TrgIdxToReturnIdx(trg_idx);
            this.enterEfctStart();
        }

        private void enterEfctStart()
        {
            this.SetProc(new FProc(this.enterEfct));
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
            IzFadeInitEasy(0U, 1U, 30f);
            this.SetProc(new FProc(this.fadeOut));
        }

        private void fadeOut()
        {
            if (!IzFadeIsEnd())
                return;
            for (int index = 0; index < 9; ++index)
                AoActDelete(this.m_act[index].act);
            this.m_flag[7] = false;
            this.m_pTaskLink.DetachTask();
        }

        private void releasingStart()
        {
            for (int index = 0; index < 2; ++index)
                AoTexRelease(this.m_tex[index]);
            this.m_pTaskLink.AttachTask("dmBuyScreen::Flush", c_priority, c_user, c_attribute);
            this.SetProc(new FProc(this.releasing));
        }

        public static void releasingS(object pArg)
        {
            ((CMain)pArg).releasing();
        }

        private void releasing()
        {
            bool flag = true;
            for (int index = 0; index < 2; ++index)
            {
                if (!AoTexIsReleased(this.m_tex[index]))
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
            return c_return_table[trg_idx];
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

        public class EFile : EMemFile
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
            public AOS_ACTION act;
            public AOS_TEXTURE tex;
            public CArray2<float> scale;
            public CArray3<float> pos;
            public AOS_ACT_COL color;

            public void AcmInit()
            {
                this.pos = CArray3<float>.initializer(0.0f, 0.0f, 0.0f);
                this.scale = CArray2<float>.initializer(1f, 1f);
                this.color.c = uint.MaxValue;
                this.act.sprite.texlist = this.tex.texlist;
            }

            public void Update()
            {
                AoActAcmPush();
                float frame = this.flag[0] ? 0.0f : 1f;
                AoActSetTexture(AoTexGetTexList(this.tex));
                if (!CArray2<float>.initializer(1f, 1f).equals(this.scale))
                    AoActAcmApplyScale(this.scale.x, this.scale.y);
                if (!CArray3<float>.initializer(0.0f, 0.0f, 0.0f).equals(this.pos))
                    AoActAcmApplyTrans(this.pos.x, this.pos.y, this.pos.z);
                if (uint.MaxValue != this.color.c)
                    AoActAcmApplyColor(this.color);
                AoActUpdate(this.act, frame);
                AoActAcmPop();
            }

            public void Draw()
            {
                if (this.flag[1])
                    return;
                if (this.flag[2])
                    AoActSortRegAction(this.act);
                else
                    AoActDraw(this.act);
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
            public EMemFile.Type file;
            public uint index;

            public SLocalUnfoldTable(EMemFile.Type type, uint _index)
            {
                this.file = type;
                this.index = _index;
            }
        }

        public struct SLocalCreateActionTable
        {
            public EFile.EFileEnum file;
            public ETex.Type tex;
            public int idx;

            public SLocalCreateActionTable(
              EFile.EFileEnum file,
              ETex.Type tex,
              int idx)
            {
                this.file = file;
                this.tex = tex;
                this.idx = idx;
            }
        }
    }
}