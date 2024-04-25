using System.IO;

public partial class AppMain
{
    public class NNS_OBJECT
    {
        public readonly NNS_VECTOR Center = new NNS_VECTOR();
        public float Radius;
        public int nMaterial;
        public NNS_MATERIALPTR[] pMatPtrList;
        public int nVtxList;
        public NNS_VTXLISTPTR[] pVtxListPtrList;
        public int nPrimList;
        public NNS_PRIMLISTPTR[] pPrimListPtrList;
        public int nNode;
        public int MaxNodeDepth;
        public NNS_NODE[] pNodeList;
        public int nMtxPal;
        public int nSubobj;
        public NNS_SUBOBJ[] pSubobjList;
        public int nTex;
        public uint fType;
        public int Version;
        public float BoundingBoxX;
        public float BoundingBoxY;
        public float BoundingBoxZ;

        public NNS_OBJECT()
        {
        }

        public NNS_OBJECT(NNS_OBJECT nnsObject)
        {
            this.Center.Assign(nnsObject.Center);
            this.Radius = nnsObject.Radius;
            this.nMaterial = nnsObject.nMaterial;
            this.pMatPtrList = nnsObject.pMatPtrList;
            this.nVtxList = nnsObject.nVtxList;
            this.pVtxListPtrList = nnsObject.pVtxListPtrList;
            this.nPrimList = nnsObject.nPrimList;
            this.pPrimListPtrList = nnsObject.pPrimListPtrList;
            this.nNode = nnsObject.nNode;
            this.MaxNodeDepth = nnsObject.MaxNodeDepth;
            this.pNodeList = nnsObject.pNodeList;
            this.nMtxPal = nnsObject.nMtxPal;
            this.nSubobj = nnsObject.nSubobj;
            this.pSubobjList = nnsObject.pSubobjList;
            this.nTex = nnsObject.nTex;
            this.fType = nnsObject.fType;
            this.Version = nnsObject.Version;
            this.BoundingBoxX = nnsObject.BoundingBoxX;
            this.BoundingBoxY = nnsObject.BoundingBoxY;
            this.BoundingBoxZ = nnsObject.BoundingBoxZ;
        }

        public NNS_OBJECT Assign(NNS_OBJECT nnsObject)
        {
            if (this != nnsObject)
            {
                this.Center.Assign(nnsObject.Center);
                this.Radius = nnsObject.Radius;
                this.nMaterial = nnsObject.nMaterial;
                this.pMatPtrList = nnsObject.pMatPtrList;
                this.nVtxList = nnsObject.nVtxList;
                this.pVtxListPtrList = nnsObject.pVtxListPtrList;
                this.nPrimList = nnsObject.nPrimList;
                this.pPrimListPtrList = nnsObject.pPrimListPtrList;
                this.nNode = nnsObject.nNode;
                this.MaxNodeDepth = nnsObject.MaxNodeDepth;
                this.pNodeList = nnsObject.pNodeList;
                this.nMtxPal = nnsObject.nMtxPal;
                this.nSubobj = nnsObject.nSubobj;
                this.pSubobjList = nnsObject.pSubobjList;
                this.nTex = nnsObject.nTex;
                this.fType = nnsObject.fType;
                this.Version = nnsObject.Version;
                this.BoundingBoxX = nnsObject.BoundingBoxX;
                this.BoundingBoxY = nnsObject.BoundingBoxY;
                this.BoundingBoxZ = nnsObject.BoundingBoxZ;
            }
            return this;
        }

        public static NNS_OBJECT Read(BinaryReader reader, long data0Pos)
        {
            NNS_OBJECT nnsObject = new NNS_OBJECT();
            nnsObject.Center.x = reader.ReadSingle();
            nnsObject.Center.y = reader.ReadSingle();
            nnsObject.Center.z = reader.ReadSingle();
            nnsObject.Radius = reader.ReadSingle();
            nnsObject.nMaterial = reader.ReadInt32();
            uint num1 = reader.ReadUInt32();
            if (num1 != 0U)
            {
                bool flag = false;
                nnsObject.pMatPtrList = new NNS_MATERIALPTR[nnsObject.nMaterial];
                long position = reader.BaseStream.Position;
                reader.BaseStream.Seek(data0Pos + num1, SeekOrigin.Begin);
                for (int index = 0; index < nnsObject.nMaterial; ++index)
                {
                    bool transparentMaterial;
                    nnsObject.pMatPtrList[index] = NNS_MATERIALPTR.Read(reader, data0Pos, out transparentMaterial);
                    flag |= transparentMaterial;
                }
                if (flag)
                {
                    for (int index = 0; index < nnsObject.nMaterial; ++index)
                        ((NNS_MATERIAL_GLES11_DESC)nnsObject.pMatPtrList[index].pMaterial).fFlag |= 1U;
                }
                reader.BaseStream.Seek(position, SeekOrigin.Begin);
            }
            nnsObject.nVtxList = reader.ReadInt32();
            uint num2 = reader.ReadUInt32();
            if (num2 != 0U)
            {
                nnsObject.pVtxListPtrList = new NNS_VTXLISTPTR[nnsObject.nVtxList];
                long position = reader.BaseStream.Position;
                reader.BaseStream.Seek(data0Pos + num2, SeekOrigin.Begin);
                for (int index = 0; index < nnsObject.nVtxList; ++index)
                    nnsObject.pVtxListPtrList[index] = NNS_VTXLISTPTR.Read(reader, data0Pos);
                reader.BaseStream.Seek(position, SeekOrigin.Begin);
            }
            nnsObject.nPrimList = reader.ReadInt32();
            uint num3 = reader.ReadUInt32();
            if (num3 != 0U)
            {
                nnsObject.pPrimListPtrList = new NNS_PRIMLISTPTR[nnsObject.nPrimList];
                long position = reader.BaseStream.Position;
                reader.BaseStream.Seek(data0Pos + num3, SeekOrigin.Begin);
                for (int index = 0; index < nnsObject.nPrimList; ++index)
                    nnsObject.pPrimListPtrList[index] = NNS_PRIMLISTPTR.Read(reader, data0Pos);
                reader.BaseStream.Seek(position, SeekOrigin.Begin);
            }
            nnsObject.nNode = reader.ReadInt32();
            nnsObject.MaxNodeDepth = reader.ReadInt32();
            uint num4 = reader.ReadUInt32();
            if (num4 != 0U)
            {
                nnsObject.pNodeList = new NNS_NODE[nnsObject.nNode];
                long position = reader.BaseStream.Position;
                reader.BaseStream.Seek(data0Pos + num4, SeekOrigin.Begin);
                for (int index = 0; index < nnsObject.nNode; ++index)
                    nnsObject.pNodeList[index] = NNS_NODE.Read(reader, data0Pos);
                reader.BaseStream.Seek(position, SeekOrigin.Begin);
            }
            nnsObject.nMtxPal = reader.ReadInt32();
            nnsObject.nSubobj = reader.ReadInt32();
            uint num5 = reader.ReadUInt32();
            if (num5 != 0U)
            {
                nnsObject.pSubobjList = new NNS_SUBOBJ[nnsObject.nSubobj];
                long position = reader.BaseStream.Position;
                reader.BaseStream.Seek(data0Pos + num5, SeekOrigin.Begin);
                for (int index = 0; index < nnsObject.nSubobj; ++index)
                    nnsObject.pSubobjList[index] = NNS_SUBOBJ.Read(reader, data0Pos);
                reader.BaseStream.Seek(position, SeekOrigin.Begin);
            }
            nnsObject.nTex = reader.ReadInt32();
            nnsObject.fType = reader.ReadUInt32();
            nnsObject.Version = reader.ReadInt32();
            nnsObject.BoundingBoxX = reader.ReadSingle();
            nnsObject.BoundingBoxY = reader.ReadSingle();
            nnsObject.BoundingBoxZ = reader.ReadSingle();
            return nnsObject;
        }
    }
}
