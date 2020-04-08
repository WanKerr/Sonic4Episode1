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
    public class CProcCount : AppMain.CProc
    {
        private uint m_counter;

        public new void operator_brackets()
        {
            ++this.m_counter;
            base.operator_brackets();
        }

        public uint GetCount()
        {
            return this.m_counter;
        }

        public new void SetProc(AppMain.ITaskAsv.FProc proc)
        {
            this.ResetCounter();
            base.SetProc(proc);
        }

        public new void SetProc()
        {
            this.ResetCounter();
            base.SetProc();
        }

        public CProcCount()
        {
            this.ResetCounter();
        }

        public CProcCount(object it)
          : base(it)
        {
            this.ResetCounter();
        }

        protected void ResetCounter()
        {
            this.m_counter = uint.MaxValue;
        }
    }
}
