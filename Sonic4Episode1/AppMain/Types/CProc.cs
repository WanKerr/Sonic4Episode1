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
    public class CProc
    {
        private AppMain.ITaskAsv.FProc m_proc;
        private object m_it;

        public void operator_brackets()
        {
            if (this.m_it == null || this.IsNoneProc())
                return;
            this.m_proc();
        }

        public bool IsNoneProc()
        {
            return this.m_proc == null;
        }

        public bool IsProc(AppMain.ITaskAsv.FProc proc)
        {
            return this.m_proc == proc;
        }

        public bool IsProc()
        {
            return this.IsNoneProc();
        }

        public AppMain.ITaskAsv.FProc GetProc()
        {
            return this.m_proc;
        }

        public void SetTarget(object it)
        {
            this.m_it = it;
            this.SetProc();
        }

        public void SetTarget()
        {
            this.m_it = (object)null;
            this.SetProc();
        }

        public void SetProc(AppMain.ITaskAsv.FProc proc)
        {
            this.m_proc = proc;
        }

        public void SetProc()
        {
            this.m_proc = (AppMain.ITaskAsv.FProc)null;
        }

        public CProc()
        {
            this.m_it = (object)null;
            this.m_proc = (AppMain.ITaskAsv.FProc)null;
        }

        public CProc(object it)
        {
            this.m_it = it;
            this.m_proc = (AppMain.ITaskAsv.FProc)null;
        }
    }
}
