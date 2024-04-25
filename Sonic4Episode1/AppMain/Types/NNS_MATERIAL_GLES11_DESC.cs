using System.IO;

public partial class AppMain
{
    public class NNS_MATERIAL_GLES11_DESC
    {
        public uint fFlag;
        public uint User;
        public NNS_MATERIAL_STDSHADER_COLOR pColor;
        public NNS_MATERIAL_GLES11_LOGIC pLogic;
        public int nTex;
        public NNS_MATERIAL_GLES11_TEXMAP_DESC[] pTexDesc;

        public static NNS_MATERIAL_GLES11_DESC Read(
          BinaryReader reader,
          long data0Pos,
          out bool transparentMaterial)
        {
            transparentMaterial = false;
            NNS_MATERIAL_GLES11_DESC materialGleS11Desc = new NNS_MATERIAL_GLES11_DESC();
            materialGleS11Desc.fFlag = reader.ReadUInt32();
            materialGleS11Desc.User = reader.ReadUInt32();
            uint num1 = reader.ReadUInt32();
            if (num1 != 0U)
            {
                long position = reader.BaseStream.Position;
                reader.BaseStream.Seek(data0Pos + num1, SeekOrigin.Begin);
                materialGleS11Desc.pColor = NNS_MATERIAL_STDSHADER_COLOR.Read(reader);
                reader.BaseStream.Seek(position, SeekOrigin.Begin);
                transparentMaterial = 1.0 != materialGleS11Desc.pColor.Diffuse.a;
            }
            uint num2 = reader.ReadUInt32();
            if (num2 != 0U)
            {
                long position = reader.BaseStream.Position;
                reader.BaseStream.Seek(data0Pos + num2, SeekOrigin.Begin);
                materialGleS11Desc.pLogic = NNS_MATERIAL_GLES11_LOGIC.Read(reader);
                reader.BaseStream.Seek(position, SeekOrigin.Begin);
            }
            materialGleS11Desc.nTex = reader.ReadInt32();
            uint num3 = reader.ReadUInt32();
            if (num3 != 0U)
            {
                long position = reader.BaseStream.Position;
                reader.BaseStream.Seek(data0Pos + num3, SeekOrigin.Begin);
                materialGleS11Desc.pTexDesc = new NNS_MATERIAL_GLES11_TEXMAP_DESC[materialGleS11Desc.nTex];
                for (int index = 0; index < materialGleS11Desc.nTex; ++index)
                    materialGleS11Desc.pTexDesc[index] = NNS_MATERIAL_GLES11_TEXMAP_DESC.Read(reader, data0Pos);
                reader.BaseStream.Seek(position, SeekOrigin.Begin);
            }
            return materialGleS11Desc;
        }

        public NNS_MATERIAL_GLES11_DESC()
        {
        }

        public NNS_MATERIAL_GLES11_DESC(NNS_MATERIAL_GLES11_DESC desc)
        {
            this.fFlag = desc.fFlag;
            this.User = desc.User;
            this.pColor = desc.pColor;
            this.pLogic = desc.pLogic;
            this.nTex = desc.nTex;
            this.pTexDesc = desc.pTexDesc;
        }

        public NNS_MATERIAL_GLES11_DESC Assign(NNS_MATERIAL_GLES11_DESC desc)
        {
            this.fFlag = desc.fFlag;
            this.User = desc.User;
            this.pColor = desc.pColor;
            this.pLogic = desc.pLogic;
            this.nTex = desc.nTex;
            this.pTexDesc = desc.pTexDesc;
            return this;
        }
    }
}
