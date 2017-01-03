using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text;

namespace NoAssetBundlePrj
{

    public class TextureConvertBatchCreator
    {
        #region CONVERTER_INNER
        private enum TextureFormat
        {
            PVRTC1_2_RGB,
            PVRTC1_2,
            PVRTC1_4_RGB,
            PVRTC1_4,
            ETC1,
            ETC2_RGB,
            ETC2_RGB_A1,
            ETC2_RGBA,
        };
        private enum EditorOs{
            Windows,
            MacOS,
        };

        private enum TargetPlatform
        {
            IOS,
            Android,
        };


        private struct TextureConvertRule
        {
            public TextureFormat format;
            public EditorOs editorOs;
            public string pathIn;
        }
        #endregion CONVERTER_INNER


        [MenuItem("Tools/TextureConvertBatch/CreateBatch")]
        public static void CreateWindowsBatch()
        {
            StringBuilder sb = new StringBuilder();
            string pvrExePath = GetPvrTexToolForWindows();

            var rules = GetRules( TargetPlatform.Android, EditorOs.Windows );

            foreach (var rule in rules)
            {
                sb.Append(pvrExePath);
                sb.Append( GetTextureConvertOption(rule, "ConvertOutput") ) ;
                sb.Append( "\n" );
            }
            System.IO.File.WriteAllText("ConvertAndroid.bat", sb.ToString());
            Debug.Log(pvrExePath);
        }


        private static string GetPvrTexToolForWindows()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('"');
            string exeFile = EditorApplication.applicationPath;
            sb.Append( exeFile.Substring(0, exeFile.LastIndexOf('/')) );
            sb.Append("/Data/Tools/PVRTexTool.exe");
            sb.Append('"');
            sb.Replace('/', '\\');
            return ConvertPath( sb.ToString(),true);
        }

        public static string ConvertPath(string path, bool isWindows)
        {
            if (!isWindows) { return path; }
            return path.Replace('/', '\\');
        }

        private static List<TextureConvertRule> GetRules(TargetPlatform target, EditorOs editorOs)
        {
            var guids = AssetDatabase.FindAssets("t:Texture2D");
            List<TextureConvertRule> rules = new List<TextureConvertRule>(guids.Length);
            foreach (var guid in guids)
            {
                string texturePath = AssetDatabase.GUIDToAssetPath(guid);
                TextureImporter importer = TextureImporter.GetAtPath(texturePath) as TextureImporter;
                if (importer == null) { continue; }


                TextureConvertRule rule = new TextureConvertRule();
                rule.pathIn = texturePath;
                rule.editorOs = editorOs;
                if (!GetFormatFromImporter(target, importer, out rule.format))
                {
                    continue;
                }
                rules.Add(rule);
            }
            return rules;
        }

        private static bool GetFormatFromImporter(TargetPlatform target, TextureImporter importer, out TextureFormat targetFormat )
        {
            TextureImporterFormat importFormat = importer.textureFormat;
            int maxSize = importer.maxTextureSize;
            targetFormat = TextureFormat.ETC1;

            if (target == TargetPlatform.Android)
            {
                importer.GetPlatformTextureSettings("Android", out maxSize, out importFormat);
                switch (importFormat)
                {
                    case TextureImporterFormat.ETC_RGB4:
                        targetFormat = TextureFormat.ETC1;
                        return true;
                    case TextureImporterFormat.ETC2_RGB4:
                        targetFormat = TextureFormat.ETC2_RGB;
                        return true;
                    case TextureImporterFormat.ETC2_RGB4_PUNCHTHROUGH_ALPHA:
                        targetFormat = TextureFormat.ETC2_RGB_A1;
                        return true;
                    case TextureImporterFormat.ETC2_RGBA8:
                        targetFormat = TextureFormat.ETC2_RGBA;
                        return true;
                }
            }
            else if (target == TargetPlatform.IOS)
            {
                importer.GetPlatformTextureSettings("iOS", out maxSize, out importFormat);
                switch (importFormat)
                {
                    case TextureImporterFormat.PVRTC_RGB2:
                        targetFormat = TextureFormat.PVRTC1_2_RGB;
                        return true;
                    case TextureImporterFormat.PVRTC_RGB4:
                        targetFormat = TextureFormat.PVRTC1_4_RGB;
                        return true;
                    case TextureImporterFormat.PVRTC_RGBA2:
                        targetFormat = TextureFormat.PVRTC1_2;
                        return true;
                    case TextureImporterFormat.PVRTC_RGBA4:
                        targetFormat = TextureFormat.PVRTC1_4;
                        return true;
                }
            }
            return false;
        }

        private static string GetTextureConvertOption(TextureConvertRule rule,string outputDir) {
            StringBuilder sb = new StringBuilder();

            sb.Append(" -flip y");
            // format 
            sb.Append(" -f ");
            switch (rule.format)
            {
                case TextureFormat.PVRTC1_2_RGB:
                    sb.Append("PVRTC1_2_RGB");
                    break;
                case TextureFormat.PVRTC1_2:
                    sb.Append("PVRTC1_2");
                    break;
                case TextureFormat.PVRTC1_4_RGB:
                    sb.Append("PVRTC1_4_RGB");
                    break;
                case TextureFormat.PVRTC1_4:
                    sb.Append("PVRTC1_4");
                    break;
                case TextureFormat.ETC1:
                    sb.Append("ETC1");
                    break;
                case TextureFormat.ETC2_RGB:
                    sb.Append("ETC2_RGB");
                    break;
                case TextureFormat.ETC2_RGB_A1:
                    sb.Append("ETC2_RGB_A1");
                    break;
                case TextureFormat.ETC2_RGBA:
                    sb.Append("ETC2_RGBA");
                    break;
            }
            // Quality
            sb.Append(" -q "); switch (rule.format)
            {
                case TextureFormat.PVRTC1_2_RGB:
                case TextureFormat.PVRTC1_2:
                case TextureFormat.PVRTC1_4_RGB:
                case TextureFormat.PVRTC1_4:
                    sb.Append("pvrtcbest");
                    break;
                case TextureFormat.ETC1:
                case TextureFormat.ETC2_RGB:
                case TextureFormat.ETC2_RGB_A1:
                case TextureFormat.ETC2_RGBA:
                    sb.Append("etcslowperceptual");
                    break;
            }
            bool isWindows = (rule.editorOs == EditorOs.Windows);

            sb.Append(" -i ");
            sb.Append( ConvertPath( rule.pathIn,isWindows) );

            sb.Append(" -o ");

            string outputPath = System.IO.Path.Combine(outputDir, rule.pathIn.Substring(0, rule.pathIn.LastIndexOf('.') ) + ".pvr");
            outputPath = ConvertPath(outputPath, isWindows);
            sb.Append('"').Append(outputPath).Append('"');
            return sb.ToString();
        }
    }
}