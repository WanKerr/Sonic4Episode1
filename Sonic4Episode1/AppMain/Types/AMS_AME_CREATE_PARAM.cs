public partial class AppMain
{
    public class AMS_AME_CREATE_PARAM : IClearable
    {
        public AMS_AME_ECB ecb;
        public AMS_AME_RUNTIME runtime;
        public AMS_AME_NODE node;
        public AMS_AME_RUNTIME_WORK work;
        public NNS_VECTOR4D position;
        public NNS_VECTOR4D velocity;
        public NNS_VECTOR4D parent_position;
        public NNS_VECTOR4D parent_velocity;

        public void Clear()
        {
            this.ecb = null;
            this.runtime = null;
            this.node = null;
            this.work = null;
            this.position = null;
            this.velocity = null;
            this.parent_position = null;
            this.parent_velocity = null;
        }
    }
}
