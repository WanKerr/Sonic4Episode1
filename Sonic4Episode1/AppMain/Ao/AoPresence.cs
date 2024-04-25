using System;
using System.Diagnostics;

#if HAS_DISCORD
using Discord;
#endif

public partial class AppMain
{
#if HAS_DISCORD
    private const long g_discordClientId = 686320985669894185;
    private static DiscordPresence g_discord;
    private static ActivityManager g_discordActivity;
#endif

    public const int AO_PRESENCE_STAGE_ID_TITLE = GSD_MAIN_STAGE_ID_ENDING + 1;
    public const int AO_PRESENCE_STAGE_ID_STAGE_SELECT = GSD_MAIN_STAGE_ID_ENDING + 2;

    private static readonly string[] ao_presence_stage_title_tbl = {
        "Splash Hill Zone - Act 1",
        "Splash Hill Zone - Act 2",
        "Splash Hill Zone - Act 3",
        "Splash Hill Zone - Boss",
        "Casino Street Zone - Act 1",
        "Casino Street Zone - Act 2",
        "Casino Street Zone - Act 3",
        "Casino Street Zone - Boss",
        "Lost Labyrinth Zone - Act 1",
        "Lost Labyrinth Zone - Act 2",
        "Lost Labyrinth Zone - Act 3",
        "Lost Labyrinth Zone - Boss",
        "Mad Gear Zone - Act 1",
        "Mad Gear Zone - Act 2",
        "Mad Gear Zone - Act 3",
        "Mad Gear Zone - Boss",
        "E.G.G. Station Zone",
        null,
        null,
        null,
        null,
        "Special Stage 1",
        "Special Stage 2",
        "Special Stage 3",
        "Special Stage 4",
        "Special Stage 5",
        "Special Stage 6",
        "Special Stage 7",
        "Ending",
        "Title Screen",
        "Stage Select",
    };

    private static readonly string[] ao_presence_stage_subtitle_tbl = {
        "The Adventure Begins",
        "High-Speed Athletics",
        "Sunset Dash",
        "Showdown with Dr. Eggman",
        "Neon City Adrift in the Night",
        "100,000 Point Challenge",
        "Casino Climax",
        "Dr. Eggman's Party",
        "Ancient Maze of Mystery",
        "Strange Mine Cart",
        "Underwater Maze Escape",
        "Trap-Filed Ruins",
        "Dr. Eggman's Secret Base",
        "Escape the Cog Trap",
        "Impending Doom",
        "Defeat the Real Dr. Eggman",
        "Final Showdown in Space",
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
    };

    private static readonly string[] ao_presence_stage_thumb_asset_tbl = {
        "stage-z1s1",
        "stage-z1s2",
        "stage-z1s3",
        "stage-z1sb",
        "stage-z2s1",
        "stage-z2s2",
        "stage-z2s3",
        "stage-z2sb",
        "stage-z3s1",
        "stage-z3s2",
        "stage-z3s3",
        "stage-z3sb",
        "stage-z4s1",
        "stage-z4s2",
        "stage-z4s3",
        "stage-z4sb",
        "stage-zfsb",
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        "title-screen",
        "title-screen",
    };

    private enum AOE_PRESENCE
    {
        AOD_PRESENCE_DEFAULT = 0,
        AOD_PRESENCE_STANDBY = 0,
        AOD_PRESENCE_Z11T = 1,
        AOD_PRESENCE_Z11S = 2,
        AOD_PRESENCE_Z12T = 3,
        AOD_PRESENCE_Z12S = 4,
        AOD_PRESENCE_Z13T = 5,
        AOD_PRESENCE_Z13S = 6,
        AOD_PRESENCE_Z1BT = 7,
        AOD_PRESENCE_Z1BS = 8,
        AOD_PRESENCE_Z21T = 9,
        AOD_PRESENCE_Z21S = 10, // 0x0000000A
        AOD_PRESENCE_Z22T = 11, // 0x0000000B
        AOD_PRESENCE_Z22S = 12, // 0x0000000C
        AOD_PRESENCE_Z23T = 13, // 0x0000000D
        AOD_PRESENCE_Z23S = 14, // 0x0000000E
        AOD_PRESENCE_Z2BT = 15, // 0x0000000F
        AOD_PRESENCE_Z2BS = 16, // 0x00000010
        AOD_PRESENCE_Z31T = 17, // 0x00000011
        AOD_PRESENCE_Z31S = 18, // 0x00000012
        AOD_PRESENCE_Z32T = 19, // 0x00000013
        AOD_PRESENCE_Z32S = 20, // 0x00000014
        AOD_PRESENCE_Z33T = 21, // 0x00000015
        AOD_PRESENCE_Z33S = 22, // 0x00000016
        AOD_PRESENCE_Z3BT = 23, // 0x00000017
        AOD_PRESENCE_Z3BS = 24, // 0x00000018
        AOD_PRESENCE_Z41T = 25, // 0x00000019
        AOD_PRESENCE_Z41S = 26, // 0x0000001A
        AOD_PRESENCE_Z42T = 27, // 0x0000001B
        AOD_PRESENCE_Z42S = 28, // 0x0000001C
        AOD_PRESENCE_Z43T = 29, // 0x0000001D
        AOD_PRESENCE_Z43S = 30, // 0x0000001E
        AOD_PRESENCE_Z4BT = 31, // 0x0000001F
        AOD_PRESENCE_Z4BS = 32, // 0x00000020
        AOD_PRESENCE_ZFBT = 33, // 0x00000021
        AOD_PRESENCE_ZFBS = 34, // 0x00000022
        AOD_PRESENCE_SS1T = 35, // 0x00000023
        AOD_PRESENCE_SS1S = 36, // 0x00000024
        AOD_PRESENCE_SS2T = 37, // 0x00000025
        AOD_PRESENCE_SS2S = 38, // 0x00000026
        AOD_PRESENCE_SS3T = 39, // 0x00000027
        AOD_PRESENCE_SS3S = 40, // 0x00000028
        AOD_PRESENCE_SS4T = 41, // 0x00000029
        AOD_PRESENCE_SS4S = 42, // 0x0000002A
        AOD_PRESENCE_SS5T = 43, // 0x0000002B
        AOD_PRESENCE_SS5S = 44, // 0x0000002C
        AOD_PRESENCE_SS6T = 45, // 0x0000002D
        AOD_PRESENCE_SS6S = 46, // 0x0000002E
        AOD_PRESENCE_SS7T = 47, // 0x0000002F
        AOD_PRESENCE_SS7S = 48, // 0x00000030
        AOD_PRESENCE_TITLET = 49,
        AOD_PRESENCE_TITLES = 50,
        AOD_PRESENCE_MENUT = 51,
        AOD_PRESENCE_MENUS = 52,
        AOD_PRESENCE_NUM = 51, // 0x00000031
        AOD_PRESENCE_NONE = 52, // 0x00000032
    }

    private static void AoPresenceInit()
    {
#if HAS_DISCORD
        try
        {
            g_discord = new DiscordPresence(g_discordClientId, (ulong)CreateFlags.NoRequireDiscord);
#if DEBUG
            g_discord.SetLogHook(LogLevel.Debug, (level, msg) => Debug.WriteLine(msg));
#endif
            g_discordActivity = g_discord.GetActivityManager();
        }
        catch (ResultException ex)
        {
            Debug.WriteLine("Discord failed to initialize: " + ex.Result);
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Discord failed to initialize: " + ex.Message);
        }
#endif

        AoPresenceSet(AO_PRESENCE_STAGE_ID_TITLE, false);
    }

    private static bool AoPresenceInitialized()
    {
        return true;
    }

    private static void AoPresenceExit()
    {
#if HAS_DISCORD
        g_discord?.Dispose();
#endif
    }

    private static void AoPresenceSet()
    {
        AoPresenceSet(GsGetMainSysInfo().stage_id, GsTrialIsTrial());
    }

    private static void AoPresenceSet(int presence, bool is_trial)
    {
#if HAS_DISCORD
        if (g_discordActivity == null) return;

        // we're kinda not using AOE_PRESENCE because it makes no sense based on the stage order lol
        var stage_id = (int)presence;
        var activity = new Discord.Activity();
        activity.Details = ao_presence_stage_title_tbl[stage_id];
        activity.State = ao_presence_stage_subtitle_tbl[stage_id];

        var thumb_asset = ao_presence_stage_thumb_asset_tbl[stage_id];
        if (thumb_asset != null)
            activity.Assets = new() { LargeImage = thumb_asset };

        if (GmMainCheckExeTimerCount())
        {
            var epoch = new DateTimeOffset(2015, 1, 1, 0, 0, 0, TimeSpan.Zero);
            var now = DateTimeOffset.UtcNow;
            var timeSpan = (long)(now - epoch).TotalMilliseconds;

            activity.Timestamps = new() { Start = timeSpan };
        }

        Console.WriteLine(activity.Details);

        g_discordActivity.UpdateActivity(activity, null);
#endif
    }

    private static void AoPresenceUpdate()
    {
#if HAS_DISCORD
        g_discord?.RunCallbacks();
#endif
    }
}