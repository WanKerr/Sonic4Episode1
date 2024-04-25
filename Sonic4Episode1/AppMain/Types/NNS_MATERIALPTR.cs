using System.IO;

public partial class AppMain
{
    public class NNS_MATERIALPTR
    {
        public uint fType;
        public object pMaterial;

        public NNS_MATERIALPTR()
        {
        }

        public NNS_MATERIALPTR(NNS_MATERIALPTR materialPtr)
        {
            this.fType = materialPtr.fType;
            this.pMaterial = materialPtr.pMaterial;
        }

        public NNS_MATERIALPTR Assign(NNS_MATERIALPTR materialPtr)
        {
            this.fType = materialPtr.fType;
            this.pMaterial = materialPtr.pMaterial;
            return this;
        }

        public static NNS_MATERIALPTR Read(
          BinaryReader reader,
          long data0Pos,
          out bool transparentMaterial)
        {
            transparentMaterial = false;
            NNS_MATERIALPTR nnsMaterialptr = new NNS_MATERIALPTR();
            nnsMaterialptr.fType = reader.ReadUInt32();
            uint num = reader.ReadUInt32();
            if (num != 0U)
            {
                long position = reader.BaseStream.Position;
                reader.BaseStream.Seek(data0Pos + num, SeekOrigin.Begin);
                nnsMaterialptr.pMaterial = NNS_MATERIAL_GLES11_DESC.Read(reader, data0Pos, out transparentMaterial);
                reader.BaseStream.Seek(position, SeekOrigin.Begin);
            }
            return nnsMaterialptr;
        }
    }
}
