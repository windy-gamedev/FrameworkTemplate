using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Wigi.Utilities
{
    public class ResourcesDownloader : Singleton<ResourcesDownloader>
    {
        #region LoadText
        public static void LoadText(string fileName, string url, Action<string> callback, Action<long> downloadFail = null)
        {
            Instance.StartCoroutine(DownloadTextCoroutine(fileName, url, callback, downloadFail));
        }

        static IEnumerator DownloadTextCoroutine(string fileName, string url, Action<string> callback, Action<long> downloadFail)
        {
            UnityWebRequest www = new UnityWebRequest(url);
            GenerateHeader(www);
            string path = Path.Combine(Application.persistentDataPath, fileName + ".txt");
            www.downloadHandler = new DownloadHandlerFile(path);
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
                downloadFail?.Invoke(www.responseCode);
            else
                callback(path);
            www.Dispose();
        }
        #endregion

        #region Audio
        public static void GETAudioClip(string url, Action<AudioClip> DoneCallback, AudioType audioType = AudioType.WAV)
        {

            Instance.StartCoroutine(LoadAudioClip(url, DoneCallback, audioType));
        }

        private static IEnumerator LoadAudioClip(string url, Action<AudioClip> callback, AudioType audioType = AudioType.WAV)
        {
            //Debug.Log(url);
            AudioClip clip = null;
            // try download 50 times
            int totalTry = 200;
            int count = 0;
            while (!clip)
            {
                using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, audioType))
                {
                    var handler = www.downloadHandler as DownloadHandlerAudioClip;
                    handler.compressed = false;
                    handler.streamAudio = true;

                    yield return www.SendWebRequest();

                    count++;
                    if (count >= totalTry)
                    {
                        www.Dispose();
                        break;
                    }
                    if (www.isNetworkError || www.isHttpError)
                    {
                        Debug.LogWarning("ERROR: " + www.error);
                    }
                    else
                    {
                        while (clip == null)
                        {
                            clip = DownloadHandlerAudioClip.GetContent(www);
                            clip.name = url;
                        }
                        callback(clip);
                        www.Dispose();
                        yield break;
                    }
                }
                //Wait each frame in each loop OR Unity would freeze
            }
            if (count >= totalTry)
            {
                Debug.LogWarning("VA: Try to download clip over " + totalTry + " times");
                callback(clip);
            }
            yield return null;
        }
        #endregion

        private const ulong maxImageCachedSize = 40 * 1024 * 1024;  //byte
        private static ulong currentImageCachedSize = 0;

        //Todo: add feature save file
        private const String imageStorage = "imageCache/";
        static string storageSavePath = "";

        Dictionary<string, RemoteData<Sprite>> ImageCached = new Dictionary<string, RemoteData<Sprite>>();
        Dictionary<string, Coroutine> downloadQueue = new Dictionary<string, Coroutine>();
        public delegate void OnLoadFinish(RemoteData<Sprite> data);

        /// <summary>
        /// Dowm load Image
        /// </summary>
        #region Download Image
        public Sprite ICON_LOAD_DEFAULT = null;

        public static Sprite GetIconLoad()
        {
            return Instance.ICON_LOAD_DEFAULT;
        }

        public static void LoadURLImage(string url, OnLoadFinish callback)
        {
            LoadImage(url, callback);
        }
        public static void LoadURLImage(Image image, string url)
        {
            image.sprite = GetIconLoad();
            LoadImage(url, data =>
            {
                if (data.url == url && image != null)
                {
                    image.sprite = data.data;
                }
            });
        }
        public static void LoadImageSprite(Sprite sprite, string url)
        {
            LoadImage(url, data =>
            {
                if (data.url == url && sprite != null)
                {
                    sprite = data.data;
                }
            });
        }
        

        //Process load image
        public static void LoadImage(string url, OnLoadFinish callback, Action<long> errorServer = null, Action<long> errorInternet = null, bool ignoreCache = false)
        {
            if (LoadCacheData(url, out RemoteData<Sprite> value))
            {
                callback(value);
            }
            else if (Instance.downloadQueue.ContainsKey(url))
            {
                Instance.StartCoroutine(waitLoaderFromCache(url, callback));
            }
            else
            {
                var filePath = GetFilePath(url);
                Coroutine loader;
                if (File.Exists(filePath))
                {
                    Debug.Log("Start Load Image from File: " + url);
                    loader = Instance.StartCoroutine(LoadImageFromFile(url, filePath, callback));
                }
                //Download
                else
                {
                    Debug.Log("Start Download Image: " + url);
                    loader = Instance.StartCoroutine(DownloadImageCoroutine(url, filePath, callback, errorServer, errorInternet, ignoreCache));
                }
                Instance.downloadQueue.Add(url, loader);
            }
        }

        static IEnumerator waitLoaderFromCache(string key, OnLoadFinish callback)
        {
            yield return new WaitUntil(() =>
            {
                return Instance.ImageCached.ContainsKey(key);
            });
            callback(Instance.ImageCached[key]);
        }

        static IEnumerator DownloadImageCoroutine(string url, string filePath, OnLoadFinish callback, Action<long> errorServer, Action<long> errorInternet, bool ignoreCache)
        {

            if (!ignoreCache && Instance.ImageCached.TryGetValue(url, out RemoteData<Sprite> value))
            {
                callback(value);
            }
            else
            {
                UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
                GenerateHeader(www);
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log("<color=red>Download error: " + url + ": " + www.error + "</color>");
                }
                else
                {
                    SaveFileTexture(filePath, www.downloadHandler.data);
                    //ulong downloadedSize = www.downloadedBytes;
                    Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                    value = SaveCacheData(url, texture);
                    callback(value);

                }
                www.Dispose();
            }
            Instance.downloadQueue.Remove(url);
        }

        static IEnumerator LoadImageFromFile(string url, string filePath, OnLoadFinish callback)
        {
            var now = TimeUtil.Now();
            var texture = new Texture2D(2, 2);
            var taskLoad = File.ReadAllBytesAsync(filePath);

            yield return new WaitUntil(() =>
            {
                return taskLoad.IsCompleted;
            });
            /*
                        byte[] fileData = taskLoad.Result;
                        texture.LoadImage(fileData); //..this will auto-resize the texture dimensions.
                        RemoteData<Sprite> value = SaveCacheData(url, texture);*/

            var noww = TimeUtil.Now();
            byte[] fileData = taskLoad.Result;
            texture.LoadImage(fileData); //..this will auto-resize the texture dimensions.
            RemoteData<Sprite> value = SaveCacheData(url, texture);
            callback(value);
            Instance.downloadQueue.Remove(url);
        }

        static void SaveFileTexture(string filePath, byte[] data)
        {
            File.WriteAllBytesAsync(filePath, data);
        }

        static bool LoadCacheData(string url, out RemoteData<Sprite> value)
        {
            return Instance.ImageCached.TryGetValue(url, out value);
        }

        static RemoteData<Sprite> SaveCacheData(string url, Texture2D texture)
        {
            ulong sizeTexture = (ulong)(texture.width * texture.height * 4);
            currentImageCachedSize += sizeTexture;
            if (currentImageCachedSize >= maxImageCachedSize)
            {
                RemoveCachedImage();
            }

            var value = new RemoteData<Sprite>()
            {
                url = url,
                textureSize = sizeTexture,
                data = ConvertTexture2DToSprite(texture),
                created = DateTime.Now
            };

            Instance.ImageCached[url] = value;
            return value;
        }

        static void RemoveCachedImage()
        {
            ulong offsetTextureSizeRemove = 0;
            List<RemoteData<Sprite>> allImage = Instance.ImageCached.Values.ToList();
            allImage.Sort((x, y) => DateTime.Compare(x.created, y.created));

            ulong sizeRemove = (ulong)(maxImageCachedSize * 0.2f);
            foreach (var image in allImage)
            {
                offsetTextureSizeRemove += image.textureSize;
                Instance.ImageCached.Remove(image.url);
                if (offsetTextureSizeRemove >= sizeRemove)
                {
                    break;
                }
            }

            currentImageCachedSize -= offsetTextureSizeRemove;
            Resources.UnloadUnusedAssets();
        }

        static string GetFilePath(string url)
        {
            if(storageSavePath == "")
            {
                storageSavePath = Application.persistentDataPath + "/" + imageStorage;
                if (!Directory.Exists(storageSavePath))
                {
                    Directory.CreateDirectory(storageSavePath);
                }
            }

            return storageSavePath + "/" + CreateMD5(url);
        }

        static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        private static void GenerateHeader(UnityWebRequest www)
        {

        }

        static Sprite ConvertTexture2DToSprite(Texture2D texture)
        {
            try
            {
                Sprite result = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                return result;
            }
            catch (Exception e)
            {
                return null;
            }

        }
        #endregion
    }

    public class RemoteData<T>
    {
        public string url;
        public T data;
        public ulong textureSize;
        public DateTime created;
    }
}
