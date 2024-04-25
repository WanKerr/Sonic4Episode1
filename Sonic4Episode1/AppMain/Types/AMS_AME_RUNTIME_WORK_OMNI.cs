using System;
using mpp;

public partial class AppMain
{
    public struct AMS_AME_RUNTIME_WORK_OMNI
    {
        private AMS_AME_RUNTIME_WORK rtm_work_;

        public AMS_AME_RUNTIME_WORK_OMNI(AMS_AME_RUNTIME_WORK rtm_work)
        {
            this.rtm_work_ = rtm_work;
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

        public float offset
        {
            get => BitConverter.ToSingle(this.rtm_work_.dummy, 0);
            set => MppBitConverter.GetBytes(value, this.rtm_work_.dummy, 0);
        }

        public float offset_chaos
        {
            get => BitConverter.ToSingle(this.rtm_work_.dummy, 4);
            set => MppBitConverter.GetBytes(value, this.rtm_work_.dummy, 4);
        }

        public static explicit operator AMS_AME_LIST(
          AMS_AME_RUNTIME_WORK_OMNI work)
        {
            return work.rtm_work_;
        }
    }
}
