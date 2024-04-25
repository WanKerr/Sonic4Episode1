namespace gs.backup
{
    public class SRemaster : SBase
    {
        private Sonic4Save save;

        public SRemaster(Sonic4Save save)
        {
            this.save = save;
        }

        public void Init()
        {

        }

        public bool LoopCamera
        {
            get => save.Remaster.LoopCamera;
            set
            {
                save.Remaster.LoopCamera = value;
                isDirty = true;
            }
        }

        public bool BetterGears
        {
            get => save.Remaster.BetterGears;
            set
            {
                save.Remaster.BetterGears = value;
                isDirty = true;
            }
        }

        public bool BetterMusic
        {
            get => save.Remaster.BetterMusic;
            set
            {
                save.Remaster.BetterMusic = value;
                isDirty = true;
            }
        }

        public bool BugFixes
        {
            get => save.Remaster.BugFixes;
            set
            {
                save.Remaster.BugFixes = value;
                isDirty = true;
            }
        }

        public bool Misc
        {
            get => save.Remaster.Misc;
            set
            {
                save.Remaster.Misc = value;
                isDirty = true;
            }
        }

        public bool BetterSoundEffects
        {
            get => save.Remaster.BetterSoundEffects;
            set
            {
                save.Remaster.BetterSoundEffects = value;
                isDirty = true;
            }
        }

        public bool ModernSoundEffects
        {
            get => save.Remaster.ModernSoundEffects;
            set
            {
                save.Remaster.ModernSoundEffects = value;
                isDirty = true;
            }
        }
    }
}