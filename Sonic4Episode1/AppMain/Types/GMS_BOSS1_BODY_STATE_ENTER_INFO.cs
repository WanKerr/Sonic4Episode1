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
    public class GMS_BOSS1_BODY_STATE_ENTER_INFO
    {
        public AppMain.GMF_BOSS1_BODY_STATE_ENTER_FUNC enter_func;
        public bool is_wrapped;

        public GMS_BOSS1_BODY_STATE_ENTER_INFO(
          AppMain.GMF_BOSS1_BODY_STATE_ENTER_FUNC _enter_func,
          bool _is_wrapped)
        {
            this.enter_func = _enter_func;
            this.is_wrapped = _is_wrapped;
        }
    }
}
