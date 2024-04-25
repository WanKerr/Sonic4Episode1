using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public partial class AppMain
{
    public class AMS_FS
    {
        public string dir;
        public int count;
        public string[] files;
        public sbyte[] types;
        public sbyte[] flag;
        public sbyte type;
        public byte[] data;
        public int[] offsets;
        public int[] lengths;
        public sbyte stat;
        public string file_name;
        public Stream stream;
        public Task readTask;
        public FsBackgroundReadComplete callback;
        public AMS_AMB_HEADER amb_header;
        public bool is_real_amb = false;

        public static explicit operator AMS_AMB_HEADER(AMS_FS fs)
        {
            return readAMBFile(fs);
        }

        public void makeAmbHeader()
        {
            this.amb_header = new AMS_AMB_HEADER();
            this.amb_header.dir = this.dir;
            this.amb_header.file_num = this.count;
            this.amb_header.files = new string[this.files.Length];
            this.amb_header.types = new sbyte[this.types.Length];
            this.amb_header.flag = new sbyte[this.flag.Length];
            this.amb_header.offsets = new int[this.files.Length];
            this.amb_header.lengths = new int[this.files.Length];
            this.amb_header.data = this.data;
            this.amb_header.buf = new object[this.count];
            Array.Copy(files, 0, amb_header.files, 0, this.files.Length);
            Buffer.BlockCopy(types, 0, amb_header.types, 0, this.types.Length);
            Buffer.BlockCopy(flag, 0, amb_header.flag, 0, this.flag.Length);
            Buffer.BlockCopy(offsets, 0, amb_header.offsets, 0, this.offsets.Length * 4);
            Buffer.BlockCopy(lengths, 0, amb_header.lengths, 0, this.lengths.Length * 4);
            this.amb_header.is_real_amb = is_real_amb;
            amPreLoadAmbItems(this.amb_header);
        }
    }

    private static bool amFsIsComplete(AMS_FS cdfsp)
    {
        return cdfsp.stat == 3;
    }

    public static AMS_FS amFsReadBackground(string file_name)
    {
        FsReadSpeedBytesPerFrame = 32768;
        return amFsReadBackground(file_name, null);
    }

    public static AMS_FS amFsReadBackground(string file_name, int BytesPerFrame)
    {
        FsReadSpeedBytesPerFrame = BytesPerFrame;
        return amFsReadBackground(file_name, null);
    }

    public static AMS_FS amFsReadBackground(
        string file_name,
        FsBackgroundReadComplete callback)
    {
        //Console.WriteLine(file_name);

        AMS_FS amsFs;
        if (ams_fsList.Count > 0 && lastReadAMS_FS != null && lastReadAMS_FS.file_name == file_name)
        {
            amsFs = ams_fsList.First.Value;
            amFsExecuteBackgroundRead();
        }
        else
        {
            amsFs = new AMS_FS();
            amsFs.callback = callback;
            amsFs.file_name = file_name;
            amsFs.stat = 2;
            ams_fsList.AddLast(amsFs);
            lastReadAMS_FS = amsFs;
        }

        return amsFs;
    }

    private static AMS_AMB_HEADER readAMBFile(AMS_FS fs)
    {
        if (fs.amb_header == null)
            fs.makeAmbHeader();
        return fs.amb_header;
    }

    private static AMS_AMB_HEADER readAMBFile(object data)
    {
        switch (data)
        {
            case AMS_AMB_HEADER _:
                return (AMS_AMB_HEADER) data;
            case AmbChunk chunk:
                return readAMBFile(chunk);
            default:
                return readAMBFile((AMS_FS) data);
        }
    }

    private static AMS_AMB_HEADER readAMBFile(AmbChunk buf)
    {
        byte[] array = buf.array;
        int offset = buf.offset;
        AMS_AMB_HEADER amsAmbHeader = searchPreloadedAmb(buf.amb, buf.offset);
        if (amsAmbHeader != null)
            return amsAmbHeader;
        AMS_AMB_HEADER amb = new AMS_AMB_HEADER();
        using (Stream input = new MemoryStream(array, offset, buf.length))
        {
            // Console.WriteLine($"{offset}-{offset + buf.length}");
            if (offset == 0)
            {
                amb.data = new byte[input.Length];
                input.Read(amb.data, 0, amb.data.Length);
            }
            else if (buf.amb.is_real_amb)
            {
                amb.data = new byte[buf.length];
                input.Read(amb.data, 0, amb.data.Length);
            }
            else
            {
                amb.data = array;
            }

            input.Position = 0L;
            readAmbHeader(input, ref amb);

            amb.parent = buf.amb;
            amPreLoadAmbItems(amb);
        }

        return amb;
    }

    public static void readAmbHeader(Stream stream, ref AMS_AMB_HEADER amb)
    {
        using (BinaryReader binaryReader = new BinaryReader(stream))
        {
            var tmp = binaryReader.ReadInt32();
            if (tmp == 0x424d4123)
            {
                binaryReader.BaseStream.Seek(12, SeekOrigin.Current);
                amb.file_num = binaryReader.ReadInt32();
                var entryTableOffset = binaryReader.ReadInt32();
                binaryReader.BaseStream.Position = 0x1C;
                var stringTableOffset = binaryReader.ReadInt32();

                amb.is_real_amb = true;
                amb.files = new string[amb.file_num];
                amb.types = new sbyte[amb.file_num];
                amb.offsets = new int[amb.file_num];
                amb.lengths = new int[amb.file_num];
                amb.buf = new object[amb.file_num];
                amb.flag = new sbyte[4] {127, 127, 127, 127};
                for (int i = 0; i < amb.file_num; i++)
                {
                    binaryReader.BaseStream.Seek(entryTableOffset + (i * 0x10), SeekOrigin.Begin);
                    amb.offsets[i] = binaryReader.ReadInt32();
                    amb.lengths[i] = binaryReader.ReadInt32();
                    if (stringTableOffset != 0)
                    {
                        binaryReader.BaseStream.Seek(stringTableOffset + (i * 0x20), SeekOrigin.Begin);
                        amb.files[i] = readChars(binaryReader);
                    }
                    else
                    {
                        amb.files[i] = i.ToString();
                    }
                }
            }
            else
            {
                amb.file_num = tmp;
                amb.files = new string[amb.file_num];
                amb.types = new sbyte[amb.file_num];
                amb.offsets = new int[amb.file_num];
                amb.lengths = new int[amb.file_num];
                amb.buf = new object[amb.file_num];
                int num = binaryReader.ReadInt32();
                amb.flag = new sbyte[num];
                for (int i = 0; i < num; i++)
                {
                    amb.flag[i] = binaryReader.ReadSByte();
                }

                for (int j = 0; j < amb.file_num; j++)
                {
                    amb.files[j] = binaryReader.ReadString();
                    amb.types[j] = binaryReader.ReadSByte();
                }

                for (int k = 0; k < amb.file_num; k++)
                {
                    amb.offsets[k] = binaryReader.ReadInt32();
                }

                for (int l = 0; l < amb.file_num; l++)
                {
                    amb.lengths[l] = binaryReader.ReadInt32();
                }
            }
        }
    }


    public static AMS_AMB_HEADER searchPreloadedAmb(
        AMS_AMB_HEADER amb,
        int offset)
    {
        for (int index = 0; index < amb.file_num; ++index)
        {
            if (amb.offsets[index] == offset && amb.buf[index] != null)
                return (AMS_AMB_HEADER) amb.buf[index];
        }

        for (int index = 0; index < amb.file_num; ++index)
        {
            if (amb.buf[index] is AMS_AMB_HEADER)
            {
                AMS_AMB_HEADER amsAmbHeader = searchPreloadedAmb((AMS_AMB_HEADER) amb.buf[index], offset);
                if (amsAmbHeader != null)
                    return amsAmbHeader;
            }
        }

        return null;
    }

    public static void amPreLoadAmbItems(AMS_AMB_HEADER amb)
    {
        AmbChunk ambChunk = new AmbChunk(amb.data, 0, 0, amb);
        for (int index = 0; index < amb.files.Length; ++index)
        {
            ambChunk.offset = amb.offsets[index];
            ambChunk.length = amb.lengths[index];
            string extension = Path.GetExtension(amb.files[index]);
            if (extension.Equals(".INM", StringComparison.OrdinalIgnoreCase) ||
                extension.Equals(".INV", StringComparison.OrdinalIgnoreCase))
            {
                NNS_MOTION motion;
                amMotionSetup(out motion, ambChunk);
                amb.buf[index] = motion;
            }
            else if (extension.Equals(g_dm_buy_screen_amb_ext, StringComparison.OrdinalIgnoreCase))
            {
                amb.buf[index] = readAMBFile(ambChunk);
                amPreLoadAmbItems((AMS_AMB_HEADER) amb.buf[index]);
            }
            else if (extension.Equals(".AME", StringComparison.OrdinalIgnoreCase))
                amb.buf[index] = readAMEfile(ambChunk);
        }
    }

    public static void amFsExecuteBackgroundRead()
    {
        if (ams_fsList.Count > 0)
        {
            AMS_FS value = ams_fsList.First.Value;

            if (value.stream == null)
            {
                var stream = TitleContainer.OpenStream("Content\\" + value.file_name);

                value.stream = new MemoryStream();
#if ASYNC_IO
#if ASYNC_TARGETING_PACK
                value.readTask = Task.Factory.StartNew(() => stream.CopyTo(value.stream));
#else
                value.readTask = stream.CopyToAsync(value.stream);
#endif
#else
                stream.CopyTo(value.stream);
#endif
            }

            if (value.readTask != null && !value.readTask.IsCompleted)
            {
                return;
            }

            value.data = (value.stream as MemoryStream).ToArray();
            value.stream.Seek(0, SeekOrigin.Begin);

            //Console.WriteLine("{0}", value.data.Length);

            using (BinaryReader binaryReader = new BinaryReader(value.stream))
            {
                // 23 41 4D 42
                var tmp = binaryReader.ReadInt32();
                if (tmp == 0x424d4123)
                {
                    binaryReader.BaseStream.Position += 12;
                    value.count = binaryReader.ReadInt32();
                    var entryTableOffset = binaryReader.ReadInt32();
                    binaryReader.BaseStream.Position = 0x1C;
                    var stringTableOffset = binaryReader.ReadInt32();

                    value.is_real_amb = true;
                    value.files = new string[value.count];
                    value.types = new sbyte[value.count];
                    value.offsets = new int[value.count];
                    value.lengths = new int[value.count];
                    value.flag = new sbyte[0];
                    for (int i = 0; i < value.count; i++)
                    {
                        binaryReader.BaseStream.Seek(entryTableOffset + (i * 0x10), SeekOrigin.Begin);
                        value.offsets[i] = binaryReader.ReadInt32();
                        value.lengths[i] = binaryReader.ReadInt32();

                        if (stringTableOffset != 0)
                        {
                            binaryReader.BaseStream.Seek(stringTableOffset + (i * 0x20), SeekOrigin.Begin);
                            //var bytes = binaryReader.ReadBytes(0x20);
                            value.files[i] = readChars(binaryReader);
                        }
                        else
                        {
                            value.files[i] = i.ToString();
                        }
                    }
                }
                else
                {
                    value.count = tmp;
                    value.files = new string[value.count];
                    value.types = new sbyte[value.count];
                    value.offsets = new int[value.count];
                    value.lengths = new int[value.count];
                    int num2 = binaryReader.ReadInt32();
                    value.flag = new sbyte[num2];
                    for (int i = 0; i < num2; i++)
                    {
                        value.flag[i] = binaryReader.ReadSByte();
                    }

                    for (int j = 0; j < value.count; j++)
                    {
                        value.files[j] = binaryReader.ReadString();
                        value.types[j] = binaryReader.ReadSByte();
                    }

                    for (int k = 0; k < value.count; k++)
                    {
                        value.offsets[k] = binaryReader.ReadInt32();
                    }

                    for (int l = 0; l < value.count; l++)
                    {
                        value.lengths[l] = binaryReader.ReadInt32();
                    }
                }
            }

            value.makeAmbHeader();
            value.stat = 3;
            value.stream = null;
            ams_fsList.RemoveFirst();
            if (value.callback != null)            
                value.callback(value);            
        }
    }


    public AMS_FS amFsReadBackground(int file_id, byte[] buf, int cache)
    {
        mppAssertNotImpl();
        return null;
    }

    private AMS_FS amFsReadBackground(
        int afs_id,
        string file_name,
        byte[] buf,
        int cache)
    {
        mppAssertNotImpl();
        return null;
    }

    private AMS_FS amFsReadFileList(int afs_id, string file_name, byte[] buf)
    {
        mppAssertNotImpl();
        return null;
    }

    public static int amFsRead(string file_name, out byte[] buf)
    {
        int count = 0;
        using (Stream stream = TitleContainer.OpenStream(file_name))
        {
            count = (int) stream.Length;
            buf = new byte[count];
            stream.Read(buf, 0, count);
        }

        return count;
    }

    public static byte[] amFsRead(string file_name)
    {
        byte[] buffer = null;
        using (Stream stream = TitleContainer.OpenStream("Content\\" + file_name))
        {
            int length = (int) stream.Length;
            buffer = new byte[length];
            stream.Read(buffer, 0, length);
        }

        return buffer;
    }

    public static int amFsRead(string file_name, out Texture2D buf)
    {
        mppAssertNotImpl();
        buf = null;
        return 0;
    }

    private static void amFsClearRequest(AMS_FS cdfsp)
    {
        ams_fsList.Remove(cdfsp);
    }

    private int amFsSearchPartition(string pname)
    {
        return -1;
    }

    private int amFsSearchName(string fname)
    {
        return -1;
    }

    private int amFsReadStart()
    {
        return 1;
    }

    private void amFsReadListNext(AMS_FS cdfsp)
    {
    }

    private void amFsConvertPath(ref string out_name, ref string in_name)
    {
    }

    private string amFsGetColumn(string cp)
    {
        return cp;
    }

    private string amFsGetNextColumn(string cp)
    {
        return this.amFsGetColumn(cp);
    }

    private string amFsGetNumber(string cp, int num)
    {
        return cp;
    }

    public string amFsGetString(string cp, string str)
    {
        return cp;
    }
}