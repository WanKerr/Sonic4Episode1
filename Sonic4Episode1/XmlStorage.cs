
using System;
using System.IO;
using gs;
using System.Text;
using System.Xml.Serialization;

#if WINDOWSPHONE7_5
using System.IO.IsolatedStorage;
#endif

#if AOT
using System.Text.Json;
using System.Text.Json.Serialization;
#if WASM
using System.Runtime.InteropServices.JavaScript;
#endif
[JsonSerializable(typeof(Sonic4Save))]
[JsonSourceGenerationOptions(
#if WASM
        PropertyNamingPolicy = JsonKnownNamingPolicy.Unspecified,
#else
        PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
#endif
        GenerationMode = JsonSourceGenerationMode.Default)]
internal partial class Sonic4SerializerContext : JsonSerializerContext { }
#endif

public partial class XmlStorage
{
    private static bool saveSuccess = false;
    private static int lastError = 0;

#if WASM
    [JSImport("XmlStorage_GetSaveFile", "main.js")]
    internal static partial string GetSaveFile();
    [JSImport("XmlStorage_SetSaveFile", "main.js")]
    internal static partial void SetSaveFile([JSMarshalAs<JSType.String>] string value);
#endif

    internal static void Save(Sonic4Save save, bool createNew, bool delete)
    {
        try
        {
            var str = Path.Combine(AppMain.storePath, AppMain.g_ao_storage_filename);

#if AOT
            var encoding = new UTF8Encoding(false);
            var text = JsonSerializer.Serialize(save, Sonic4SerializerContext.Default.Sonic4Save);
#if WASM
            SetSaveFile(text);
#else
            using (var storageFileStream = File.Open(Path.ChangeExtension(str, "json"), FileMode.OpenOrCreate))
            using (var writer = new StreamWriter(storageFileStream, encoding))
            {
                storageFileStream.SetLength(0);
                writer.Write(text);
            }
#endif
#else
            var writer = new XmlSerializer(typeof(Sonic4Save));
#if WINDOWSPHONE7_5
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            using (var storageFileStream = store.OpenFile(str, FileMode.OpenOrCreate))
#else
            using (var storageFileStream = File.Open(str, FileMode.OpenOrCreate))
#endif
            {
                storageFileStream.SetLength(0);
                writer.Serialize(storageFileStream, save);
            }
#endif

            saveSuccess = true;
        }
        catch (Exception ex)
        {
            saveSuccess = false;
            lastError = 2;
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
#if WASM
            var text = GetSaveFile();
            save = JsonSerializer.Deserialize(text, Sonic4SerializerContext.Default.Sonic4Save);
#else
#if AOT
            using (var storageFileStream = File.Open(Path.ChangeExtension(str, "json"), FileMode.Open))
            {
                save = JsonSerializer.Deserialize(storageFileStream, Sonic4SerializerContext.Default.Sonic4Save);
            }
#else
            var serializer = new XmlSerializer(typeof(Sonic4Save));
#if WINDOWSPHONE7_5
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            using (var storageFileStream = store.OpenFile(str, FileMode.Open))
#else
            using (var storageFileStream = File.Open(str, FileMode.Open))
#endif
                save = (Sonic4Save)serializer.Deserialize(storageFileStream);
#endif
#endif
            saveSuccess = true;
            return save;
        }
        catch (Exception ex)
        {
            saveSuccess = false;
            lastError = 2;
        }

        return null;
    }
}