// Decompiled with JetBrains decompiler
// Type: SaveState
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

using Sonic4ep1;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading;

public class SaveState
{
    public static bool saveLater = false;
    private static bool resumeStarting = false;
    private static bool beginResume = false;
    private static bool firstShow = true;
    public const int SAVE_AFTER_RESPAWN = 0;
    public const int SAVE_AFTER_CHECKPOINT = 1;
    private const string filename = "laststate.dat";
    public static SaveState.SaveData save;
    private static Thread saveThread;

    public static void saveCurrentState(int mode)
    {
        if (AppMain.GsTrialIsTrial())
            return;
        SaveState.save.player_pos_x = AppMain.g_gm_main_system.ply_work[0].obj_work.pos.x;
        SaveState.save.player_pos_y = AppMain.g_gm_main_system.ply_work[0].obj_work.pos.y;
        SaveState.save.resume_pos_x = AppMain.g_gm_main_system.resume_pos_x;
        SaveState.save.resume_pos_y = AppMain.g_gm_main_system.resume_pos_y;
        SaveState.save.game_time = AppMain.g_gm_main_system.game_time;
        SaveState.save.time_save = AppMain.g_gm_main_system.time_save;
        SaveState.save.marker_pri = AppMain.g_gm_main_system.marker_pri;
        SaveState.save.water_levell = AppMain.g_gm_main_system.water_level;
        SaveState.save.pseudofall_dir = AppMain.g_gm_main_system.pseudofall_dir;
        SaveState.save.rest_num = AppMain.g_gm_main_system.player_rest_num[0];
        SaveState.save.stage_id = AppMain.g_gs_main_sys_info.stage_id;
        SaveState.save.level = AppMain.g_gs_main_sys_info.level;
        SaveState.save.game_mode = AppMain.g_gs_main_sys_info.game_mode;
        SaveState.save.boss_load_no = AppMain.g_gm_main_system.boss_load_no;
        SaveState.save.player_flag = AppMain.g_gm_main_system.ply_work[0].player_flag;
        SaveState.save.ring_num = AppMain.g_gm_main_system.ply_work[0].ring_num;
        SaveState.save.ring_stage_num = AppMain.g_gm_main_system.ply_work[0].ring_stage_num;
        SaveState.save.score = AppMain.g_gm_main_system.ply_work[0].score;
        if (mode == 1)
        {
            SaveState.save.gm_eve_data = AppMain.gm_eve_data.saveData();
            SaveState.save.gm_ring_data = AppMain.gm_ring_data.saveData();
            SaveState.saveLater = false;
        }
        else
        {
            SaveState.save.gm_eve_data = (byte[])null;
            SaveState.save.gm_ring_data = (byte[])null;
            SaveState.saveLater = false;
        }
        if (SaveState.saveLater)
            return;
        if (SaveState.saveThread != null)
            SaveState.saveThread = (Thread)null;
        SaveState.saveThread = new Thread(new ParameterizedThreadStart(SaveState._saveFile));
        SaveState.SaveData save = SaveState.save;
        SaveState.saveThread.Start((object)save);
    }

    public static void _saveFile(object o)
    {
        SaveState.saveLater = false;
        SaveState.SaveData saveData = (SaveState.SaveData)o;
        var path = Path.Combine(AppMain.storePath, "laststate.dat");
        if (File.Exists(path))
            File.Delete(path);
        using (var file = File.Create(path))
            saveData.Serialize((Stream)file);
    }

    public static bool isSaveAvailable()
    {
        if (AppMain.GsTrialIsTrial())
            return false;
        var path = Path.Combine(AppMain.storePath, "laststate.dat");
        try
        {
            return File.Exists(path);
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public static bool loadState()
    {
        if (AppMain.GsTrialIsTrial())
            return false;

        var path = Path.Combine(AppMain.storePath, "laststate.dat");
        try
        {
            if (!File.Exists(path))
                return false;
            using (var storageFileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                SaveState.save.UnSerialize((Stream)storageFileStream);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public static void resumePlayerState()
    {
        if (AppMain.GsTrialIsTrial())
            return;
        AppMain.g_gm_main_system.resume_pos_x = SaveState.save.resume_pos_x;
        AppMain.g_gm_main_system.resume_pos_y = SaveState.save.resume_pos_y;
        AppMain.g_gm_main_system.game_time = SaveState.save.game_time;
        AppMain.g_gm_main_system.time_save = SaveState.save.time_save;
        AppMain.g_gm_main_system.marker_pri = SaveState.save.marker_pri;
        AppMain.g_gm_main_system.water_level = SaveState.save.water_levell;
        AppMain.g_gm_main_system.pseudofall_dir = SaveState.save.pseudofall_dir;
        AppMain.g_gm_main_system.player_rest_num[0] = SaveState.save.rest_num;
        SaveState.resumeStarting = true;
    }

    public static bool resumePlayer_2(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (AppMain.GsTrialIsTrial())
            return false;
        SaveState.beginResume = false;
        if (!SaveState.resumeStarting)
            return false;
        AppMain.GMS_PLAYER_WORK ply_work1 = ply_work;
        ply_work1.ring_num = SaveState.save.ring_num;
        ply_work1.ring_stage_num = SaveState.save.ring_stage_num;
        ply_work1.score = SaveState.save.score;
        ply_work1.obj_work.pos.x = SaveState.save.player_pos_x;
        ply_work1.obj_work.pos.y = SaveState.save.player_pos_y;
        AppMain.g_gm_main_system.resume_pos_x = SaveState.save.resume_pos_x;
        AppMain.g_gm_main_system.resume_pos_y = SaveState.save.resume_pos_y;
        AppMain.GmCameraPosSet(AppMain.g_gm_main_system.resume_pos_x, AppMain.g_gm_main_system.resume_pos_y, 0);
        AppMain.OBS_CAMERA obj_camera = AppMain.ObjCameraGet(AppMain.g_obj.glb_camera_id);
        AppMain.ObjObjectCameraSet(AppMain.FXM_FLOAT_TO_FX32(obj_camera.disp_pos.x - (float)((int)AppMain.OBD_LCD_X / 2)), AppMain.FXM_FLOAT_TO_FX32(-obj_camera.disp_pos.y - (float)((int)AppMain.OBD_LCD_Y / 2)), AppMain.FXM_FLOAT_TO_FX32(obj_camera.disp_pos.x - (float)((int)AppMain.OBD_LCD_X / 2)), AppMain.FXM_FLOAT_TO_FX32(-obj_camera.disp_pos.y - (float)((int)AppMain.OBD_LCD_Y / 2)));
        AppMain.GmCameraSetClipCamera(obj_camera);
        if ((16384 & (int)SaveState.save.player_flag) != 0)
        {
            ply_work1.obj_work.user_timer = 249856;
            AppMain.gmPlySeqTransformSuperMain(ply_work1);
            ply_work1.obj_work.user_timer = 0;
            AppMain.gmPlySeqTransformSuperMain(ply_work1);
        }
        if (AppMain.g_gm_main_system.boss_load_no == -1)
        {
            int bossLoadNo = SaveState.save.boss_load_no;
        }
        SaveState.resumeStarting = false;
        return true;
    }

    public static void resumeStageState()
    {
        if (AppMain.GsTrialIsTrial())
            return;
        AppMain.g_gs_main_sys_info.stage_id = SaveState.save.stage_id;
        AppMain.g_gs_main_sys_info.level = SaveState.save.level;
        AppMain.g_gs_main_sys_info.game_mode = SaveState.save.game_mode;
    }

    public static void resumeMapData()
    {
        if (AppMain.GsTrialIsTrial())
            return;
        if (SaveState.save.gm_eve_data != null && SaveState.save.gm_ring_data != null)
        {
            if (SaveState.save.boss_load_no == -1)
                AppMain.gm_eve_data.loadData(SaveState.save.gm_eve_data);
            AppMain.gm_ring_data.loadData(SaveState.save.gm_ring_data);
        }
        else
            AppMain.g_gs_main_sys_info.game_flag |= 4U;
    }

    public static void deleteSave()
    {
        if (AppMain.GsTrialIsTrial())
            return;
        SaveState.saveLater = false;
        SaveState.beginResume = false;
        SaveState.save = new SaveState.SaveData();
        var path = Path.Combine(AppMain.storePath, "laststate.dat");
        if (!File.Exists(path))
            return;
        File.Delete(path);
    }

    public static bool shouldResume()
    {
        return SaveState.beginResume;
    }

    public static void showResumeWarning()
    {
        if (!SaveState.firstShow)
            return;
        SaveState.firstShow = false;
        if (!SaveState.isSaveAvailable() || !SaveState.loadState())
            return;
        List<string> stringList = new List<string>();
        stringList.Add(Strings.ID_YES);
        stringList.Add(Strings.ID_NO);
        string idResumeCaption = Strings.ID_RESUME_CAPTION;
        string idResumeText = Strings.ID_RESUME_TEXT;
        //AppMain.g_ao_sys_global.is_show_ui = true;
        //Guide.BeginShowMessageBox(idResumeCaption, idResumeText, (IEnumerable<string>)stringList, 0, MessageBoxIcon.Warning, new AsyncCallback(SaveState.GetMBResult), (object)null);
    }

    public struct SaveData
    {
        public ushort stage_id;
        public int level;
        public int game_mode;
        public uint time_save;
        public int player_pos_x;
        public int player_pos_y;
        public int resume_pos_x;
        public int resume_pos_y;
        public uint marker_pri;
        public ushort water_levell;
        public ushort pseudofall_dir;
        public uint game_time;
        public uint rest_num;
        public int boss_load_no;
        public uint player_flag;
        public short ring_num;
        public short ring_stage_num;
        public uint score;
        public byte[] gm_eve_data;
        public byte[] gm_ring_data;

        public void Serialize(Stream stream)
        {
            using (BinaryWriter binaryWriter = new BinaryWriter(stream))
            {
                binaryWriter.Write(0);
                binaryWriter.Write(this.stage_id);
                binaryWriter.Write(this.level);
                binaryWriter.Write(this.game_mode);
                binaryWriter.Write(this.time_save);
                binaryWriter.Write(this.player_pos_x);
                binaryWriter.Write(this.player_pos_y);
                binaryWriter.Write(this.resume_pos_x);
                binaryWriter.Write(this.resume_pos_y);
                binaryWriter.Write(this.marker_pri);
                binaryWriter.Write(this.water_levell);
                binaryWriter.Write(this.pseudofall_dir);
                binaryWriter.Write(this.game_time);
                binaryWriter.Write(this.rest_num);
                binaryWriter.Write(this.boss_load_no);
                binaryWriter.Write(this.player_flag);
                binaryWriter.Write(this.ring_num);
                binaryWriter.Write(this.ring_stage_num);
                binaryWriter.Write(this.score);
                binaryWriter.Write(this.gm_eve_data != null ? this.gm_eve_data.Length : 0);
                if (this.gm_eve_data == null)
                    return;
                binaryWriter.Write(this.gm_eve_data);
                binaryWriter.Write(this.gm_ring_data.Length);
                binaryWriter.Write(this.gm_ring_data);
            }
        }

        public void UnSerialize(Stream stream)
        {
            using (BinaryReader binaryReader = new BinaryReader(stream))
            {
                binaryReader.ReadInt32();
                this.stage_id = binaryReader.ReadUInt16();
                this.level = binaryReader.ReadInt32();
                this.game_mode = binaryReader.ReadInt32();
                this.time_save = binaryReader.ReadUInt32();
                this.player_pos_x = binaryReader.ReadInt32();
                this.player_pos_y = binaryReader.ReadInt32();
                this.resume_pos_x = binaryReader.ReadInt32();
                this.resume_pos_y = binaryReader.ReadInt32();
                this.marker_pri = binaryReader.ReadUInt32();
                this.water_levell = binaryReader.ReadUInt16();
                this.pseudofall_dir = binaryReader.ReadUInt16();
                this.game_time = binaryReader.ReadUInt32();
                this.rest_num = binaryReader.ReadUInt32();
                this.boss_load_no = binaryReader.ReadInt32();
                this.player_flag = binaryReader.ReadUInt32();
                this.ring_num = binaryReader.ReadInt16();
                this.ring_stage_num = binaryReader.ReadInt16();
                this.score = binaryReader.ReadUInt32();
                int count = binaryReader.ReadInt32();
                if (count == 0)
                    return;
                this.gm_eve_data = binaryReader.ReadBytes(count);
                this.gm_ring_data = binaryReader.ReadBytes(binaryReader.ReadInt32());
            }
        }
    }
}
