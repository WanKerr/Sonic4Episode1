public partial class AppMain
{
    public class NNS_LIGHTPTR
    {
        public uint fType;
        public object pLight;

        public NNS_LIGHTPTR()
        {
        }

        public NNS_LIGHTPTR(NNS_LIGHTPTR lightPtr)
        {
            this.fType = lightPtr.fType;
            this.pLight = lightPtr.pLight;
        }

        public NNS_LIGHTPTR Assign(NNS_LIGHTPTR lightPtr)
        {
            this.fType = lightPtr.fType;
            this.pLight = lightPtr.pLight;
            return this;
        }
    }
}
