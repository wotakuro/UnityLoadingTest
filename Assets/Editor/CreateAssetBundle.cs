using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreateAssetBundle {
    [MenuItem("Tools/AssetBundleCreate")]
    public static void Execute()
    {
        AssetDatabase.RemoveUnusedAssetBundleNames();
        BuildPipeline.BuildAssetBundles("Assets/StreamingAssets", BuildAssetBundleOptions.UncompressedAssetBundle,EditorUserBuildSettings.activeBuildTarget);
    }
}
