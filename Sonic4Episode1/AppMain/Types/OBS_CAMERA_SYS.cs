using System;

public partial class AppMain
{
    public class OBS_CAMERA_SYS
    {
        public readonly OBS_CAMERA[] obj_camera = new OBS_CAMERA[8];
        public int camera_num;

        public void Clear()
        {
            Array.Clear(obj_camera, 0, this.obj_camera.Length);
            this.camera_num = 0;
        }
    }
}
