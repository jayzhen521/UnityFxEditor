using System;
using UnityEditor;
using UnityEngine;

namespace Packages.FxEditor
{
    public class ImportResourceProcessor : AssetPostprocessor
    {
        private void OnPostprocessTexture(Texture2D texture)
        {
            EditorUtility.CompressTexture(texture, TextureFormat.RGB24, TextureCompressionQuality.Normal);
            
        }
    }
}