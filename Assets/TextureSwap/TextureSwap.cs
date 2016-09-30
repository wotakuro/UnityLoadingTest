using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;

public class TextureSwap : MonoBehaviour {
    public TextMesh result;
	// Use this for initialization
	void Start () {
        RawImage img = this.gameObject.GetComponentInChildren<RawImage>();
        Texture2D tex = img.texture as Texture2D;

        try
        {
            string path = Path.Combine(Application.streamingAssetsPath, "swap.png");
#if UNITY_ANDROID

            WWW www = new WWW(path);
            while (!www.isDone) { }

            byte[] bin = www.bytes;

#else            
            byte[] bin = File.ReadAllBytes(path);
#endif
            bool loadRes = tex.LoadImage(bin);

            if (result != null)
            {
                result.text = loadRes.ToString();
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
