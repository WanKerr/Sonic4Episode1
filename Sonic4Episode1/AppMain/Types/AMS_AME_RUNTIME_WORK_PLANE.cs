using System;
using mpp;

public partial class AppMain
{
    public struct AMS_AME_RUNTIME_WORK_PLANE
    {
        private Vector4D_Buf rotate_axis_;
        private Vector4D_Buf st_;
        private Vector4D_Buf size_;
        private AMS_RGBA8888_Buf color_;
        private AMS_AME_RUNTIME_WORK rtm_work_;

        public AMS_AME_RUNTIME_WORK_PLANE(AMS_AME_RUNTIME_WORK rtm_work)
        {
            this.rtm_work_ = rtm_work;
            this.rotate_axis_ = new Vector4D_Buf(rtm_work.dummy, 0);
            this.st_ = new Vector4D_Buf(rtm_work.dummy, Vector4D_Buf.SizeBytes);
            this.size_ = new Vector4D_Buf(rtm_work.dummy, Vector4D_Buf.SizeBytes + Vector4D_Buf.SizeBytes);
            this.color_ = new AMS_RGBA8888_Buf(rtm_work.dummy, Vector4D_Buf.SizeBytes * 3);
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

        public NNS_QUATERNION rotate
        {
            get => this.rtm_work_.rotate[0];
            set => this.rtm_work_.rotate[0] = value;
        }

        public Vector4D_Buf rotate_axis => this.rotate_axis_;

        public void set_rotate_axis(float x, float y, float z, float w)
        {
            this.rotate_axis_.x = x;
            this.rotate_axis_.y = y;
            this.rotate_axis_.z = z;
            this.rotate_axis_.w = w;
        }

        public Vector4D_Buf st => this.st_;

        public void set_st(float x, float y, float z, float w)
        {
            this.st_.x = x;
            this.st_.y = y;
            this.st_.z = z;
            this.st_.w = w;
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
            get => BitConverter.ToSingle(this.rtm_work_.dummy, Vector4D_Buf.SizeBytes * 3 + AMS_RGBA8888_Buf.SizeBytes);
            set
            {
                int offset = Vector4D_Buf.SizeBytes * 3 + AMS_RGBA8888_Buf.SizeBytes;
                MppBitConverter.GetBytes(value, this.rtm_work_.dummy, offset);
            }
        }

        public int tex_no
        {
            get => BitConverter.ToInt32(this.rtm_work_.dummy, Vector4D_Buf.SizeBytes * 3 + AMS_RGBA8888_Buf.SizeBytes + 4);
            set
            {
                int offset = Vector4D_Buf.SizeBytes * 3 + AMS_RGBA8888_Buf.SizeBytes + 4;
                MppBitConverter.GetBytes(value, this.rtm_work_.dummy, offset);
            }
        }

        public static explicit operator AMS_AME_LIST(
          AMS_AME_RUNTIME_WORK_PLANE work)
        {
            return work.rtm_work_;
        }
    }
}
