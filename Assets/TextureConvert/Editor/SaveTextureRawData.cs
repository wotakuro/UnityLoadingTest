using UnityEngine;
using UnityEditor;

public class SaveTextureRawData {
    [MenuItem("Tools/SaveTest")]
    public static void SaveData()
    {
        Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/test.png");
        byte[] binData = tex.GetRawTextureData();

        System.IO.File.WriteAllBytes("Assets/StreamingAssets/test.data", binData);
    }
}
