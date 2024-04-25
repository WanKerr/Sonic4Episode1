using System.IO;

public partial class AppMain
{
    public struct NNS_MATERIAL_GLES11_TEXMAP_DESC
    {
        public uint fType;
        public int iTexIdx;
        public int EnvMode;
        public NNS_TEXTURE_GLES11_COMBINE pCombine;
        public NNS_TEXCOORD Offset;
        public NNS_TEXCOORD Scale;
        public int WrapS;
        public int WrapT;
        public NNS_TEXTURE_FILTERMODE pFilterMode;
        public float LODBias;
        public object pTexInfo;

        public void Assign(ref NNS_MATERIAL_GLES11_TEXMAP_DESC pPtr)
        {
            this.fType = pPtr.fType;
            this.iTexIdx = pPtr.iTexIdx;
            this.EnvMode = pPtr.EnvMode;
            this.pCombine = pPtr.pCombine;
            this.Offset = pPtr.Offset;
            this.Scale = pPtr.Scale;
            this.WrapS = pPtr.WrapS;
            this.WrapT = pPtr.WrapT;
            this.pFilterMode = pPtr.pFilterMode;
            this.LODBias = pPtr.LODBias;
            this.pTexInfo = pPtr.pTexInfo;
        }

        public NNS_MATERIAL_GLES11_TEXMAP_DESC(ref NNS_MATERIAL_GLES11_TEXMAP_DESC desc)
        {
            this.fType = desc.fType;
            this.iTexIdx = desc.iTexIdx;
            this.EnvMode = desc.EnvMode;
            this.pCombine = desc.pCombine;
            this.Offset = desc.Offset;
            this.Scale = desc.Scale;
            this.WrapS = desc.WrapS;
            this.WrapT = desc.WrapT;
            this.pFilterMode = desc.pFilterMode;
            this.LODBias = desc.LODBias;
            this.pTexInfo = desc.pTexInfo;
        }

        public static NNS_MATERIAL_GLES11_TEXMAP_DESC Read(
          BinaryReader reader,
          long data0Pos)
        {
            NNS_MATERIAL_GLES11_TEXMAP_DESC gleS11TexmapDesc = new NNS_MATERIAL_GLES11_TEXMAP_DESC();
            gleS11TexmapDesc.fType = reader.ReadUInt32();
            gleS11TexmapDesc.iTexIdx = reader.ReadInt32();
            gleS11TexmapDesc.EnvMode = reader.ReadInt32();
            uint num1 = reader.ReadUInt32();
            if (num1 != 0U)
            {
                long position = reader.BaseStream.Position;
                reader.BaseStream.Seek(data0Pos + num1, SeekOrigin.Begin);
                gleS11TexmapDesc.pCombine = NNS_TEXTURE_GLES11_COMBINE.Read(reader);
                reader.BaseStream.Seek(position, SeekOrigin.Begin);
            }
            gleS11TexmapDesc.Offset.u = reader.ReadSingle();
            gleS11TexmapDesc.Offset.v = reader.ReadSingle();
            gleS11TexmapDesc.Scale.u = reader.ReadSingle();
            gleS11TexmapDesc.Scale.v = reader.ReadSingle();
            gleS11TexmapDesc.WrapS = reader.ReadInt32();
            gleS11TexmapDesc.WrapT = reader.ReadInt32();
            uint num2 = reader.ReadUInt32();
            if (num2 != 0U)
            {
                long position = reader.BaseStream.Position;
                reader.BaseStream.Seek(data0Pos + num2, SeekOrigin.Begin);
                gleS11TexmapDesc.pFilterMode = NNS_TEXTURE_FILTERMODE.Read(reader);
                reader.BaseStream.Seek(position, SeekOrigin.Begin);
            }
            gleS11TexmapDesc.LODBias = reader.ReadSingle();
            uint num3 = reader.ReadUInt32();
            if (num3 != 0U)
            {
                long position = reader.BaseStream.Position;
                reader.BaseStream.Seek(data0Pos + num3, SeekOrigin.Begin);
                gleS11TexmapDesc.pTexInfo = NNS_TEXINFO.Read(reader);
                reader.BaseStream.Seek(position, SeekOrigin.Begin);
            }
            return gleS11TexmapDesc;
        }
    }
}
