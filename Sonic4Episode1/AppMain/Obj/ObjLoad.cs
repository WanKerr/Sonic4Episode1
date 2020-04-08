using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using mpp;

public partial class AppMain
{
    private static bool ObjLoadInitDraw()
    {
        AppMain.OBS_LOAD_INITIAL_WORK objLoadInitialWork = AppMain.obj_load_initial_work;
        for (int index = 0; index < objLoadInitialWork.obj_num; ++index)
        {
            uint? p_disp_flag = new uint?();
            AppMain.ObjDrawAction3DNN(objLoadInitialWork.obj_3d[index], new AppMain.VecFx32?(), new AppMain.VecU16?(), new AppMain.VecFx32?(), ref p_disp_flag);
        }
        for (int index = 0; index < objLoadInitialWork.es_num; ++index)
        {
            uint? p_disp_flag = new uint?();
            AppMain.ObjDrawAction3DES(objLoadInitialWork.obj_3des[index], new AppMain.VecFx32?(), new AppMain.VecU16?(), new AppMain.VecFx32?(), ref p_disp_flag);
        }
        return true;
    }

    private static void ObjLoadClearDraw()
    {
        AppMain.obj_load_initial_work.obj_num = 0;
        AppMain.obj_load_initial_work.es_num = 0;
    }

    private static void ObjLoadSetInitDrawFlag(bool flag)
    {
        AppMain.obj_load_initial_set_flag = flag;
        AppMain.ObjLoadClearDraw();
    }


}