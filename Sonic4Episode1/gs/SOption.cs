// Decompiled with JetBrains decompiler
// Type: gs.backup.SOption
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace gs.backup
{
    public class SOption : SBase
    {
        private Sonic4Save save;
        public const uint c_false = 0;
        public const uint c_true = 1;
        public const uint c_volume_bgm_max_limit = 100;
        public const uint c_volume_bgm_unit = 10;
        public const uint c_volume_se_max_limit = 100;
        public const uint c_volume_se_unit = 10;
        public const uint c_name_length_limit = 10;
        public const uint c_name_length_limit_pad = 16;

        public SOption(Sonic4Save save)
        {
            this.save = save;
        }

        public bool IsVibration()
        {
            return save.Options.Vibration;
        }

        public uint GetVolumeBgm()
        {
            return (uint)(save.Options.BGMVolume * 100f);
        }

        public uint GetVolumeSe()
        {
            return (uint)(save.Options.SEVolume * 100f);
        }

        public SOption.EControl.Type GetControl()
        {
            return (EControl.Type)save.Options.ControlType;
        }

        public static SOption CreateInstance(uint save_index)
        {
            return SSave.CreateInstance().GetOption(save_index);
        }

        public static SOption CreateInstance()
        {
            return SSave.CreateInstance().GetOption();
        }

        public void Init()
        {

        }

        private void SetVibration(bool is_vibration)
        {
            this.isDirty = true;
            save.Options.Vibration = is_vibration;
        }

        public void SetVolumeBgm(uint volume_bgm)
        {
            this.isDirty = true;
            volume_bgm = Math.Min(volume_bgm, 100U);
            save.Options.BGMVolume = (volume_bgm / 100f);
        }

        public void SetVolumeSe(uint volume_se)
        {
            this.isDirty = true;
            volume_se = Math.Min(volume_se, 100U);
            save.Options.SEVolume = (volume_se / 100f);
        }

        public void SetControl(SOption.EControl.Type control)
        {
            this.isDirty = true;
            control = (SOption.EControl.Type)Math.Min((uint)control, 3U);
            save.Options.ControlType = (ControlType)control;
        }

        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct EControl
        {
            public enum Type
            {
                Tilt,
                VirtualPadDown,
                VirtualPadUp,
                Max,
                None,
            }
        }
    }
}
