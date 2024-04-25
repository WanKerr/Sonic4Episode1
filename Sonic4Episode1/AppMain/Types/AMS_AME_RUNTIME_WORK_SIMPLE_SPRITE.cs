using System;
using mpp;

public partial class AppMain
{
    public struct AMS_AME_RUNTIME_WORK_SIMPLE_SPRITE
    {
        private Vector4D_Quat st_;
        private Vector4D_Buf size_;
        private AMS_RGBA8888_Buf color_;
        private AMS_AME_RUNTIME_WORK rtm_work_;

        public AMS_AME_RUNTIME_WORK_SIMPLE_SPRITE(AMS_AME_RUNTIME_WORK rtm_work)
        {
            this.rtm_work_ = rtm_work;
            this.st_ = new Vector4D_Quat(rtm_work.rotate);
            this.size_ = new Vector4D_Buf(rtm_work.dummy, 0);
            this.color_ = new AMS_RGBA8888_Buf(rtm_work.dummy, Vector4D_Buf.SizeBytes);
        }

        public AMS_AME_LIST next
        {
            get => this.rtm_work_.next;
            set => this.rtm_work_.next = value;
        }

        public AMS_AME_LIST prev
        {
            get => this.rtm_work_.prev;
            set => this.rtm_work_.prev = value;
        }

        public float time
        {
            get => this.rtm_work_.time;
            set => this.rtm_work_.time = value;
        }

        public uint flag
        {
            get => this.rtm_work_.flag;
            set => this.rtm_work_.flag = value;
        }

        public NNS_VECTOR4D position
        {
            get => this.rtm_work_.position;
            set => this.rtm_work_.position.Assign(value);
        }

        public NNS_VECTOR4D velocity
        {
            get => this.rtm_work_.velocity;
            set => this.rtm_work_.velocity.Assign(value);
        }

        public Vector4D_Quat st => this.st_;

        public void set_st(float x, float y, float z, float w)
        {
            this.st.Assign(x, y, z, w);
        }

        public Vector4D_Buf size => this.size_;

        public void set_size(float x, float y, float z, float w)
        {
            this.size_.x = x;
            this.size_.y = y;
            this.size_.z = z;
            this.size_.w = w;
        }

        public void set_size(NNS_VECTOR4D v)
        {
            this.size_.Assign(v);
        }

        public AMS_RGBA8888_Buf color => this.color_;

        public void set_color(byte r, byte g, byte b, byte a)
        {
            this.color_.r = r;
            this.color_.g = g;
            this.color_.b = b;
            this.color_.a = a;
        }

        public void set_color(uint c)
        {
            this.color_.color = c;
        }

        public float tex_time
        {
            get => BitConverter.ToSingle(this.rtm_work_.dummy, Vector4D_Buf.SizeBytes + AMS_RGBA8888_Buf.SizeBytes);
            set
            {
                int offset = Vector4D_Buf.SizeBytes + AMS_RGBA8888_Buf.SizeBytes;
                MppBitConverter.GetBytes(value, this.rtm_work_.dummy, offset);
            }
        }

        public int tex_no
        {
            get => BitConverter.ToInt32(this.rtm_work_.dummy, Vector4D_Buf.SizeBytes + AMS_RGBA8888_Buf.SizeBytes + 4);
            set
            {
                int offset = Vector4D_Buf.SizeBytes + AMS_RGBA8888_Buf.SizeBytes + 4;
                MppBitConverter.GetBytes(value, this.rtm_work_.dummy, offset);
            }
        }

        public static explicit operator AMS_AME_LIST(
          AMS_AME_RUNTIME_WORK_SIMPLE_SPRITE work)
        {
            return work.rtm_work_;
        }

        public static implicit operator AMS_AME_RUNTIME_WORK(
          AMS_AME_RUNTIME_WORK_SIMPLE_SPRITE work)
        {
            return work.rtm_work_;
        }
    }
}
