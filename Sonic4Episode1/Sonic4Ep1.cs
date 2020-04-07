// Decompiled with JetBrains decompiler
// Type: Sonic4Ep1
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using mpp;
using gs;

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

    public Sonic4Ep1()
    {
        Sonic4Ep1.pInstance = this;

        AppMain.storePath = System.IO.Directory.GetCurrentDirectory();
        
        var config = XmlStorage.Load();
        if (config == null)
        {
            config = new Sonic4Save();
        }

        var backup = gs.backup.SSave.CreateInstance();
        backup.SetSave(config);
        
        XmlStorage.Save(config, false, false);
        
        this.graphics = new GraphicsDeviceManager((Game)this);
        this.graphics.PreferredBackBufferWidth = 480 * 3;
        this.graphics.PreferredBackBufferHeight = 288 * 3;
        this.graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft;
        this.graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(this.graphics_PreparingDeviceSettings);
        this.graphics.SynchronizeWithVerticalRetrace = true;
        this.graphics.IsFullScreen = false;
        this.IsMouseVisible = true;
        this.Content.RootDirectory = "Content";
        this.TargetElapsedTime = TimeSpan.FromSeconds(1d / 60);
        
        
        this.Activated += new EventHandler<EventArgs>(OnActivated);
        this.Deactivated += new EventHandler<EventArgs>(OnDeactivated);
    }

    private void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
    {
        PresentationParameters presentationParameters = e.GraphicsDeviceInformation.PresentationParameters;
    }

    protected override void Initialize()
    {
        this.scissorState = new RasterizerState()
        {
            ScissorTestEnable = true,
            CullMode = CullMode.None
        };
        //Guide.IsScreenSaverEnabled = false;
        LiveFeature.GAME = this;
        LiveFeature.getInstance();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
        this.fntKootenay = this.Content.Load<SpriteFont>("Kootenay");
        this.fnts[0] = this.Content.Load<SpriteFont>("small");
        this.fnts[1] = this.Content.Load<SpriteFont>("medium");
        this.fnts[2] = this.Content.Load<SpriteFont>("large");
        try
        {
            this.appMain = new AppMain((Game)this, this.graphics, this.GraphicsDevice);
            this.appMain.AppInit(System.IO.Directory.GetCurrentDirectory());
            //if (this.accelerometer == null)
            //{
            //    this.accelerometer = Accelerometer.GetDefault();
            //    if (this.accelerometer != null)
            //    {
            //        this.accelerometer.ReadingChanged += accelerometer_ReadingChanged;
            //    }
            //}
        }
        catch (Exception ex)
        {
        }
    }

    protected override void OnDeactivated(object sender, EventArgs args)
    {
        AppMain.isForeground = false;
        if (SaveState.saveLater)
            SaveState._saveFile((object)SaveState.save);

        AppMain._am_sample_count = 2;
    }

    protected override void OnActivated(object sender, EventArgs args)
    {
        AppMain.isForeground = true;
        AppMain._am_sample_count = 1;
        if (((int)(AppMain.g_gm_main_system?.game_flag ?? 0) & 64) != 0)
            return;

        //  AppMain.g_pause_flag = true;
    }

    //private void accelerometer_ReadingChanged(object sender, AccelerometerReadingChangedEventArgs e)
    //{
    //    this.accel.X = (float)e.Reading.AccelerationX;
    //    this.accel.Y = (float)e.Reading.AccelerationY;
    //    this.accel.Z = (float)e.Reading.AccelerationZ;
    //}

    protected override void UnloadContent()
    {
    }

    protected override void Update(GameTime gameTime)
    {
        AppMain.lastGameTime = gameTime;
        if (!AppMain.g_ao_sys_global.is_show_ui)
        {
            if (Sonic4Ep1.inputDataRead)
            {
                Sonic4Ep1.inputDataRead = false;
                if (!LiveFeature.getInstance().InputOverride() && !Upsell.inputUpsellScreen() && AppMain.isForeground)
                {
                    AppMain.onTouchEvents();
                    AppMain.amIPhoneAccelerate(ref this.accel);
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                        AppMain.back_key_is_pressed = true;
                }
            }
        }
        base.Update(gameTime);
        //catch (GameUpdateRequiredException ex)
        //{
        //    XBOXLive.HandleGameUpdateRequired(ex);
        //}
    }

    protected override void Draw(GameTime gameTime)
    {
        Sonic4Ep1.inputDataRead = true;
        OpenGL.drawPrimitives_Count = 0;
        OpenGL.drawVertexBuffer_Count = 0;
        this.appMain.AppMainLoop();
        this.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
        this.spriteBatch.End();
        LiveFeature.getInstance().ShowOverride();
        Upsell.drawUpsellScreen();
        base.Draw(gameTime);
    }
}
