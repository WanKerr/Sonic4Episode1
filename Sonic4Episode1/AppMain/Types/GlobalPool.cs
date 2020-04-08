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
    public class GlobalPool<T> where T : class, AppMain.IClearable, new()
    {
        private static List<T> cache_ = new List<T>();

        public static T Alloc()
        {
            T obj;
            if (AppMain.GlobalPool<T>.cache_.Count > 0)
            {
                int index = AppMain.GlobalPool<T>.cache_.Count - 1;
                obj = AppMain.GlobalPool<T>.cache_[index];
                obj.Clear();
                AppMain.GlobalPool<T>.cache_.RemoveAt(index);
            }
            else
                obj = new T();
            return obj;
        }

        public static void Release(T obj)
        {
            AppMain.GlobalPool<T>.cache_.Add(obj);
        }
    }
}
