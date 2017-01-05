using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoAssetBundleTestPrj
{
    public struct RawTextureDataInfo
    {
        public int width;
        public int height;
        public int formatType;
        public int flags;

        public UnityEngine.TextureFormat unityFormat{
            get{
                return (TextureFormat)formatType;
            }
        }
        public bool mipmapFlag
        {
            get
            {
                return ((flags & 0x01) != 0);
            }
            set
            {
                if (value)
                {
                    flags |= 0x01;
                }
                else
                {
                    flags &= ~0x01;
                }
            }
        }

        public void LoadFromMemory( byte[] bin  ){
            width = ReadInt(bin, 0);
            height = ReadInt(bin, 4);
            formatType = ReadInt(bin, 8);
            flags = ReadInt(bin, 12);
        }

        public byte[] GetByteData(){
            byte[] bin = new byte[16];
            WriteInt(width, bin, 0);
            WriteInt(height, bin, 4);
            WriteInt(formatType, bin, 8);
            WriteInt(flags, bin, 12);
            return bin;
        }

        private static void WriteInt( int val , byte[] buf , int idx){
            buf[idx + 0 ] = (byte)( (val >> 0) & 0xff);
            buf[idx + 1 ] = (byte)( (val >> 8) & 0xff);
            buf[idx + 2 ] = (byte)( (val >> 16) & 0xff);
            buf[idx + 3 ] = (byte)( (val >> 24) & 0xff);
        }

        private int ReadInt( byte[] buf , int idx){
            int val = ((int)buf[idx + 0] << 0) + 
                ((int)buf[idx + 1] << 8) + 
                ((int)buf[idx + 2] << 16) + 
                ((int)buf[idx + 3] << 24) ;

            return val;
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(32);
            sb.Append(width).Append(" X ").Append(height).Append("::").Append(unityFormat.ToString()).Append("\n").
                Append("mipMap ").Append(mipmapFlag);
            return sb.ToString();
        }
    }
}
