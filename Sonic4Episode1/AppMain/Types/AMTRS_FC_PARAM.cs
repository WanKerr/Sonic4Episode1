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
    public class AMTRS_FC_PARAM
    {
        public float[] m_x = new float[4];
        public float[] m_y = new float[4];
        public float[] m_z = new float[4];
        public float[] m_Dx = new float[4];
        public float[] m_Dy = new float[4];
        public float[] m_Dz = new float[4];
        public AppMain.NNS_VECTOR4D m_CalcParam = new AppMain.NNS_VECTOR4D();
        public float m_t;
        public uint m_flag;
    }
}
