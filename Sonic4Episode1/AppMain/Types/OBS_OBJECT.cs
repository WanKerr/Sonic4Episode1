public partial class AppMain
{
    public class OBS_OBJECT
    {
        public VecFx32 glb_scale = new VecFx32();
        public VecFx32 draw_scale = new VecFx32();
        public readonly short[] offset = new short[2];
        public readonly int[] scroll = new int[2];
        public readonly short[] lcd_size = new short[2];
        public readonly short[] clip_lcd_size = new short[2];
        public readonly int[][] camera = New<int>(2, 2);
        public readonly int[] clip_camera = new int[2];
        public readonly short[][] cam_scale_center = New<short>(2, 2);
        public NNS_RGB ambient_color = new NNS_RGB();
        public readonly OBS_LIGHT[] light = New<OBS_LIGHT>(NNE_LIGHT_MAX);
        public VecFx32 scale = new VecFx32();
        public VecFx32 inv_scale = new VecFx32();
        public VecFx32 inv_glb_scale = new VecFx32();
        public VecFx32 inv_draw_scale = new VecFx32();
        public int speed;
        public int depth;
        public uint timer;
        public int timer_fx;
        public uint flag;
        public int pause_level;
        public float disp_width;
        public float disp_height;
        public OBJECT_Cam_Delegate pp3dCam;
        public int glb_camera_id;
        public int glb_camera_type;
        public uint load_drawflag;
        public uint drawflag;
        public uint def_user_light_flag;
        public sbyte col_through_dot;
        public OBS_DATA_WORK[] pData;
        public int data_max;
        public OBS_OBJECT_WORK obj_list_head;
        public OBS_OBJECT_WORK obj_list_tail;
        public OBS_OBJECT_WORK obj_draw_list_head;
        public OBS_OBJECT_WORK obj_draw_list_tail;
        public OBJECT_Delegate ppPre;
        public OBJECT_Delegate ppPost;
        public OBJECT_Delegate ppDrawSort;
        public OBJECT_WORK_Delegate ppCollision;
        public OBJECT_WORK_Delegate ppObjPre;
        public OBJECT_WORK_Delegate ppObjPost;
        public OBJECT_WORK_Delegate ppRegRecAuto;
    }
}
