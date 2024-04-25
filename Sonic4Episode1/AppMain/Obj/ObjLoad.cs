public partial class AppMain
{
    private static bool ObjLoadInitDraw()
    {
        OBS_LOAD_INITIAL_WORK objLoadInitialWork = obj_load_initial_work;
        for (int index = 0; index < objLoadInitialWork.obj_num; ++index)
        {
            uint? p_disp_flag = new uint?();
            ObjDrawAction3DNN(objLoadInitialWork.obj_3d[index], new VecFx32?(), new AppMain.VecU16?(), new VecFx32?(), ref p_disp_flag);
        }
        for (int index = 0; index < objLoadInitialWork.es_num; ++index)
        {
            uint? p_disp_flag = new uint?();
            ObjDrawAction3DES(objLoadInitialWork.obj_3des[index], new VecFx32?(), new AppMain.VecU16?(), new VecFx32?(), ref p_disp_flag);
        }
        return true;
    }

    private static void ObjLoadClearDraw()
    {
        obj_load_initial_work.obj_num = 0;
        obj_load_initial_work.es_num = 0;
    }

    private static void ObjLoadSetInitDrawFlag(bool flag)
    {
        obj_load_initial_set_flag = flag;
        ObjLoadClearDraw();
    }


}