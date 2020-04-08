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
    public class NNS_NODENAMELIST
    {
        public AppMain.NNE_NODENAME_SORTTYPE SortType;
        public int nNode;
        public AppMain.NNS_NODENAME[] pNodeNameList;

        public NNS_NODENAMELIST()
        {
        }

        public NNS_NODENAMELIST(AppMain.NNS_NODENAMELIST nodeNameList)
        {
            this.SortType = nodeNameList.SortType;
            this.nNode = nodeNameList.nNode;
            this.pNodeNameList = nodeNameList.pNodeNameList;
        }

        public AppMain.NNS_NODENAMELIST Assign(AppMain.NNS_NODENAMELIST nodeNameList)
        {
            if (this != nodeNameList)
            {
                this.SortType = nodeNameList.SortType;
                this.nNode = nodeNameList.nNode;
                this.pNodeNameList = nodeNameList.pNodeNameList;
            }
            return this;
        }
    }
}
