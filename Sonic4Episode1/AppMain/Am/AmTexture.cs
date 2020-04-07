using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using mpp;

public partial class AppMain
{
    public static int amTextureLoad(
      AppMain.NNS_TEXLIST texlist,
      AppMain.NNS_TEXFILELIST texfilelist,
      string filepath)
    {
        AppMain.mppAssertNotImpl();
        return AppMain.amTextureLoad(texlist, texfilelist, filepath, (AppMain.AMS_AMB_HEADER)null);
    }

    public static int amTextureLoad(
      AppMain.NNS_TEXLIST texlist,
      AppMain.NNS_TEXFILELIST texfilelist,
      string filepath,
      AppMain.AMS_AMB_HEADER amb)
    {
        int num = 0;
        AppMain.ArrayPointer<AppMain.NNS_TEXINFO> arrayPointer1 = new AppMain.ArrayPointer<AppMain.NNS_TEXINFO>(texlist.pTexInfoList);
        AppMain.ArrayPointer<AppMain.NNS_TEXFILE> arrayPointer2 = new AppMain.ArrayPointer<AppMain.NNS_TEXFILE>(texfilelist.pTexFileList);
        int nTex = texfilelist.nTex;
        while (nTex > 0)
        {
            if (amb == null)
            {
                num = AppMain.amTextureLoad((AppMain.NNS_TEXINFO)arrayPointer1, (AppMain.NNS_TEXFILE)arrayPointer2, filepath, (Texture2D)null, 0);
            }
            else
            {
                Texture2D texbuf;
                try
                {
                    string str = ((AppMain.NNS_TEXFILE)~arrayPointer2).Filename;
                    int startIndex = str.LastIndexOf(".pvr", StringComparison.OrdinalIgnoreCase);
                    if (startIndex > 0)
                        str = str.Remove(startIndex) + ".PNG";
                    int index = Array.IndexOf<string>(amb.files, str);
                    if (amb.buf[index] != null)
                    {
                        texbuf = (Texture2D)amb.buf[index];
                    }
                    else
                    {
                        using (MemoryStream memoryStream = new MemoryStream(amb.data, amb.offsets[index], amb.data.Length - amb.offsets[index]))
                        {
                            texbuf = Texture2D.FromStream(AppMain.m_game.GraphicsDevice, (Stream)memoryStream);
                            amb.buf[index] = (object)texbuf;
                        }
                    }
                }
                catch (Exception)
                {
                    string str = ((AppMain.NNS_TEXFILE)~arrayPointer2).Filename;
                    int index = Array.IndexOf<string>(amb.files, str);
                    using (var stream = File.OpenRead("Content/dummy.png"))
                    {
                        texbuf = Texture2D.FromStream(AppMain.m_game.GraphicsDevice, (Stream)stream);
                        amb.buf[index] = (object)texbuf;
                    }
                }

                num = AppMain.amTextureLoad((AppMain.NNS_TEXINFO)arrayPointer1, (AppMain.NNS_TEXFILE)arrayPointer2, filepath, texbuf, 0);
            }
            --nTex;
            ++arrayPointer1;
            ++arrayPointer2;
        }
        return num;
    }

    public static int amTextureLoad(
      AppMain.NNS_TEXINFO texinfo,
      AppMain.NNS_TEXFILE texfile,
      string filepath,
      Texture2D texbuf,
      int size)
    {
        AppMain.AMS_PARAM_LOAD_TEXTURE paramLoadTexture = new AppMain.AMS_PARAM_LOAD_TEXTURE();
        paramLoadTexture.pTexInfo = texinfo;
        paramLoadTexture.tex = texbuf;
        paramLoadTexture.size = (uint)size;
        AppMain._amTextureSetupLoadParam(ref paramLoadTexture, ref texfile, texbuf);
        return AppMain.amDrawRegistCommand(1, (object)paramLoadTexture);
    }

    public static int amTextureLoad(
      Texture2D texture,
      byte[] image,
      int size,
      int minfilter,
      int magfilter,
      int u_wrap,
      int v_wrap,
      byte[] gvrobj)
    {
        AppMain.mppAssertNotImpl();
        return AppMain.amDrawRegistCommand(11, (object)new AppMain.AMS_PARAM_LOAD_TEXTURE_IMAGE()
        {
            texture = texture,
            size = size,
            minfilter = (short)minfilter,
            magfilter = (short)magfilter,
            u_wrap = (short)u_wrap,
            v_wrap = (short)v_wrap
        });
    }

    public static int amTextureRelease(AppMain.NNS_TEXLIST texlist)
    {
        return AppMain.amDrawRegistCommand(2, (object)new AppMain.AMS_PARAM_RELEASE_TEXTURE()
        {
            texlist = texlist
        });
    }

    public static int amTextureRelease(byte[] texture)
    {
        AppMain.mppAssertNotImpl();
        return 0;
    }

    public static byte[] _amTextureConvertHeader(ref AppMain.NNS_TEXINFO texinfo, ref byte[] texbuf)
    {
        AppMain.mppAssertNotImpl();
        return texbuf;
    }

    public static void _amTextureSetupLoadParam(
      ref AppMain.AMS_PARAM_LOAD_TEXTURE param,
      ref AppMain.NNS_TEXFILE texfile,
      Texture2D texload)
    {
        if (((int)texfile.fType & 512) != 0)
        {
            param.minfilter = (ushort)0;
            param.magfilter = (ushort)0;
        }
        else
        {
            param.minfilter = texfile.MinFilter;
            param.magfilter = texfile.MagFilter;
        }
        param.globalIndex = ((int)texfile.fType & 1024) == 0 ? 0U : texfile.GlobalIndex;
        if (((int)texfile.fType & 2048) != 0)
            param.bank = texfile.Bank;
        else
            param.bank = 0U;
    }

}