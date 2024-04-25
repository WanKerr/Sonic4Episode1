// Decompiled with JetBrains decompiler
// Type: GameFramework.SpriteObject
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameFramework
{
    public class SpriteObject : GameObjectBase
    {
        public SpriteObject(Game game)
          : base(game)
        {
            this.ScaleX = 1f;
            this.ScaleY = 1f;
            this.SpriteColor = Color.White;
        }

        public SpriteObject(Game game, Vector2 position)
          : this(game)
        {
            this.Position = position;
        }

        public SpriteObject(Game game, Vector2 position, Texture2D texture)
          : this(game, position)
        {
            this.SpriteTexture = texture;
        }

        public virtual Texture2D SpriteTexture { get; set; }

        public virtual float PositionX { get; set; }

        public virtual float PositionY { get; set; }

        public virtual float OriginX { get; set; }

        public virtual float OriginY { get; set; }

        public virtual float Angle { get; set; }

        public virtual float ScaleX { get; set; }

        public virtual float ScaleY { get; set; }

        public virtual Rectangle SourceRect { get; set; }

        public virtual Color SpriteColor { get; set; }

        public virtual float LayerDepth { get; set; }

        public Vector2 Position
        {
            get => new Vector2(this.PositionX, this.PositionY);
            set
            {
                this.PositionX = value.X;
                this.PositionY = value.Y;
            }
        }

        public Vector2 Origin
        {
            get => new Vector2(this.OriginX, this.OriginY);
            set
            {
                this.OriginX = value.X;
                this.OriginY = value.Y;
            }
        }

        public Vector2 Scale
        {
            get => new Vector2(this.ScaleX, this.ScaleY);
            set
            {
                this.ScaleX = value.X;
                this.ScaleY = value.Y;
            }
        }

        public virtual Rectangle BoundingBox
        {
            get
            {
                Vector2 vector2 = !this.SourceRect.IsEmpty ? new Vector2(SourceRect.Width, SourceRect.Height) : new Vector2(SpriteTexture.Width, SpriteTexture.Height);
                Rectangle rectangle = new Rectangle((int)this.PositionX, (int)this.PositionY, (int)(vector2.X * (double)this.ScaleX), (int)(vector2.Y * (double)this.ScaleY));
                rectangle.Offset((int)(-OriginX * (double)this.ScaleX), (int)(-OriginY * (double)this.ScaleY));
                return rectangle;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!this.IsEnabled)
                return;

            this.DrawCore(gameTime, spriteBatch);
        }

        protected virtual void DrawCore(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (this.SpriteTexture == null)
                return;
            if (this.SourceRect.IsEmpty)
                spriteBatch.Draw(this.SpriteTexture, this.Position, new Rectangle?(), this.SpriteColor, this.Angle, this.Origin, this.Scale, SpriteEffects.None, this.LayerDepth);
            else
                spriteBatch.Draw(this.SpriteTexture, this.Position, new Rectangle?(this.SourceRect), this.SpriteColor, this.Angle, this.Origin, this.Scale, SpriteEffects.None, this.LayerDepth);
        }
    }
}
