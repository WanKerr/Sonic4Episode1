public partial class AppMain
{
    public class NNS_MORPHTARGETNAME
    {
        public int iMorphTarget;
        public string Name;

        public NNS_MORPHTARGETNAME()
        {
        }

        public NNS_MORPHTARGETNAME(NNS_MORPHTARGETNAME morthTargetName)
        {
            this.iMorphTarget = morthTargetName.iMorphTarget;
            this.Name = morthTargetName.Name;
        }

        public NNS_MORPHTARGETNAME Assign(NNS_MORPHTARGETNAME morthTargetName)
        {
            this.iMorphTarget = morthTargetName.iMorphTarget;
            this.Name = morthTargetName.Name;
            return this;
        }
    }
}
