using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using accel;
using dbg;
using er;
using er.web;
using gs;
using gs.backup;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using mpp;
using setting;

public partial class AppMain
{
    public class NNS_NODEUSRMOT_CALLBACK_VAL
    {
        public int iNode;
        public float Frame;
        public uint IValue;
        public AppMain.NNS_MOTION pMotion;
        public int iSubmot;
        public uint fSubmotType;
        public uint fSubmotIPType;
        public AppMain.NNS_OBJECT pObject;

        public float FValue
        {
            get
            {
                return BitConverter.ToSingle(BitConverter.GetBytes(this.IValue), 0);
            }
            set
            {
                this.IValue = BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
            }
        }
    }
}
