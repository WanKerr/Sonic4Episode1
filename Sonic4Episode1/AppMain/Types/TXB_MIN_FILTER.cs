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
    private enum TXB_MIN_FILTER
    {
        TXB_MINF_N,
        TXB_MINF_L,
        TXB_MINF_N_M_N,
        TXB_MINF_N_M_L,
        TXB_MINF_L_M_N,
        TXB_MINF_L_M_L,
        TXB_MINF_A2,
        TXB_MINF_A2_M_N,
        TXB_MINF_A2_M_L,
        TXB_MINF_A4,
        TXB_MINF_A4_M_N,
        TXB_MINF_A4_M_L,
        TXB_MINF_A8,
        TXB_MINF_A8_M_N,
        TXB_MINF_A8_M_L,
        TXB_MINF_NUM,
    }
}
