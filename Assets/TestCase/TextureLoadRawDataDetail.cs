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
    public class TextureLoadRawDataDetail : MonoBehaviour
    {


        public Text formatInfo;
        public Text result;
        public RawImage assetBundleImageCheck;
        public RawImage rawImageCheck;

        private byte[] rawImageBinData;// = LoadDataFromStreamingAssets("test.data");
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

            if (result != null)
            {
                result.text = "Ready";
            }
        }

        public void CheckImage()
        {
            this.LoadTextureFromRawData(rawImageBinData, true);
        }

        // Use this for initialization
        public void TestExecute()
        {
            int tryNum = 1;


            result.text = "";
            this.LoadTextureFromRawData(rawImageBinData);
        }

        void LoadTextureFromRawData(byte[] bin,bool checkImage = false)
        {
            try
            {
                Texture2D newTex = null;

                float start = Time.realtimeSinceStartup;
                newTex = new Texture2D(rawDataInfo.width, rawDataInfo.height, rawDataInfo.unityFormat, rawDataInfo.mipmapFlag );
                float afterNew = Time.realtimeSinceStartup;
                newTex.LoadRawTextureData(bin);
                float afterLoad = Time.realtimeSinceStartup;
                newTex.Apply(true, true);
                float afterApply = Time.realtimeSinceStartup;

                if (checkImage)
                {
                    this.rawImageCheck.texture = newTex;
                }
                else
                {                    
                    Object.DestroyImmediate(newTex, false);
                }
                float afterDestroy = Time.realtimeSinceStartup;


                if (!checkImage)
                {
                    float newTime = (afterNew - start);
                    float loadTime = (afterLoad - afterNew);
                    float applyTime = (afterApply - afterLoad);
                    float deleteTime = (afterDestroy-afterApply);
                    float allTime = (afterDestroy-start);
                    result.text = "TextureNew:" + newTime + "\nLoad:" + loadTime + "\nApply:" + applyTime + "\nDelete:" + deleteTime +
                        "\n" + allTime + " " + ( (loadTime+applyTime) / allTime ) ;
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