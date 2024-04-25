// Decompiled with JetBrains decompiler
// Type: gs.backup.SStageSolo
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

using System;
using System.Runtime.InteropServices;

namespace gs.backup
{
    public class SStageSolo : SBase
    {
        private Stage stage;
        public const uint c_false = 0;
        public const uint c_true = 1;
        public const uint c_high_score_max_limit = 1000000000;
        public const uint c_high_score_unit = 10;
        public const uint c_fast_time_max_limit = 36000;

        public SStageSolo(Stage stage)
        {
            this.stage = stage;
        }

        public bool IsNew()
        {
            return stage.New;
        }

        public bool IsHighScoreUseSuperSonic()
        {
            return stage.HighScoreSuperSonic;
        }

        public bool IsFastTimeUseSuperSonic()
        {
            return stage.BestTimeSuperSonic;
        }

        public bool IsScoreUploadedOnce()
        {
            return true;
        }

        public bool IsTimeUploadedOnce()
        {
            return true;
        }

        public bool IsUseSuperSonicOnce()
        {
            return stage.UsedSuperSonic;
        }

        public void Init()
        {

        }

        private StageRecord getRecord(ERecordKind.Type record_kind)
        {
            return stage.StageRecords[(int)record_kind];
        }

        private StageRecord getRecord(bool is_supersonic)
        {
            return this.getRecord(is_supersonic ? ERecordKind.Type.SuperSonic : ERecordKind.Type.Sonic);
        }

        public bool IsHighScoreEnable(bool is_supersonic)
        {
            return this.getRecord(is_supersonic).HighScore != 0;
        }

        public bool IsFastTimeEnable(bool is_supersonic)
        {
            return this.getRecord(is_supersonic).BestTime.Ticks != 0;
        }

        public uint GetHighScore(bool is_supersonic)
        {
            return this.getRecord(is_supersonic).HighScore * 10U;
        }

        public uint GetFastTime(bool is_supersonic)
        {
            return (uint)(this.getRecord(is_supersonic).BestTime.TotalMilliseconds / 16.6666666666);
        }

        public bool IsHighScoreUploaded(bool is_supersonic)
        {
            return true;
        }

        public bool IsFastTimeUploaded(bool is_supersonic)
        {
            return true;
        }

        public void SetNew(bool is_new)
        {
            isDirty = true;
            stage.New = is_new;
        }

        public void SetHighScore(uint high_score, bool is_use_supersonic)
        {
            isDirty = true;
            var record = this.getRecord(is_use_supersonic);
            high_score = Math.Min(high_score, 1000000000U);
            record.HighScore = high_score / 10U;

            this.stage.HighScoreSuperSonic = this.getRecord(!is_use_supersonic).HighScore < record.HighScore;

            if (!is_use_supersonic)
                return;

            this.stage.UsedSuperSonic = true;
        }

        public void SetFastTime(uint fast_time, bool is_use_supersonic)
        {
            isDirty = true;

            var record = this.getRecord(is_use_supersonic);
            double best_time = Math.Min(fast_time, 36000U);
            best_time *= 16.6666666666;
            record.BestTime = TimeSpan.FromMilliseconds(best_time);

            this.stage.BestTimeSuperSonic = this.getRecord(!is_use_supersonic).BestTime < record.BestTime;

            if (!is_use_supersonic)
                return;

            this.stage.UsedSuperSonic = true;
        }

        public void SetHighScoreUploaded(bool is_supersonic, bool is_uploaded)
        {
            //this.getRecord(is_supersonic).is_high_score_uploaded = is_uploaded ? 1U : 0U;
        }

        public void SetFastTimeUploaded(bool is_supersonic, bool is_uploaded)
        {
            //this.getRecord(is_supersonic).is_fast_time_uploaded = is_uploaded ? 1U : 0U;
        }

        public void SetScoreUploadedOnce(bool is_uploaded_once)
        {
            //this.m_is_score_uploaded_once = is_uploaded_once ? 1U : 0U;
        }

        public void SetTimeUploadedOnce(bool is_uploaded_once)
        {
            //this.m_is_time_uploaded_once = is_uploaded_once ? 1U : 0U;
        }

        public void SetUseSuperSonicOnce(bool is_use_supersonic_once)
        {
            isDirty = true;
            this.stage.UsedSuperSonic = is_use_supersonic_once;
        }

        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct ERecordKind
        {
            public enum Type
            {
                Sonic,
                SuperSonic,
                Max,
                None,
            }
        }
    }
}
