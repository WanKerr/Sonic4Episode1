// Decompiled with JetBrains decompiler
// Type: Upsell
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

public class Upsell
{
    public static bool showUpsell = false;
    public static int ss_num = 1;
    public static int pressed_button = -1;
    public static Rectangle[] rects = new Rectangle[5]
    {
    new Rectangle(60, 245, 90, 30),
    new Rectangle(200, 230, 272, 52),
    new Rectangle(190, 145, 37, 59),
    new Rectangle(433, 145, 37, 59),
    new Rectangle(240, 115, 180, 110)
    };
    public static bool[] hl_buttons = new bool[2];
    private static bool wasUpsell = false;
    public static int anm_progress = -1;
    private const int SS_MAX = 5;
    private const int CUR_STATE_NONE = -1;
    private const int CUR_STATE_PRESSED = 0;
    private const int CUR_STATE_RELEASED = 1;
    public static Texture2D bg;
    public static Texture2D cursor1;
    public static Texture2D cursor2;
    public static Texture2D button1;
    public static Texture2D button1hl;
    public static Texture2D button2;
    public static Texture2D button2hl;
    public static Texture2D screenshot;
    private static AppMain.DMS_BUY_SCR_WORK buy_scr_work;
    public static int px;
    public static int py;
    public static int cx;
    public static int cy;
    public static int curState;

    public static void launchUpsellScreen(AppMain.DMS_BUY_SCR_WORK buy_scr)
    {
        AppMain.DmSndBgmPlayerBgmStop();
        ss_num = 1;
        buy_scr_work = buy_scr;
        loadUpsellScreen();
    }

    public static void loadUpsellScreen()
    {
        try
        {
            bg = Texture2D.FromStream(LiveFeature.GAME.GraphicsDevice, TitleContainer.OpenStream("Content\\UPSELL\\s4us_bg.png"));
            cursor1 = Texture2D.FromStream(LiveFeature.GAME.GraphicsDevice, TitleContainer.OpenStream("Content\\UPSELL\\s4us_arrow.png"));
            screenshot = Texture2D.FromStream(LiveFeature.GAME.GraphicsDevice, TitleContainer.OpenStream("Content\\UPSELL\\s4us_ss_" + ss_num + ".png"));
            string str = LiveFeature.lang_suffix[AppMain.GsEnvGetLanguage()];
            button1 = Texture2D.FromStream(LiveFeature.GAME.GraphicsDevice, TitleContainer.OpenStream("Content\\UPSELL\\s4" + str + "_back.png"));
            button1hl = Texture2D.FromStream(LiveFeature.GAME.GraphicsDevice, TitleContainer.OpenStream("Content\\UPSELL\\s4" + str + "_back_HL.png"));
            button2 = Texture2D.FromStream(LiveFeature.GAME.GraphicsDevice, TitleContainer.OpenStream("Content\\UPSELL\\s4" + str + "_buy.png"));
            button2hl = Texture2D.FromStream(LiveFeature.GAME.GraphicsDevice, TitleContainer.OpenStream("Content\\UPSELL\\s4" + str + "_buy_HL.png"));
            showUpsell = true;
        }
        catch (Exception ex)
        {
        }
    }

    public static bool inputUpsellScreen()
    {
        if (!showUpsell)
            return false;
        pressed_button = -1;
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
        {
            if (anm_progress != -1)
            {
                anm_progress = -1;
                return true;
            }
            disposeUpsellScreen();
            if (buy_scr_work != null)
            {
                buy_scr_work.result[0] = 2;
                AppMain.DmSndBgmPlayerPlayBgm(0);
            }
            else
            {
                AppMain.SyDecideEvtCase(1);
                AppMain.SyChangeNextEvt();
            }
            return true;
        }
        TouchCollection state = TouchPanel.GetState();
        if (state.Count == 0)
        {
            if (px == 0 && py == 0)
                return true;
            curState = 1;
            cx = px;
            cy = py;
            px = 0;
            py = 0;
        }
        else
        {
            TouchLocation touchLocation = state[0];
            if (touchLocation.State == TouchLocationState.Pressed || touchLocation.State == TouchLocationState.Moved)
            {
                curState = 0;
                cx = (int)touchLocation.Position.X;
                cy = (int)touchLocation.Position.Y;
            }
            if (touchLocation.State == TouchLocationState.Released || touchLocation.State == TouchLocationState.Invalid)
            {
                curState = 1;
                cx = px;
                cy = py;
                px = 0;
                py = 0;
            }
        }
        hl_buttons[0] = false;
        hl_buttons[1] = false;
        if (anm_progress > 100)
        {
            anm_progress = -anm_progress;
            curState = 1;
            cx = px;
            cy = py;
            px = 0;
            py = 0;
            return true;
        }
        if (anm_progress != -1)
            return true;
        for (int index = 0; index < 5; ++index)
        {
            if (rects[index].Contains(cx, cy))
            {
                pressed_button = index;
                break;
            }
        }
        switch (pressed_button)
        {
            case 0:
                if (curState == 0)
                {
                    hl_buttons[0] = true;
                    break;
                }
                disposeUpsellScreen();
                if (buy_scr_work != null)
                {
                    buy_scr_work.result[0] = 2;
                    AppMain.DmSndBgmPlayerPlayBgm(0);
                    break;
                }
                AppMain.SyDecideEvtCase(1);
                AppMain.SyChangeNextEvt();
                break;
            case 1:
                if (curState == 0)
                {
                    hl_buttons[1] = true;
                    break;
                }
                wasUpsell = true;
                XBOXLive.showGuide();
                break;
            case 2:
                if (curState == 1)
                {
                    --ss_num;
                    if (ss_num < 1)
                        ss_num = 5;
                    screenshot.Dispose();
                    screenshot = Texture2D.FromStream(LiveFeature.GAME.GraphicsDevice, TitleContainer.OpenStream("Content\\UPSELL\\s4us_ss_" + ss_num + ".png"));
                    break;
                }
                break;
            case 3:
                if (curState == 1)
                {
                    ++ss_num;
                    if (ss_num >= 5)
                        ss_num = 1;
                    screenshot.Dispose();
                    screenshot = Texture2D.FromStream(LiveFeature.GAME.GraphicsDevice, TitleContainer.OpenStream("Content\\UPSELL\\s4us_ss_" + ss_num + ".png"));
                    break;
                }
                break;
            case 4:
                if (curState == 0)
                {
                    anm_progress = 0;
                    break;
                }
                break;
        }
        if (curState == 0)
        {
            px = cx;
            py = cy;
        }
        else
            curState = -1;
        return true;
    }

    public static void updateUpsellScreen()
    {
        if (anm_progress != -1)
        {
            anm_progress += 25;
            if (anm_progress > byte.MaxValue)
                anm_progress = byte.MaxValue;
            if (anm_progress < -1)
            {
                anm_progress += 25;
                if (anm_progress > -50)
                    anm_progress = -1;
            }
        }
        if (!wasUpsell || XBOXLive.isTrial(true))
            return;
        disposeUpsellScreen();
        if (buy_scr_work != null)
        {
            buy_scr_work.result[0] = 0;
            AppMain.DmSndBgmPlayerPlayBgm(0);
        }
        else
        {
            AppMain.event_after_buy = true;
            AppMain.SyDecideEvtCase(1);
            AppMain.SyChangeNextEvt();
        }
    }

    public static void drawUpsellScreen()
    {
        if (!showUpsell)
            return;
        SpriteBatch spriteBatch = LiveFeature.GAME.spriteBatch;
        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
        spriteBatch.Draw(bg, Vector2.Zero, Color.White);
        spriteBatch.Draw(hl_buttons[1] ? button2hl : button2, rects[1], Color.White);
        spriteBatch.Draw(hl_buttons[0] ? button1hl : button1, rects[0], Color.White);
        spriteBatch.Draw(cursor1, rects[2], Color.White);
        spriteBatch.Draw(cursor1, rects[3], new Rectangle?(), Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);
        spriteBatch.Draw(screenshot, rects[4], Color.White);
        int y1 = 5;
        int y2 = y1 + LiveFeature._drawWrapText("Download full Game!", 335, y1, 300, Color.White, true, LiveFeature.GAME.fnts[1], spriteBatch, 0.8f);
        int y3 = y2 + LiveFeature._drawWrapText("- 17 diverse stages", 195, y2, 300, Color.White, false, LiveFeature.GAME.fnts[0], spriteBatch, 0.8f);
        int y4 = y3 + LiveFeature._drawWrapText("- Two exclusive stages!", 195, y3, 300, Color.White, false, LiveFeature.GAME.fnts[0], spriteBatch, 0.8f);
        int y5 = y4 + LiveFeature._drawWrapText("- 7 special stages with tilt control!", 195, y4, 300, Color.White, false, LiveFeature.GAME.fnts[0], spriteBatch, 0.8f);
        int num1 = y5 + LiveFeature._drawWrapText("- Collect all 7 Chaos Emeralds to Unlock Super Sonic!", 195, y5, 300, Color.White, false, LiveFeature.GAME.fnts[0], spriteBatch, 0.8f);
        if (anm_progress != -1)
        {
            int num2 = Math.Abs(anm_progress);
            spriteBatch.Draw(screenshot, Vector2.Zero, new Color(num2, num2, num2, num2));
        }
        LiveFeature.GAME.spriteBatch.End();
        updateUpsellScreen();
    }

    public static void disposeUpsellScreen()
    {
        try
        {
            showUpsell = false;
            bg.Dispose();
            bg = null;
            cursor1.Dispose();
            cursor1 = null;
            screenshot.Dispose();
            screenshot = null;
            button1.Dispose();
            button1 = null;
            button1hl.Dispose();
            button1hl = null;
            button2.Dispose();
            button2 = null;
            button2hl.Dispose();
            button2hl = null;
        }
        catch (Exception ex)
        {
        }
    }

    private enum RectTypes
    {
        None = -1, // 0xFFFFFFFF
        Back = 0,
        Purchase = 1,
        CurLeft = 2,
        CurRight = 3,
        SS = 4,
        MAX = 5,
    }

    private enum HLTypes
    {
        Back,
        Purchase,
        MAX,
    }
}
