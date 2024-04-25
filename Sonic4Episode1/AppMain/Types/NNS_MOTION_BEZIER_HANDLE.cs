using System.IO;

public partial class AppMain
{
    public class NNS_MOTION_BEZIER_HANDLE
    {
        public readonly NNS_VECTOR2D In = new NNS_VECTOR2D();
        public readonly NNS_VECTOR2D Out = new NNS_VECTOR2D();

        public NNS_MOTION_BEZIER_HANDLE()
        {
        }

        public NNS_MOTION_BEZIER_HANDLE(NNS_MOTION_BEZIER_HANDLE bezierHandle)
        {
            this.In.Assign(bezierHandle.In);
            this.Out.Assign(bezierHandle.Out);
        }

        public NNS_MOTION_BEZIER_HANDLE Assign(
          NNS_MOTION_BEZIER_HANDLE bezierHandle)
        {
            if (this != bezierHandle)
            {
                this.In.Assign(bezierHandle.In);
                this.Out.Assign(bezierHandle.Out);
            }
            return this;
        }

        public static NNS_MOTION_BEZIER_HANDLE Read(BinaryReader reader)
        {
            NNS_MOTION_BEZIER_HANDLE motionBezierHandle = new NNS_MOTION_BEZIER_HANDLE();
            motionBezierHandle.In.Assign(NNS_VECTOR2D.Read(reader));
            motionBezierHandle.Out.Assign(NNS_VECTOR2D.Read(reader));
            return motionBezierHandle;
        }
    }
}
