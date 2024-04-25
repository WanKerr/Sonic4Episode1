using System.IO;

public partial class AppMain
{
    public class NNS_MESHSET
    {
        public readonly NNS_VECTOR Center = GlobalPool<NNS_VECTOR>.Alloc();
        public float Radius;
        public int iNode;
        public int iMatrix;
        public int iMaterial;
        public int iVtxList;
        public int iPrimList;
        public uint Reserved2;
        public uint Reserved1;
        public uint Reserved0;

        public NNS_MESHSET()
        {
        }

        public NNS_MESHSET(NNS_MESHSET meshSet)
        {
            this.Center.Assign(meshSet.Center);
            this.Radius = meshSet.Radius;
            this.iNode = meshSet.iNode;
            this.iMatrix = meshSet.iMatrix;
            this.iMaterial = meshSet.iMaterial;
            this.iVtxList = meshSet.iVtxList;
            this.iPrimList = meshSet.iPrimList;
            this.Reserved2 = meshSet.Reserved2;
            this.Reserved1 = meshSet.Reserved1;
            this.Reserved0 = meshSet.Reserved0;
        }

        public NNS_MESHSET Assign(NNS_MESHSET meshSet)
        {
            if (this != meshSet)
            {
                this.Center.Assign(meshSet.Center);
                this.Radius = meshSet.Radius;
                this.iNode = meshSet.iNode;
                this.iMatrix = meshSet.iMatrix;
                this.iMaterial = meshSet.iMaterial;
                this.iVtxList = meshSet.iVtxList;
                this.iPrimList = meshSet.iPrimList;
                this.Reserved2 = meshSet.Reserved2;
                this.Reserved1 = meshSet.Reserved1;
                this.Reserved0 = meshSet.Reserved0;
            }
            return this;
        }

        public static NNS_MESHSET Read(BinaryReader reader)
        {
            return new NNS_MESHSET()
            {
                Center = {
          x = reader.ReadSingle(),
          y = reader.ReadSingle(),
          z = reader.ReadSingle()
        },
                Radius = reader.ReadSingle(),
                iNode = reader.ReadInt32(),
                iMatrix = reader.ReadInt32(),
                iMaterial = reader.ReadInt32(),
                iVtxList = reader.ReadInt32(),
                iPrimList = reader.ReadInt32(),
                Reserved2 = reader.ReadUInt32(),
                Reserved1 = reader.ReadUInt32(),
                Reserved0 = reader.ReadUInt32()
            };
        }
    }
}
