// Decompiled with JetBrains decompiler
// Type: gs.backup.SSpecial
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

using System;
using System.IO;

namespace gs.backup
{
    public class SSpecial : SBase
    {
        private Sonic4Save save;
        private const uint c_size = 7;
        private SSpecialSolo[] stages = new SSpecialSolo[7];

        public SSpecial(Sonic4Save save)
        {
            this.save = save;
            for (int i = 0; i < 7; i++)
            {
                stages[i] = new SSpecialSolo(save.SpecialStages[i]);
            }
        }

        public override bool GetDirty()
        {
            for (int i = 0; i < stages.Length; i++)
            {
                if (stages[i].isDirty)
                    return true;
            }

            return base.GetDirty();
        }

        private SSpecialSolo getSpecialSolo(uint index)
        {
            return stages[(int) index];
        }

        private SSpecialSolo getSpecialSolo(SStage.EStage.Type index)
        {
            return stages[(int) index];
        }

        public SSpecialSolo this[int index] => stages[(int) index];

        public static uint GetSize()
        {
            return 7;
        }

        public static SSpecial CreateInstance(uint save_index)
        {
            return SSave.CreateInstance().GetSpecial(save_index);
        }

        public static SSpecial CreateInstance()
        {
            return SSave.CreateInstance().GetSpecial();
        }

        public void Init()
        {

        }
    }
}
