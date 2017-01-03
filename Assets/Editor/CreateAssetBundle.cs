using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreateAssetBundle {
    [MenuItem("Tools/AssetBundleCreate")]
    public static void Execute()
    {
        AssetDatabase.RemoveUnusedAssetBundleNames();
#if UNITY_ANDROID
        BuildPipeline.BuildAssetBundles("Assets/StreamingAssets/Android", BuildAssetBundleOptions.UncompressedAssetBundle,EditorUserBuildSettings.activeBuildTarget);
#elif UNITY_IOS
        BuildPipeline.BuildAssetBundles("Assets/StreamingAssets/ios", BuildAssetBundleOptions.UncompressedAssetBundle,EditorUserBuildSettings.activeBuildTarget);
#else
        EditorUtility.DisplayDialog("Only mobile", "Only mobile device support.Please switch to ios/android.", "OK");
#endif
    }
}
