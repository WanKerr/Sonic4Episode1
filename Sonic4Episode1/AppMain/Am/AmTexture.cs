using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

public partial class AppMain
{
    public static int amTextureLoad(
        NNS_TEXLIST texlist,
        NNS_TEXFILELIST texfilelist,
        string filepath)
    {
        mppAssertNotImpl();
        return amTextureLoad(texlist, texfilelist, filepath, null);
    }

    public static int amTextureLoad(
        NNS_TEXLIST texlist,
        NNS_TEXFILELIST texfilelist,
        string filepath,
        AMS_AMB_HEADER amb)
    {
        int num = 0;
        ArrayPointer<NNS_TEXINFO> arrayPointer1 = new ArrayPointer<NNS_TEXINFO>(texlist.pTexInfoList);
        ArrayPointer<NNS_TEXFILE> arrayPointer2 = new ArrayPointer<NNS_TEXFILE>(texfilelist.pTexFileList);
        int nTex = texfilelist.nTex;
        while (nTex > 0)
        {
            if (amb == null)
            {
                num = amTextureLoad(arrayPointer1, arrayPointer2, filepath, null, 0);
            }
            else
            {
                Texture2D texbuf;
                string str = (~arrayPointer2).Filename;
                int index = Array.IndexOf(amb.files, str);
                if (index == -1)
                {
                    int startIndex = str.LastIndexOf(".pvr", StringComparison.OrdinalIgnoreCase);
                    if (startIndex > 0)
                        str = str.Remove(startIndex) + ".PNG";
                    index = Array.IndexOf(amb.files, str);
                }

                if (index == -1)
                {
                    int startIndex = str.LastIndexOf(".png", StringComparison.OrdinalIgnoreCase);
                    if (startIndex > 0)
                        str = str.Remove(startIndex) + ".DDS";
                    index = Array.IndexOf(amb.files, str);
                }

                if (index == -1)
                {
                    for (int i = 0; i < amb.files.Length; i++)
                    {
                        if (string.Compare(amb.files[i], (~arrayPointer2).Filename,
                                StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            index = i;
                            break;
                        }
                    }
                }

                //try
                //{
                if (amb.buf[index] != null)
                {
                    texbuf = (Texture2D) amb.buf[index];
                }
                else
                {
                    using (MemoryStream memoryStream = new MemoryStream(amb.data, amb.offsets[index],
                               amb.data.Length - amb.offsets[index]))
                    {
#if FNA
                        if (str.EndsWith(".DDS"))
                        {
                            texbuf = Texture2D.DDSFromStreamEXT(m_game.GraphicsDevice, memoryStream);
                            amb.buf[index] = texbuf;
                        }
                        else
                        {
#endif
                            texbuf = Texture2D.FromStream(m_game.GraphicsDevice, memoryStream);
                            amb.buf[index] = texbuf;

#if FNA
                        }
#endif
                    }
                }
                //}
                //catch (Exception)
                //{
                //    using (var stream = File.OpenRead("Content/dummy.png"))
                //    {
                //        texbuf = Texture2D.FromStream(AppMain.m_game.GraphicsDevice, (Stream)stream);
                //        amb.buf[index] = (object)texbuf;
                //    }
                //}

                num = amTextureLoad(arrayPointer1, arrayPointer2, filepath, texbuf, 0);
            }

            --nTex;
            ++arrayPointer1;
            ++arrayPointer2;
        }

        return num;
    }

    public static int amTextureLoad(
        NNS_TEXINFO texinfo,
        NNS_TEXFILE texfile,
        string filepath,
        Texture2D texbuf,
        int size)
    {
        AMS_PARAM_LOAD_TEXTURE paramLoadTexture = new AMS_PARAM_LOAD_TEXTURE();
        paramLoadTexture.pTexInfo = texinfo;
        paramLoadTexture.tex = texbuf;
        paramLoadTexture.size = (uint) size;
        _amTextureSetupLoadParam(ref paramLoadTexture, ref texfile, texbuf);
        return amDrawRegistCommand(1, paramLoadTexture);
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
        mppAssertNotImpl();
        return amDrawRegistCommand(11, new AMS_PARAM_LOAD_TEXTURE_IMAGE()
        {
            texture = texture,
            size = size,
            minfilter = (short) minfilter,
            magfilter = (short) magfilter,
            u_wrap = (short) u_wrap,
            v_wrap = (short) v_wrap
        });
    }

    public static int amTextureRelease(NNS_TEXLIST texlist)
    {
        return amDrawRegistCommand(2, new AMS_PARAM_RELEASE_TEXTURE()
        {
            texlist = texlist
        });
    }

    public static int amTextureRelease(byte[] texture)
    {
        mppAssertNotImpl();
        return 0;
    }

    public static byte[] _amTextureConvertHeader(ref NNS_TEXINFO texinfo, ref byte[] texbuf)
    {
        mppAssertNotImpl();
        return texbuf;
    }

    public static void _amTextureSetupLoadParam(
        ref AMS_PARAM_LOAD_TEXTURE param,
        ref NNS_TEXFILE texfile,
        Texture2D texload)
    {
        if (((int) texfile.fType & 512) != 0)
        {
            param.minfilter = 0;
            param.magfilter = 0;
        }
        else
        {
            param.minfilter = texfile.MinFilter;
            param.magfilter = texfile.MagFilter;
        }

        param.globalIndex = ((int) texfile.fType & 1024) == 0 ? 0U : texfile.GlobalIndex;
        if (((int) texfile.fType & 2048) != 0)
            param.bank = texfile.Bank;
        else
            param.bank = 0U;
    }
}