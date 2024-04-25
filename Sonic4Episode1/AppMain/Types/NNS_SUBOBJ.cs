using System.IO;

public partial class AppMain
{
    public class NNS_SUBOBJ
    {
        public uint fType;
        public int nMeshset;
        public NNS_MESHSET[] pMeshsetList;
        public int nTex;
        public int[] pTexNumList;

        public NNS_SUBOBJ()
        {
        }

        public NNS_SUBOBJ(NNS_SUBOBJ subObj)
        {
            this.fType = subObj.fType;
            this.nMeshset = subObj.nMeshset;
            this.pMeshsetList = subObj.pMeshsetList;
            this.nTex = subObj.nTex;
            this.pTexNumList = subObj.pTexNumList;
        }

        public NNS_SUBOBJ Assign(NNS_SUBOBJ subObj)
        {
            if (this != subObj)
            {
                this.fType = subObj.fType;
                this.nMeshset = subObj.nMeshset;
                this.pMeshsetList = subObj.pMeshsetList;
                this.nTex = subObj.nTex;
                this.pTexNumList = subObj.pTexNumList;
            }
            return this;
        }

        public static NNS_SUBOBJ Read(BinaryReader reader, long data0Pos)
        {
            NNS_SUBOBJ nnsSubobj = new NNS_SUBOBJ();
            nnsSubobj.fType = reader.ReadUInt32();
            nnsSubobj.nMeshset = reader.ReadInt32();
            uint num1 = reader.ReadUInt32();
            if (num1 != 0U)
            {
                nnsSubobj.pMeshsetList = new NNS_MESHSET[nnsSubobj.nMeshset];
                long position = reader.BaseStream.Position;
                reader.BaseStream.Seek(data0Pos + num1, SeekOrigin.Begin);
                for (int index = 0; index < nnsSubobj.nMeshset; ++index)
                    nnsSubobj.pMeshsetList[index] = NNS_MESHSET.Read(reader);
                reader.BaseStream.Seek(position, SeekOrigin.Begin);
            }
            nnsSubobj.nTex = reader.ReadInt32();
            uint num2 = reader.ReadUInt32();
            if (num2 != 0U)
            {
                nnsSubobj.pTexNumList = new int[nnsSubobj.nTex];
                long position = reader.BaseStream.Position;
                reader.BaseStream.Seek(data0Pos + num2, SeekOrigin.Begin);
                for (int index = 0; index < nnsSubobj.nTex; ++index)
                    nnsSubobj.pTexNumList[index] = reader.ReadInt32();
                reader.BaseStream.Seek(position, SeekOrigin.Begin);
            }
            return nnsSubobj;
        }
    }
}
