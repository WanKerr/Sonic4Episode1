// Decompiled with JetBrains decompiler
// Type: GameFramework.GameObjectBase
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

using Microsoft.Xna.Framework;

namespace GameFramework
{
    public abstract class GameObjectBase
    {
        public GameObjectBase(Game game)
        {
            this.Game = game;
            this.IsEnabled = true;
        }

        public bool IsEnabled { get; set; }

        protected Game Game { get; set; }

        public int UpdateCount { get; set; }

        public void Update(GameTime time)
        {
            if (IsEnabled)
                this.UpdateCore(time);
        }

        protected virtual void UpdateCore(GameTime gameTime)
        {
            ++this.UpdateCount;
        }
    }
}
