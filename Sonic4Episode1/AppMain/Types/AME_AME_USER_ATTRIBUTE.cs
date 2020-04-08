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
    private enum AME_AME_USER_ATTRIBUTE : uint
    {
        AME_AME_ATTR_INCLUSIVE = 0,
        AME_AME_ATTR_EXCLUSIVE = 1,
        AME_AME_ATTR_USER_MASK = 65535, // 0x0000FFFF
        AME_AME_ATTR_GROUP_00 = 65536, // 0x00010000
        AME_AME_ATTR_GROUP_01 = 131072, // 0x00020000
        AME_AME_ATTR_GROUP_02 = 262144, // 0x00040000
        AME_AME_ATTR_GROUP_03 = 524288, // 0x00080000
        AME_AME_ATTR_GROUP_04 = 1048576, // 0x00100000
        AME_AME_ATTR_GROUP_05 = 2097152, // 0x00200000
        AME_AME_ATTR_GROUP_06 = 4194304, // 0x00400000
        AME_AME_ATTR_GROUP_07 = 8388608, // 0x00800000
        AME_AME_ATTR_GROUP_08 = 16777216, // 0x01000000
        AME_AME_ATTR_GROUP_09 = 33554432, // 0x02000000
        AME_AME_ATTR_GROUP_0a = 67108864, // 0x04000000
        AME_AME_ATTR_GROUP_0b = 134217728, // 0x08000000
        AME_AME_ATTR_GROUP_0c = 268435456, // 0x10000000
        AME_AME_ATTR_GROUP_0d = 536870912, // 0x20000000
        AME_AME_ATTR_GROUP_0e = 1073741824, // 0x40000000
        AME_AME_ATTR_GROUP_0f = 2147483648, // 0x80000000
        AME_AME_ATTR_GROUP_ALL = 4294901760, // 0xFFFF0000
        AME_AME_ATTR_GROUP_MASK = 4294901760, // 0xFFFF0000
    }
}
