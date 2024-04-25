// Decompiled with JetBrains decompiler
// Type: SaveState
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Sonic4ep1;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;

#if WASM
using System.Runtime.InteropServices.JavaScript;
#endif

public partial class SaveState
{
#if WASM
    [JSImport("SaveState_GetSaveState", "main.js")]
    internal static partial string GetSaveState();
    [JSImport("SaveState_SetSaveState", "main.js")]
    internal static partial void SetSaveState(string value);

    [JSImport("SaveState_HasSaveState", "main.js")]
    internal static partial bool HasSaveState();

    [JSImport("SaveState_DeleteSaveState", "main.js")]
    internal static partial void DeleteSaveState();
#endif

    public static bool saveLater = false;
    private static bool resumeStarting = false;
    private static bool beginResume = false;
    private static bool firstShow = true;
    public const int SAVE_AFTER_RESPAWN = 0;
    public const int SAVE_AFTER_CHECKPOINT = 1;
    private const string filename = "laststate.dat";
    public static SaveData save;

#if USE_THREADS
    private static Thread saveThread;
#else
    private static Task saveThread;
#endif

    public static void saveCurrentState(int mode)
    {
        if (AppMain.GsTrialIsTrial())
            return;
        save.player_pos_x = AppMain.g_gm_main_system.ply_work[0].obj_work.pos.x;
        save.player_pos_y = AppMain.g_gm_main_system.ply_work[0].obj_work.pos.y;
        save.resume_pos_x = AppMain.g_gm_main_system.resume_pos_x;
        save.resume_pos_y = AppMain.g_gm_main_system.resume_pos_y;
        save.game_time = AppMain.g_gm_main_system.game_time;
        save.time_save = AppMain.g_gm_main_system.time_save;
        save.marker_pri = AppMain.g_gm_main_system.marker_pri;
        save.water_levell = AppMain.g_gm_main_system.water_level;
        save.pseudofall_dir = AppMain.g_gm_main_system.pseudofall_dir;
        save.rest_num = AppMain.g_gm_main_system.player_rest_num[0];
        save.stage_id = AppMain.g_gs_main_sys_info.stage_id;
        save.level = AppMain.g_gs_main_sys_info.level;
        save.game_mode = AppMain.g_gs_main_sys_info.game_mode;
        save.boss_load_no = AppMain.g_gm_main_system.boss_load_no;
        save.player_flag = AppMain.g_gm_main_system.ply_work[0].player_flag;
        save.ring_num = AppMain.g_gm_main_system.ply_work[0].ring_num;
        save.ring_stage_num = AppMain.g_gm_main_system.ply_work[0].ring_stage_num;
        save.score = AppMain.g_gm_main_system.ply_work[0].score;
        if (mode == 1)
        {
            save.gm_eve_data = AppMain.gm_eve_data.saveData();
            save.gm_ring_data = AppMain.gm_ring_data.saveData();
            saveLater = false;
        }
        else
        {
            save.gm_eve_data = null;
            save.gm_ring_data = null;
            saveLater = false;
        }
        if (saveLater)
            return;

#if WASM
        _saveFile(save);
#else
        if (saveThread != null)
            saveThread = null;
#if USE_THREADS
        saveThread = new Thread(() => _saveFile(save));
        saveThread.Start();
#elif ASYNC_TARGETING_PACK
        saveThread = TaskEx.Run(() => _saveFile(save));
#else
        saveThread = Task.Run(() => _saveFile(save));
#endif
#endif
    }

    public static void _saveFile(SaveData saveData)
    {
        try
        {
            saveLater = false;
#if WASM
            using var memoryStream = new MemoryStream();
            saveData.Serialize(memoryStream);

            var str = Convert.ToBase64String(memoryStream.ToArray());
            SetSaveState(str);
#else
            // TODO: WP7 IsolatedStorage implementation
            var path = Path.Combine(AppMain.storePath, "laststate.dat");
            if (File.Exists(path))
                File.Delete(path);
            using (var file = File.Create(path))
                saveData.Serialize(file);
#endif
        }
        catch (Exception)
        {

        }
    }

    public static bool isSaveAvailable()
    {
        if (AppMain.GsTrialIsTrial())
            return false;
        var path = Path.Combine(AppMain.storePath, "laststate.dat");
        try
        {
#if WASM
            return HasSaveState();
#else
            return File.Exists(path);
#endif
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
#if WASM
            if (!HasSaveState()) return false;

            var str = GetSaveState();
            using var memoryStream = new MemoryStream(Convert.FromBase64String(str));
            save.UnSerialize(memoryStream);

            return true;
#else
            if (!File.Exists(path))
                return false;
            using (var storageFileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                save.UnSerialize(storageFileStream);
            return true;
#endif
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
        AppMain.g_gm_main_system.resume_pos_x = save.resume_pos_x;
        AppMain.g_gm_main_system.resume_pos_y = save.resume_pos_y;
        AppMain.g_gm_main_system.game_time = save.game_time;
        AppMain.g_gm_main_system.time_save = save.time_save;
        AppMain.g_gm_main_system.marker_pri = save.marker_pri;
        AppMain.g_gm_main_system.water_level = save.water_levell;
        AppMain.g_gm_main_system.pseudofall_dir = save.pseudofall_dir;
        AppMain.g_gm_main_system.player_rest_num[0] = save.rest_num;
        resumeStarting = true;
    }

    public static bool resumePlayer_2(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (AppMain.GsTrialIsTrial())
            return false;
        beginResume = false;
        if (!resumeStarting)
            return false;
        AppMain.GMS_PLAYER_WORK ply_work1 = ply_work;
        ply_work1.ring_num = save.ring_num;
        ply_work1.ring_stage_num = save.ring_stage_num;
        ply_work1.score = save.score;
        ply_work1.obj_work.pos.x = save.player_pos_x;
        ply_work1.obj_work.pos.y = save.player_pos_y;
        AppMain.g_gm_main_system.resume_pos_x = save.resume_pos_x;
        AppMain.g_gm_main_system.resume_pos_y = save.resume_pos_y;
        AppMain.GmCameraPosSet(AppMain.g_gm_main_system.resume_pos_x, AppMain.g_gm_main_system.resume_pos_y, 0);
        AppMain.OBS_CAMERA obj_camera = AppMain.ObjCameraGet(AppMain.g_obj.glb_camera_id);
        AppMain.ObjObjectCameraSet(AppMain.FXM_FLOAT_TO_FX32(obj_camera.disp_pos.x - AppMain.OBD_LCD_X / 2), AppMain.FXM_FLOAT_TO_FX32(-obj_camera.disp_pos.y - AppMain.OBD_LCD_Y / 2), AppMain.FXM_FLOAT_TO_FX32(obj_camera.disp_pos.x - AppMain.OBD_LCD_X / 2), AppMain.FXM_FLOAT_TO_FX32(-obj_camera.disp_pos.y - AppMain.OBD_LCD_Y / 2));
        AppMain.GmCameraSetClipCamera(obj_camera);
        if ((AppMain.GMD_PLF_SUPER_SONIC & (int)save.player_flag) != 0)
        {
            ply_work1.obj_work.user_timer = 249856;
            AppMain.gmPlySeqTransformSuperMain(ply_work1);
            ply_work1.obj_work.user_timer = 0;
            AppMain.gmPlySeqTransformSuperMain(ply_work1);
        }
        if (AppMain.g_gm_main_system.boss_load_no == -1)
        {
            int bossLoadNo = save.boss_load_no;
        }
        resumeStarting = false;
        return true;
    }

    public static void resumeStageState()
    {
        if (AppMain.GsTrialIsTrial())
            return;
        AppMain.g_gs_main_sys_info.stage_id = save.stage_id;
        AppMain.g_gs_main_sys_info.level = save.level;
        AppMain.g_gs_main_sys_info.game_mode = save.game_mode;
    }

    public static void resumeMapData()
    {
        if (AppMain.GsTrialIsTrial())
            return;
        if (save.gm_eve_data != null && save.gm_ring_data != null)
        {
            if (save.boss_load_no == -1)
                AppMain.gm_eve_data.loadData(save.gm_eve_data);
            AppMain.gm_ring_data.loadData(save.gm_ring_data);
        }
        else
            AppMain.g_gs_main_sys_info.game_flag |= 4U;
    }

    public static void deleteSave()
    {
        if (AppMain.GsTrialIsTrial())
            return;
        saveLater = false;
        beginResume = false;

        try
        {
            save = new SaveData();
#if WASM
            DeleteSaveState();
#else
            var path = Path.Combine(AppMain.storePath, "laststate.dat");
            File.Delete(path);
#endif
        }
        catch (Exception)
        {
        }
    }

    public static bool shouldResume()
    {
        return beginResume;
    }

    public static void showResumeWarning()
    {
        if (!firstShow)
            return;
        firstShow = false;
        if (!isSaveAvailable() || !loadState())
            return;
        List<string> stringList = new List<string> { Strings.ID_YES, Strings.ID_NO };
        string idResumeCaption = Strings.ID_RESUME_CAPTION;
        string idResumeText = Strings.ID_RESUME_TEXT;
        AppMain.g_ao_sys_global.is_show_ui = true;
        Guide.BeginShowMessageBox(idResumeCaption, idResumeText, stringList, 0, MessageBoxIcon.Warning, new AsyncCallback(GetMBResult), null);
    }

    protected static void GetMBResult(IAsyncResult r)
    {
        try
        {
            if (Guide.EndShowMessageBox(r) == 0 && SaveState.loadState())
            {
                SaveState.beginResume = true;
            }
        }
        finally
        {
            AppMain.g_ao_sys_global.is_show_ui = false;
        }
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
