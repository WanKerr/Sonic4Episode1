// Decompiled with JetBrains decompiler
// Type: gs.backup.SSpecialSolo
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

using System;
using System.IO;

namespace gs.backup
{
    public class SSpecialSolo : SBase
    {
        private SpecialStage specialStage;
        public const uint c_false = 0;
        public const uint c_true = 1;
        public const uint c_high_score_max_limit = 1000000000;
        public const uint c_high_score_unit = 10;
        public const uint c_fast_time_max_limit = 36000;

        public SSpecialSolo(SpecialStage specialStage)
        {
            this.specialStage = specialStage;
        }

        public void Init()
        {

        }

        public bool IsNew()
        {
            return false;
        }

        public bool IsHighScoreEnable()
        {
            return specialStage.HighScore != 0;
        }

        public bool IsFastTimeEnable()
        {
            return specialStage.BestTime.Ticks != 0;
        }

        public uint GetHighScore()
        {
            return specialStage.HighScore * 10U;
        }

        public uint GetFastTime()
        {
            return (uint)specialStage.BestTime.TotalMilliseconds;
        }

        public bool IsHighScoreUploaded()
        {
            return true;
        }

        public bool IsFastTimeUploaded()
        {
            return true;
        }

        public bool IsGetEmerald()
        {
            return specialStage.EmeraldStage != 0U;
        }

        public EEmeraldStage.Type GetEmeraldStage()
        {
            return (EEmeraldStage.Type)specialStage.EmeraldStage;
        }

        public bool IsScoreUploadedOnce()
        {
            return true;
        }

        public bool IsTimeUploadedOnce()
        {
            return true;
        }

        public void SetNew(bool val)
        {
            this.isDirty = true;
        }

        public void SetHighScore(uint high_score)
        {
            this.isDirty = true;
            high_score = Math.Min(high_score, 1000000000U);
            specialStage.HighScore = high_score / 10U;
        }

        public void SetFastTime(uint fast_time)
        {
            this.isDirty = true;
            fast_time = Math.Min(fast_time, 36000U);
            specialStage.BestTime = TimeSpan.FromMilliseconds(fast_time);
        }

        public void SetHighScoreUploaded(bool is_uploaded)
        {
            //this.m_is_high_score_uploaded = is_uploaded ? 1U : 0U;
        }

        public void SetFastTimeUploaded(bool is_uploaded)
        {
            //this.m_is_fast_time_uploaded = is_uploaded ? 1U : 0U;
        }

        public void SetEmeraldStage(EEmeraldStage.Type emerald_stage)
        {
            this.isDirty = true;
            specialStage.EmeraldStage = Math.Min((uint)emerald_stage, 13U);
        }

        public void SetScoreUploadedOnce(bool is_uploaded_once)
        {
            //this.m_is_score_uploaded_once = is_uploaded_once ? 1U : 0U;
        }

        public void SetTimeUploadedOnce(bool is_uploaded_once)
        {
            //this.m_is_time_uploaded_once = is_uploaded_once ? 1U : 0U;
        }
    }
}
