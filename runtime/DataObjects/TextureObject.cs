using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using Object = System.Object;

namespace Packages.FxEditor
{
    public class ExternaTexturelDataBlock
    {
        public string path="";
        public UInt64 objectid = 0;
        public int position = 0;
        public int size = 0;
        public int format = 0;
        
    }

    public class TextureObject : DataObjectBase
    {
        private static Dictionary<TextureFormat, int> registeredFormat = new Dictionary<TextureFormat, int>();
        private static Dictionary<TextureWrapMode, int> registeredWrap = new Dictionary<TextureWrapMode, int>();
        private static Dictionary<FilterMode, int> registeredFilter = new Dictionary<FilterMode, int>();
        public Texture _texture;
        private int[] dim = new int[3];
        private int type = 0;

        //-------for external data-------
        //public List<ExternaTexturelDataBlock> externalTextures = new List<ExternaTexturelDataBlock>();
        public ExternaTexturelDataBlock externalTextureData = null;
        //-------------------------------


        public static void RegisterFormat()
        {
            //format
            registeredFormat[TextureFormat.RGB24] = 0;
            registeredFormat[TextureFormat.R8] = 1;
            registeredFormat[TextureFormat.RGBA32] = 2;
            registeredFormat[TextureFormat.RGB565] = 3;
            registeredFormat[TextureFormat.RFloat] = 4;
            registeredFormat[TextureFormat.RGBA4444] = 5;
            registeredFormat[TextureFormat.ASTC_4x4] = 6;
            registeredFormat[TextureFormat.ASTC_5x5] = 7;
            registeredFormat[TextureFormat.ASTC_6x6] = 8;
            registeredFormat[TextureFormat.ASTC_8x8] = 9;
            registeredFormat[TextureFormat.ASTC_10x10] = 10;
            registeredFormat[TextureFormat.ASTC_12x12] = 11;
            registeredFormat[TextureFormat.DXT1] = 12;
            registeredFormat[TextureFormat.DXT5] = 13;
            registeredFormat[TextureFormat.ARGB32] = 14;
            registeredFormat[TextureFormat.Alpha8] = 15;
            registeredFormat[TextureFormat.ETC2_RGB] = 16;
            registeredFormat[TextureFormat.ETC2_RGBA8] = 17;
            registeredFormat[TextureFormat.ETC_RGB4] = 16;
            

            //filter
            registeredFilter[FilterMode.Point] = 0;
            registeredFilter[FilterMode.Bilinear] = 1;
            registeredFilter[FilterMode.Trilinear] = 2;

            //wrap
            registeredWrap[TextureWrapMode.Clamp] = 0;
            registeredWrap[TextureWrapMode.Repeat] = 1;
            registeredWrap[TextureWrapMode.Mirror] = 2;
        }

        public TextureObject(Texture texture)
        {
            ObjectType = ObjectTypeTexture;
            //-------------------
            // if (!(texture is RenderTexture))
            //     Debug.Log("texture:" + texture.name + "," + texture.width);

            _texture = texture;
            dim[0] = texture.width;
            dim[1] = texture.height;

            if (texture is Texture2D)
            {
                type = 1;
            }

            if (texture is Texture3D)
            {
                type = 2;
                dim[2] = (texture as Texture3D).depth;
            }
        }
        
        private void WriteLevel(Stream stream, int level)
        {
            var tex2d = _texture as Texture2D;
            if (tex2d == null)
            {
                tex2d = Texture2D.whiteTexture;
            }

            Write(stream, level);
            Write(stream, tex2d.width);
            Write(stream, tex2d.height);
            Write(stream, (int) 0);

            if (!registeredFormat.ContainsKey(tex2d.format))
            {
                Debug.LogError("纹理使用了不支持的像素格式:" + _texture.name + ",format:" + tex2d.format);
                return;
            }

            Write(stream, registeredFilter[tex2d.filterMode]);
            Write(stream, registeredWrap[tex2d.wrapMode]);
            Write(stream, registeredFormat[tex2d.format]);
            var data = tex2d.GetRawTextureData();
            Write(stream, data.Length);
            //---------------for external data------
            string path = AssetDatabase.GetAssetPath(tex2d);

            var config = UnityEngine.Object.FindObjectOfType<SceneConfig>(); 
            if(config.isExternalTexture){
                
                int size = data.Length;
                data = new byte[size];
                for (var i = 0; i < size; i++) data[i] = 0;
                var etexture = new ExternaTexturelDataBlock();

                
                etexture.path = path;
                etexture.position = (int) stream.Position;
                etexture.size = size;
                etexture.format = registeredFormat[tex2d.format];
                etexture.objectid = ObjectID;
                //externalTextures.Add(etexture);
                externalTextureData = etexture;
            }
            //--------------------------------------
            Write(stream, data);
        }

        public override void Write(Stream stream)
        {
            //Debug.Log(_texture.name);
            Write(stream, type);
            Write(stream, (int) 1);

            for (int i = 0; i < 1; i++)
            {
                WriteLevel(stream, i);
            }
        }
    }
}