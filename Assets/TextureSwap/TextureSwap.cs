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

        void Start()
        {
            StartCoroutine(LoadBinData() );
        }
        IEnumerator LoadBinData()
        {
            yield return null;
            pvrBin = LoadDataFromStreamingAssets("test.data");

            string path = Path.Combine(Application.streamingAssetsPath, "assetbundletest.ab");
            WWW www = new WWW(path);
            while (!www.isDone) { yield return null; }
            //AbBin = www.bytes;

            if (result != null)
            {
                result.text = "Ready";
            }
            AbBin = www.bytes;
            Debug.Log("Complete Load");
        }

        public TextMesh result;
        // Use this for initialization
        public void TestExecute()
        {
            Debug.Log("KurokawaTest TestExecute");
            int tryNum = 2;

            float abStart = Time.realtimeSinceStartup;

            Profiler.BeginSample("LoadFromAssetBundle");
            for (int i = 0; i < tryNum; ++i)
            {
                this.LoadTextureFromAb(AbBin);
            }
            Profiler.EndSample();
            float abEnd = Time.realtimeSinceStartup;

            float rowStart = Time.realtimeSinceStartup;
            Profiler.BeginSample("LoadFromRawData");
            for (int i = 0; i < tryNum; ++i)
            {
                this.LoadTexture(pvrBin);
            }
            Profiler.EndSample();
            float rowEnd = Time.realtimeSinceStartup;

            result.text = "Ab : " + (abEnd - abStart) + "\nRaw:" + (rowEnd - rowStart);
        }

        void LoadTextureFromAb(byte[] bin)
        {

            try
            {
                if (bin == null)
                {
                    bin = LoadDataFromStreamingAssets("assetbundletest.ab");
                }
                AssetBundle ab = AssetBundle.LoadFromMemory(bin);
                var all = ab.LoadAllAssets<Texture2D>();
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