using System;

public partial class AppMain
{
    public class AMS_AME_RUNTIME_WORK : AMS_AME_LIST
    {
        public readonly NNS_VECTOR4D position = new NNS_VECTOR4D();
        public readonly NNS_VECTOR4D velocity = new NNS_VECTOR4D();
        public readonly NNS_QUATERNION[] rotate = new NNS_QUATERNION[1];
        public readonly byte[] dummy = new byte[64];
        public float time;
        public uint flag;

        public override void Clear()
        {
            this.time = 0.0f;
            this.flag = 0U;
            this.position.Clear();
            this.velocity.Clear();
            this.rotate[0].Clear();
            Array.Clear(dummy, 0, 64);
            this.next = null;
            this.prev = null;
        }

        public static explicit operator AMS_AME_RUNTIME_WORK_MODEL(
          AMS_AME_RUNTIME_WORK work)
        {
            return new AMS_AME_RUNTIME_WORK_MODEL(work);
        }

        public static explicit operator AMS_AME_RUNTIME_WORK_DIRECTIONAL(
          AMS_AME_RUNTIME_WORK work)
        {
            return new AMS_AME_RUNTIME_WORK_DIRECTIONAL(work);
        }

        public static explicit operator AMS_AME_RUNTIME_WORK_OMNI(
          AMS_AME_RUNTIME_WORK work)
        {
            return new AMS_AME_RUNTIME_WORK_OMNI(work);
        }

        public static explicit operator AMS_AME_RUNTIME_WORK_SIMPLE_SPRITE(
          AMS_AME_RUNTIME_WORK work)
        {
            return new AMS_AME_RUNTIME_WORK_SIMPLE_SPRITE(work);
        }

        public static explicit operator AMS_AME_RUNTIME_WORK_SPRITE(
          AMS_AME_RUNTIME_WORK work)
        {
            return new AMS_AME_RUNTIME_WORK_SPRITE(work);
        }

        public static explicit operator AMS_AME_RUNTIME_WORK_LINE(
          AMS_AME_RUNTIME_WORK work)
        {
            return new AMS_AME_RUNTIME_WORK_LINE(work);
        }

        public static explicit operator AMS_AME_RUNTIME_WORK_PLANE(
          AMS_AME_RUNTIME_WORK work)
        {
            return new AMS_AME_RUNTIME_WORK_PLANE(work);
        }

        public static explicit operator AMS_AME_RUNTIME_WORK_SURFACE(
          AMS_AME_RUNTIME_WORK work)
        {
            return new AMS_AME_RUNTIME_WORK_SURFACE(work);
        }

        public static explicit operator AMS_AME_RUNTIME_WORK_CIRCLE(
          AMS_AME_RUNTIME_WORK work)
        {
            return new AMS_AME_RUNTIME_WORK_CIRCLE(work);
        }
    }
}
