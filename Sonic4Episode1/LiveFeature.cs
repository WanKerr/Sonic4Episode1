// Decompiled with JetBrains decompiler
// Type: LiveFeature
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sonic4ep1;

public class LiveFeature : XBOXLive
{
    static Rectangle SCALE(Rectangle rect)
    {
        return new Rectangle(rect.X * 2, rect.Y * 2, rect.Width * 2, rect.Height * 2);
    }


    private static readonly string[] zone_names =
    [
        Strings.ID_STAGE1,
        Strings.ID_STAGE2,
        Strings.ID_STAGE3,
        Strings.ID_STAGE4,
        Strings.ID_FINALZONE,
        Strings.ID_SSTAGE
    ];
    public static readonly string[] lang_suffix =
    [
        "ja", "us", "fr", "it", "ge", "es", "fi", "pt", "ru",
        "cn", "hk"
    ];
    public static int interruptMainLoop = 0;
    public static readonly int[,] table_x = new int[4, 2]
    {
        { 60, 70 }, { 145, 160 },
        { 260, 275 }, { 385, 395 }
    };

    public static int arrow_offset = 0;
    public static readonly Rectangle arrow1_Left = SCALE(new Rectangle(165, 240, 40, 40));
    public static readonly Rectangle arrow1_Right = SCALE(new Rectangle(285, 240, 40, 40));
    public static readonly Rectangle arrow2_Up = SCALE(new Rectangle(430, 60, 35, 35));
    public static readonly Rectangle arrow2_Down = SCALE(new Rectangle(430, 180, 35, 35));
    public static Rectangle num_src_rect = SCALE(new Rectangle(0, 0, 16, 32));
    public static Rectangle num_dst_rect = SCALE(new Rectangle(0, 240, 16, 32));
    public static readonly Color transparent_achiev = new Color(128, 128, 128, 128);
    private HISCORE_ENTRY[][] hiScoresTables = new HISCORE_ENTRY[48][];
    private LBStatus[] l_status = new LBStatus[48];
    public int startedRead = -1;
    private const int screenw = 480;
    private const int screenh = 288;
    private const int title_y = 22;
    private const int title_x = 15;
    private const int center_x = 240;
    private const int pad_x1 = 10;
    private const int pad_w = 230;
    private const int pad_h = 170;
    private const int pad_h2 = 220;
    private const int pad_x2 = 240;
    private const int pad_y = 55;
    private const int pad_y2 = 15;
    private const int title_y1 = 27;
    private const int title_y2 = 60;
    private const int clip_x = 15;
    private const int clip_y = 80;
    private const int clip_w = 450;
    private const int clip_h = 140;
    private const int title_y3 = 47;
    private const int title_y4 = 67;
    private const int title_y5 = 87;
    private const int arrow_left_x = 165;
    private const int arrow_right_x = 285;
    private const int arrow_y = 240;
    private const int left_x = 30;
    private const int list_y = 80;
    private const int bottom_y = 220;
    private const int draw_width = 420;
    private const int icon_paddle = 40;
    private static LiveFeature instance;
    public static Sonic4Ep1 GAME;
    public string readErrorMSG;
    public static int achievements_total;
    public static int achievements_current;
    public static Texture2D[] a_images;
    public static Texture2D a_bg;
    private int pX;
    private int pY;
    private int cX;
    private int cY;
    private int deltaX;
    private int deltaY;
    private int offsetY;
    private int curStage;
    private static Texture2D arrowImg;
    private static Texture2D arrowImg2;
    private static Texture2D titleImg;
    private static Texture2D nums;
    private static AchievementText[] achievementTextArray;

    public LiveFeature()
    {
        try
        {
            signinStatus = SigninStatus.SigningIn;
            isTrial(true);
            for (int index = 0; index < 48; ++index)
            {
                this.hiScoresTables[index] = new HISCORE_ENTRY[0];
                this.l_status[index] = LBStatus.NotRead;
            }
        }
        //catch (GameUpdateRequiredException ex)
        //{
        //    XBOXLive.HandleGameUpdateRequired(ex);
        //}
        catch (Exception ex)
        {
            signinStatus = SigninStatus.Error;
        }
        instance = this;
        instanceXBOX = this;
    }


    public LBStatus getLeaderBoardStatus(int mode)
    {
        return this.l_status[mode];
    }

    public void startReadingLeaderBoard(int mode)
    {
        return;
    }

    public bool saveLeaderBoardScore(int index, int value)
    {
        return true;
    }

    public static LiveFeature getInstance()
    {
        if (instance == null)
            instance = new LiveFeature();
        return instance;
    }

    public bool GotAchievment(string achievementKey)
    {
        return true;
    }

    public void clearHiScores()
    {
        for (int index = 0; index < 48; ++index)
        {
            this.l_status[index] = LBStatus.NotRead;
            this.hiScoresTables[index] = null;
        }
    }

    public override void _initTextDialog(
      out string dlgYes,
      out string dlgNo,
      out string dlgCaption,
      out string dlgText)
    {
        dlgYes = Strings.ID_YES;
        dlgNo = Strings.ID_NO;
        dlgCaption = Strings.ID_UPDATE_CAPTION;
        dlgText = Strings.ID_UPDATE_TEXT;
    }

    internal void ShowAchievements()
    {
        interruptMainLoop = 1;
        this.offsetY = 0;
        int length = AppMain.achievements.Length;
        achievements_total = 0;
        achievements_current = 0;
        if (a_images == null)
            a_images = new Texture2D[AppMain.achievements.Length];
        achievementTextArray = new AchievementText[length];
        for (int index = 0; index < length; ++index)
        {
            achievements_total += AppMain.achievements[index].cost;
            if (AppMain.gs_trophy_acquisition_tbl[index] == 2)
                achievements_current += AppMain.achievements[index].cost;
            a_images[index] = GAME.Content.Load<Texture2D>("LIVE\\live_a" + (index + 1));
            achievementTextArray[index].verticalSize = -1;
        }
        if (a_bg == null)
            a_bg = GAME.Content.Load<Texture2D>(("LIVE\\tab"));
        if (arrowImg2 == null)
            arrowImg2 = GAME.Content.Load<Texture2D>(("LIVE\\arrow2"));
        if (titleImg != null)
            return;
        titleImg = GAME.Content.Load<Texture2D>(("LIVE\\ach_top_" + lang_suffix[AppMain.GsEnvGetLanguage()]));
    }

    internal void ShowLeaderboards()
    {
        interruptMainLoop = 2;
        this.offsetY = 0;
        this.curStage = 24;
        this.clearHiScores();
        if (arrowImg == null)
            arrowImg = GAME.Content.Load<Texture2D>(("LIVE\\arrow"));
        if (a_bg == null)
            a_bg = GAME.Content.Load<Texture2D>(("LIVE\\tab"));
        if (nums == null)
            nums = GAME.Content.Load<Texture2D>(("LIVE\\nums"));
        this.startReadingLeaderBoard(this.curStage);
    }

    public bool InputOverride()
    {
        if (interruptMainLoop == 0)
            return false;
        MouseState state = Mouse.GetState();
        this.cX = state.X;
        this.cY = state.Y;
        if ((state.LeftButton == ButtonState.Pressed && this.pX != 0 && this.pY != 0 || state.LeftButton == ButtonState.Released) && (this.cY > 240 && this.cX > 350))
            return false;
        if (interruptMainLoop == 2)
        {
            if (AppMain.GsTrialIsTrial() || signinStatus == SigninStatus.Local)
                return false;
            if (state.LeftButton == ButtonState.Pressed)
            {
                this.pX = this.cX;
                this.pY = this.cY;
            }
            if (state.LeftButton == ButtonState.Released && this.pX != 0 && this.pY != 0)
            {
                bool flag = false;
                if (arrow1_Left.Contains(this.cX, this.cY))
                {
                    --this.curStage;
                    if (this.curStage < 24)
                        this.curStage = 47;
                    flag = true;
                }
                else if (arrow1_Right.Contains(this.cX, this.cY))
                {
                    ++this.curStage;
                    if (this.curStage > 47)
                        this.curStage = 24;
                    flag = true;
                }
                this.cX = this.pX = this.pY = this.cY = 0;
                if (flag && this.l_status[this.curStage] != LBStatus.ReadSuccess && this.l_status[this.curStage] != LBStatus.ReadFail)
                    this.startReadingLeaderBoard(this.curStage);
            }
        }
        else
        {
            if (state.LeftButton == ButtonState.Released)
            {
                this.pX = this.pY = this.cY = this.cX = 0;
                this.deltaX = 0;
                this.deltaY = 0;
                return true;
            }
            if (state.LeftButton == ButtonState.Pressed)
            {
                if (this.pX == 0 && this.pY == 0)
                {
                    this.pX = this.cX;
                    this.pY = this.cY;
                    if (arrow2_Up.Contains(this.cX, this.cY))
                    {
                        this.offsetY += 5;
                        if (this.offsetY > 0)
                            this.offsetY = 0;
                        this.pX = 0;
                        this.pY = 0;
                    }
                    if (arrow2_Down.Contains(this.cX, this.cY))
                    {
                        this.offsetY -= 5;
                        this.pX = 0;
                        this.pY = 0;
                    }
                }
                else
                {
                    this.deltaX = this.pX - this.cX;
                    this.deltaY = this.pY - this.cY;
                    this.offsetY -= this.deltaY;
                    if (this.offsetY > 0)
                        this.offsetY = 0;
                    this.pX = this.cX;
                    this.pY = this.cY;
                }
            }
        }
        return true;
    }

    private void _drawAchievements(SpriteBatch spriteBatch, SpriteFont[] fonts)
    {
        int y = 0;
        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
        try
        {
            if (AppMain.dm_xbox_show_progress != 100)
            {
                spriteBatch.Draw(a_bg, SCALE(new Rectangle(10, 55, 230, (int)(170.0 * (AppMain.dm_xbox_show_progress / 100f)))), new Rectangle(), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(a_bg, SCALE(new Rectangle(240, 55, 230, (int)(170.0 * (AppMain.dm_xbox_show_progress / 100f)))), new Rectangle(), Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);
            }
            else
            {
                spriteBatch.Draw(a_bg, SCALE(new Rectangle(10, 55, 230, 170)), new Rectangle(), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(a_bg, SCALE(new Rectangle(240, 55, 230, 170)), new Rectangle(), Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);
            }
            spriteBatch.Draw(titleImg, SCALE(new Rectangle(15, 22, 128, 16)), Color.White);
            if (AppMain.dm_xbox_show_progress == 100)
            {
                string text = achievements_current.ToString() + AppMain.AMD_FS_PATH_CHAR + achievements_total + " G";
                Vector2 vector2 = fonts[1].MeasureString(text);
                spriteBatch.DrawString(fonts[0], text, new Vector2(240 - ((int)vector2.X >> 1), 60f), Color.White);
            }
        }
        finally
        {
            spriteBatch.End();
        }
        if (AppMain.dm_xbox_show_progress == 100)
        {
            GAME.GraphicsDevice.ScissorRectangle = SCALE(new Rectangle(15, 80, 450, 140));
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, GAME.scissorState);
            try
            {
                y = this.offsetY + (80 * 2);
                Rectangle destinationRectangle = SCALE(new Rectangle(30, y, 35, 35));
                for (int index = 0; index < AppMain.achievements.Length; ++index)
                {
                    bool flag = AppMain.gs_trophy_acquisition_tbl[index] == 2;
                    spriteBatch.Draw(a_images[index], destinationRectangle, flag ? Color.White : transparent_achiev);
                    if (!flag)
                    {
                        Color gray = Color.Gray;
                    }
                    else
                    {
                        Color white = Color.White;
                    }
                    string text = AppMain.achievements[index].name + " (" + AppMain.achievements[index].cost + " G)";
                    Vector2 vector2 = fonts[1].MeasureString(text);
                    if (y > 0 && y < 220)
                        spriteBatch.DrawString(fonts[1], text, new Vector2(70f * 2, y), Color.White);
                    y += (int)vector2.Y;
                    if (y > 0 && y < 220 || achievementTextArray[index].verticalSize == -1)
                    {
                        if (achievementTextArray[index].text == null)
                            achievementTextArray[index].text = _wrapString(AppMain.achievements[index].description, 380, fonts[0]);
                        achievementTextArray[index].verticalSize = _drawWrapText(achievementTextArray[index].text, 70 * 2, y, 380, Color.White, false, fonts[0], spriteBatch);
                    }
                    y += achievementTextArray[index].verticalSize;
                    destinationRectangle.Y = y;
                }
                if (y < 220)
                    this.offsetY -= y - 220;
            }
            finally
            {
                spriteBatch.End();
                GAME.GraphicsDevice.ScissorRectangle = SCALE(new Rectangle(0, 0, 480, 288));
            }
        }
        if (AppMain.dm_xbox_show_progress != 100)
            return;
        arrow_offset += 2;
        if (arrow_offset > 16)
            arrow_offset = -16;
        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
        try
        {
            if (this.offsetY != 0)
            {
                Rectangle arrow2Up = arrow2_Up;
                arrow2Up.Y -= Math.Abs(arrow_offset);
                spriteBatch.Draw(arrowImg2, arrow2Up, Color.White);
            }
            if (y <= 220)
                return;
            Rectangle arrow2Down = arrow2_Down;
            arrow2Down.Y += Math.Abs(arrow_offset);
            spriteBatch.Draw(arrowImg2, arrow2Down, new Rectangle(), Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipVertically, 0.0f);
        }
        finally
        {
            spriteBatch.End();
        }
    }

    private void _drawLeaderboards(SpriteBatch spriteBatch, SpriteFont[] fonts)
    {
        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
        try
        {
            if (AppMain.dm_xbox_show_progress != 100)
            {
                spriteBatch.Draw(a_bg, SCALE(new Rectangle(10, 55 - (int)(40.0 * (AppMain.dm_xbox_show_progress / 100f)), 230, (int)(220.0 * (AppMain.dm_xbox_show_progress / 100f)))), new Rectangle(), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(a_bg, SCALE(new Rectangle(240, 55 - (int)(40.0 * (AppMain.dm_xbox_show_progress / 100f)), 230, (int)(220.0 * (AppMain.dm_xbox_show_progress / 100f)))), new Rectangle(), Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);
            }
            else
            {
                spriteBatch.Draw(a_bg, SCALE(new Rectangle(10, 15, 230, 220)), new Rectangle(), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(a_bg, SCALE(new Rectangle(240, 15, 230, 220)), new Rectangle(), Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);
            }
            if (AppMain.dm_xbox_show_progress != 100)
                return;
            int curStage = this.curStage;
            if (curStage > 23)
                curStage -= 24;
            string text1;
            if (curStage < 16)
            {
                string zoneName = zone_names[curStage / 4];
                if (curStage % 4 == 3)
                    text1 = string.Format(zoneName, Strings.ID_BOSS);
                else
                    text1 = string.Format(zoneName, string.Format(Strings.ID_ACT, new string[1]
                    {
            string.Concat( curStage % 4 + 1)
                    }));
            }
            else if (curStage == 16)
                text1 = Strings.ID_FINALZONE;
            else
                text1 = string.Format(zone_names[5], new string[1]
                {
          string.Concat( curStage - 16)
                });
            Vector2 vector2 = fonts[2].MeasureString(text1);
            spriteBatch.DrawString(fonts[2], text1, new Vector2(240 - ((int)vector2.X >> 1), 27f), Color.White);
            arrow_offset += 2;
            if (arrow_offset > 16)
                arrow_offset = -16;
            Rectangle arrow1Left = arrow1_Left;
            arrow1Left.X -= Math.Abs(arrow_offset);
            spriteBatch.Draw(arrowImg, arrow1Left, new Rectangle(), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
            Rectangle arrow1Right = arrow1_Right;
            arrow1Right.X += Math.Abs(arrow_offset);
            spriteBatch.Draw(arrowImg, arrow1Right, new Rectangle(), Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);
            string str = string.Concat(curStage + 1);
            int index1 = 0;
            int length = str.Length;
            while (index1 < str.Length)
            {
                num_dst_rect.X = 240 - 10 * length;
                num_src_rect.X = str[index1] != '0' ? 16 * (str[index1] - 49) : 144;
                spriteBatch.Draw(nums, num_dst_rect, (num_src_rect), Color.White);
                ++index1;
                --length;
            }
            num_src_rect.X = 160;
            num_src_rect.Width = 32;
            num_dst_rect.X = 240;
            spriteBatch.Draw(nums, num_dst_rect, (num_src_rect), Color.White);
            num_src_rect.Width = 16;
            num_dst_rect.X = 250;
            num_src_rect.X = 16;
            spriteBatch.Draw(nums, num_dst_rect, (num_src_rect), Color.White);
            num_dst_rect.X = 260;
            num_src_rect.X = 48;
            spriteBatch.Draw(nums, num_dst_rect, (num_src_rect), Color.White);
            int index2 = AppMain.g_gs_env_language == 0 || AppMain.g_gs_env_language == 1 || (AppMain.g_gs_env_language == 4 || AppMain.g_gs_env_language == 5) ? 0 : 1;
            string idLbRank = Strings.ID_LB_RANK;
            vector2 = fonts[1].MeasureString(idLbRank);
            spriteBatch.DrawString(fonts[1], idLbRank, new Vector2(table_x[0, index2] - ((int)vector2.X >> 1), 47f), Color.White);
            string idLbGtag = Strings.ID_LB_GTAG;
            vector2 = fonts[1].MeasureString(idLbGtag);
            spriteBatch.DrawString(fonts[1], idLbGtag, new Vector2(table_x[1, index2] - ((int)vector2.X >> 1), 47f), Color.White);
            string idBesttime = Strings.ID_BESTTIME;
            vector2 = fonts[1].MeasureString(idBesttime);
            spriteBatch.DrawString(fonts[1], idBesttime, new Vector2(table_x[2, index2] - ((int)vector2.X >> 1), 47f), Color.White);
            string idLbDate = Strings.ID_LB_DATE;
            vector2 = fonts[1].MeasureString(idLbDate);
            spriteBatch.DrawString(fonts[1], idLbDate, new Vector2(table_x[3, index2] - ((int)vector2.X >> 1), 47f), Color.White);
            int num = 67;
            if (this.l_status[this.curStage] == LBStatus.StartedRead)
                _drawWrapText(Strings.ID_LOADING, 240, 144, 450, Color.White, true, fonts[1], spriteBatch);
            else if (this.l_status[this.curStage] == LBStatus.ReadFail)
                _drawWrapText(Strings.ID_LB_UNABLE, 240, 144, 450, Color.White, true, fonts[1], spriteBatch);
            else if (this.hiScoresTables[this.curStage] != null && this.hiScoresTables[this.curStage].Length != 0)
            {
                if (AppMain.dm_xbox_show_progress != 100)
                    return;
                Color white = Color.White;
                for (int index3 = 0; index3 < this.hiScoresTables[this.curStage].Length; ++index3)
                {
                    if (index3 < 5 || index3 >= 5 && this.hiScoresTables[this.curStage][index3].isMe)
                    {
                        Color color = this.hiScoresTables[this.curStage][index3].isMe ? new Color(154 - Math.Abs(arrow_offset << 3), byte.MaxValue, 100 - Math.Abs(arrow_offset << 2)) : Color.WhiteSmoke;
                        string text2 = string.Concat(hiScoresTables[this.curStage][index3].index);
                        vector2 = fonts[0].MeasureString(text2);
                        spriteBatch.DrawString(fonts[0], text2, new Vector2(table_x[0, index2] - ((int)vector2.X >> 1), num), color);
                        string name = this.hiScoresTables[this.curStage][index3].name;
                        vector2 = fonts[0].MeasureString(name);
                        spriteBatch.DrawString(fonts[0], name, new Vector2(table_x[1, index2] - ((int)vector2.X >> 1), num), color);
                        string text3;
                        if (this.curStage > 23)
                        {
                            ushort min = 0;
                            ushort sec = 0;
                            ushort msec = 0;
                            AppMain.AkUtilFrame60ToTime((uint)this.hiScoresTables[this.curStage][index3].value, ref min, ref sec, ref msec);
                            StringBuilder stringBuilder = new StringBuilder();
                            if (min < 10)
                                stringBuilder.Append("0");
                            stringBuilder.Append(min);
                            stringBuilder.Append("'");
                            if (sec < 10)
                                stringBuilder.Append("0");
                            stringBuilder.Append(sec);
                            stringBuilder.Append("''");
                            if (msec < 10)
                                stringBuilder.Append("0");
                            stringBuilder.Append(msec);
                            text3 = stringBuilder.ToString();
                        }
                        else
                            text3 = string.Concat(hiScoresTables[this.curStage][index3].value);
                        vector2 = fonts[0].MeasureString(text3);
                        spriteBatch.DrawString(fonts[0], text3, new Vector2(table_x[2, index2] - ((int)vector2.X >> 1), num), color);
                        string text4 = string.Concat(hiScoresTables[this.curStage][index3].date);
                        vector2 = fonts[0].MeasureString(text4);
                        spriteBatch.DrawString(fonts[0], text4, new Vector2(table_x[3, index2] - ((int)vector2.X >> 1), num), color);
                        num += (int)vector2.Y;
                    }
                }
            }
            else
                _drawWrapText(Strings.ID_LB_NORECORDS, 240, 144, 450, Color.White, true, fonts[1], spriteBatch);
        }
        finally
        {
            spriteBatch.End();
        }
    }

    public void ShowOverride()
    {
        if (interruptMainLoop == 0)
            return;
        if (interruptMainLoop == 2)
        {
            this._drawLeaderboards(GAME.spriteBatch, GAME.fnts);
        }
        else
        {
            if (interruptMainLoop != 1)
                return;
            this._drawAchievements(GAME.spriteBatch, GAME.fnts);
        }
    }

    public static bool isInterrupted()
    {
        return interruptMainLoop != 0;
    }

    internal static void endInterrupt()
    {
        if (interruptMainLoop == 1)
        {
            int length = AppMain.achievements.Length;
            for (int index = 0; index < length; ++index)
            {
                a_images[index].Dispose();
                a_images[index] = null;
            }
            a_images = null;
            a_bg.Dispose();
            a_bg = null;
            arrowImg2.Dispose();
            arrowImg2 = null;
            titleImg.Dispose();
            titleImg = null;
            achievementTextArray = null;
        }
        if (interruptMainLoop == 2)
        {
            a_bg.Dispose();
            a_bg = null;
            arrowImg.Dispose();
            arrowImg = null;
            nums.Dispose();
            nums = null;
            getInstance().clearHiScores();
        }
        interruptMainLoop = 0;
    }

    public static string[] _wrapString(string s, int width, SpriteFont font)
    {
        List<string> stringList = new List<string>(5);
        s = s.Trim();
        string[] strArray = s.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        int num1 = 0;
        StringBuilder stringBuilder = new StringBuilder();
        foreach (string str in strArray)
        {
            Vector2 vector2 = font.MeasureString(str + " ");
            if (vector2.X + (double)num1 > width)
            {
                int num2 = 0;
                stringList.Add(stringBuilder.ToString());
                stringBuilder.Remove(0, stringBuilder.Length);
                stringBuilder.Append(str);
                stringBuilder.Append(" ");
                num1 = num2 + (int)vector2.X;
            }
            else
            {
                stringBuilder.Append(str);
                stringBuilder.Append(" ");
                num1 += (int)vector2.X;
                if (str.Contains("\r\n"))
                    num1 = 0;
            }
        }
        stringList.Add(stringBuilder.ToString());
        return stringList.ToArray();
    }

    public static int _drawWrapText(
      string text,
      int x,
      int y,
      int width,
      Color color,
      bool isXCenter,
      SpriteFont font,
      SpriteBatch sb,
      float corrector)
    {
        return _drawWrapText(_wrapString(text, width, font), x, y, width, color, isXCenter, font, sb, corrector);
    }

    public static int _drawWrapText(
      string text,
      int x,
      int y,
      int width,
      Color color,
      bool isXCenter,
      SpriteFont font,
      SpriteBatch sb)
    {
        return _drawWrapText(_wrapString(text, width, font), x, y, width, color, isXCenter, font, sb);
    }

    public static int _drawWrapText(
      string[] s,
      int x,
      int y,
      int width,
      Color color,
      bool isXCenter,
      SpriteFont font,
      SpriteBatch sb,
      float corrector)
    {
        Vector2 position = new Vector2(x, y);
        int num = 0;
        foreach (string text in s)
        {
            Vector2 vector2 = font.MeasureString(text);
            if (isXCenter)
                position.X = x - ((int)vector2.X >> 1);
            sb.DrawString(font, text, position, color);
            position.Y += vector2.Y * corrector;
            num += (int)(vector2.Y * (double)corrector);
        }
        return num;
    }

    public static int _drawWrapText(
      string[] s,
      int x,
      int y,
      int width,
      Color color,
      bool isXCenter,
      SpriteFont font,
      SpriteBatch sb)
    {
        return _drawWrapText(s, x, y, width, color, isXCenter, font, sb, 1f);
    }

    public enum Stages
    {
        Zone11_Score,
        Zone12_Score,
        Zone13_Score,
        Zone14_Score,
        Zone21_Score,
        Zone22_Score,
        Zone23_Score,
        Zone24_Score,
        Zone31_Score,
        Zone32_Score,
        Zone33_Score,
        Zone34_Score,
        Zone41_Score,
        Zone42_Score,
        Zone43_Score,
        Zone44_Score,
        ZoneF_Score,
        ZoneSS1_Score,
        ZoneSS2_Score,
        ZoneSS3_Score,
        ZoneSS4_Score,
        ZoneSS5_Score,
        ZoneSS6_Score,
        ZoneSS7_Score,
        Zone11_Time,
        Zone12_Time,
        Zone13_Time,
        Zone14_Time,
        Zone21_Time,
        Zone22_Time,
        Zone23_Time,
        Zone24_Time,
        Zone31_Time,
        Zone32_Time,
        Zone33_Time,
        Zone34_Time,
        Zone41_Time,
        Zone42_Time,
        Zone43_Time,
        Zone44_Time,
        ZoneF_Time,
        ZoneSS1_Time,
        ZoneSS2_Time,
        ZoneSS3_Time,
        ZoneSS4_Time,
        ZoneSS5_Time,
        ZoneSS6_Time,
        ZoneSS7_Time,
        LeaderboardsCount,
    }

    public enum LBStatus
    {
        NotRead,
        StartedRead,
        ReadSuccess,
        ReadFail,
    }

    public struct HISCORE_ENTRY
    {
        public int index;
        public bool isMe;
        public string name;
        public int value;
        public DateTime date;
    }

    private struct AchievementText
    {
        public string[] text;
        public int verticalSize;
    }
}
