// Decompiled with JetBrains decompiler
// Type: mpp.BlendStateExt
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

using Microsoft.Xna.Framework.Graphics;

namespace mpp
{
    internal static class BlendStateExt
    {
        public static BlendState clone(this BlendState original)
        {
            return new BlendState()
            {
                AlphaBlendFunction = original.AlphaBlendFunction,
                AlphaDestinationBlend = original.AlphaDestinationBlend,
                AlphaSourceBlend = original.AlphaSourceBlend,
                BlendFactor = original.BlendFactor,
                ColorBlendFunction = original.ColorBlendFunction,
                ColorDestinationBlend = original.ColorDestinationBlend,
                ColorSourceBlend = original.ColorSourceBlend,
                ColorWriteChannels = original.ColorWriteChannels,
                ColorWriteChannels1 = original.ColorWriteChannels1,
                ColorWriteChannels2 = original.ColorWriteChannels2,
                ColorWriteChannels3 = original.ColorWriteChannels3,
                MultiSampleMask = original.MultiSampleMask
            };
        }
    }
}
