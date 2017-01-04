using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

namespace NoAssetBundleTestPrj
{
    public class CreateLoadTestData
    {
        [MenuItem("Tools/LoadTest/CreateData")]
        public static void CreateTestData()
        {
            TryCreateDirectory("Assets/StreamingAssets");
            TryCreateDirectory("Assets/StreamingAssets/android");
            TryCreateDirectory("Assets/StreamingAssets/ios");
            CreateAssetBundle();
            CreateRawTextureData();
        }

        private static void TryCreateDirectory(string dir)
        {

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        public static void CreateAssetBundle()
        {
            AssetDatabase.RemoveUnusedAssetBundleNames();
#if UNITY_ANDROID
            BuildPipeline.BuildAssetBundles("Assets/StreamingAssets/android", BuildAssetBundleOptions.UncompressedAssetBundle, EditorUserBuildSettings.activeBuildTarget);
#elif UNITY_IOS
        BuildPipeline.BuildAssetBundles("Assets/StreamingAssets/ios", BuildAssetBundleOptions.UncompressedAssetBundle,EditorUserBuildSettings.activeBuildTarget);
#else
        EditorUtility.DisplayDialog("Only mobile", "Only mobile device support.Please switch to ios/android.", "OK");
#endif
        }

        public static void CreateRawTextureData()
        {
            // save Raw Data
            Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/test.png");
            byte[] binData = tex.GetRawTextureData();
#if UNITY_ANDROID
            File.WriteAllBytes("Assets/StreamingAssets/android/test.data", binData);
#elif UNITY_IOS
            File.WriteAllBytes("Assets/StreamingAssets/ios/test.data", binData);
#else 
            EditorUtility.DisplayDialog("Only mobile", "Only mobile device support.Please switch to ios/android.", "OK");
#endif
            RawTextureDataInfo info = new RawTextureDataInfo();
            info.width = tex.width;
            info.height = tex.width;
            info.formatType = (int)tex.format;
            info.mipmapFlag = (tex.mipmapCount > 1);
#if UNITY_ANDROID
            File.WriteAllBytes("Assets/StreamingAssets/android/test.header", info.GetByteData() );
#elif UNITY_IOS
            File.WriteAllBytes("Assets/StreamingAssets/ios/test.header", info.GetByteData() );
#endif
        }

    }
}