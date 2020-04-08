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
    public class AMS_TCB : AppMain.IClearable
    {
        public readonly AppMain.AMS_TCB_FOOTER footer = new AppMain.AMS_TCB_FOOTER();
        public string name;
        public uint user_id;
        public uint attribute;
        public uint priority;
        public AppMain.TaskProc procedure;
        public int proc_addr;
        public AppMain.TaskProc destructor;
        public AppMain.AMS_TCB prev;
        public AppMain.AMS_TCB next;
        public AppMain.AMS_TASK taskp;
        public int wkbegin;
        public object work;

        public void Clear()
        {
            this.name = string.Empty;
            this.user_id = 0U;
            this.attribute = 0U;
            this.priority = 0U;
            this.procedure = (AppMain.TaskProc)null;
            this.proc_addr = 0;
            this.destructor = (AppMain.TaskProc)null;
            this.prev = (AppMain.AMS_TCB)null;
            this.next = (AppMain.AMS_TCB)null;
            this.taskp = (AppMain.AMS_TASK)null;
            this.footer.wkend = 0;
        }
    }
}
