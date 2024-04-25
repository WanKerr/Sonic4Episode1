public partial class AppMain
{
    public class NNS_MATERIAL_STDSHADER_DESC_USER_PROFILE : NNS_MATERIAL_STDSHADER_DESC
    {
        public uint UserProfile;

        public NNS_MATERIAL_STDSHADER_DESC_USER_PROFILE()
        {
        }

        public NNS_MATERIAL_STDSHADER_DESC_USER_PROFILE(
          NNS_MATERIAL_STDSHADER_DESC_USER_PROFILE desc)
          : base(desc)
        {
            this.UserProfile = desc.UserProfile;
        }

        public NNS_MATERIAL_STDSHADER_DESC_USER_PROFILE Assign(
          NNS_MATERIAL_STDSHADER_DESC_USER_PROFILE desc)
        {
            this.Assign((NNS_MATERIAL_STDSHADER_DESC)desc);
            this.UserProfile = desc.UserProfile;
            return this;
        }
    }
}
