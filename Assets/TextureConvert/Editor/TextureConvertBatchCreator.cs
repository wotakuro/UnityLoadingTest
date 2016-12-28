using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text;

namespace NoAssetBundlePrj
{

    public class TextureConvertBatchCreator 
    {
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


        private struct TextureConvertRule
        {
            public TextureFormat format;
            public EditorOs editorOs;
            public string pathInUnityEditor;
        }


        [MenuItem("Tools/TextureConvertBatch/Create")]
        public static void CreateBatch()
        {
            StringBuilder sb = new StringBuilder();
            string pvrExePath = GetPvrTexToolForWindows();
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

            sb.Append(" -i ");

            return "";
        }
    }
}