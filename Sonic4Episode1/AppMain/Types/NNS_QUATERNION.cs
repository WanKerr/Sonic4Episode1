public partial class AppMain
{
    public struct NNS_QUATERNION
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public NNS_QUATERNION(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public void Assign(NNS_QUATERNION q)
        {
            this.x = q.x;
            this.y = q.y;
            this.z = q.z;
            this.w = q.w;
        }

        public void Clear()
        {
            this.x = 0.0f;
            this.y = 0.0f;
            this.z = 0.0f;
            this.w = 0.0f;
        }
    }
}
