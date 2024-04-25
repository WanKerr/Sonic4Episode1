// Decompiled with JetBrains decompiler
// Type: gs.backup.SSystem
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

using System;

namespace gs.backup
{
    public class SSystem : SBase
    {
        private Sonic4Save save;

        public SSystem(Sonic4Save save)
        {
            this.save = save;
        }
        public uint PlayerStock
        {
            get => save.System.Lives;
            set => save.System.Lives = Math.Min(value, 1000U);
        }

        public uint Killed
        {
            get => save.System.Killed;
            set => save.System.Killed = Math.Min(value, 1000U);
        }

        public uint ClearCount
        {
            get => save.System.ClearCount;
            set => save.System.ClearCount = Math.Min(value, 2U);
        }

        public static SSystem CreateInstance(uint save_index)
        {
            return SSave.CreateInstance().GetSystem(save_index);
        }

        public static SSystem CreateInstance()
        {
            return SSave.CreateInstance().GetSystem();
        }

        public void Init()
        {

        }

        public bool IsAnnounce(EAnnounce index)
        {
            switch (index)
            {
                case EAnnounce.OpenZoneSelect:
                    return save.System.Announce.ZoneSelect;
                case EAnnounce.OpenZone1Boss:
                    return save.System.Announce.Zone1Boss;
                case EAnnounce.OpenZone2Boss:
                    return save.System.Announce.Zone2Boss;
                case EAnnounce.OpenZone3Boss:
                    return save.System.Announce.Zone3Boss;
                case EAnnounce.OpenZone4Boss:
                    return save.System.Announce.Zone4Boss;
                case EAnnounce.OpenFinalZone:
                    return save.System.Announce.FinalZone;
                case EAnnounce.OpenSuperSonic:
                    return save.System.Announce.SuperSonic;
                case EAnnounce.OpenSpecialStage:
                    return save.System.Announce.SpecialStage;
                case EAnnounce.TruckTilt:
                    return save.System.Announce.MinecartTiltControls;
                case EAnnounce.TruckFlick:
                    return save.System.Announce.MinecartFlickControls;
                case EAnnounce.SpecialStageTilt:
                    return save.System.Announce.SpecialStageTiltControls;
                case EAnnounce.SpecialStageFlick:
                    return save.System.Announce.SpecialStageFlickControls;
                default:
                    return false;
            }
        }

        public void SetAnnounce(EAnnounce index, bool is_announce)
        {
            switch (index)
            {
                case EAnnounce.OpenZoneSelect:
                    save.System.Announce.ZoneSelect = is_announce;
                    break;
                case EAnnounce.OpenZone1Boss:
                    save.System.Announce.Zone1Boss = is_announce;
                    break;
                case EAnnounce.OpenZone2Boss:
                    save.System.Announce.Zone2Boss = is_announce;
                    break;
                case EAnnounce.OpenZone3Boss:
                    save.System.Announce.Zone3Boss = is_announce;
                    break;
                case EAnnounce.OpenZone4Boss:
                    save.System.Announce.Zone4Boss = is_announce;
                    break;
                case EAnnounce.OpenFinalZone:
                    save.System.Announce.FinalZone = is_announce;
                    break;
                case EAnnounce.OpenSuperSonic:
                    save.System.Announce.SuperSonic = is_announce;
                    break;
                case EAnnounce.OpenSpecialStage:
                    save.System.Announce.SpecialStage = is_announce;
                    break;
                case EAnnounce.TruckTilt:
                    save.System.Announce.MinecartTiltControls = is_announce;
                    break;
                case EAnnounce.TruckFlick:
                    save.System.Announce.MinecartFlickControls = is_announce;
                    break;
                case EAnnounce.SpecialStageTilt:
                    save.System.Announce.SpecialStageTiltControls = is_announce;
                    break;
                case EAnnounce.SpecialStageFlick:
                    save.System.Announce.SpecialStageFlickControls = is_announce;
                    break;
            }
        }

        public enum EAnnounce
        {
            OpenZoneSelect,
            OpenZone1Boss,
            OpenZone2Boss,
            OpenZone3Boss,
            OpenZone4Boss,
            OpenFinalZone,
            OpenSuperSonic,
            OpenSpecialStage,
            TruckTilt,
            TruckFlick,
            SpecialStageTilt,
            SpecialStageFlick,
            Max,
            None,
        }
    }
}
