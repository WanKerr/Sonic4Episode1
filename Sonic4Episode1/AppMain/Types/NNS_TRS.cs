public partial class AppMain
{
    public class NNS_TRS
    {
        public NNS_VECTORFAST Translation;
        public NNS_QUATERNION Rotation;
        public NNS_VECTORFAST Scaling;

        public NNS_TRS()
        {
        }

        public NNS_TRS(NNS_TRS trs)
        {
            this.Translation = trs.Translation;
            this.Rotation = trs.Rotation;
            this.Scaling = trs.Scaling;
        }

        public NNS_TRS Assign(NNS_TRS trs)
        {
            if (this != trs)
            {
                this.Translation = trs.Translation;
                this.Rotation = trs.Rotation;
                this.Scaling = trs.Scaling;
            }
            return this;
        }
    }
}
