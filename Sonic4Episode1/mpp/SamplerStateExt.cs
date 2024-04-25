// Decompiled with JetBrains decompiler
// Type: mpp.SamplerStateExt
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

using Microsoft.Xna.Framework.Graphics;

namespace mpp
{
    internal static class SamplerStateExt
    {
        public static SamplerState clone(this SamplerState original)
        {
            return new SamplerState()
            {
                AddressU = original.AddressU,
                AddressV = original.AddressV,
                AddressW = original.AddressW,
                Filter = original.Filter,
                MaxAnisotropy = original.MaxAnisotropy,
                MaxMipLevel = original.MaxMipLevel,
                MipMapLevelOfDetailBias = original.MipMapLevelOfDetailBias
            };
        }
    }
}
