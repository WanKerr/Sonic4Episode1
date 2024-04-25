using System.IO;

public partial class AppMain
{
    public class NNS_NODE
    {
        public readonly NNS_VECTOR Translation = GlobalPool<NNS_VECTOR>.Alloc();
        public readonly NNS_VECTOR Scaling = GlobalPool<NNS_VECTOR>.Alloc();
        public readonly NNS_MATRIX InvInitMtx = GlobalPool<NNS_MATRIX>.Alloc();
        public readonly NNS_VECTOR Center = GlobalPool<NNS_VECTOR>.Alloc();

        public uint fType;
        public short iMatrix;
        public short iParent;
        public short iChild;
        public short iSibling;
        public NNS_ROTATE_A32 Rotation;
        public float Radius;
        public uint User;
        public float SIIKBoneLength;
        public float BoundingBoxY;
        public float BoundingBoxZ;

        public float BoundingBoxX
        {
            get => this.SIIKBoneLength;
            set => this.SIIKBoneLength = value;
        }

        public NNS_NODE()
        {
        }

        public NNS_NODE(NNS_NODE node)
        {
            this.fType = node.fType;
            this.iMatrix = node.iMatrix;
            this.iParent = node.iParent;
            this.iChild = node.iChild;
            this.iSibling = node.iSibling;
            this.Translation.Assign(node.Translation);
            this.Rotation = node.Rotation;
            this.Scaling.Assign(node.Scaling);
            this.InvInitMtx.Assign(node.InvInitMtx);
            this.Center.Assign(node.Center);
            this.Radius = node.Radius;
            this.User = node.User;
            this.SIIKBoneLength = node.SIIKBoneLength;
            this.BoundingBoxY = node.BoundingBoxY;
            this.BoundingBoxZ = node.BoundingBoxZ;
        }

        public NNS_NODE Assign(NNS_NODE node)
        {
            if (this != node)
            {
                this.fType = node.fType;
                this.iMatrix = node.iMatrix;
                this.iParent = node.iParent;
                this.iChild = node.iChild;
                this.iSibling = node.iSibling;
                this.Translation.Assign(node.Translation);
                this.Rotation = node.Rotation;
                this.Scaling.Assign(node.Scaling);
                this.InvInitMtx.Assign(node.InvInitMtx);
                this.Center.Assign(node.Center);
                this.Radius = node.Radius;
                this.User = node.User;
                this.SIIKBoneLength = node.SIIKBoneLength;
                this.BoundingBoxY = node.BoundingBoxY;
                this.BoundingBoxZ = node.BoundingBoxZ;
            }
            return this;
        }

        public static NNS_NODE Read(BinaryReader reader, long data0Pos)
        {
            return new NNS_NODE()
            {
                fType = reader.ReadUInt32(),
                iMatrix = reader.ReadInt16(),
                iParent = reader.ReadInt16(),
                iChild = reader.ReadInt16(),
                iSibling = reader.ReadInt16(),
                Translation = {
                    x = reader.ReadSingle(),
                    y = reader.ReadSingle(),
                    z = reader.ReadSingle()
                },
                Rotation = {
                    x = reader.ReadInt32(),
                    y = reader.ReadInt32(),
                    z = reader.ReadInt32()
                },
                Scaling = {
                    x = reader.ReadSingle(),
                    y = reader.ReadSingle(),
                    z = reader.ReadSingle()
                },
                InvInitMtx = {
                    M00 = reader.ReadSingle(),
                    M10 = reader.ReadSingle(),
                    M20 = reader.ReadSingle(),
                    M30 = reader.ReadSingle(),
                    M01 = reader.ReadSingle(),
                    M11 = reader.ReadSingle(),
                    M21 = reader.ReadSingle(),
                    M31 = reader.ReadSingle(),
                    M02 = reader.ReadSingle(),
                    M12 = reader.ReadSingle(),
                    M22 = reader.ReadSingle(),
                    M32 = reader.ReadSingle(),
                    M03 = reader.ReadSingle(),
                    M13 = reader.ReadSingle(),
                    M23 = reader.ReadSingle(),
                    M33 = reader.ReadSingle()
                },
                Center = {
                    x = reader.ReadSingle(),
                    y = reader.ReadSingle(),
                    z = reader.ReadSingle()
                },
                Radius = reader.ReadSingle(),
                User = reader.ReadUInt32(),
                BoundingBoxX = reader.ReadSingle(),
                BoundingBoxY = reader.ReadSingle(),
                BoundingBoxZ = reader.ReadSingle()
            };
        }
    }
}
