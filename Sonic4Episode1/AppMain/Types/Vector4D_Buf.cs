using System;
using mpp;

public partial class AppMain
{
    public struct Vector4D_Buf
    {
        private readonly byte[] data_;
        private readonly int offset_;

        public Vector4D_Buf(byte[] data, int offset)
        {
            this.data_ = data;
            this.offset_ = offset;
        }

        public float x
        {
            get => BitConverter.ToSingle(this.data_, this.offset_);
            set => MppBitConverter.GetBytes(value, this.data_, this.offset_);
        }

        public float y
        {
            get => BitConverter.ToSingle(this.data_, this.offset_ + 4);
            set => MppBitConverter.GetBytes(value, this.data_, this.offset_ + 4);
        }

        public float z
        {
            get => BitConverter.ToSingle(this.data_, this.offset_ + 8);
            set => MppBitConverter.GetBytes(value, this.data_, this.offset_ + 8);
        }

        public float w
        {
            get => BitConverter.ToSingle(this.data_, this.offset_ + 12);
            set => MppBitConverter.GetBytes(value, this.data_, this.offset_ + 12);
        }

        public void Assign(Vector4D_Buf v)
        {
            this.x = v.x;
            this.y = v.y;
            this.z = v.z;
            this.w = v.w;
        }

        public void Assign(NNS_VECTOR4D v)
        {
            this.x = v.x;
            this.y = v.y;
            this.z = v.z;
            this.w = v.w;
        }

        public void Assign(NNS_VECTOR v)
        {
            this.x = v.x;
            this.y = v.y;
            this.z = v.z;
        }

        public static int SizeBytes => 16;
    }
}
