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
    public class GMS_MAP_PRIM_DRAW_TVX_UV_WORK
    {
        public int mgr_index_tbl_num;
        public DoubleType<uint[], AppMain.GMS_MAP_PRIM_DRAW_TVX_MGR_INDEX[]> mgr_index_tbl_addr;
        public int[] mgr_tbl_num;
        public DoubleType<uint[], AppMain.GMS_MAP_PRIM_DRAW_TVX_MGR[][]> mgr_tbl_addr;
        public DoubleType<uint[], AppMain.NNS_TEXCOORD[][]> uv_mgr_tbl_addr;
        public uint[] frame_index_tbl;
        public uint[] frame_tbl;
        public int[] tex_uv_index_tbl;
    }
}
