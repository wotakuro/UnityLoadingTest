using UnityEngine;
using UnityEditor;

public class SaveTextureRawData {
    [MenuItem("Tools/CreateTextureRawData")]
    public static void SaveData()
    {
        Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/test.png");
        byte[] binData = tex.GetRawTextureData();
#if UNITY_ANDROID
        System.IO.File.WriteAllBytes("Assets/StreamingAssets/android/test.data", binData);
#elif UNITY_IOS
        System.IO.File.WriteAllBytes("Assets/StreamingAssets/ios/test.data", binData);
#else 
        EditorUtility.DisplayDialog("Only mobile", "Only mobile device support.Please switch to ios/android.", "OK");
#endif
    }
}
