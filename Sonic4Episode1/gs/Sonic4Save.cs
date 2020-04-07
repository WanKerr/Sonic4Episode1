using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;

namespace gs
{
    [XmlRoot("Sonic4Save")]
    public class Sonic4Save
    {
        public Sonic4Save()
        {
            System = new System();
            Options = new Options();
            Remaster = new Remaster();
            Debug = new Debug();

            Stages = new Stage[17];
            for (int i = 0; i < 17; i++)
            {
                Stages[i] = new Stage();
            }

            SpecialStages = new SpecialStage[7];
            for (int i = 0; i < 7; i++)
            {
                SpecialStages[i] = new SpecialStage();
            }
        }

        [XmlElement("System")] public System System { get; set; }

        [XmlElement("Options")] public Options Options { get; set; }

        [XmlElement("Remaster")] public Remaster Remaster { get; set; }

        [XmlElement("Debug")] public Debug Debug { get; set; }

        [XmlElement("Stage")] public Stage[] Stages { get; set; }

        [XmlElement("SpecialStage")] public SpecialStage[] SpecialStages { get; set; }
    }

    public class System
    {
        [XmlElement("Lives")] public uint Lives { get; set; } = 3;

        [XmlElement("Killed")] public uint Killed { get; set; } = 0;

        [XmlElement("ClearCount")] public uint ClearCount { get; set; } = 0;

        [XmlElement("Announce")] public Announce Announce { get; set; } = new Announce();
    }

    public class Announce
    {
        [XmlElement("ZoneSelect")] public bool ZoneSelect { get; set; }

        [XmlElement("Zone1Boss")] public bool Zone1Boss { get; set; }

        [XmlElement("Zone2Boss")] public bool Zone2Boss { get; set; }

        [XmlElement("Zone3Boss")] public bool Zone3Boss { get; set; }

        [XmlElement("Zone4Boss")] public bool Zone4Boss { get; set; }

        [XmlElement("FinalZone")] public bool FinalZone { get; set; }

        [XmlElement("SpecialStage")] public bool SpecialStage { get; set; }

        [XmlElement("SuperSonic")] public bool SuperSonic { get; set; }

        [XmlElement("SpecialStageTilt")] public bool SpecialStageTiltControls { get; set; }

        [XmlElement("SpecialStageFlick")] public bool SpecialStageFlickControls { get; set; }

        [XmlElement("MinecartTiltControls")] public bool MinecartTiltControls { get; set; }

        [XmlElement("MinecartFlickControls")] public bool MinecartFlickControls { get; set; }
    }

    public class Stage
    {
        public Stage()
        {
            New = true;
            StageRecords = new StageRecord[2];
            for (int i = 0; i < 2; i++)
            {
                StageRecords[i] = new StageRecord();
            }
        }

        [XmlAttribute("New")] public bool New { get; set; }

        [XmlAttribute("HighScoreSuperSonic")] public bool HighScoreSuperSonic { get; set; }

        [XmlAttribute("BestTimeSuperSonic")] public bool BestTimeSuperSonic { get; set; }

        [XmlAttribute("UsedSuperSonic")] public bool UsedSuperSonic { get; set; }

        [XmlElement("StageRecord")] public StageRecord[] StageRecords { get; set; }
    }

    public class StageRecord
    {
        [XmlAttribute("HighScore")] public uint HighScore { get; set; }

        [XmlIgnore] public TimeSpan BestTime { get; set; }

        [Browsable(false)]
        [XmlAttribute("BestTime")]
        public string BestTimetring
        {
            get { return BestTime.ToString("c"); }
            set
            {
                BestTime = string.IsNullOrEmpty(value)
                    ? TimeSpan.Zero
                    : TimeSpan.Parse(value, CultureInfo.InvariantCulture);
            }
        }
    }

    public class SpecialStage
    {
        [XmlAttribute("EmeraldStage")] public uint EmeraldStage { get; set; }

        [XmlAttribute("HighScore")] public uint HighScore { get; set; }

        [XmlIgnore] public TimeSpan BestTime { get; set; }

        [Browsable(false)]
        [XmlAttribute("BestTime")]
        public string BestTimeString
        {
            get { return BestTime.ToString("c"); }
            set
            {
                BestTime = string.IsNullOrEmpty(value)
                    ? TimeSpan.Zero
                    : TimeSpan.Parse(value, CultureInfo.InvariantCulture);
            }
        }
    }

    public class Options
    {
        [XmlElement("Vibration")] public bool Vibration { get; set; } = true;

        [XmlElement("BGMVolume")] public float BGMVolume { get; set; } = 1;

        [XmlElement("SEVolume")] public float SEVolume { get; set; } = 1;

        [XmlElement("ControlStyle")] public ControlType ControlType { get; set; } = ControlType.VirtualPadDown;
    }

    public class Remaster
    {
        [XmlElement("LoopCamera")] public bool LoopCamera { get; set; } = false;
        [XmlElement("BetterSoundEffects")] public bool BetterSoundEffects { get; set; } = true;
        [XmlElement("ModernSoundEffects")] public bool ModernSoundEffects { get; set; } = true;
    }

    public class Debug
    {
        [XmlElement("GodMode")] public bool GodMode { get; set; } = false;
    }

    public enum ControlType
    {
        Tilt, VirtualPadDown, VirtualPadUp,
        Max, None,
    }
}