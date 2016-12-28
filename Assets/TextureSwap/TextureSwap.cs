using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
namespace NoAssetBundlePrj
{
    public class TextureSwap : MonoBehaviour
    {
        public TextMesh result;
        // Use this for initialization
        void Start()
        {
            this.LoadTexture();
        }
        void LoadTexture()
        {
            RawImage img = this.gameObject.GetComponentInChildren<RawImage>();
            Texture2D tex = img.texture as Texture2D;

            try
            {
                string path = Path.Combine(Application.streamingAssetsPath, "test.data");
#if UNITY_ANDROID && !UNITY_EDITOR

            WWW www = new WWW(path);
            while (!www.isDone) { }

            byte[] bin = www.bytes;

#else
                byte[] bin = File.ReadAllBytes(path);
#endif
                Texture2D newTex = new Texture2D(2048,2048,TextureFormat.ETC2_RGB,false);
                newTex.LoadRawTextureData(bin);
                newTex.Apply();
                img.texture = newTex;

                if (result != null)
                {
                }
                Debug.Log(tex.name);
            }
            catch (System.Exception e)
            {
                if (result != null)
                {
                    result.text = e.ToString();
                }

            }
        }
    }
}