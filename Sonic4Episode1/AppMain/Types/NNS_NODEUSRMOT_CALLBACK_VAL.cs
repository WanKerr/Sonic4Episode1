using System;

public partial class AppMain
{
    public class NNS_NODEUSRMOT_CALLBACK_VAL
    {
        public int iNode;
        public float Frame;
        public uint IValue;
        public NNS_MOTION pMotion;
        public int iSubmot;
        public uint fSubmotType;
        public uint fSubmotIPType;
        public NNS_OBJECT pObject;

        public float FValue
        {
            get => BitConverter.ToSingle(BitConverter.GetBytes(this.IValue), 0);
            set => this.IValue = BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
        }
    }
}
