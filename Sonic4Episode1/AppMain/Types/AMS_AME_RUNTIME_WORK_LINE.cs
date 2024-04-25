using System;
using mpp;

public partial class AppMain
{
    public struct AMS_AME_RUNTIME_WORK_LINE
    {
        private Vector4D_Quat st_;
        private AMS_RGBA8888_Buf inside_color_;
        private AMS_RGBA8888_Buf outside_color_;
        private AMS_AME_RUNTIME_WORK rtm_work_;

        public AMS_AME_RUNTIME_WORK_LINE(AMS_AME_RUNTIME_WORK rtm_work)
        {
            this.rtm_work_ = rtm_work;
            this.st_ = new Vector4D_Quat(rtm_work.rotate);
            this.inside_color_ = new AMS_RGBA8888_Buf(rtm_work.dummy, 0);
            this.outside_color_ = new AMS_RGBA8888_Buf(rtm_work.dummy, AMS_RGBA8888_Buf.SizeBytes);
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
            this.st_.Assign(x, y, z, w);
        }

        public AMS_RGBA8888_Buf inside_color => this.inside_color_;

        public void set_inside_color(byte r, byte g, byte b, byte a)
        {
            this.inside_color_.r = r;
            this.inside_color_.g = g;
            this.inside_color_.b = b;
            this.inside_color_.a = a;
        }

        public void set_inside_color(uint c)
        {
            this.inside_color_.color = c;
        }

        public AMS_RGBA8888_Buf outside_color => this.outside_color_;

        public void set_outside_color(byte r, byte g, byte b, byte a)
        {
            this.outside_color_.r = r;
            this.outside_color_.g = g;
            this.outside_color_.b = b;
            this.outside_color_.a = a;
        }

        public void set_outside_color(uint c)
        {
            this.outside_color_.color = c;
        }

        public float inside_width
        {
            get => BitConverter.ToSingle(this.rtm_work_.dummy, AMS_RGBA8888_Buf.SizeBytes + AMS_RGBA8888_Buf.SizeBytes);
            set
            {
                int offset = AMS_RGBA8888_Buf.SizeBytes + AMS_RGBA8888_Buf.SizeBytes;
                MppBitConverter.GetBytes(value, this.rtm_work_.dummy, offset);
            }
        }

        public float outside_width
        {
            get => BitConverter.ToSingle(this.rtm_work_.dummy, AMS_RGBA8888_Buf.SizeBytes + AMS_RGBA8888_Buf.SizeBytes + 4);
            set
            {
                int offset = AMS_RGBA8888_Buf.SizeBytes + AMS_RGBA8888_Buf.SizeBytes + 4;
                MppBitConverter.GetBytes(value, this.rtm_work_.dummy, offset);
            }
        }

        public float length
        {
            get => BitConverter.ToSingle(this.rtm_work_.dummy, AMS_RGBA8888_Buf.SizeBytes + AMS_RGBA8888_Buf.SizeBytes + 8);
            set
            {
                int offset = AMS_RGBA8888_Buf.SizeBytes + AMS_RGBA8888_Buf.SizeBytes + 8;
                MppBitConverter.GetBytes(value, this.rtm_work_.dummy, offset);
            }
        }

        public float tex_time
        {
            get => BitConverter.ToSingle(this.rtm_work_.dummy, Vector4D_Buf.SizeBytes + AMS_RGBA8888_Buf.SizeBytes + 12);
            set
            {
                int offset = Vector4D_Buf.SizeBytes + AMS_RGBA8888_Buf.SizeBytes + 12;
                MppBitConverter.GetBytes(value, this.rtm_work_.dummy, offset);
            }
        }

        public int tex_no
        {
            get => BitConverter.ToInt32(this.rtm_work_.dummy, Vector4D_Buf.SizeBytes + AMS_RGBA8888_Buf.SizeBytes + 16);
            set
            {
                int offset = Vector4D_Buf.SizeBytes + AMS_RGBA8888_Buf.SizeBytes + 16;
                MppBitConverter.GetBytes(value, this.rtm_work_.dummy, offset);
            }
        }

        public static explicit operator AMS_AME_LIST(
          AMS_AME_RUNTIME_WORK_LINE work)
        {
            return work.rtm_work_;
        }

        public static implicit operator AMS_AME_RUNTIME_WORK(
          AMS_AME_RUNTIME_WORK_LINE work)
        {
            return work.rtm_work_;
        }
    }
}
