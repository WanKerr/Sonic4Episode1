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
    public class AMS_AME_CREATE_PARAM : AppMain.IClearable
    {
        public AppMain.AMS_AME_ECB ecb;
        public AppMain.AMS_AME_RUNTIME runtime;
        public AppMain.AMS_AME_NODE node;
        public AppMain.AMS_AME_RUNTIME_WORK work;
        public AppMain.NNS_VECTOR4D position;
        public AppMain.NNS_VECTOR4D velocity;
        public AppMain.NNS_VECTOR4D parent_position;
        public AppMain.NNS_VECTOR4D parent_velocity;

        public void Clear()
        {
            this.ecb = (AppMain.AMS_AME_ECB)null;
            this.runtime = (AppMain.AMS_AME_RUNTIME)null;
            this.node = (AppMain.AMS_AME_NODE)null;
            this.work = (AppMain.AMS_AME_RUNTIME_WORK)null;
            this.position = (AppMain.NNS_VECTOR4D)null;
            this.velocity = (AppMain.NNS_VECTOR4D)null;
            this.parent_position = (AppMain.NNS_VECTOR4D)null;
            this.parent_velocity = (AppMain.NNS_VECTOR4D)null;
        }
    }
}
