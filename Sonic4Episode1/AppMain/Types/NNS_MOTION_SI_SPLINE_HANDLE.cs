using System.IO;

public partial class AppMain
{
    public class NNS_MOTION_SI_SPLINE_HANDLE
    {
        public float In;
        public float Out;

        public NNS_MOTION_SI_SPLINE_HANDLE()
        {
        }

        public NNS_MOTION_SI_SPLINE_HANDLE(NNS_MOTION_SI_SPLINE_HANDLE splineHandle)
        {
            this.In = splineHandle.In;
            this.Out = splineHandle.Out;
        }

        public NNS_MOTION_SI_SPLINE_HANDLE Assign(
          NNS_MOTION_SI_SPLINE_HANDLE splineHandle)
        {
            if (this != splineHandle)
            {
                this.In = splineHandle.In;
                this.Out = splineHandle.Out;
            }
            return this;
        }

        public static NNS_MOTION_SI_SPLINE_HANDLE Read(BinaryReader reader)
        {
            return new NNS_MOTION_SI_SPLINE_HANDLE()
            {
                In = reader.ReadSingle(),
                Out = reader.ReadSingle()
            };
        }
    }
}
