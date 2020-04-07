// Decompiled with JetBrains decompiler
// Type: Upsell
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;

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
    Upsell.ss_num = 1;
    Upsell.buy_scr_work = buy_scr;
    Upsell.loadUpsellScreen();
  }

  public static void loadUpsellScreen()
  {
    try
    {
      Upsell.bg = Texture2D.FromStream(LiveFeature.GAME.GraphicsDevice, TitleContainer.OpenStream("Content\\UPSELL\\s4us_bg.png"));
      Upsell.cursor1 = Texture2D.FromStream(LiveFeature.GAME.GraphicsDevice, TitleContainer.OpenStream("Content\\UPSELL\\s4us_arrow.png"));
      Upsell.screenshot = Texture2D.FromStream(LiveFeature.GAME.GraphicsDevice, TitleContainer.OpenStream("Content\\UPSELL\\s4us_ss_" + (object) Upsell.ss_num + ".png"));
      string str = LiveFeature.lang_suffix[AppMain.GsEnvGetLanguage()];
      Upsell.button1 = Texture2D.FromStream(LiveFeature.GAME.GraphicsDevice, TitleContainer.OpenStream("Content\\UPSELL\\s4" + str + "_back.png"));
      Upsell.button1hl = Texture2D.FromStream(LiveFeature.GAME.GraphicsDevice, TitleContainer.OpenStream("Content\\UPSELL\\s4" + str + "_back_HL.png"));
      Upsell.button2 = Texture2D.FromStream(LiveFeature.GAME.GraphicsDevice, TitleContainer.OpenStream("Content\\UPSELL\\s4" + str + "_buy.png"));
      Upsell.button2hl = Texture2D.FromStream(LiveFeature.GAME.GraphicsDevice, TitleContainer.OpenStream("Content\\UPSELL\\s4" + str + "_buy_HL.png"));
      Upsell.showUpsell = true;
    }
    catch (Exception ex)
    {
    }
  }

  public static bool inputUpsellScreen()
  {
    if (!Upsell.showUpsell)
      return false;
    Upsell.pressed_button = -1;
    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
    {
      if (Upsell.anm_progress != -1)
      {
        Upsell.anm_progress = -1;
        return true;
      }
      Upsell.disposeUpsellScreen();
      if (Upsell.buy_scr_work != null)
      {
        Upsell.buy_scr_work.result[0] = 2;
        AppMain.DmSndBgmPlayerPlayBgm(0);
      }
      else
      {
        AppMain.SyDecideEvtCase((short) 1);
        AppMain.SyChangeNextEvt();
      }
      return true;
    }
    TouchCollection state = TouchPanel.GetState();
    if (state.Count == 0)
    {
      if (Upsell.px == 0 && Upsell.py == 0)
        return true;
      Upsell.curState = 1;
      Upsell.cx = Upsell.px;
      Upsell.cy = Upsell.py;
      Upsell.px = 0;
      Upsell.py = 0;
    }
    else
    {
      TouchLocation touchLocation = state[0];
      if (touchLocation.State == TouchLocationState.Pressed || touchLocation.State == TouchLocationState.Moved)
      {
        Upsell.curState = 0;
        Upsell.cx = (int) touchLocation.Position.X;
        Upsell.cy = (int) touchLocation.Position.Y;
      }
      if (touchLocation.State == TouchLocationState.Released || touchLocation.State == TouchLocationState.Invalid)
      {
        Upsell.curState = 1;
        Upsell.cx = Upsell.px;
        Upsell.cy = Upsell.py;
        Upsell.px = 0;
        Upsell.py = 0;
      }
    }
    Upsell.hl_buttons[0] = false;
    Upsell.hl_buttons[1] = false;
    if (Upsell.anm_progress > 100)
    {
      Upsell.anm_progress = -Upsell.anm_progress;
      Upsell.curState = 1;
      Upsell.cx = Upsell.px;
      Upsell.cy = Upsell.py;
      Upsell.px = 0;
      Upsell.py = 0;
      return true;
    }
    if (Upsell.anm_progress != -1)
      return true;
    for (int index = 0; index < 5; ++index)
    {
      if (Upsell.rects[index].Contains(Upsell.cx, Upsell.cy))
      {
        Upsell.pressed_button = index;
        break;
      }
    }
    switch (Upsell.pressed_button)
    {
      case 0:
        if (Upsell.curState == 0)
        {
          Upsell.hl_buttons[0] = true;
          break;
        }
        Upsell.disposeUpsellScreen();
        if (Upsell.buy_scr_work != null)
        {
          Upsell.buy_scr_work.result[0] = 2;
          AppMain.DmSndBgmPlayerPlayBgm(0);
          break;
        }
        AppMain.SyDecideEvtCase((short) 1);
        AppMain.SyChangeNextEvt();
        break;
      case 1:
        if (Upsell.curState == 0)
        {
          Upsell.hl_buttons[1] = true;
          break;
        }
        Upsell.wasUpsell = true;
        XBOXLive.showGuide();
        break;
      case 2:
        if (Upsell.curState == 1)
        {
          --Upsell.ss_num;
          if (Upsell.ss_num < 1)
            Upsell.ss_num = 5;
          Upsell.screenshot.Dispose();
          Upsell.screenshot = Texture2D.FromStream(LiveFeature.GAME.GraphicsDevice, TitleContainer.OpenStream("Content\\UPSELL\\s4us_ss_" + (object) Upsell.ss_num + ".png"));
          break;
        }
        break;
      case 3:
        if (Upsell.curState == 1)
        {
          ++Upsell.ss_num;
          if (Upsell.ss_num >= 5)
            Upsell.ss_num = 1;
          Upsell.screenshot.Dispose();
          Upsell.screenshot = Texture2D.FromStream(LiveFeature.GAME.GraphicsDevice, TitleContainer.OpenStream("Content\\UPSELL\\s4us_ss_" + (object) Upsell.ss_num + ".png"));
          break;
        }
        break;
      case 4:
        if (Upsell.curState == 0)
        {
          Upsell.anm_progress = 0;
          break;
        }
        break;
    }
    if (Upsell.curState == 0)
    {
      Upsell.px = Upsell.cx;
      Upsell.py = Upsell.cy;
    }
    else
      Upsell.curState = -1;
    return true;
  }

  public static void updateUpsellScreen()
  {
    if (Upsell.anm_progress != -1)
    {
      Upsell.anm_progress += 25;
      if (Upsell.anm_progress > (int) byte.MaxValue)
        Upsell.anm_progress = (int) byte.MaxValue;
      if (Upsell.anm_progress < -1)
      {
        Upsell.anm_progress += 25;
        if (Upsell.anm_progress > -50)
          Upsell.anm_progress = -1;
      }
    }
    if (!Upsell.wasUpsell|| XBOXLive.isTrial(true))
      return;
    Upsell.disposeUpsellScreen();
    if (Upsell.buy_scr_work != null)
    {
      Upsell.buy_scr_work.result[0] = 0;
      AppMain.DmSndBgmPlayerPlayBgm(0);
    }
    else
    {
      AppMain.event_after_buy = true;
      AppMain.SyDecideEvtCase((short) 1);
      AppMain.SyChangeNextEvt();
    }
  }

  public static void drawUpsellScreen()
  {
    if (!Upsell.showUpsell)
      return;
    SpriteBatch spriteBatch = LiveFeature.GAME.spriteBatch;
    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
    spriteBatch.Draw(Upsell.bg, Vector2.Zero, Color.White);
    spriteBatch.Draw(Upsell.hl_buttons[1] ? Upsell.button2hl : Upsell.button2, Upsell.rects[1], Color.White);
    spriteBatch.Draw(Upsell.hl_buttons[0] ? Upsell.button1hl : Upsell.button1, Upsell.rects[0], Color.White);
    spriteBatch.Draw(Upsell.cursor1, Upsell.rects[2], Color.White);
    spriteBatch.Draw(Upsell.cursor1, Upsell.rects[3], new Rectangle?(), Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);
    spriteBatch.Draw(Upsell.screenshot, Upsell.rects[4], Color.White);
    int y1 = 5;
    int y2 = y1 + LiveFeature._drawWrapText("Download full Game!", 335, y1, 300, Color.White, true, LiveFeature.GAME.fnts[1], spriteBatch, 0.8f);
    int y3 = y2 + LiveFeature._drawWrapText("- 17 diverse stages", 195, y2, 300, Color.White, false, LiveFeature.GAME.fnts[0], spriteBatch, 0.8f);
    int y4 = y3 + LiveFeature._drawWrapText("- Two exclusive stages!", 195, y3, 300, Color.White, false, LiveFeature.GAME.fnts[0], spriteBatch, 0.8f);
    int y5 = y4 + LiveFeature._drawWrapText("- 7 special stages with tilt control!", 195, y4, 300, Color.White, false, LiveFeature.GAME.fnts[0], spriteBatch, 0.8f);
    int num1 = y5 + LiveFeature._drawWrapText("- Collect all 7 Chaos Emeralds to Unlock Super Sonic!", 195, y5, 300, Color.White, false, LiveFeature.GAME.fnts[0], spriteBatch, 0.8f);
    if (Upsell.anm_progress != -1)
    {
      int num2 = Math.Abs(Upsell.anm_progress);
      spriteBatch.Draw(Upsell.screenshot, Vector2.Zero, new Color(num2, num2, num2, num2));
    }
    LiveFeature.GAME.spriteBatch.End();
    Upsell.updateUpsellScreen();
  }

  public static void disposeUpsellScreen()
  {
    try
    {
      Upsell.showUpsell = false;
      Upsell.bg.Dispose();
      Upsell.bg = (Texture2D) null;
      Upsell.cursor1.Dispose();
      Upsell.cursor1 = (Texture2D) null;
      Upsell.screenshot.Dispose();
      Upsell.screenshot = (Texture2D) null;
      Upsell.button1.Dispose();
      Upsell.button1 = (Texture2D) null;
      Upsell.button1hl.Dispose();
      Upsell.button1hl = (Texture2D) null;
      Upsell.button2.Dispose();
      Upsell.button2 = (Texture2D) null;
      Upsell.button2hl.Dispose();
      Upsell.button2hl = (Texture2D) null;
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
