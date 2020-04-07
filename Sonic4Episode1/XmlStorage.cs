using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using gs;

public class XmlStorage
{
    private static bool saveSuccess = false;
    private static int lastError = 0;

    internal static void Save(Sonic4Save save, bool createNew, bool delete)
    {
        try
        {
            var str = Path.Combine(AppMain.storePath, AppMain.g_ao_storage_filename);
            var writer = new XmlSerializer(typeof(gs.Sonic4Save));
            using (var storageFileStream = File.Open(str, FileMode.OpenOrCreate))
            {
                storageFileStream.SetLength(0);
                writer.Serialize(storageFileStream, save);
            }

            saveSuccess = true;
        }
        catch (Exception ex)
        {
            saveSuccess = false;
            lastError = 2;
            Console.WriteLine(ex);
        }
    }

    internal static bool SaveSuccess()
    {
        return saveSuccess;
    }

    internal static int GetLastError()
    {
        var error = lastError;
        lastError = 0;
        return error;
    }

    internal static Sonic4Save Load()
    {
        try
        {
            Sonic4Save save;
            var str = Path.Combine(AppMain.storePath, AppMain.g_ao_storage_filename);
            if (!File.Exists(str))
                throw new FileNotFoundException();

            var serializer = new XmlSerializer(typeof(gs.Sonic4Save));
            using (var storageFileStream = File.Open(str, FileMode.Open))
            {
                save = (gs.Sonic4Save)serializer.Deserialize(storageFileStream);
            }

            saveSuccess = true;
            return save;
        }
        catch (Exception ex)
        {
            saveSuccess = false;
            lastError = 2;
            Console.WriteLine(ex);
        }

        return null;
    }
}
