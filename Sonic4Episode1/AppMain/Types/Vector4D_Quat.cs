public partial class AppMain
{
    public struct Vector4D_Quat
    {
        private readonly NNS_QUATERNION[] quat_;

        public Vector4D_Quat(NNS_QUATERNION[] quat)
        {
            this.quat_ = quat;
        }

        public float x => this.quat_[0].x;

        public float y => this.quat_[0].y;

        public float z => this.quat_[0].z;

        public float w => this.quat_[0].w;

        public void Assign(float x, float y, float z, float w)
        {
            this.quat_[0] = new NNS_QUATERNION(x, y, z, w);
        }

        public void Assign(NNS_VECTOR4D v)
        {
            this.quat_[0] = new NNS_QUATERNION(v.x, v.y, v.z, v.w);
        }

        public static explicit operator NNS_QUATERNION(Vector4D_Quat v)
        {
            return v.quat_[0];
        }
    }
}
