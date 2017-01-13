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
    public class TestureLoadAssetBundleDetail : MonoBehaviour
    {

        #region ManagedMemoryPreAlloc
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID )	&& ENABLE_IL2CPP
    [DllImport("__Internal")]
    private static extern void GC_expand_hp(int bytes);
#endif

        /// <summary>
        /// Preallocate for Managed Memory
        /// </summary>
        /// <param name="size">Size of prealloc memory</param>
        public static void AllocateGcMemory(int size)
        {
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID ) && ENABLE_IL2CPP
        GC_expand_hp(size);
#endif
        }

        /// <summary>
        /// before scene load
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
            byte[] rawImageBinData = LoadDataFromStreamingAssets("test.data");

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
            this.LoadTextureFromAssetBundle(assetBundleBinData, true);
        }

        // Use this for initialization
        public void TestExecute()
        {
            int tryNum = 1;



            for (int i = 0; i < tryNum; ++i)
            {
                this.LoadTextureFromAssetBundle(assetBundleBinData);
            }

        }

        void LoadTextureFromAssetBundle(byte[] bin, bool checkImage = false)
        {

            try
            {
                this.result.text = "";
                float start = Time.realtimeSinceStartup;
                AssetBundle ab = AssetBundle.LoadFromMemory(bin);

                float afterCreateAssetBundle = Time.realtimeSinceStartup;
                var all = ab.LoadAllAssets<Texture2D>();
                float afterLoadAsset = Time.realtimeSinceStartup;

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
                float allOver = Time.realtimeSinceStartup;

                float loadFromMemory = (afterCreateAssetBundle - start);
                float loadAllAssets = (afterLoadAsset - afterCreateAssetBundle);
                float deleteAssetBundle = (allOver - afterLoadAsset);
                float allTime = ( allOver - start);
                this.result.text = "LoadFromMemory:" + loadFromMemory + "\nLoadAllAssets:" + loadAllAssets + "\nDelete:" + deleteAssetBundle +"\n" ;

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