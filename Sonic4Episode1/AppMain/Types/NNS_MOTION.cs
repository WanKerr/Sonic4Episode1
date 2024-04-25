using System.IO;

public partial class AppMain
{
    public class NNS_MOTION : IClearable
    {
        public uint fType;
        public float StartFrame;
        public float EndFrame;
        public int nSubmotion;
        public NNS_SUBMOTION[] pSubmotion;
        public float FrameRate;
        public uint Reserved0;
        public uint Reserved1;

        public NNS_MOTION()
        {
        }

        public NNS_MOTION(NNS_MOTION motion)
        {
            this.fType = motion.fType;
            this.StartFrame = motion.StartFrame;
            this.EndFrame = motion.EndFrame;
            this.nSubmotion = motion.nSubmotion;
            this.pSubmotion = motion.pSubmotion;
            this.FrameRate = motion.FrameRate;
            this.Reserved0 = motion.Reserved0;
            this.Reserved1 = motion.Reserved1;
        }

        public NNS_MOTION Assign(NNS_MOTION motion)
        {
            if (this != motion)
            {
                this.fType = motion.fType;
                this.StartFrame = motion.StartFrame;
                this.EndFrame = motion.EndFrame;
                this.nSubmotion = motion.nSubmotion;
                this.pSubmotion = motion.pSubmotion;
                this.FrameRate = motion.FrameRate;
                this.Reserved0 = motion.Reserved0;
                this.Reserved1 = motion.Reserved1;
            }
            return this;
        }

        public void Clear()
        {
            this.fType = 0U;
            this.StartFrame = 0.0f;
            this.EndFrame = 0.0f;
            this.nSubmotion = 0;
            this.pSubmotion = null;
            this.FrameRate = 0.0f;
            this.Reserved0 = 0U;
            this.Reserved1 = 0U;
        }

        public static NNS_MOTION Read(BinaryReader reader, long data0Pos)
        {
            NNS_MOTION nnsMotion = new NNS_MOTION();
            nnsMotion.fType = reader.ReadUInt32();
            nnsMotion.StartFrame = reader.ReadSingle();
            nnsMotion.EndFrame = reader.ReadSingle();
            nnsMotion.nSubmotion = reader.ReadInt32();
            uint num = reader.ReadUInt32();
            if (num != 0U)
            {
                long position = reader.BaseStream.Position;
                reader.BaseStream.Seek(data0Pos + num, SeekOrigin.Begin);
                nnsMotion.pSubmotion = new NNS_SUBMOTION[nnsMotion.nSubmotion];
                for (int index = 0; index < nnsMotion.nSubmotion; ++index)
                    nnsMotion.pSubmotion[index] = NNS_SUBMOTION.Read(reader, nnsMotion.fType & 31U, data0Pos);
                reader.BaseStream.Seek(position, SeekOrigin.Begin);
            }
            nnsMotion.FrameRate = reader.ReadSingle();
            nnsMotion.Reserved0 = reader.ReadUInt32();
            nnsMotion.Reserved1 = reader.ReadUInt32();
            return nnsMotion;
        }
    }
}
