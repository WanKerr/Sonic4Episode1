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
    public enum AME_AME_NODE_TYPE : ushort
    {
        AME_AME_CLASS_ID_MASK = 255, // 0x00FF
        AME_AME_NODE_TYPE_EMITTER = 256, // 0x0100
        AME_AME_NODE_TYPE_OMNI = 256, // 0x0100
        AME_AME_NODE_TYPE_DIRECTIONAL = 257, // 0x0101
        AME_AME_NODE_TYPE_SURFACE = 258, // 0x0102
        AME_AME_NODE_TYPE_CIRCLE = 259, // 0x0103
        AME_AME_NODE_TYPE_USER_EMITTER = 264, // 0x0108
        AME_AME_NODE_TYPE_PARTICLE = 512, // 0x0200
        AME_AME_NODE_TYPE_SIMPLE_SPRITE = 512, // 0x0200
        AME_AME_NODE_TYPE_SPRITE = 513, // 0x0201
        AME_AME_NODE_TYPE_LINE = 514, // 0x0202
        AME_AME_NODE_TYPE_PLANE = 515, // 0x0203
        AME_AME_NODE_TYPE_MODEL = 516, // 0x0204
        AME_AME_NODE_TYPE_USER_PARTICLE = 520, // 0x0208
        AME_AME_NODE_TYPE_FIELD = 768, // 0x0300
        AME_AME_NODE_TYPE_GRAVITY = 768, // 0x0300
        AME_AME_NODE_TYPE_UNIFORM = 769, // 0x0301
        AME_AME_NODE_TYPE_RADIAL = 770, // 0x0302
        AME_AME_NODE_TYPE_VORTEX = 771, // 0x0303
        AME_AME_NODE_TYPE_DRAG = 772, // 0x0304
        AME_AME_NODE_TYPE_NOISE = 773, // 0x0305
        AME_AME_NODE_TYPE_USER_FIELD = 776, // 0x0308
        AME_AME_SUPER_CLASS_ID_MASK = 65280, // 0xFF00
    }
}
