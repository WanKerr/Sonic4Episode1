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
    public class NNS_MORPHTARGETNAMELIST
    {
        public AppMain.NNE_MORPHTARGETNAME_SORTTYPE SortType;
        public int nMorphTarget;
        public AppMain.NNS_MORPHTARGETNAME[] pMorphTargetNameList;

        public NNS_MORPHTARGETNAMELIST()
        {
        }

        public NNS_MORPHTARGETNAMELIST(
          AppMain.NNS_MORPHTARGETNAMELIST morthTargetNameList)
        {
            this.SortType = morthTargetNameList.SortType;
            this.nMorphTarget = morthTargetNameList.nMorphTarget;
            this.pMorphTargetNameList = morthTargetNameList.pMorphTargetNameList;
        }

        public AppMain.NNS_MORPHTARGETNAMELIST Assign(
          AppMain.NNS_MORPHTARGETNAMELIST morthTargetNameList)
        {
            this.SortType = morthTargetNameList.SortType;
            this.nMorphTarget = morthTargetNameList.nMorphTarget;
            this.pMorphTargetNameList = morthTargetNameList.pMorphTargetNameList;
            return this;
        }
    }
}
