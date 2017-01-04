using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;

#if UNITY_5_5_OR_NEWER
using UnityEngine.Profiling;
#endif

namespace NoAssetBundleTestPrj
{
    public class TextureSwapTest : MonoBehaviour
    {

        #region ManagedMemoryPreAlloc
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID )	&& ENABLE_IL2CPP
    [DllImport("__Internal")]
    private static extern void GC_expand_hp(int bytes);
#endif

        /// <summary>
        /// 事前に ManagedHeapを確保しに行きます
        /// </summary>
        /// <param name="size">何バイト分確保したいか</param>
        public static void AllocateGcMemory(int size)
        {
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID ) && ENABLE_IL2CPP
        GC_expand_hp(size);
#endif
        }

        /// <summary>
        /// シーンロード前に呼び出されます
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void AllocateGcBeforeStartApp()
        {
            AllocateGcMemory(1024 * 1024 * 40);
        }

        #endregion ManagedMemoryPreAlloc

        public Text formatInfo;
        public Text result;
        public RawImage assetBundleImageCheck;
        public RawImage rawImageCheck;

        private byte[] rawImageBinData;// = LoadDataFromStreamingAssets("test.data");
        private byte[] assetBundleBinData;// = LoadDataFromStreamingAssets("assetbundletest.ab");
        private RawTextureDataInfo rawDataInfo;

        void Start()
        {
            StartCoroutine(LoadBinData() );
        }
        IEnumerator LoadBinData()
        {
            byte[] headerBinData = LoadDataFromStreamingAssets("test.header");
            this.rawDataInfo.LoadFromMemory(headerBinData);
            yield return null;
            rawImageBinData = LoadDataFromStreamingAssets("test.data");

            if (formatInfo != null)
            {
                formatInfo.text = this.rawDataInfo.ToString() + "\nFileSize:" + (rawImageBinData.Length /1024) +" KB";
            }

#if UNITY_ANDROID && !UNITY_EDITOR
            string path = Path.Combine(Application.streamingAssetsPath, "android/assetbundletest.ab");
            WWW www = new WWW(path);
            while (!www.isDone) { yield return null; }
            assetBundleBinData = www.bytes;
#else
            assetBundleBinData = LoadDataFromStreamingAssets("assetbundletest.ab");
#endif
            if (result != null)
            {
                result.text = "Ready";
            }
        }

        public void CheckImage()
        {
            this.LoadTextureFromRawData(rawImageBinData, true);
            this.LoadTextureFromAssetBundle(assetBundleBinData, true);
        }

        // Use this for initialization
        public void TestExecute()
        {
            int tryNum = 1;


            float rawStart = Time.realtimeSinceStartup;
            Profiler.BeginSample("LoadFromRawData");
            for (int i = 0; i < tryNum; ++i)
            {
                this.LoadTextureFromRawData(rawImageBinData);
            }
            Profiler.EndSample();
            float rawEnd = Time.realtimeSinceStartup;


            float abStart = Time.realtimeSinceStartup;
            Profiler.BeginSample("LoadTextureFromAssetBundle");
            for (int i = 0; i < tryNum; ++i)
            {
                this.LoadTextureFromAssetBundle(assetBundleBinData);
            }
            Profiler.EndSample();
            float abEnd = Time.realtimeSinceStartup;



            result.text = "Ab : " + (abEnd - abStart) + "\nRaw:" + (rawEnd - rawStart);
        }

        void LoadTextureFromAssetBundle(byte[] bin, bool checkImage = false)
        {

            try
            {
                AssetBundle ab = AssetBundle.LoadFromMemory(bin);
                var all = ab.LoadAllAssets<Texture2D>();

                if (checkImage)
                {
                    this.assetBundleImageCheck.texture = all[0];
                    ab.Unload(false);
                }
                else
                {
                    ab.Unload(true);
                    ab = null;
                }
            }
            catch (System.Exception e)
            {
                if (result != null)
                {
                    result.text = e.ToString();
                }
            }
        }

        void LoadTextureFromRawData(byte[] bin,bool checkImage = false)
        {
            try
            {
                Texture2D newTex = null;
                newTex = new Texture2D(rawDataInfo.width, rawDataInfo.height, rawDataInfo.unityFormat, rawDataInfo.mipmapFlag );

                newTex.LoadRawTextureData(bin);
                newTex.Apply();
                if (checkImage)
                {
                    this.rawImageCheck.texture = newTex;
                }
                else
                {
                    Object.DestroyImmediate(newTex, false);
                }
            }
            catch (System.Exception e)
            {
                if (result != null)
                {
                    result.text = e.ToString();
                }

            }
        }

        byte[] LoadDataFromStreamingAssets(string file)
        {

            string path = null;
            
#if UNITY_ANDROID
            path = Path.Combine(Application.streamingAssetsPath,"android");
#elif UNITY_IOS
            path = Path.Combine(Application.streamingAssetsPath,"ios");
#else
#endif

            path=  Path.Combine(path, file);
#if UNITY_ANDROID && !UNITY_EDITOR

            WWW www = new WWW(path);
            while (!www.isDone) { }

            byte[] bin = www.bytes;

#else
            byte[] bin = File.ReadAllBytes(path);
#endif
            return bin;
        }

    }
}