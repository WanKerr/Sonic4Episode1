// Decompiled with JetBrains decompiler
// Type: gs.backup.SStage
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

using System.Runtime.InteropServices;

namespace gs.backup
{
    public class SStage : SBase
    {
        private Sonic4Save save;
        private const uint c_size = 17;
        private SStageSolo[] stages = new SStageSolo[17];

        public SStage(Sonic4Save save)
        {
            this.save = save;
            for (int i = 0; i < c_size; i++)
            {
                stages[i] = new SStageSolo(save.Stages[i]);
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

        public SStageSolo getStageSolo(uint index)
        {
            return stages[(int)index];
        }

        public SStageSolo getStageSolo(EStage.Type index)
        {
            return stages[(int)index];
        }

        public SStageSolo this[int index]
        {
            get => stages[index];
        }

        public static uint GetSize()
        {
            return 17;
        }

        public static SStage CreateInstance(uint save_index)
        {
            return SSave.CreateInstance().GetStage(save_index);
        }

        public static SStage CreateInstance()
        {
            return SSave.CreateInstance().GetStage();
        }

        public void Init()
        {

        }

        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct EStage
        {
            public enum Type
            {
                Zone1Act1,
                Zone1Act2,
                Zone1Act3,
                Zone1Boss,
                Zone2Act1,
                Zone2Act2,
                Zone2Act3,
                Zone2Boss,
                Zone3Act1,
                Zone3Act2,
                Zone3Act3,
                Zone3Boss,
                Zone4Act1,
                Zone4Act2,
                Zone4Act3,
                Zone4Boss,
                Final,
                Max,
                None,
            }
        }
    }
}
