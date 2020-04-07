// Decompiled with JetBrains decompiler
// Type: GameFramework.BenchmarkObject
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace GameFramework
{
  public class BenchmarkObject : TextObject
  {
    private StringBuilder _strBuilder = new StringBuilder();
    private double _lastUpdateMilliseconds;
    private int _drawCount;
    private int _lastDrawCount;
    private int _lastUpdateCount;

    public BenchmarkObject(Game game, SpriteFont font, Vector2 position, Color textColor)
      : base(game, font, position)
    {
      this.SpriteColor = textColor;
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      if (gameTime.TotalGameTime.TotalMilliseconds <= this._lastUpdateMilliseconds + 1000.0)
        return;
      int num1 = this._drawCount - this._lastDrawCount;
      int updateCount = this.UpdateCount;
      double num2 = gameTime.TotalGameTime.TotalMilliseconds - this._lastUpdateMilliseconds;
      this._strBuilder.Length = 0;
      this._strBuilder.AppendLine(((double) num1 / num2 * 1000.0).ToString("0.0") + " FPS");
      this.Text = this._strBuilder.ToString();
      this._lastUpdateMilliseconds = gameTime.TotalGameTime.TotalMilliseconds;
      this._lastDrawCount = this._drawCount;
      this._lastUpdateCount = this.UpdateCount;
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      ++this._drawCount;
      base.Draw(gameTime, spriteBatch);
    }
  }
}
