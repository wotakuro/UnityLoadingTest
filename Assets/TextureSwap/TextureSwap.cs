using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;


namespace NoAssetBundlePrj
{
    public class TextureSwap : MonoBehaviour
    {
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

        byte[] pvrBin;// = LoadDataFromStreamingAssets("test.data");
        byte[] AbBin;// = LoadDataFromStreamingAssets("assetbundletest.ab");
        byte[] AbBinInBin;// = LoadDataFromStreamingAssets("binarytest.ab");

        void Start()
        {
            StartCoroutine(LoadBinData() );
        }
        IEnumerator LoadBinData()
        {
            yield return null;
            pvrBin = LoadDataFromStreamingAssets("test.data");
#if UNITY_ANDROID && !UNITY_EDITOR
            string path = Path.Combine(Application.streamingAssetsPath, "assetbundletest.ab");
            WWW www = new WWW(path);
            while (!www.isDone) { yield return null; }
            AbBin = www.bytes;

            path = Path.Combine(Application.streamingAssetsPath, "binarytest.ab");
            WWW www2 = new WWW(path);
            while (!www2.isDone) { yield return null; }
            AbBinInBin = www.bytes;

#else
            AbBin = LoadDataFromStreamingAssets("assetbundletest.ab");
            AbBinInBin = LoadDataFromStreamingAssets("assetbundletest.ab");
#endif
            if (result != null)
            {
                result.text = "Ready";
            }

        }

        public TextMesh result;
        // Use this for initialization
        public void TestExecute()
        {
            Debug.Log("KurokawaTest TestExecute");
            int tryNum = 2;


            float rawStart = Time.realtimeSinceStartup;
            Profiler.BeginSample("LoadFromRawData");
            for (int i = 0; i < tryNum; ++i)
            {
                this.LoadTexture(pvrBin);
            }
            Profiler.EndSample();
            float rawEnd = Time.realtimeSinceStartup;


            float abStart = Time.realtimeSinceStartup;
            Profiler.BeginSample("LoadTextureFromAssetBundle");
            for (int i = 0; i < tryNum; ++i)
            {
                this.LoadDataFromAb < Texture2D>(AbBin);
            }
            Profiler.EndSample();
            float abEnd = Time.realtimeSinceStartup;

            float abBinStart = Time.realtimeSinceStartup;
            Profiler.BeginSample("LoadBinFromAssetBundle");
            for (int i = 0; i < tryNum; ++i)
            {
                this.LoadDataFromAb<TextAsset>(AbBinInBin);
            }
            Profiler.EndSample();
            float abBinEnd = Time.realtimeSinceStartup;


            result.text = "Ab : " + (abEnd - abStart) + "\nRaw:" + (rawEnd - rawStart)
                + "\nBinFromAb:" + (abBinEnd - abBinStart);
        }

        void LoadDataFromAb<T>(byte[] bin) where T:UnityEngine.Object
        {

            try
            {
                if (bin == null)
                {
                    bin = LoadDataFromStreamingAssets("assetbundletest.ab");
                }
                AssetBundle ab = AssetBundle.LoadFromMemory(bin);
                var all = ab.LoadAllAssets<T>();
                ab.Unload(true);
                ab = null;
            }
            catch (System.Exception e)
            {
                if (result != null)
                {
                    result.text = e.ToString();
                }
            }
        }

        void LoadTexture(byte[] bin)
        {

            try
            {
                if (bin == null)
                {
                    bin = LoadDataFromStreamingAssets("test.data");
                }
                Texture2D newTex = new Texture2D(2048, 2048, TextureFormat.ETC2_RGB, false);
                newTex.LoadRawTextureData(bin);
                newTex.Apply();
                Object.DestroyImmediate(newTex, false);
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

            string path = Path.Combine(Application.streamingAssetsPath, file );
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