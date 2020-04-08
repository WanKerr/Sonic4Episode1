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
    public class NNS_TRS
    {
        public AppMain.NNS_VECTORFAST Translation;
        public AppMain.NNS_QUATERNION Rotation;
        public AppMain.NNS_VECTORFAST Scaling;

        public NNS_TRS()
        {
        }

        public NNS_TRS(AppMain.NNS_TRS trs)
        {
            this.Translation = trs.Translation;
            this.Rotation = trs.Rotation;
            this.Scaling = trs.Scaling;
        }

        public AppMain.NNS_TRS Assign(AppMain.NNS_TRS trs)
        {
            if (this != trs)
            {
                this.Translation = trs.Translation;
                this.Rotation = trs.Rotation;
                this.Scaling = trs.Scaling;
            }
            return this;
        }
    }
}
