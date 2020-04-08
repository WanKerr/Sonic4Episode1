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
    public class GMS_EFCT_ENE_CREATE_PARAM
    {
        public readonly AppMain.GMS_EFFECT_CREATE_PARAM create_param;
        public int arc_dwork_no;
        public int ambtex_dwork_no;
        public int ame_dwork_no;
        public int ambtex_idx;
        public uint stage_flag;

        public GMS_EFCT_ENE_CREATE_PARAM()
        {
            this.create_param = new AppMain.GMS_EFFECT_CREATE_PARAM();
        }

        public GMS_EFCT_ENE_CREATE_PARAM(
          AppMain.GMS_EFFECT_CREATE_PARAM create_param,
          int arc_dwork_no,
          int ambtex_dwork_no,
          int ame_dwork_no,
          int ambtex_idx,
          uint stage_flag)
        {
            this.create_param = create_param;
            this.arc_dwork_no = arc_dwork_no;
            this.ambtex_dwork_no = ambtex_dwork_no;
            this.ame_dwork_no = ame_dwork_no;
            this.ambtex_idx = ambtex_idx;
            this.stage_flag = stage_flag;
        }
    }
}
