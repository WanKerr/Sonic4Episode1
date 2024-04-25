using System;
using mpp;

public partial class AppMain
{
    public struct AMS_RGBA8888_Buf
    {
        private readonly byte[] data_;
        private readonly int offset_;

        public AMS_RGBA8888_Buf(byte[] data, int offset)
        {
            this.data_ = data;
            this.offset_ = offset;
        }

        public byte r
        {
            get => this.data_[this.offset_];
            set => this.data_[this.offset_] = value;
        }

        public byte g
        {
            get => this.data_[this.offset_ + 1];
            set => this.data_[this.offset_ + 1] = value;
        }

        public byte b
        {
            get => this.data_[this.offset_ + 2];
            set => this.data_[this.offset_ + 2] = value;
        }

        public byte a
        {
            get => this.data_[this.offset_ + 3];
            set => this.data_[this.offset_ + 3] = value;
        }

        public uint color
        {
            get => BitConverter.ToUInt32(this.data_, this.offset_);
            set => MppBitConverter.GetBytes(value, this.data_, this.offset_);
        }

        public static int SizeBytes => 4;
    }
}
