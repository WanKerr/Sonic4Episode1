using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using mpp;

public partial class AppMain
{
    public void amIPhoneInitNN(PresentationParameters presParams)
    {
        OpenGL.init(AppMain.m_game, AppMain.m_graphicsDevice);
        OpenGL.glViewport(0, 0, presParams.BackBufferWidth, presParams.BackBufferHeight);
        int num1 = 480;
        int num2 = 320;
        AppMain.amRenderInit();
        AppMain.NNS_MATRIX mtx = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.NNS_VECTOR nnsVector = new AppMain.NNS_VECTOR(0.0f, 0.0f, -1f);
        AppMain.NNS_RGBA nnsRgba = new AppMain.NNS_RGBA(1f, 1f, 1f, 1f);
        AppMain.NNS_CONFIG_GL config;
        config.WindowWidth = num1;
        config.WindowHeight = num2;
        this.nnConfigureSystemGL(config);
        AppMain.nnMakePerspectiveMatrix(mtx, AppMain.NNM_DEGtoA32(45f), (float)num2 / (float)num1, 1f, 10000f);
        AppMain._am_draw_video.draw_aspect = (float)num2 / (float)num1;
        AppMain.nnSetProjection(mtx, 0);
        AppMain._am_draw_video.draw_width = (float)num1;
        AppMain._am_draw_video.draw_height = (float)num2;
        AppMain._am_draw_video.disp_width = (float)num1;
        AppMain._am_draw_video.disp_height = (float)num2;
        AppMain._am_draw_video.width_2d = (float)num1;
        AppMain._am_draw_video.height_2d = (float)num2;
        AppMain._am_draw_video.scale_x_2d = 1f;
        AppMain._am_draw_video.scale_y_2d = 1f;
        AppMain._am_draw_video.base_x_2d = 0.0f;
        AppMain._am_draw_video.base_y_2d = 0.0f;
        AppMain._am_draw_video.wide_screen = true;
        AppMain._am_draw_video.refresh_rate = 60f;
        AppMain.amRenderInit();
        AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(mtx);
    }

    public static void amIPhoneExitNN()
    {
    }

    public static void amIPhoneSetTextureAttribute(AppMain.AMS_PARAM_LOAD_TEXTURE param)
    {
    }

    public static bool IsGLExtensionSupported(string extension)
    {
        return true;
    }

    public static void amIPhoneAccelerate(ref Vector3 accel)
    {
        AppMain.NNS_VECTOR core = AppMain._am_iphone_accel_data.core;
        AppMain.NNS_VECTOR sensor = AppMain._am_iphone_accel_data.sensor;
        core.x = accel.X;
        core.y = accel.Y;
        core.z = accel.Z;
        sensor.x = -core.y;
        sensor.y = core.x;
        sensor.z = core.z;
        AppMain._am_iphone_accel_data.rot_x = AppMain.nnArcTan2(-(double)sensor.z, -(double)sensor.y);
        AppMain._am_iphone_accel_data.rot_z = AppMain.nnArcTan2((double)sensor.x, -(double)sensor.y);
    }

    private static void amIPhoneRequestTouch(AppMain.AMS_IPHONE_TP_DATA DispData, int TouchIndex)
    {
        DispData?.Assign(AppMain._am_iphone_tp_ctrl_data[TouchIndex].tpdata);
    }

    public static void setBackKeyRequest(bool val)
    {
        AppMain._am_is_back_key_pressed = val;
    }

    public static bool isBackKeyPressed()
    {
        return AppMain._am_is_back_key_pressed;
    }

    public static void amKeyGetData()
    {
        AppMain._am_is_back_key_pressed = AppMain.back_key_is_pressed;
        AppMain.back_key_is_pressed = false;
    }

    static bool wasPressed = false;
    public static void onTouchEvents()
    {
        // PATCH
        var state = new List<TouchLocation>();
        if (TouchPanel.GetCapabilities().IsConnected)
        {
            state.AddRange(TouchPanel.GetState());
        }
        else
        {
            var mouseState = Mouse.GetState();
            if (wasPressed)
                state.Add(new TouchLocation(0, mouseState.LeftButton == ButtonState.Released ?
                    (wasPressed ? TouchLocationState.Released : TouchLocationState.Moved) : TouchLocationState.Pressed,
                    new Vector2(mouseState.X, mouseState.Y), TouchLocationState.Invalid, new Vector2()));
            wasPressed = mouseState.LeftButton == ButtonState.Pressed;
        }

        for (int i = 0; i < 4; i++)
        {
            touchMarked[i] = false;
        }
        for (int j = 0; j < 4; j++)
        {
            int id = 0;
            TouchLocationState touchLocationState;
            if (j >= state.Count)
            {
                touchLocationState = TouchLocationState.Invalid;
                posVector.X = -1f;
                posVector.Y = -1f;
                int num = 0;
                while (num < 4 && touchMarked[num])
                {
                    num++;
                }
                int num2 = num;
                touchMarked[num2] = true;
            }
            else
            {
                TouchLocation touchLocation = state[j];
                float x = touchLocation.Position.X;
                float y = touchLocation.Position.Y;
                screen2real(ref x, ref y);
                posVector.X = x;
                posVector.Y = y;
                touchLocationState = touchLocation.State;
                id = touchLocation.Id;
                int num2 = amFindTouchIndex(id);
                touchMarked[num2] = true;
            }

            switch (touchLocationState)
            {
                case TouchLocationState.Invalid:
                    amIPhoneTouchCanceled(j);
                    break;
                case TouchLocationState.Released:
                    amIPhoneTouchEnded(j);
                    break;
                case TouchLocationState.Pressed:
                    amIPhoneTouchBegan(ref posVector, j, id);
                    break;
                case TouchLocationState.Moved:
                    amIPhoneTouchMoved(ref posVector, j, id);
                    break;
            }
        }
    }

    // Token: 0x06001858 RID: 6232 RVA: 0x000DB528 File Offset: 0x000D9728
    private static void screen2real(ref float X, ref float Y)
    {
        var bounds = m_game.Window.ClientBounds;

        // nominal 480, 288
        X /= (float)bounds.Width;
        X *= 480;

        Y /= (float)(bounds.Height);
        Y *= 288;
        Y += 16;

        // Debug.WriteLine($"{X}, {Y}");
    }

    private static int amFindTouchIndex(int id)
    {
        for (int index = 0; index < 4; ++index)
        {
            if (AppMain._am_iphone_tp_ctrl_data[index].tpdata.id == id)
                return index;
        }
        for (int index = 0; index < 4; ++index)
        {
            if (!AppMain.touchMarked[index])
                return index;
        }
        return 0;
    }

    private static void amIPhoneTouchBegan(ref Vector2 touch, int i, int id)
    {
        AppMain.AMS_IPHONE_TP_DATA tpdata = AppMain._am_iphone_tp_ctrl_data[i].tpdata;
        tpdata.x = (ushort)touch.X;
        tpdata.y = (ushort)touch.Y;
        tpdata.id = id;
        tpdata.touch = (ushort)1;
        tpdata.validity = (ushort)1;
    }

    private static void amIPhoneTouchMoved(ref Vector2 touch, int i, int id)
    {
        AppMain.AMS_IPHONE_TP_DATA tpdata = AppMain._am_iphone_tp_ctrl_data[i].tpdata;
        tpdata.x = (ushort)touch.X;
        tpdata.y = (ushort)touch.Y;
        tpdata.id = id;
        tpdata.touch = (ushort)1;
        tpdata.validity = (ushort)1;
    }

    private static void amIPhoneTouchCanceled()
    {
        for (int index = 0; index < 4; ++index)
        {
            AppMain._am_iphone_tp_ctrl_data[index].tpdata.touch = (ushort)0;
            AppMain._am_iphone_tp_ctrl_data[index].tpdata.validity = (ushort)0;
        }
    }

    private static void amIPhoneTouchCanceled(int i)
    {
        AppMain._am_iphone_tp_ctrl_data[i].tpdata.touch = (ushort)0;
        AppMain._am_iphone_tp_ctrl_data[i].tpdata.validity = (ushort)0;
    }

    private static void amIPhoneTouchEnded(int i)
    {
        AppMain._am_iphone_tp_ctrl_data[i].tpdata.touch = (ushort)0;
    }
}