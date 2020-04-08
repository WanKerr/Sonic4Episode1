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
    public class CActionDraw
    {
        public CCircularBuffer<AppMain.AOS_ACTION> m_action_array = new CCircularBuffer<AppMain.AOS_ACTION>(100);

        public void Entry(AppMain.A2S_AMA_HEADER ama, uint id, float frame, float x, float y)
        {
            if (!AppMain._am_sample_draw_enable)
                return;
            AppMain.AOS_ACTION act = AppMain.AoActCreate(ama, id, frame);
            AppMain.AoActAcmPush();
            AppMain.AoActAcmInit();
            AppMain.AoActAcmApplyTrans(x, y, 0.0f);
            AppMain.AoActUpdate(act, 0.0f);
            AppMain.AoActSortRegAction(act);
            AppMain.AoActAcmPop();
            this.m_action_array.push_back(act);
        }

        public void Entry(
          AppMain.A2S_AMA_HEADER ama,
          uint id,
          float frame,
          float x,
          float y,
          float scalex,
          float scaley)
        {
            if (!AppMain._am_sample_draw_enable)
                return;
            AppMain.AOS_ACTION act = AppMain.AoActCreate(ama, id, frame);
            AppMain.AoActAcmPush();
            AppMain.AoActAcmInit();
            AppMain.AoActAcmApplyTrans(x, y, 0.0f);
            AppMain.AoActAcmApplyScale(scalex, scaley);
            AppMain.AoActUpdate(act, 0.0f);
            AppMain.AoActSortRegAction(act);
            AppMain.AoActAcmPop();
            this.m_action_array.push_back(act);
        }

        public void Clear()
        {
            for (int index = 0; index < this.m_action_array.size(); ++index)
                AppMain.AoActDelete(this.m_action_array[index]);
            this.m_action_array.clear();
        }

        public void Draw()
        {
            if (AppMain._am_sample_draw_enable)
            {
                AppMain.AoActSortExecute();
                AppMain.AoActSortDraw();
            }
            AppMain.AoActSortUnregAll();
        }

        public void CActionDraw_destructor()
        {
            this.Clear();
        }
    }
}
