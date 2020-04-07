// Decompiled with JetBrains decompiler
// Type: mpp.DepthStencilStateExt
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

using Microsoft.Xna.Framework.Graphics;

namespace mpp
{
  internal static class DepthStencilStateExt
  {
    public static DepthStencilState clone(this DepthStencilState original)
    {
      return new DepthStencilState()
      {
        CounterClockwiseStencilDepthBufferFail = original.CounterClockwiseStencilDepthBufferFail,
        CounterClockwiseStencilFail = original.CounterClockwiseStencilFail,
        CounterClockwiseStencilFunction = original.CounterClockwiseStencilFunction,
        CounterClockwiseStencilPass = original.CounterClockwiseStencilPass,
        DepthBufferEnable = original.DepthBufferEnable,
        DepthBufferFunction = original.DepthBufferFunction,
        DepthBufferWriteEnable = original.DepthBufferWriteEnable,
        ReferenceStencil = original.ReferenceStencil,
        StencilDepthBufferFail = original.StencilDepthBufferFail,
        StencilEnable = original.StencilEnable,
        StencilFail = original.StencilFail,
        StencilFunction = original.StencilFunction,
        StencilMask = original.StencilMask,
        StencilPass = original.StencilPass,
        StencilWriteMask = original.StencilWriteMask,
        TwoSidedStencilMode = original.TwoSidedStencilMode
      };
    }
  }
}
