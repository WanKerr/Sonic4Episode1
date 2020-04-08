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
    public class NNS_MORPHTARGETNAME
    {
        public int iMorphTarget;
        public string Name;

        public NNS_MORPHTARGETNAME()
        {
        }

        public NNS_MORPHTARGETNAME(AppMain.NNS_MORPHTARGETNAME morthTargetName)
        {
            this.iMorphTarget = morthTargetName.iMorphTarget;
            this.Name = morthTargetName.Name;
        }

        public AppMain.NNS_MORPHTARGETNAME Assign(AppMain.NNS_MORPHTARGETNAME morthTargetName)
        {
            this.iMorphTarget = morthTargetName.iMorphTarget;
            this.Name = morthTargetName.Name;
            return this;
        }
    }
}
