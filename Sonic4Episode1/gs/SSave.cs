// Decompiled with JetBrains decompiler
// Type: gs.backup.SBackup
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

using System.IO;

namespace gs.backup
{
    public class SSave : SBase
    {
        private Sonic4Save save;
        private SSystem[] m_system = new SSystem[1];
        private SOption[] m_option = new SOption[1];
        private SStage[] m_stage = new SStage[1];
        private SSpecial[] m_special = new SSpecial[1];
        private SRemaster[] m_remaster = new SRemaster[1];
        private SDebug[] m_debug = new SDebug[1];
        private const uint c_save_size = 1;
        const int STAGE_COUNT = 17;
        const int SPECIAL_STAGE_COUNT = 7;

        public void Init(Sonic4Save save = null)
        {
            if (this.save != null)
                return;

            this.save = save = save ?? new Sonic4Save();
            for (int index = 0; index < 1; ++index)
            {
                this.m_system[index] = new SSystem(save);
                this.m_system[index].Init();
                this.m_option[index] = new SOption(save);
                this.m_option[index].Init();
                this.m_stage[index] = new SStage(save);
                this.m_stage[index].Init();
                this.m_special[index] = new SSpecial(save);
                this.m_special[index].Init();
                this.m_remaster[index] = new SRemaster(save);
                this.m_remaster[index].Init();
                this.m_debug[index] = new SDebug(save);
                this.m_debug[index].Init();
            }
        }

        public override bool GetDirty()
        {
            if (GetSystem().GetDirty())
                return true;
            if (GetOption().GetDirty())
                return true;
            if (GetStage().GetDirty())
                return true;
            if (GetSpecial().GetDirty())
                return true;
            if (GetRemaster().GetDirty())
                return true;
            if (GetDebug().GetDirty())
                return true;

            return base.GetDirty();
        }

        public Sonic4Save GetSave()
        {
            return save;
        }

        public void SetSave(Sonic4Save value)
        {
            Init(value);
        }

        public uint GetSaveIndex()
        {
            return 0;
        }

        public SSystem GetSystem(uint save_index)
        {
            return this.m_system[(int)save_index];
        }

        public SSystem GetSystem()
        {
            return this.GetSystem(this.GetSaveIndex());
        }

        public SOption GetOption(uint save_index)
        {
            return this.m_option[(int)save_index];
        }

        public SOption GetOption()
        {
            return this.GetOption(this.GetSaveIndex());
        }

        public SStage GetStage(uint save_index)
        {
            return this.m_stage[(int)save_index];
        }

        public SStage GetStage()
        {
            return this.GetStage(this.GetSaveIndex());
        }

        public SSpecial GetSpecial(uint save_index)
        {
            return this.m_special[(int)save_index];
        }

        public SDebug GetDebug()
        {
            return this.GetDebug(this.GetSaveIndex());
        }
        public SDebug GetDebug(uint save_index)
        {
            return this.m_debug[(int)save_index];
        }

        public SRemaster GetRemaster()
        {
            return this.GetRemaster(this.GetSaveIndex());
        }
        public SRemaster GetRemaster(uint save_index)
        {
            return this.m_remaster[(int)save_index];
        }

        public SSpecial GetSpecial()
        {
            return this.GetSpecial(this.GetSaveIndex());
        }

        private void SetSaveIndex(uint save_index)
        {
        }

        public static SSave CreateInstance()
        {
            return (SSave)AppMain.GsGetMainSysInfo().Save;
        }
    }
}
