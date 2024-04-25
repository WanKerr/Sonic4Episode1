// Decompiled with JetBrains decompiler
// Type: Sonic4Ep1
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll
#define SPLIT_LOOP

using System;
using System.Diagnostics;
using gs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using mpp;
using GameFramework;
using System.Linq;

#if WINDOWSPHONE7_5 || WASM
#if !WASM
using System.Windows;
#endif
using Microsoft.Devices.Sensors;
#endif

public class Sonic4Ep1 : Game
{
    public static bool cheat = false;
    private static bool inputDataRead = true;
    public SpriteFont[] fnts = new SpriteFont[3];
    private WeakReference wr = new WeakReference(new object());
    protected bool storeSystemVolume = true;
    public static Sonic4Ep1 pInstance;
    private GraphicsDeviceManager graphics;
    private SpriteFont fntKootenay;
    private int GCCount;
    private double _lastUpdateMilliseconds;
    public RasterizerState scissorState;
    private AppMain appMain;
    public SpriteBatch spriteBatch;
    protected float deviceMusicVolume;
    private Vector3 accel;
    private long timestamp;
    private BenchmarkObject _benchmark;

    private string storePath;

#if WINDOWSPHONE7_5 || WASM
    private Accelerometer _accelerometer;
#endif

    public Sonic4Ep1()
    {
        pInstance = this;

#if WINDOWSPHONE7_5
        // hacky exception handler
        Application.Current.UnhandledException += (object sender, ApplicationUnhandledExceptionEventArgs e) =>
        {
            var result = MessageBox.Show(e.ExceptionObject.ToString(), "An error occurred", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.Cancel)
                e.Handled = true;
        };
#endif

#if WINDOWSPHONE7_5
        AppMain.storePath = storePath = ""; // unused, works via IsolatedStorage
#elif WINDOWS_UAP
        AppMain.storePath = storePath = Windows.Storage.ApplicationData.Current.RoamingFolder.Path;
#else
        AppMain.storePath = storePath = System.IO.Directory.GetCurrentDirectory();
#endif

        this.graphics = new GraphicsDeviceManager(this);
#if WINDOWSPHONE7_5
        this.graphics.PreferredBackBufferWidth = 533;
        this.graphics.PreferredBackBufferHeight = 320;
#else
        this.graphics.PreferredBackBufferWidth = 1200;
        this.graphics.PreferredBackBufferHeight = 480 * 3/2;
#endif
#if WINDOWS_UAP || __IOS__ || __ANDROID__ || WINDOWSPHONE7_5
        this.graphics.IsFullScreen = true;
        this.graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
#else
        this.graphics.IsFullScreen = false;
#endif
        this.graphics.SynchronizeWithVerticalRetrace = true;
        this.graphics.PreparingDeviceSettings += (object sender, PreparingDeviceSettingsEventArgs e) =>
            e.GraphicsDeviceInformation.PresentationParameters.PresentationInterval = PresentInterval.One;

        this.Content.RootDirectory = "Content";

        this.IsMouseVisible = true;
        this.IsFixedTimeStep = true;
        this.TargetElapsedTime = TimeSpan.FromSeconds(1d / 60);

        this.Activated += new EventHandler<EventArgs>(OnActivated);
        this.Deactivated += new EventHandler<EventArgs>(OnDeactivated);
    }

    protected override void Initialize()
    {
        this.scissorState = new RasterizerState() { ScissorTestEnable = true, CullMode = CullMode.None };
        Guide.IsScreenSaverEnabled = false;
        LiveFeature.GAME = this;
        LiveFeature.getInstance();

#if WINDOWSPHONE7_5 || WASM
        try
        {
            if (_accelerometer == null)
                _accelerometer = new Accelerometer();
            _accelerometer.TimeBetweenUpdates = TimeSpan.FromSeconds(1.0 / 30.0);
            _accelerometer.CurrentValueChanged += (object sender, SensorReadingEventArgs<AccelerometerReading> args) =>
                accel = new Vector3(args.SensorReading.Acceleration.X, args.SensorReading.Acceleration.Y, args.SensorReading.Acceleration.Z);
            _accelerometer.Start();
        }
        catch (AccelerometerFailedException)
        {
            _accelerometer = null;
        }
#endif
#if WASM
        // InputInjector.Init();
#endif
        base.Initialize();
    }

    protected override void LoadContent()
    {
        this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
        this.fntKootenay = this.Content.Load<SpriteFont>("Kootenay");
        this.fnts[0] = this.Content.Load<SpriteFont>("small");
        this.fnts[1] = this.Content.Load<SpriteFont>("medium");
        this.fnts[2] = this.Content.Load<SpriteFont>("large");
#if false
        _benchmark = new BenchmarkObject(this, fnts[1], Vector2.Zero, Color.Red);
#if DEBUG
        _benchmark.IsEnabled = true;
#endif
#endif

        var config = XmlStorage.Load() ?? new Sonic4Save();
        var backup = gs.backup.SSave.CreateInstance();
        backup.SetSave(config);

        XmlStorage.Save(config, false, false);

        this.appMain = new AppMain(this, this.graphics, this.GraphicsDevice);
        this.appMain.AppInit(storePath);
    }

    protected override void OnDeactivated(object sender, EventArgs args)
    {
        AppMain.isForeground = false;
        if (SaveState.saveLater)
            SaveState._saveFile(SaveState.save);

#if WINDOWSPHONE7_5
        if (Guide.IsVisible)
        {
            storeSystemVolume = false;
            return;
        }

        storeSystemVolume = true;
        try
        {
            if (!AppMain.g_ao_sys_global.is_playing_device_bgm_music)
                MediaPlayer.Pause();
            MediaPlayer.Volume = deviceMusicVolume;
        }
        catch (Exception) { }
#endif
    }

    protected override void OnActivated(object sender, EventArgs args)
    {
        AppMain.isForeground = true;

        if (storeSystemVolume)
            deviceMusicVolume = MediaPlayer.Volume;

        if (((int)(AppMain.g_gm_main_system?.game_flag ?? 0) & 64) == 0)
            AppMain.g_pause_flag = true;
    }

    protected override void UnloadContent()
    {
    }

    protected override void Update(GameTime gameTime)
    {
        AppMain.lastGameTime = gameTime;
        if (!AppMain.g_ao_sys_global.is_show_ui && inputDataRead)
        {
            inputDataRead = false;
            if (!LiveFeature.getInstance().InputOverride() && !Upsell.inputUpsellScreen() && AppMain.isForeground)
            {
                AppMain.onTouchEvents();
                AppMain.amIPhoneAccelerate(ref this.accel);
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    AppMain.back_key_is_pressed = true;
            }
        }

        _benchmark?.Update(gameTime);

#if SPLIT_LOOP
        inputDataRead = true;
        OpenGL.drawPrimitives_Count = 0;
        OpenGL.drawVertexBuffer_Count = 0;
        this.appMain.AppMainUpdate();
#endif

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
#if SPLIT_LOOP
        this.appMain.AppMainDraw();
#else
        this.appMain.AppMainLoop();
#endif
        if (_benchmark != null && this.spriteBatch != null)
        {
            this.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            _benchmark.Draw(gameTime, this.spriteBatch);
            this.spriteBatch.End();
        }

        // these are disabled but we might as well keep them around
        LiveFeature.getInstance().ShowOverride();
        Upsell.drawUpsellScreen();
        base.Draw(gameTime);
    }
}